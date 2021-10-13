Imports System.Drawing.Drawing2D
Imports CollisionDetection.Model
Imports CollisionDetection.Model.Rendering
Imports CollisionDetection.Model.Rendering.Drawables

Public Class GraphicsWrapper
    Implements IGraphics

    Private ReadOnly _graphics As Graphics
    Private Shared ReadOnly DashedPen As Pen = New Pen(Color.AliceBlue) With { .DashStyle = DashStyle.Dash }
    Private Shared ReadOnly SolidBrush As SolidBrush = New SolidBrush(Color.AliceBlue)

    Sub New(graphics As Graphics)
        _graphics = graphics
    End Sub

    Public Sub DrawDashedRectangle(bounds As Bounds, color As Color) Implements IGraphics.DrawDashedRectangle
        DashedPen.Color = color
        _graphics.DrawRectangle(DashedPen, bounds)
    End Sub

    Public Sub FillEllipse(bounds As Bounds, color As Color) Implements IGraphics.FillEllipse
        SolidBrush.Color = color
        _graphics.FillEllipse(SolidBrush, bounds)
    End Sub

    Public Sub DrawText(text As String, fontSize As Single, fontName As FontName, color As Color, location As Point) Implements IGraphics.DrawText
        SolidBrush.Color = color
        _graphics.DrawString(text, New Font(FontFamily.GenericMonospace, fontSize), SolidBrush, location)
    End Sub
End Class