Namespace Collision.Detectors.Discrete
    Public NotInheritable Class CircleVsWorldBounds
        Inherits Specialized
        Implements INarrowPhaseCollisionDetector

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            RequireType(Of Bounds)(0)
            RequireType(Of IFinite)(1)
        End Sub

        Public Function Test(ParamArray args() As Object) As CollisionTestResult Implements INarrowPhaseCollisionDetector.Test
            Return Test(DirectCast(args(0), Bounds), DirectCast(args(1), IFinite))
        End Function

        Private Shared Function Test(worldBounds As Bounds, item As IFinite) As CollisionTestResult
            If worldBounds.Encompasses(item.Bounds()) Then
                Return CollisionTestResult.NoIntersection()
            Else
                Return CollisionTestResult.CurrentlyIntersecting()
            End If
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return TypeOf obj Is CircleVsWorldBounds
        End Function

        Public Overrides Function ToString() As String
            Return $"{NameOf(INarrowPhaseCollisionDetector)}: {NameOf(CircleVsWorldBounds)}" 
        End Function

    End Class
End NameSpace