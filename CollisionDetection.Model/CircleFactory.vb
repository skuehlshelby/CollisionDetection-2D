Imports System.Drawing

Public NotInheritable Class CircleFactory
    Implements IObserver(Of Bounds)
    Implements IShapeFactory

    Private ReadOnly _random As Random = New Random()
    Private ReadOnly _colorTracker As IColorTracker
    Private ReadOnly _minimumSize As Integer
    Private ReadOnly _maximumSize As Integer
    Private ReadOnly _minimumVelocity As Integer
    Private ReadOnly _maximumVelocity As Integer
    Private _worldBounds As Bounds

    Public Sub New(colorTracker As IColorTracker, worldBounds As Bounds, minimumSize As Integer, maximumSize As Integer, minimumVelocity As Integer, maximumVelocity As Integer)
        _colorTracker = colorTracker
        _worldBounds = worldBounds
        _minimumSize = minimumSize
        _maximumSize = maximumSize
        _minimumVelocity = minimumVelocity
        _maximumVelocity = maximumVelocity
    End Sub

#Region "IObserver Members"

    Private Sub OnCompleted() Implements IObserver(Of Bounds).OnCompleted
        Return
    End Sub

    Private Sub OnError([error] As Exception) Implements IObserver(Of Bounds).OnError
        Return
    End Sub

    Private Sub OnNext(value As Bounds) Implements IObserver(Of Bounds).OnNext
        _worldBounds = value
    End Sub

#End Region

    Private Function Create() As IShape Implements IShapeFactory.Create
        Dim radius As Single = _random.Next(_minimumSize, _maximumSize)
        Dim color As Color = _colorTracker.GetLeastCommonColorAndIncrementCount()
        Dim position As Point = (_random.Next(0, CInt(_worldBounds.Width)), _random.Next(0, CInt(_worldBounds.Height)))
        Dim velocity As Vector = (_random.Next(_minimumVelocity, _maximumVelocity), _random.Next(_minimumVelocity, _maximumVelocity))

        return new Circle(radius, color, position, velocity)
    End Function
End Class