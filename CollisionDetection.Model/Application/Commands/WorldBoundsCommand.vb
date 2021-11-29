Namespace Application.Commands

    Public Class WorldBoundsCommand
        Implements ICommand(Of IApplication)

        Private ReadOnly _bounds As Bounds

        Public Sub New(bounds As Bounds)
            _bounds = bounds
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.WorldBounds = _bounds
        End Sub

    End Class

End NameSpace