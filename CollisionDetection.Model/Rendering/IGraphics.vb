Imports System.Drawing
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Rendering

    Public Interface IGraphics
    
        Sub DrawDashedRectangle(bounds As Bounds, color As Color)

        Sub FillEllipse(bounds As Bounds, color As Color)

        Sub DrawText(text As String, fontSize As Single, fontName As FontName, color As Color, location As Point)

    End Interface
End NameSpace