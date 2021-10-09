Imports System.Diagnostics.Contracts

Public Structure Point
    Implements IEquatable(Of Point)

    Sub New(x As Single, y As Single)
        Me.X = x
        Me.Y = y
    End Sub

    Public ReadOnly Property X As Single

    Public ReadOnly Property Y As Single

    Public Shared ReadOnly Property Origin As Point = (0.0F, 0.0F)

    <Pure>
    Public Function Max() As Single
        Return Math.Max(X, Y)
    End Function

    <Pure>
    Public Function Max(other As Point) As Point
        Return New Point(Math.Max(X, other.X), Math.Max(Y, other.Y))
    End Function

    <Pure>
    Public Function Min() As Single
        Return Math.Min(X, Y)
    End Function

    <Pure>
    Public Function Min(other As Point) As Point
        Return New Point(Math.Min(X, other.X), Math.Min(Y, other.Y))
    End Function

    <Pure>
    Public Function Abs() As Point
        Return New Point(Math.Abs(X), Math.Abs(Y))
    End Function

    <Pure>
    Public Function Lerp(other As Point, weight As Single) As Point
        Return ((1.0F - weight) * Me) + (weight * other)
    End Function

    <Pure>
    Public Function Distance(other As Point) As Double
        Return Math.Sqrt(DistanceSquared(other))
    End Function

    <Pure>
    Public Function DistanceSquared(other As Point) As Double
        Dim xDistance As Single = Math.Abs(X - other.X)
        Dim yDistance As Single = Math.Abs(Y - other.Y)
        Return (xDistance * xDistance) + (yDistance * yDistance)
    End Function

#Region "Operators"

    Public Shared Operator +(left As Point, right As Vector) As Point
        Return New Point(left.X + right.X, left.Y + right.Y)
    End Operator

    Public Shared Operator +(left As Point, right As Point) As Point
        Return New Point(left.X + right.X, left.Y + right.Y)
    End Operator

    Public Shared Operator -(left As Point, right As Vector) As Point
        Return New Point(left.X - right.X, left.Y - right.Y)
    End Operator

    Public Shared Operator -(left As Point, right As Point) As Vector
        Return New Vector(left.X - right.X, left.Y - right.Y)
    End Operator

    Public Shared Operator *(left As Point, right As Single) As Point
        Return New Point(left.X * right, left.Y * right)
    End Operator

    Public Shared Operator *(left As Single, right As Point) As Point
        Return New Point(left * right.X, left * right.Y)
    End Operator

    Public Shared Operator =(left As Point, right As Point) As Boolean
        Return left.Equals(right)
    End Operator

    Public Shared Operator <>(left As Point, right As Point) As Boolean
        Return Not left.Equals(right)
    End Operator

    Public Shared Widening Operator CType(tuple As ValueTuple(Of Single, Single)) As Point
        Return New Point(tuple.Item1, tuple.Item2)
    End Operator

    Public Shared Narrowing Operator CType(point As Point) As Drawing.Point
        Return New Drawing.Point(CInt(point.X), CInt(point.Y))
    End Operator

    Public Shared Widening Operator CType(point As Point) As Drawing.PointF
        Return New Drawing.PointF(point.X, point.Y)
    End Operator

#End Region

#Region "Overrides"

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso TypeOf obj Is Point AndAlso Equals(DirectCast(obj, Point))
    End Function

    Public Overloads Function Equals(other As Point) As Boolean Implements IEquatable(Of Point).Equals
        Return Math.Abs(other.X - X) < 0.01 AndAlso Math.Abs(other.Y - Y) < 0.01
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (X, Y).GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return $"({Math.Round(X, 2)}, {Math.Round(Y, 2)})"
    End Function

#End Region

End Structure