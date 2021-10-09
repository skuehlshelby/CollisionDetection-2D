Imports System.Drawing
Imports CollisionDetection.Model.Geometry.ShapeWrappers

Namespace Rendering.Drawables

    Public NotInheritable Class SolidCircle
        Inherits Specialized
        Implements IDrawable
        Implements IDrawableFactory

        Private ReadOnly _circle As Circle

        Private Sub New(circle As Circle)
            _circle = circle
        End Sub

        Private Sub New()
        End Sub

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            CouldBeType(Of Circle)(0)
            CouldBeType(Of IUnwrappable(Of Circle))(0)
        End Sub

        Private Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            graphics.FillEllipse(_circle.Bounds(), _circle.Color)
        End Sub

        Private Function GetDrawable(Paramarray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New SolidCircle(CType(args(0), Circle))
        End Function

        Public Shared Function AsFactory() As IDrawableFactory
            Return New SolidCircle()
        End Function
    End Class
End NameSpace