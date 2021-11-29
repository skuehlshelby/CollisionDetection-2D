Imports System.Drawing

Public NotInheritable Class Circle
    Implements IFinite
    Implements IMoveable
    Implements IPhysical
    Implements IPointQueryable
    Implements ICloneable
    Implements IShape
    Implements IEquatable(Of Circle)

    Private Shared _lastId As Integer = 0

    Private ReadOnly _id As Integer
    Private _center As Point
    Private _availableMovementTime As TimeSpan

    Sub New(radius As Single, color As Color, center As Point, velocity As Vector)
        Me.New(radius, color, center, velocity, Vector.Zero)
    End Sub

    Sub New(radius As Single, color As Color, center As Point, velocity As Vector, acceleration As Vector)
        _lastId += 1
        _id = _lastId
        _center = center
        Me.Color = color
        Me.Radius = radius
        Me.Velocity = velocity
        Me.Acceleration = acceleration
    End Sub

    Public Property Radius As Single

    Public Readonly Property Diameter As Single
        Get
            Return Radius * 2
        End Get
    End Property

    Public Property Color As Color Implements IShape.Color

    Public Function Bounds() As Bounds Implements IFinite.Bounds
        Return New Bounds((_center.X - Radius, _center.Y - Radius), (_center.X + Radius, _center.Y + Radius))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function

#Region "IPointQueryable Members"

    Public Function Center() As Point Implements IFinite.Center, IPointQueryable.Center, IPhysical.Center
        Return _center
    End Function

    Public Function Contains(point As Point) As Boolean Implements IPointQueryable.Contains
        Return _center.Distance(point) <= Radius
    End Function

    Public Function PointClosestTo(point As Point) As Point Implements IPointQueryable.PointClosestTo
        Return _center + (point - _center).ToUnitVec() * Radius
    End Function

#End Region

#Region "IPhysical Members"

    Public Property Velocity As Vector Implements IPhysical.Velocity

    Public Property Acceleration As Vector Implements IPhysical.Acceleration

    Public Readonly Property Mass As Single Implements IPhysical.Mass
        Get
            Return radius * radius
        End Get
    End Property

#End Region

#Region "IMoveable Members"

    Public Function CanMove() As Boolean Implements IMoveable.CanMove
        Return TimeSpan.Zero < _availableMovementTime
    End Function

    Public Function CanMove(duration As TimeSpan) As Boolean Implements IMoveable.CanMove
        Return duration <= _availableMovementTime
    End Function

    Public Sub Move() Implements IMoveable.Move
        _center += (Velocity * _availableMovementTime.TotalSeconds)
        _availableMovementTime = TimeSpan.Zero
    End Sub

    Public Sub Move(duration As TimeSpan) Implements IMoveable.Move
        If CanMove(duration) Then
            _center += (Velocity * duration.TotalSeconds)
            _availableMovementTime -= duration
        Else
            Throw New InvalidOperationException($"Attempted to use {duration} when only {_availableMovementTime} were available.")
        End If
    End Sub

    Public Sub Move(direction As Vector, duration As TimeSpan) Implements IMoveable.Move
        If CanMove(duration) Then
            _center += (direction * duration.TotalSeconds)
            _availableMovementTime -= duration
        Else
            Throw New InvalidOperationException($"Attempted to use {duration} when only {_availableMovementTime} were available.")
        End If
    End Sub

    Public Sub AddTime(duration As TimeSpan) Implements IMoveable.AddTime
        If TimeSpan.Zero < duration 
            _availableMovementTime += duration
        Else
            Throw New ArgumentException($"The duration of time added must be positive.", NameOf(duration))
        End If
    End Sub

#End Region

#Region "Overrides and Equality Comparison"

    Public Overrides Function ToString() As String
        Return $"{Color.Name} circle at {Center}"
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (Radius, Diameter).GetHashCode()
    End Function



    Public Overrides Function Equals(other As Object) As Boolean
        Return Equals(TryCast(other, Circle))
    End Function

    Public Overloads Function Equals(other As Circle) As Boolean Implements IEquatable(Of Circle).Equals
        Return other IsNot Nothing AndAlso other._id = _id
    End Function

#End Region

End Class