Public Interface IMoveable

    Property Velocity As Vector

    Property Acceleration As Vector

    Property Mass As Single

    Function CanMove() As Boolean

    Function CanMove(duration As TimeSpan) As Boolean

    Sub Move()

    Sub Move(duration As TimeSpan)

    Sub Move(direction As Vector, duration As TimeSpan)

    Sub AddTime(duration As TimeSpan)

End Interface
