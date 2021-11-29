Imports System.Drawing

Public Class ShapeFactory
    Implements IShapeFactory
    Protected Shared ReadOnly Random As Random = New Random()

    Private ReadOnly _prototype As IShape
    Private ReadOnly _percentVariance As Single
    Private ReadOnly _availableColors As Queue(Of Color)

    Public Sub New(prototype As IShape, ParamArray availableColors() As Color)
        _prototype = prototype
        _availableColors = New Queue(Of Color)(availableColors)
    End Sub

    Public Function Create() As IShape Implements IShapeFactory.Create
        Dim clone = DirectCast(_prototype.Clone(), IShape)

        With clone
            .Color = _availableColors.Peek()
            .Velocity = New Vector(.Velocity * ) 
            .Move()
        End With

        _availableColors.Enqueue(_availableColors.Dequeue())

        Return clone
    End Function
End Class