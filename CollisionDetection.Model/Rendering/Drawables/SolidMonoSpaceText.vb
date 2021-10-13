Imports System.Drawing

Namespace Rendering.Drawables
    Public NotInheritable Class SolidMonoSpaceText
        Inherits Specialized
        Implements IDrawable
        Implements IDrawableFactory

        Private ReadOnly _text As String
        Private ReadOnly _textSize As Single
        Private ReadOnly _color As Color
        Private ReadOnly _location As Point
        
        Private Sub New(textSize As Single, color As Color, location As Point)
            _textSize = textSize
            _color = color
            _location = location
        End Sub

        Private Sub New(text As String, template As SolidMonoSpaceText)
            _text = text
            _textSize = template._textSize
            _color = template._color
            _location = template._location
        End Sub

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            RequireType(Of String)(0)
        End Sub

        Public Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            graphics.DrawText(_text, _textSize, FontName.CourierNew, _color, _location)
        End Sub

        Private Function GetDrawable(ParamArray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New SolidMonoSpaceText(CStr(args(0)), Me)
        End Function

        Public Shared Function AsFactory(textSize As Single, color As Color, location As Point) As IDrawableFactory
            Return New SolidMonoSpaceText(textSize, color, location)
        End Function
    End Class
End NameSpace