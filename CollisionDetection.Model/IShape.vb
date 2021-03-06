Imports System.Drawing

Public Interface IShape
    Inherits IFinite
    Inherits IMoveable
    Inherits IPhysical
    Inherits IPointQueryable
    Inherits ICloneable

    ReadOnly Property Color As Color
End Interface