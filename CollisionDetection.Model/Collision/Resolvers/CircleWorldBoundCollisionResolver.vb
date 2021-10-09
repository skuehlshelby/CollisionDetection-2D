Imports CollisionDetection.Model.Geometry.ShapeWrappers

Namespace Collision.Resolvers
    Public NotInheritable Class CircleWorldBoundCollisionResolver
        Inherits Specialized
        Implements ICollisionResolver

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            RequireType(Of TimeSpan)(0)
            RequireType(Of Bounds)(1)
            CouldBeType(Of Circle)(2)
            CouldBeType(Of SweptVolume(Of Circle))(2)
        End Sub

        Public Sub Resolve(timeUntilCollision As TimeSpan, ParamArray args() As Object) Implements ICollisionResolver.Resolve
            Resolve(timeUntilCollision, DirectCast(args(0), Bounds), CType(args(1), Circle))
        End Sub

        Private Shared Sub Resolve(timeUntilCollision As TimeSpan, worldBounds As Bounds, circle As Circle)
            If TimeSpan.Zero <= timeUntilCollision Then
                circle.Move(timeUntilCollision)
            End If

            With circle.Bounds()
                If .BottomLeft.X <= 0.0F AndAlso circle.Velocity.X < 0.0F Then circle.Velocity = New Vector(-circle.Velocity.X, circle.Velocity.Y)
                If .BottomLeft.Y <= 0.0F AndAlso circle.Velocity.Y < 0.0F Then circle.Velocity = New Vector(circle.Velocity.X, -circle.Velocity.Y)
                If .TopRight.X >= worldBounds.Width() AndAlso circle.Velocity.X > 0.0F Then circle.Velocity = New Vector(-circle.Velocity.X, circle.Velocity.Y)
                If .TopRight.Y >= worldBounds.Height() AndAlso circle.Velocity.Y > 0.0F Then circle.Velocity = New Vector(circle.Velocity.X, -circle.Velocity.Y)
            End With
        End Sub
    End Class
End NameSpace