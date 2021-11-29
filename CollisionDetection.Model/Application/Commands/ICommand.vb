Namespace Application.Commands

    Public Interface ICommand

        Sub Execute()

    End Interface

    Public Interface ICommand(Of T)

        Sub Execute(item As T)

    End Interface

End NameSpace