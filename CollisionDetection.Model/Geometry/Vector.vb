Imports System.Diagnostics.Contracts

Public Structure Vector
    Implements IEquatable(Of Vector)

    Sub New(x As Single, y As Single)
        Me.X = x
        Me.Y = y
    End Sub

    ''' <summary>
    ''' The X component of this vector.
    ''' </summary>
    Public ReadOnly Property X As Single

    ''' <summary>
    ''' The Y component of this vector.
    ''' </summary>
    Public ReadOnly Property Y As Single

    ''' <summary>
    ''' The magnitude of this vector.
    ''' </summary>
    <Pure>
    Public Function Magnitude() As Double
        Return Math.Sqrt((X * X) + (Y * Y))
    End Function

    <Pure>
    Public Function Abs() As Vector
        Return New Vector(Math.Abs(X), Math.Abs(Y))
    End Function

    <Pure>
    Public Function Dot(other As Vector) As Single
        Return (X * other.X) + (Y * other.Y)
    End Function

    <Pure>
    Public Function AbsDot(other As Vector) As Single
        Return Math.Abs(Dot(other))
    End Function

    <Pure>
    Public Function ToUnitVec() As Vector
        Return Me / Magnitude()
    End Function

    <Pure>
    Public Function Tangent() As Vector
        Return (-Y, X)
    End Function

    Public Shared Operator +(left As Vector, right As Vector) As Vector
        Return New Vector(left.X + right.X, left.Y + right.Y)
    End Operator

    Public Shared Operator -(left As Vector, right As Vector) As Vector
        Return New Vector(left.X - right.X, left.Y - right.Y)
    End Operator

    Public Shared Operator *(left As Vector, right As Integer) As Vector
        Return New Vector(left.X * right, left.Y * right)
    End Operator

    Public Shared Operator *(left As Vector, right As Single) As Vector
        Return New Vector(left.X * right, left.Y * right)
    End Operator

    Public Shared Operator *(left As Vector, right As Double) As Vector
        Return New Vector(CSng(left.X * right), CSng(left.Y * right))
    End Operator

    Public Shared Operator /(left As Vector, right As Single) As Vector
        Dim inverse As Single = 1.0F / right
        Return New Vector(left.X * inverse, left.Y * inverse)
    End Operator

    Public Shared Operator /(left As Vector, right As Double) As Vector
        Dim inverse As Double = 1.0 / right
        Return New Vector(CSng(left.X * inverse), CSng(left.Y * inverse))
    End Operator

    Public Shared Operator -(vec As Vector) As Vector
        Return (-vec.X, -vec.Y)
    End Operator

    Public Shared Property Zero As Vector = New Vector(0.0F, 0.0F)

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso TypeOf obj Is Vector AndAlso Equals(DirectCast(obj, Vector))
    End Function

    Public Overloads Function Equals(other As Vector) As Boolean Implements IEquatable(Of Vector).Equals
        Return Math.Abs(X - other.X) < 0.01F AndAlso Math.Abs(Y - other.Y) < 0.01F
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (X, Y).GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return $"<{Math.Round(X, 2)}, {Math.Round(Y, 2)}>"
    End Function

    Public Shared Operator =(left As Vector, right As Vector) As Boolean
        Return left.Equals(right)
    End Operator

    Public Shared Operator <>(left As Vector, right As Vector) As Boolean
        Return Not left.Equals(right)
    End Operator

    Public Shared Widening Operator CType(valueTuple As ValueTuple(Of Single, Single)) As Vector
        Return New Vector(x:=valueTuple.Item1, y:=valueTuple.Item2)
    End Operator

    Public Shared Widening Operator CType(valueTuple As ValueTuple(Of Integer, Integer)) As Vector
        Return New Vector(x:=valueTuple.Item1, y:=valueTuple.Item2)
    End Operator
    
End Structure