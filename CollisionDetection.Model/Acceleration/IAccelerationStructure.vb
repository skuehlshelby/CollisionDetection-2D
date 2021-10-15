Namespace Acceleration

    Public Interface IAccelerationStructure(Of T As IFinite)

        Sub Add(item As T)

        Sub Remove(item As T)

        Sub Refit()

        Function ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of T))

        Function ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of T)

        Function ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of T)

    End Interface

End NameSpace