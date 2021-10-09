Public Structure Maybe(Of T)
    Private Readonly _value As T

    Private Readonly _hasValue As Boolean

    Private Sub New(value As T)
        _value = value
        _hasValue = value IsNot Nothing
    End Sub

    Public Function Match(Of TResult)(some As Func(Of T, TResult), none As Func(Of TResult)) As  TResult
        If _hasValue Then return some(_value)

        return none()
    End Function

    Public Sub Match(some As Action(Of T))
        If _hasValue Then some(_value)
    End Sub

    Public Sub Match(some As Action(Of T), none As Action)
        If _hasValue Then
            some(_value)
        Else
            none()
        End If
    End Sub

    Public Shared Function Some(value As T) As Maybe(Of T)
        Return New Maybe(Of T)(value)
    End Function

    Public Shared Widening Operator CType(value As T) As Maybe(Of T)
        Return New Maybe(Of T)(value)
    End Operator
End Structure