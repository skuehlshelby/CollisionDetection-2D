Imports System.Drawing

Namespace Application.Commands
    Public Class AddColorOption
        Implements ICommand(Of IApplication)

        Private Readonly _color As Color

        Sub New(color As Color)
            _color = color
        End Sub

        Public Sub Execute(item As IApplication) Implements ICommand(Of IApplication).Execute
            item.ShapeColors.Add(_color)
        End Sub

    End Class

End NameSpace