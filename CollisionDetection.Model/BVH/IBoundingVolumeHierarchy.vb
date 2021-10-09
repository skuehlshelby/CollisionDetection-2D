Namespace BVH
    Public Interface IBoundingVolumeHierarchy

        Function BoundingVolumes() As IEnumerable(Of Bounds)

        Function ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of IFinite))

        Function ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of IFinite)

        Function ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of IFinite)

    End Interface

    Public Interface IBoundingVolumeHierarchy(Of T As IFinite)

        Function BoundingVolumes() As IEnumerable(Of Bounds)

        Function ObjectsThatIntersectEachOther() As IEnumerable(Of UnorderedPair(Of T))

        Function ObjectsOutsideOrPartiallyOutside(bounds As Bounds) As IEnumerable(Of T)

        Function ObjectsInsideOrPartiallyInside(bounds As Bounds) As IEnumerable(Of T)

    End Interface

End NameSpace