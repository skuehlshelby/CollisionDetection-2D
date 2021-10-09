Imports System.Drawing

Namespace Rendering.Drawables

    Public NotInheritable Class RedDashedBounds
        Inherits Specialized
        Implements IDrawable
        Implements IDrawableFactory

        Private ReadOnly _bounds As Bounds

        Private Sub New ()
        End Sub

        Private Sub New (bounds As Bounds)
            _bounds = bounds
        End Sub

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            Require(0, Function(arg) TypeOf arg Is Bounds)
        End Sub

        Private Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            graphics.DrawDashedRectangle(_bounds, Color.FromArgb(218, 0, 55))
        End Sub

        Private Function GetDrawable(Paramarray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New RedDashedBounds(DirectCast(args(0), Bounds))
        End Function

        Public Shared Function AsFactory() As IDrawableFactory
            Return New RedDashedBounds()
        End Function
    End Class

End NameSpace