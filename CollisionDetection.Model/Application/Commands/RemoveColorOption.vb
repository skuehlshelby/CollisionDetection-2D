Imports System.Drawing

Namespace Application.Commands

    Public Class RemoveColorOption
        Implements ICommand(Of IApplication)

        Private Readonly _color As Color

        Sub New(color As Color)
            _color = color
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.ShapeColors.Remove(_color)
        End Sub
    End Class

End NameSpace