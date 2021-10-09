Imports CollisionDetection.Model.Geometry.ShapeWrappers

Namespace Collision.Resolvers
    Public NotInheritable Class CircleCircleCollisionResolver
        Inherits Specialized
        Implements ICollisionResolver

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            RequireType(Of TimeSpan)(0)
            CouldBeType(Of Circle)(1, 2)
            CouldBeType(Of SweptVolume(Of Circle))(1, 2)
        End Sub

        Private Sub Resolve(timeUntilCollision As TimeSpan, ParamArray args() As Object) Implements ICollisionResolver.Resolve
            Resolve(timeUntilCollision, CType(args(0), Circle), CType(args(1), Circle))
        End Sub

        Private Shared Sub Resolve(timeUntilCollision As TimeSpan, first As Circle, second As Circle)
            If TimeSpan.Zero < timeUntilCollision Then
                first.Move(timeUntilCollision)
                second.Move(timeUntilCollision)
            End If

            Dim collisionNormal As Vector = (first.Center() - second.Center()).ToUnitVec()
            Dim collisionTangent As Vector = collisionNormal.Tangent()
        
            Dim dpTangent1 As Single = first.Velocity.Dot(collisionTangent)
            Dim dpTangent2 As Single = second.Velocity.Dot(collisionTangent)

            Dim dpNormal1 As Single = first.Velocity.Dot(collisionNormal)
            Dim dpNormal2 As Single = second.Velocity.Dot(collisionNormal)

            Dim momentum1 As Single = (dpNormal1 * (first.Mass - second.Mass) + 2.0F * second.Mass * dpNormal2) / (first.Mass + second.Mass)
            Dim momentum2 As Single = (dpNormal2 * (second.Mass - first.Mass) + 2.0F * first.Mass * dpNormal1) / (first.Mass + second.Mass)
    
            first.Velocity = collisionTangent * dpTangent1 + collisionNormal * momentum1
            second.Velocity = collisionTangent * dpTangent2 + collisionNormal * momentum2
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            Return TypeOf obj Is CircleCircleCollisionResolver
        End Function

        Public Overrides Function ToString() As String
            Return $"{NameOf(ICollisionResolver)}: {NameOf(CircleCircleCollisionResolver)}" 
        End Function

    End Class
End NameSpace