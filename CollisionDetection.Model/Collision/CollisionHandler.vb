Imports CollisionDetection.Model.BVH
Imports CollisionDetection.Model.Collision.Detectors.Continuous
Imports CollisionDetection.Model.Collision.Detectors.Discrete
Imports CollisionDetection.Model.Collision.Resolvers
Imports CollisionDetection.Model.Geometry.ShapeWrappers

Namespace Collision
    Public MustInherit Class CollisionHandler

        Protected ReadOnly CollisionDetectors As SpecializedSet(Of INarrowPhaseCollisionDetector)
        Protected ReadOnly CollisionResolvers As SpecializedSet(Of ICollisionResolver)

        Private Sub New(name As String, ParamArray collisionDetectionMethods() As INarrowPhaseCollisionDetector)
            Me.Name = name
            CollisionDetectors = New SpecializedSet(Of INarrowPhaseCollisionDetector)(collisionDetectionMethods)
            CollisionResolvers = New SpecializedSet(Of ICollisionResolver)(New CircleCircleCollisionResolver(), New CircleWorldBoundCollisionResolver())
        End Sub

        Public ReadOnly Property Name As String

        Public MustOverride Function DetectAndResolveCollisions(Of T As {IFinite, IMoveable, ICloneable})(splitMethod As SplitMethod, objects As IEnumerable(Of T), worldBounds As Bounds) As Maybe(Of IBoundingVolumeHierarchy)

        Public Shared ReadOnly Property BruteForce As CollisionHandler = New BruteForceCollisionHandler()

        Public Shared ReadOnly Property Discrete As CollisionHandler = New DiscreteCollisionHandler()

        Public Shared ReadOnly Property Continuous As CollisionHandler = New ContinuousCollisionHandler()

        Public Shared Function Values() As IEnumerable(Of CollisionHandler)
            Return New CollisionHandler() {BruteForce, Discrete, Continuous}
        End Function

        Public Shared Function Parse(name As String) As CollisionHandler
            Return Values().Single(Function(value) value.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        End Function

        Private NotInheritable Class BruteForceCollisionHandler
            Inherits CollisionHandler

            Public Sub New()
                MyBase.New("Brute Force", New GeneralPurpose(), New CircleVsWorldBounds())
            End Sub

            Public Overrides Function DetectAndResolveCollisions(Of T As {IFinite, IMoveable, ICloneable})(splitMethod As SplitMethod, objects As IEnumerable(Of T), worldBounds As Bounds) As Maybe(Of IBoundingVolumeHierarchy)
                Dim possibleCollisions As ISet(Of UnorderedPair(Of T)) = New HashSet(Of UnorderedPair(Of T))()

                For Each shape As T In objects
                    For Each otherShape As T In objects
                        If Not shape.Equals(otherShape) Then
                            If shape.Bounds.Intersects(otherShape.Bounds()) Then
                                possibleCollisions.Add(New UnorderedPair(Of T)(shape, otherShape))
                            End If
                        End If
                    Next
                Next

                For Each possibleCollision As UnorderedPair(Of T) In possibleCollisions
                    Dim left, right As T

                    possibleCollision.Deconstruct(left, right)

                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(left, right) _
                            .Test(left, right)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, left, right) _
                            .Resolve(result.TimeUntilIntersection, left, right)
                    End If
                Next

                For Each shape As T In objects
                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(worldBounds, shape) _
                            .Test(worldBounds, shape)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, worldBounds, shape) _
                            .Resolve(result.TimeUntilIntersection, worldBounds, shape)
                    End If
                Next

                Return Nothing
            End Function
        End Class

        Private NotInheritable Class DiscreteCollisionHandler
            Inherits CollisionHandler

            Public Sub New()
                MyBase.New("Discrete", New GeneralPurpose(), New CircleVsWorldBounds())
            End Sub

            Public Overrides Function DetectAndResolveCollisions(Of T As {IFinite, IMoveable, ICloneable})(splitMethod As SplitMethod, objects As IEnumerable(Of T), worldBounds As Bounds) As Maybe(Of IBoundingVolumeHierarchy)
                Dim bvh As IBoundingVolumeHierarchy(Of T) = New BoundingVolumeHierarchy(Of T)(objects, splitMethod)

                For Each collision As UnorderedPair(Of T) In bvh.ObjectsThatIntersectEachOther()
                    Dim left, right As T

                    collision.Deconstruct(left, right)

                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(left, right) _
                            .Test(left, right)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, left, right) _
                            .Resolve(result.TimeUntilIntersection, left, right)
                    End If
                Next

                For Each escapee As T In bvh.ObjectsOutsideOrPartiallyOutside(worldBounds)
                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(worldBounds, escapee) _
                            .Test(worldBounds, escapee)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, worldBounds, escapee) _
                            .Resolve(result.TimeUntilIntersection, worldBounds, escapee)
                    End If
                Next

                Return DirectCast(bvh, BoundingVolumeHierarchy(Of T))
            End Function
        End Class

        Private NotInheritable Class ContinuousCollisionHandler
            Inherits CollisionHandler

            Public Sub New()
                MyBase.New("Continuous", New CircleVsCircle(), New CircleVsWorldBounds())
            End Sub

            Public Overrides Function DetectAndResolveCollisions(Of T As {IFinite, IMoveable, ICloneable})(splitMethod As SplitMethod, objects As IEnumerable(Of T), worldBounds As Bounds) As Maybe(Of IBoundingVolumeHierarchy)
                Dim sweptVolumes As IEnumerable(Of SweptVolume(Of T)) = objects.Select(Function(o) SweptVolume(Of T).FromMovement(o))
                Dim bvh As IBoundingVolumeHierarchy(Of SweptVolume(Of T)) = New BoundingVolumeHierarchy(Of SweptVolume(Of T))(sweptVolumes, splitMethod)

                For Each collision As UnorderedPair(Of SweptVolume(Of T)) In bvh.ObjectsThatIntersectEachOther()
                    Dim left, right As T

                    collision.Transform(Function(c) c.Original).Deconstruct(left, right)

                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(left, right) _
                            .Test(left, right)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, left, right) _
                            .Resolve(result.TimeUntilIntersection, left, right)
                    End If
                Next

                For Each escapee As SweptVolume(Of T) In bvh.ObjectsOutsideOrPartiallyOutside(worldBounds)
                    Dim result As CollisionTestResult = CollisionDetectors _
                            .Specialized(worldBounds, escapee.Original) _
                            .Test(worldBounds, escapee.Original)

                    If result.AreIntersectingOrWillIntersect Then

                        CollisionResolvers _
                            .Specialized(result.TimeUntilIntersection, worldBounds, escapee.Original) _
                            .Resolve(result.TimeUntilIntersection, worldBounds, escapee.Original)
                    End If
                Next

                Return DirectCast(bvh, BoundingVolumeHierarchy(Of SweptVolume(Of T)))
            End Function
        End Class
    End Class
End Namespace