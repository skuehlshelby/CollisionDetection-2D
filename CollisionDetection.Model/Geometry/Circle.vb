Imports System.Drawing
Imports CollisionDetection.Model.Rendering

Public NotInheritable Class Circle
    Implements IFinite
    Implements IMoveable
    Implements IPointQueryable
    Implements ICloneable
    Implements IShape
    Implements IEquatable(Of Circle)

    Private Shared lastId As Integer = 0

    Private ReadOnly id As Integer
    Private _center As Point
    Private _availableMovementTime As TimeSpan

    Sub New(radius As Single, color As Color, center As Point, velocity As Vector)
        Me.New(radius, color, center, velocity, Vector.Zero)
    End Sub

    Sub New(radius As Single, color As Color, center As Point, velocity As Vector, acceleration As Vector)
        lastId += 1
        id = lastId
        _center = center
        Me.Color = color
        Me.Radius = radius
        Me.Velocity = velocity
        Me.Acceleration = acceleration
        Diameter = radius * 2
        Mass = radius * radius
    End Sub

    Public ReadOnly Property Radius As Single

    Public ReadOnly Property Diameter As Single

    Public ReadOnly Property Color As Color

    Public Function Bounds() As Bounds Implements IFinite.Bounds
        Return New Bounds((_center.X - Radius, _center.Y - Radius), (_center.X + Radius, _center.Y + Radius))
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function

#Region "IPointQueryable Members"

    Public Function Center() As Point Implements IFinite.Center, IPointQueryable.Center
        Return _center
    End Function

    Public Function Contains(point As Point) As Boolean Implements IPointQueryable.Contains
        Return _center.Distance(point) <= Radius
    End Function

    Public Function PointClosestTo(point As Point) As Point Implements IPointQueryable.PointClosestTo
        Return _center + (point - _center).ToUnitVec() * Radius
    End Function

#End Region

#Region "IMoveable Members"

    Public Property Velocity As Vector Implements IMoveable.Velocity

    Public Property Acceleration As Vector Implements IMoveable.Acceleration

    Public Property Mass As Single Implements IMoveable.Mass

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
        Return other IsNot Nothing AndAlso other.id = id
    End Function

#End Region

End Class