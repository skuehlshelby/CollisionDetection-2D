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
        
        Private Sub New()
        End Sub

        Sub New(text As String, textSize As Single, color As Color, location As Point)
            _text = text
            _textSize = textSize
            _color = color
            _location = location
        End Sub

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            RequireType(Of String)(0)
            CouldBeType(Of Integer)(1)
            CouldBeType(Of Single)(1)
            CouldBeType(Of Double)(1)
            RequireType(Of Color)(2)
            RequireType(Of Point)(3)
        End Sub

        Public Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            graphics.DrawText(_text, _textSize, FontName.CourierNew, _color, _location)
        End Sub

        Private Function GetDrawable(ParamArray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New SolidMonoSpaceText(CStr(args(0)), CSng(args(1)), CType(args(2), Color), CType(args(3), Point))
        End Function

        Public Shared Function AsFactory() As IDrawableFactory
            Return New SolidMonoSpaceText()
        End Function
    End Class
End NameSpace