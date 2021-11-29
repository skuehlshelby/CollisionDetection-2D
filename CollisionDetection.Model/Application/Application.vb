Imports System.Collections.ObjectModel
Imports System.Drawing
Imports CollisionDetection.Model.BVH
Imports CollisionDetection.Model.Collision
Imports CollisionDetection.Model.Rendering
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Application

    Public Class Application
        Implements IApplication

        Private ReadOnly _shapes As ICollection(Of IShape)
        Private ReadOnly _colors As ObservableCollection(Of Color)
        Private ReadOnly _colorTracker As IColorTracker
        Private ReadOnly _spawner As 
        Public Property IsPaused As Boolean Implements IApplication.IsPaused

        Public Property VisualizeAccelerationStructure As Boolean Implements IApplication.VisualizeAccelerationStructure

        Public Property AccelerationStructure As IAccelerationStructure Implements IApplication.AccelerationStructure

        Public Property CollisionHandler As CollisionHandler Implements IApplication.CollisionHandler

        Public ReadOnly Property ShapeColors As ICollection(Of Color) Implements IApplication.ShapeColors
            Get
                Return _colors
            End Get
        End Property

        Public Property NumberOfShapes As Integer Implements IApplication.NumberOfShapes
            Get
                Return _shapes.Count
            End Get
            Set
                If Value < _shapes.Count Then
                Else
                    Throw New NotImplementedException()
                End If
            End Set
        End Property

        Public Property WorldBounds As Bounds Implements IApplication.WorldBounds

        Public Sub Update(timeStep As TimeSpan) Implements IApplication.Update
            If IsPaused OrElse Not _shapes.Any() Then Return

            For Each shape As IMoveable In _shapes.OfType(Of IMoveable)()
                shape.AddTime(timeStep)
            Next

            CollisionHandler.DetectAndResolveCollisions(SplitMethod.Midpoint, _shapes, WorldBounds)

            For Each shape As IMoveable In _shapes.OfType(Of IMoveable)()
                shape.Move()
            Next
        End Sub

        Public Sub Render(graphics As IGraphics, ParamArray extras() As IDrawable) Implements IApplication.Render
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace
