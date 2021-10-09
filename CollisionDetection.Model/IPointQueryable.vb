Public Interface IPointQueryable

    Function Center() As Point

    Function Contains(point As Point) As Boolean

    Function PointClosestTo(point As Point) As Point

End Interface
