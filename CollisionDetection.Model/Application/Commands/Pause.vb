Namespace Application.Commands

    Public Class Pause
        Implements ICommand(Of IApplication)

        Private ReadOnly _paused As Boolean

        Sub New(paused As Boolean)
            _paused = paused
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.IsPaused = _paused
        End Sub

    End Class

End NameSpace