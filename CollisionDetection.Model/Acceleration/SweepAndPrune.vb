Namespace Acceleration

    Public NotInheritable Class SweepAndPrune(Of T As IFinite)
        Implements IAccelerationStructure(Of T)
    
        Private Const InitialCapacity As Integer = 4
        Private _items(InitialCapacity) As T
        Private ReadOnly _comparer As IComparer(Of T)
        Private _top As Integer = 0

        Public Sub New (ParamArray items() As T)
            _comparer = New SapBoundsComparer(Axis.Horizontal)

            For Each item As T In items
                Add(item)
            Next
        End Sub

        Private NotInheritable Class SapBoundsComparer
            Implements IComparer(Of T)

            Private ReadOnly _projectedAxis As Axis

            Public Sub New(projectedAxis As Axis)
                _projectedAxis = projectedAxis
            End Sub

            Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
                Return _projectedAxis.Compare(x.Bounds().BottomLeft, y.Bounds().BottomLeft)
            End Function
        End Class

        Private Sub ReSizeArray(desiredSize As UShort)
            ReDim Preserve _items(desiredSize)
        End Sub

        Public Sub Add(item As T) Implements IAccelerationStructure(Of T).Add
            _items(_top) = item
            _top += 1
            Sort()

            If _top = _items.Length Then
                ReSizeArray(CUShort(_items.Length) * 2US)
            End If
        End Sub

        Public Sub Remove(item As T) Implements IAccelerationStructure(Of T).Remove
            For i As Integer = LBound(_items) To UBound(_items)
                If _items(i).Equals(item) Then
                    _items(i) = Nothing
                    Array.Copy(_items, i + 1, _items, i, _top - i)
                    _top -= 1

                    If _top < (_items.Length \ 4) AndAlso _items.Length > InitialCapacity Then 'If the array is less than 1/4 full and is not still the initial size
                        ReSizeArray(CUShort(_items.Length) \ 2US) 'Then halve the size of the array
                    End If
                    Exit For
                End If
            Next
        End Sub

        Public Sub Refit() Implements IAccelerationStructure(Of T).Refit
            Sort()
        End Sub

        Public Function ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of T)) Implements IAccelerationStructure(Of T).ObjectsThatIntersectEachOther
            Dim possibleCollisions As ICollection(Of UnorderedPair(Of T)) = new List(Of UnorderedPair(Of T))

            For i As Integer = LBound(_items) To UBound(_items) - 1
                For j As Integer = i + 1 To UBound(_items)
                    If _items(i).Bounds().Intersects(_items(j).Bounds()) Then
                        possibleCollisions.Add(New UnorderedPair(Of T)(_items(i), _items(j)))
                    Else
                        Exit For
                    End If
                Next
            Next

            Return possibleCollisions
        End Function

        Public Function ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of T) Implements IAccelerationStructure(Of T).ObjectsOutsideOrPartiallyOutside
            Return _items.Where(Function(item) Not bounds.Encompasses(item.Bounds())) 'There is probably a smarter way to do this.
        End Function

        Public Function ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of T) Implements IAccelerationStructure(Of T).ObjectsInsideOrPartiallyInside
            Return _items.Where(Function(item) bounds.Intersects(item.Bounds())) 'There is probably a smarter way to do this.
        End Function

        Private Sub Sort()
            Dim currentPosition = 1

            While currentPosition < _items.Length
                Dim left As Integer = currentPosition - 1
                Dim right As Integer = currentPosition

                While left >= 0 AndAlso LeftIsGreaterThanRight(_items(left), _items(right), _comparer)
                    Swap(_items, left, right)
                    left -= 1
                    right -= 1
                End While

                currentPosition += 1
            End While
        End Sub

        Private Shared Function LeftIsGreaterThanRight(left As T, right As T, comparer As IComparer(Of T)) As Boolean
            Return comparer.Compare(left, right) > 0
        End Function

        Private Shared Sub Swap(ByRef array As T(), left As Integer, right As Integer)
            If Not left = right Then
                Dim temp As T = array(left)

                array(left) = array(right)

                array(right) = temp
            End If
        End Sub
    End Class
End NameSpace