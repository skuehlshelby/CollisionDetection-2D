Namespace Application.Commands

    Public Class SetShapeCount
        Implements ICommand(Of IApplication)

        Private ReadOnly _count As Integer

        Sub New(count As Integer)
            _count = count
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.NumberOfShapes = _count
        End Sub
    End Class

End NameSpace