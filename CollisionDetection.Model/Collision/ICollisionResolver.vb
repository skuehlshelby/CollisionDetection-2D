Namespace Collision
    Public Interface ICollisionResolver
        Inherits ISpecialized

        Sub Resolve(timeUntilCollision As TimeSpan, ParamArray args() As Object)

    End Interface
End NameSpace