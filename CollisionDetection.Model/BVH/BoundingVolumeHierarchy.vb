
Namespace BVH

    Public Class BoundingVolumeHierarchy(Of T As IFinite)
        Implements IBoundingVolumeHierarchy
        Implements IBoundingVolumeHierarchy(Of T)

        Private ReadOnly _root As BvhBuildNode

        Public Sub New(items As T(), splitMethod As SplitMethod)
            _root = Build(items, splitMethod)
        End Sub

        Public Sub New(items As IEnumerable(Of T), splitMethod As SplitMethod)
            Me.New(items.ToArray(), splitMethod)
        End Sub

        Private Shared Function Build(items As T(), splitMethod As SplitMethod) As BvhBuildNode
            If items.Length > 1 Then
                Dim result As SplitResult(Of T) = splitMethod.Split(items)

                If result.Failed Then
                    Return BvhBuildNode.CreateLeafNode(items)
                Else
                    Return BvhBuildNode.CreateInteriorNode(result.SplitAxis, Build(result.FirstHalf, splitMethod), Build(result.SecondHalf, splitMethod))
                End If
            Else
                Return BvhBuildNode.CreateLeafNode(items)
            End If
        End Function

        Private NotInheritable Class BvhBuildNode
            Private Sub New(shapes As T())
                Me.Shapes = shapes
                NumberOfChildren = shapes.Length
                Bounds = Bounds.Union(shapes.Select(Function(shape) shape.Bounds()))
                Left = Nothing
                Right = Nothing
                Axis = Nothing
                IsLeaf = True
            End Sub

            Private Sub New(axis As Axis, left As BvhBuildNode, right As BvhBuildNode)
                Me.Axis = axis
                Me.Left = left
                Me.Right = right
                Bounds = left.Bounds.Union(right.Bounds)
                NumberOfChildren = left.NumberOfChildren + right.NumberOfChildren
                Shapes = Nothing
                IsLeaf = False
            End Sub

            Public ReadOnly Property Axis As Axis

            Public ReadOnly Property Bounds As Bounds

            Public ReadOnly Property Left As BvhBuildNode

            Public ReadOnly Property Right As BvhBuildNode

            Public ReadOnly Property Shapes As T()

            Public ReadOnly Property NumberOfChildren As Integer

            Public ReadOnly Property IsLeaf As Boolean

            Public Shared Function CreateLeafNode(shapes As T()) As BvhBuildNode
                Return New BvhBuildNode(shapes)
            End Function

            Public Shared Function CreateInteriorNode(axis As Axis, left As BvhBuildNode, right As BvhBuildNode) As BvhBuildNode
                Return New BvhBuildNode(axis, left, right)
            End Function

        End Class

#Region "IBoundingVolumeHierarchy Members"

        Private Function BoundingVolumes() As IEnumerable(Of Bounds) Implements IBoundingVolumeHierarchy.BoundingVolumes, IBoundingVolumeHierarchy(Of T).BoundingVolumes
            Dim results As ICollection(Of Bounds) = New List(Of Bounds)()

            If _root IsNot Nothing Then GetBoundingVolumes(_root, results)

            Return results
        End Function

        Private Shared Sub GetBoundingVolumes(node As BvhBuildNode, ByRef results As ICollection(Of Bounds))
            results.Add(node.Bounds)

            If Not node.IsLeaf Then
                GetBoundingVolumes(node.Left, results)
                GetBoundingVolumes(node.Right, results)
            End If
        End Sub

        Private Function ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of IFinite)) Implements IBoundingVolumeHierarchy.ObjectsThatIntersectEachOther
            Return Generic_ObjectsThatIntersectEachOther().Select(Function(o) o.Cast(Of IFinite))
        End Function

        Private Function ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of IFinite) Implements IBoundingVolumeHierarchy.ObjectsOutsideOrPartiallyOutside
            Return Generic_ObjectsOutsideOrPartiallyOutside(bounds).OfType(Of IFinite)
        End Function

        Private Function ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of IFinite) Implements IBoundingVolumeHierarchy.ObjectsInsideOrPartiallyInside
            Return Generic_ObjectsInsideOrPartiallyInside(bounds).OfType(Of IFinite)
        End Function

#End Region

#Region "IBoundingVolumeHierarchy(Of T) Members"

        Private Function Generic_ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of T)) Implements IBoundingVolumeHierarchy(Of T).ObjectsThatIntersectEachOther
            Dim results As ISet(Of UnorderedPair(Of T)) = New HashSet(Of UnorderedPair(Of T))()

            If _root IsNot Nothing AndAlso Not _root.IsLeaf Then
                ObjectsThatIntersectEachOther(_root.Left, _root.Right, results)
            End If

            Return results
        End Function

        Private Shared Sub ObjectsThatIntersectEachOther(left As BvhBuildNode, right As BvhBuildNode, candidates As ISet(Of UnorderedPair(Of T)))
            If Bounds.AreDisjoint(left.Bounds, right.Bounds) Then
                If Not left.IsLeaf Then
                    ObjectsThatIntersectEachOther(left.Left, left.Right, candidates)
                End If
                If Not right.IsLeaf Then
                    ObjectsThatIntersectEachOther(right.Left, right.Right, candidates)
                End If
            Else
                If left.IsLeaf AndAlso right.IsLeaf Then
                    Dim leftAndRight As T() = left.Shapes.Concat(right.Shapes).ToArray()

                    For i As Integer = 0 To leftAndRight.Length - 2
                        For j As Integer = i To leftAndRight.Length - 1
                            candidates.Add(New UnorderedPair(Of T)(leftAndRight(i), leftAndRight(j)))
                        Next
                    Next
                ElseIf left.IsLeaf Then
                    ObjectsThatIntersectEachOther(left, right.Left, candidates)
                    ObjectsThatIntersectEachOther(left, right.Right, candidates)
                    ObjectsThatIntersectEachOther(right.Left, right.Right, candidates)
                ElseIf right.IsLeaf THen
                    ObjectsThatIntersectEachOther(left.Left, right, candidates)
                    ObjectsThatIntersectEachOther(left.Right, right, candidates)
                    ObjectsThatIntersectEachOther(left.Left, left.Right, candidates)
                Else
                    ObjectsThatIntersectEachOther(left.Left, left.Right, candidates)
                    ObjectsThatIntersectEachOther(left.Left, right.Left, candidates)
                    ObjectsThatIntersectEachOther(left.Left, right.Right, candidates)
                    ObjectsThatIntersectEachOther(left.Right, right.Left, candidates)
                    ObjectsThatIntersectEachOther(left.Right, right.Right, candidates)
                    ObjectsThatIntersectEachOther(right.Left, right.Right, candidates)
                End If
            End If
        End Sub

        Private Function Generic_ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of T) Implements IBoundingVolumeHierarchy(Of T).ObjectsOutsideOrPartiallyOutside
            Dim results As ISet(Of T) = New HashSet(Of T)()

            If _root IsNot Nothing Then ObjectsOutsideOrPartiallyOutside(_root, bounds, results)

            Return results
        End Function

        Private Shared Sub ObjectsOutsideOrPartiallyOutside(node As BvhBuildNode, bounds As Bounds, results As ISet(Of T))
            If Not bounds.Encompasses(node.Bounds) Then
                If node.IsLeaf Then
                    For Each shape As T In node.Shapes
                        results.Add(shape)
                    Next
                Else
                    ObjectsOutsideOrPartiallyOutside(node.Left, bounds, results)
                    ObjectsOutsideOrPartiallyOutside(node.Right, bounds, results)
                End If
            End If
        End Sub

        Private Function Generic_ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of T) Implements IBoundingVolumeHierarchy(Of T).ObjectsInsideOrPartiallyInside
            Dim results As ISet(Of T) = New HashSet(Of T)()

            If _root IsNot Nothing Then ObjectsInsideOrPartiallyInside(_root, bounds, results)

            Return results
        End Function

        Private Shared Sub ObjectsInsideOrPartiallyInside(node As BvhBuildNode, bounds As Bounds, results As ISet(Of T))
            If bounds.Intersects(node.Bounds) Then
                If node.IsLeaf Then
                    For Each shape As T In node.Shapes
                        results.Add(shape)
                    Next
                End If

                ObjectsInsideOrPartiallyInside(node.Left, bounds, results)
                ObjectsInsideOrPartiallyInside(node.Right, bounds, results)
            End If
        End Sub

#End Region

    End Class
End NameSpace