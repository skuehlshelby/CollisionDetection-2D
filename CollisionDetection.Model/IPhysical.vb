Public Interface IPhysical
    
    Function Center() As Point

    Property Velocity As Vector

    Property Acceleration As Vector

    ReadOnly Property Mass As Single

End Interface