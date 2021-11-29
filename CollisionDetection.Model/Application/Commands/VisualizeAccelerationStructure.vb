Namespace Application.Commands
    Public Class VisualizeAccelerationStructure
        Implements ICommand(Of IApplication)

        Private ReadOnly _visualize As Boolean

        Sub New(visualize As Boolean)
            _visualize = visualize
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.VisualizeAccelerationStructure = _visualize
        End Sub

    End Class
End NameSpace