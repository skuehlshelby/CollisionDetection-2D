Imports System.Reflection

Namespace Geometry.ShapeWrappers

    Public Class SelectedVolume(Of T)
        Implements IUnwrappable(Of T)

        Private ReadOnly _original As T

        Public Sub New(original As T)
            _original = original
        End Sub

        Public Function Unwrap() As T Implements IUnwrappable(Of T).Unwrap
            Return CType(RecursiveUnwrap(_original), T)
        End Function

        Private Function RecursiveUnwrap(obj As Object) As Object
            If IsUnwrappable(obj) Then
                Return RecursiveUnwrap(GetType(IUnwrappable(Of )).InvokeMember(NameOf(Unwrap),
                                                                               BindingFlags.Public Or BindingFlags.NonPublic Or
                                                                               BindingFlags.Instance, Nothing, obj, Nothing))
            Else
                Return obj
            End If
        End Function

        Private Function IsUnwrappable(obj As Object) As Boolean
            If obj Is Nothing Then Return False
            If Not obj.GetType().IsGenericType Then Return False

            Return obj.GetType().GetGenericTypeDefinition() Is GetType(IUnwrappable(Of ))
        End Function


    End Class
End NameSpace