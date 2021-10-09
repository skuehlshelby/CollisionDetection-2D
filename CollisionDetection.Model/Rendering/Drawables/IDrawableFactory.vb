Namespace Rendering.Drawables

    Public Interface IDrawableFactory
        Inherits ISpecialized

        Function GetDrawable(Paramarray args() As Object) As IDrawable

    End Interface

End NameSpace