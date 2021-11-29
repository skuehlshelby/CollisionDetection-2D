Imports System.Drawing
Imports CollisionDetection.Model.Collision
Imports CollisionDetection.Model.Rendering
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Application
    Public Interface IApplication

        Property IsPaused As Boolean

        Property VisualizeAccelerationStructure As Boolean

        Property AccelerationStructure As IAccelerationStructure

        Property CollisionHandler As CollisionHandler

        ReadOnly Property ShapeColors As ICollection(Of Color)

        Property NumberOfShapes As Integer

        Property WorldBounds As Bounds

        Sub Update(timeStep As TimeSpan)

        Sub Render(graphics As IGraphics, ParamArray extras() As IDrawable)
    End Interface
End NameSpace