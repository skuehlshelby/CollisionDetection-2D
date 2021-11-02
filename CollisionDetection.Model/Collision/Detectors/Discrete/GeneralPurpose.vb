Namespace Collision.Detectors.Discrete
    Public NotInheritable Class GeneralPurpose
        Inherits Specialized
        Implements INarrowPhaseCollisionDetector

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            Require(Function(args) args.Length = 2)
            RequireType(Of IPointQueryable)()
            RequireType(Of IPhysical)()
        End Sub

        Public Function Test(ParamArray args() As Object) As CollisionTestResult Implements INarrowPhaseCollisionDetector.Test
            Return ActualTest(args(0), args(1))
        End Function

        Private Shared Function ActualTest(first As Object, second As Object) As CollisionTestResult
            If AreColliding(DirectCast(first, IPointQueryable), DirectCast(second, IPointQueryable)) AndAlso AreMovingTowardsEachOther(DirectCast(first, IPhysical), DirectCast(second, IPhysical)) Then
                Return CollisionTestResult.CurrentlyIntersecting()
            Else
                Return CollisionTestResult.NoIntersection()
            End If
        End Function

        Private Shared Function AreColliding(first As IPointQueryable, second As IPointQueryable) As Boolean
            Return first.Contains(second.PointClosestTo(first.Center()))
        End Function

        Private Shared Function AreMovingTowardsEachOther(first As IPhysical, second As IPhysical) As Boolean
            Dim displacement As Vector = second.Center() - first.Center()
            Dim relativeVelocity As Vector = second.Velocity - first.Velocity

            Return  relativeVelocity.Dot(displacement) < 0.0F
        End Function

        Public Overrides Function ToString() As String
            Return $"{NameOf(INarrowPhaseCollisionDetector)}: {NameOf(GeneralPurpose)}" 
        End Function

    End Class
End NameSpace