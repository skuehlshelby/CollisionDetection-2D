Public NotInheritable Class SpecializedSet(Of T As ISpecialized)
    Implements IEnumerable(Of T)
    Private ReadOnly _items As LinkedList(Of T)

    Sub New(ParamArray items() As T)
        _items = New LinkedList(Of T)(items)
    End Sub

    Public Function Specialized(ParamArray args() As Object) As T
        If Not _items.First.Value.IsApplicable(args) Then
            Dim applicable As T = _items.AsEnumerable().First(Function(item) item.IsApplicable(args))
            _items.Remove(applicable)
            _items.AddFirst(applicable)
        End If

        Return _items.First.Value
    End Function

    Public Sub Add(item As T)
        _items.AddFirst(item)
    End Sub

    Public Sub Add(ParamArray items() As T)
        For Each item As T In items
            _items.AddFirst(item)
        Next
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return DirectCast(_items, IEnumerable(Of T)).GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return DirectCast(_items, IEnumerable).GetEnumerator()
    End Function
End Class