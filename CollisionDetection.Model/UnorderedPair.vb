Imports System.Runtime.InteropServices

Public Class UnorderedPair(Of T)
    Implements IEquatable(Of UnorderedPair(Of T))

    Private Readonly _first As T
    Private Readonly _second As T

    Sub New(first As T, second As T)
        _first = first
        _second = second
    End Sub

    Public Function Neither(predicate As Predicate(Of T)) As Boolean
        Return Not predicate(_first) AndAlso Not predicate(_second)
    End Function

    Public Function Either(predicate As Predicate(Of T)) As Boolean
        Return predicate(_first) OrElse predicate(_second)
    End Function

    Public Function One(predicate As Predicate(Of T)) As Boolean
        Return (predicate(_first) AndAlso Not predicate(_second)) OrElse (Not predicate(_first) AndAlso predicate(_second))
    End Function

    Public Function Both(predicate As Predicate(Of T)) As Boolean
        Return predicate(_first) AndAlso predicate(_second)
    End Function

    Public Sub Deconstruct(<Out> ByRef first As T, <Out> ByRef second As T)
        first = _first
        second = _second
    End Sub

    Public Function Query(Of TResult)(q As Func(Of T, T, TResult)) As TResult
        Return q(_first, _second)
    End Function

    Public Function Cast(Of TResult)() As UnorderedPair(Of TResult)
        Return New UnorderedPair(Of TResult)(CType(CType(_first, Object), TResult), CType(CType(_second, Object), TResult))
    End Function

    Public Function Transform(Of TResult)(transformation As Func(Of T, TResult)) As UnorderedPair(Of TResult)
        Return New UnorderedPair(Of TResult)(transformation(_first), transformation(_second))
    End Function

#Region "Overrides and Equality Comparison"

    Public Overrides Function ToString() As String
        Return $"({_first}, {_second})"
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Equals(TryCast(obj, UnorderedPair(Of T)))
    End Function

    Public Overloads Function Equals(other As UnorderedPair(Of T)) As Boolean Implements IEquatable(Of UnorderedPair(Of T)).Equals
        If other Is Nothing Then Return False
        If other._first Is Nothing OrElse other._second Is Nothing Then Return False
        If other._first.Equals(_first) AndAlso other._second.Equals(_second) Then Return True
        If other._first.Equals(_second) AndAlso other._second.Equals(_first) Then Return True

        Return False
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return _first.GetHashCode() Xor _second.GetHashCode()
    End Function

#End Region

    Public Shared Widening Operator CType(tuple As ValueTuple(Of T, T)) As UnorderedPair(Of T)
        Return New UnorderedPair(Of T)(tuple.Item1, tuple.Item2)
    End Operator

    Public Shared Widening Operator CType(tuple As Tuple(Of T, T)) As UnorderedPair(Of T)
        Return New UnorderedPair(Of T)(tuple.Item1, tuple.Item2)
    End Operator

#Region "Equality Operators"

    Public Shared Operator =(left As UnorderedPair(Of T), right As UnorderedPair(Of T)) As Boolean
        Return left IsNot Nothing AndAlso left.Equals(right)
    End Operator

    Public Shared Operator <>(left As UnorderedPair(Of T), right As UnorderedPair(Of T)) As Boolean
        Return left Is Nothing OrElse Not left.Equals(right)
    End Operator

    Public Shared Operator =(left As UnorderedPair(Of T), right As Object) As Boolean
        Return left IsNot Nothing AndAlso left.Equals(right)
    End Operator

    Public Shared Operator <>(left As UnorderedPair(Of T), right As Object) As Boolean
        Return left Is Nothing OrElse Not left.Equals(right)
    End Operator

    Public Shared Operator =(left As Object, right As UnorderedPair(Of T)) As Boolean
        Return right IsNot Nothing AndAlso right.Equals(left)
    End Operator

    Public Shared Operator <>(left As Object, right As UnorderedPair(Of T)) As Boolean
        Return right Is Nothing OrElse Not right.Equals(left)
    End Operator

#End Region

End Class