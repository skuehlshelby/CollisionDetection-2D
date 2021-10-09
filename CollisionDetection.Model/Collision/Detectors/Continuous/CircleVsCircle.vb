Imports CollisionDetection.Model.Geometry.ShapeWrappers

Namespace Collision.Detectors.Continuous
    Public NotInheritable Class CircleVsCircle
        Inherits Specialized
        Implements INarrowPhaseCollisionDetector

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            CouldBeType(Of Circle)
            CouldBeType(Of SweptVolume(Of Circle))
        End Sub

        Private Function Test(ParamArray args() As Object) As CollisionTestResult Implements INarrowPhaseCollisionDetector.Test
            Return Test(CType(args(0), Circle), CType(args(1), Circle))
        End Function

        Private Shared Function Test(first As Circle, second As Circle) As CollisionTestResult
            Dim displacement As Vector = second.Center - first.Center
            Dim relativeVelocity As Vector = second.Velocity - first.Velocity
            Dim sumOfRadii As Single = first.Radius + second.Radius


            If Not AreMovingTowardsEachOther(relativeVelocity, displacement) Then
                Return CollisionTestResult.NoIntersection()
            End If

            If AreColliding(first, second) Then
                Return CollisionTestResult.CurrentlyIntersecting()
            End If

            If Not AreMovingRelativeToEachOther(relativeVelocity) Then
                Return CollisionTestResult.NoIntersection()
            End If

            If Not CollisionExists(relativeVelocity, displacement, sumOfRadii) Then
                Return CollisionTestResult.NoIntersection()
            End If

            Dim timeUntilCollision As TimeSpan = GetTimeUntilCollision(relativeVelocity, displacement, sumOfRadii)

            If first.CanMove(timeUntilCollision) AndAlso second.CanMove(timeUntilCollision) Then
                Return CollisionTestResult.WillIntersectAfter(timeUntilCollision)
            Else
                Return CollisionTestResult.NoIntersection()
            End If
        End Function

        Private Shared Function AreColliding(first As Circle, second As Circle) As Boolean
            Return first.Center.Distance(second.Center) < (first.Radius + second.Radius)
        End Function

        Private Function CanCollide(first As Circle, second As Circle) As Boolean
            Dim closestPoints As UnorderedPair(Of Point) = (first.PointClosestTo(second.Center), second.PointClosestTo(first.Center))
            Dim distance As Double = closestPoints.Query(Function(l, r) l.Distance(r))
            Dim maximumMovement As Vector = first.Velocity + second.Velocity

            Return maximumMovement.Magnitude() > distance
        End Function

        Private Shared Function AreMovingRelativeToEachOther(relativeVelocity As Vector) As Boolean
            Return relativeVelocity.Dot(relativeVelocity) >= Single.Epsilon
        End Function

        Private Shared Function AreMovingTowardsEachOther(relativeVelocity As Vector, displacement As Vector) As Boolean
            Return relativeVelocity.Dot(displacement) < 0.0F
        End Function

        Private Shared Function CollisionExists(relativeVelocity As Vector, displacement As Vector, sumOfRadii As Single) As Boolean
            Dim a As Single = relativeVelocity.Dot(relativeVelocity)
            Dim b As Single = relativeVelocity.Dot(displacement)
            Dim c As Single = displacement.Dot(displacement) - (sumOfRadii * sumOfRadii)
            Dim d As Single = b * b - a * c

            Return b * b - a * c > 0.0F
        End Function

        Private Shared Function GetTimeUntilCollision(relativeVelocity As Vector, displacement As Vector, sumOfRadii As Single) As TimeSpan
            Dim a As Single = relativeVelocity.Dot(relativeVelocity)
            Dim b As Single = relativeVelocity.Dot(displacement)
            Dim c As Single = displacement.Dot(displacement) - (sumOfRadii * sumOfRadii)
            Dim d As Single = b * b - a * c

            Return TimeSpan.FromSeconds((-b - CSng(Math.Sqrt(d))) / a)
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return TypeOf obj Is CircleVsCircle
        End Function

        Public Overrides Function ToString() As String
            Return $"{NameOf(INarrowPhaseCollisionDetector)}: {NameOf(CircleVsCircle)}" 
        End Function
    End Class
End NameSpace