Public Interface IShapeFactory
    
    Function SpawnAt(point As Point) As IShape

    Function SpawnRandomly(worldBounds As Bounds) As IShape

End Interface