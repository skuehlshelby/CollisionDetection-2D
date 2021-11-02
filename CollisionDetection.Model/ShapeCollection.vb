Imports System.Collections.ObjectModel

Public NotInheritable Class ShapeCollection
    Implements IEnumerable(Of IShape)
    Implements ICollection(Of IShape)

    Private Readonly _items As ICollection(Of IShape)
    Private Readonly _colorTracker As IColorTracker
    Private Readonly _shapeFactory As IShapeFactory

    Public Sub New(colorTracker As IColorTracker, shapeFactory As IShapeFactory, Optional initialCount As Integer = 0)
        _items = New Collection(Of IShape)
        _colorTracker = colorTracker
        _shapeFactory = shapeFactory

        Add(initialCount)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of IShape).Count
        Get
            Return _items.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IShape).IsReadOnly
        Get
            Return _items.IsReadOnly
        End Get
    End Property

    Public Sub Add(Optional itemCount As Integer = 1)
        For i = 1 To itemCount
            Add(_shapeFactory.Create())
        Next
    End Sub

    Public Sub Add(item As IShape) Implements ICollection(Of IShape).Add
        _items.Add(item)
    End Sub

    Public Sub Remove(Optional itemCount As Integer = 1)
        For i = 1 To itemCount
            Dim color = _colorTracker.GetMostCommonColorAndDecrementCount()

            Remove(_items.First(Function(item) item.Color.Equals(color)))
        Next
    End Sub

    Public Function Remove(item As IShape) As Boolean Implements ICollection(Of IShape).Remove
        Return _items.Remove(item)
    End Function

    Public Sub Clear() Implements ICollection(Of IShape).Clear
        _items.Clear()
    End Sub

    Public Function Contains(item As IShape) As Boolean Implements ICollection(Of IShape).Contains
        Return _items.Contains(item)
    End Function

    Public Sub CopyTo(array() As IShape, arrayIndex As Integer) Implements ICollection(Of IShape).CopyTo
        _items.CopyTo(array, arrayIndex)
    End Sub

    Private Function GetEnumerator() As IEnumerator(Of IShape) Implements IEnumerable(Of IShape).GetEnumerator
        Return _items.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DirectCast(_items, IEnumerable).GetEnumerator()
    End Function
End Class
