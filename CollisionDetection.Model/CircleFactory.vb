Imports System.Drawing

Public NotInheritable Class CircleFactory
    Implements IShapeFactory

    Private ReadOnly _random As Random = New Random()
    Private ReadOnly _colorTracker As IColorTracker

    Public Sub New(colorTracker As IColorTracker)
        _colorTracker = colorTracker
    End Sub

    Public Function SpawnAt(point As Point) As IShape Implements IShapeFactory.SpawnAt
        Dim radius As Single = _random.Next(5, 40)
        Dim color As Color = _colorTracker.GetLeastCommonColorAndIncrementCount()
        Dim velocity As Vector = (_random.Next(-40, 40), _random.Next(-40, 40))

        return new Circle(radius, color, point, velocity)
    End Function

    Public Function SpawnRandomly(worldBounds As Bounds) As IShape Implements IShapeFactory.SpawnRandomly
        Return SpawnAt((_random.Next(10, CInt(worldBounds.Width()) - 10), _random.Next(10, CInt(worldBounds.Height()) - 10)))
    End Function
End Class