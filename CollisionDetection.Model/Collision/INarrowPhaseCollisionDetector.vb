Namespace Collision
    Public Interface INarrowPhaseCollisionDetector
        Inherits ISpecialized

        Function Test(ParamArray args() As Object) As CollisionTestResult

    End Interface
End NameSpace