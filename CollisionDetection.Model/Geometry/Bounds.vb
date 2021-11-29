Imports System.Diagnostics.Contracts
Imports System.Drawing

Public Structure Bounds
    Implements IEquatable(Of Bounds)
    Implements IFinite

    Sub New(bottomLeft As Point, topRight As Point)
        Me.BottomLeft = bottomLeft
        Me.TopRight = topRight
    End Sub

    Public ReadOnly Property BottomLeft As Point

    Public ReadOnly Property TopRight As Point

    <Pure>
    Public Function TopLeft() As Point
        Return New Point(BottomLeft.X, TopRight.Y)
    End Function

    <Pure>
    Public Function BottomRight() As Point
        Return New Point(TopRight.X, BottomLeft.Y)
    End Function

    <Pure>
    Public Function Width() As Single
        Return TopRight.X - BottomLeft.X
    End Function

    <Pure>
    Public Function Height() As Single
        Return TopRight.Y - BottomLeft.Y
    End Function

    <Pure>
    Public Function Area() As Double
        Return Width() * Height()
    End Function

    <Pure>
    Public Function Encompasses(point As Point) As Boolean
        Return BottomLeft.X <= point.X AndAlso TopRight.Y >= point.Y AndAlso TopRight.X >= point.X AndAlso BottomLeft.Y <= point.Y
    End Function

    <Pure>
    Public Function Encompasses(bounds As Bounds) As Boolean
        Return BottomLeft.X <= bounds.BottomLeft.X AndAlso BottomLeft.Y <= bounds.BottomLeft.Y AndAlso bounds.TopRight.X <= TopRight.X AndAlso bounds.TopRight.Y <= TopRight.Y
    End Function

    <Pure>
    Public Function Intersects(other As Bounds) As Boolean
        If TopRight.Y < other.BottomLeft.Y Then
            Return False
        ElseIf BottomLeft.X > other.TopRight.X Then
            Return False
        ElseIf other.BottomLeft.X > TopRight.X Then
            Return False
        ElseIf other.TopRight.Y < BottomLeft.Y Then
            Return False
        Else
            Return True
        End If
    End Function

    <Pure>
    Public Function Intersect(other As Bounds) As Bounds
        If Intersects(other) Then
            Return New Bounds(BottomLeft.Max(other.BottomLeft), TopRight.Min(other.TopRight))
        Else
            Return New Bounds()
        End If
    End Function

    <Pure>
    Public Function Union(other As Bounds) As Bounds
        Return New Bounds(BottomLeft.Min(other.BottomLeft), TopRight.Max(other.TopRight))
    End Function

    Public Shared Function Union(boxes As IEnumerable(Of Bounds)) As Bounds
        Dim maxX As Single = Single.NegativeInfinity
        Dim maxY As Single = Single.NegativeInfinity
        Dim minX As Single = Single.PositiveInfinity
        Dim minY As Single = Single.PositiveInfinity

        Dim boxEnumerator As IEnumerator(Of Bounds) = boxes.GetEnumerator()

        If Not boxEnumerator.MoveNext() Then
            Return New Bounds()
        Else
            Do
                With boxEnumerator.Current
                    If maxX < .TopRight.X Then maxX = .TopRight.X
                    If maxY < .TopRight.Y Then maxY = .TopRight.Y
                    If .BottomLeft.X < minX Then minX = .BottomLeft.X
                    If .BottomLeft.Y < minY Then minY = .BottomLeft.Y
                End With
            Loop While boxEnumerator.MoveNext()
        End If

        Return New Bounds((minX, minY), (maxX, maxY))
    End Function

    Public Shared Function Union(points As IEnumerable(Of Point)) As Bounds
        Dim maxX As Single = Single.NegativeInfinity
        Dim maxY As Single = Single.NegativeInfinity
        Dim minX As Single = Single.PositiveInfinity
        Dim minY As Single = Single.PositiveInfinity

        Dim pointEnumerator As IEnumerator(Of Point) = points.GetEnumerator()

        If Not pointEnumerator.MoveNext() Then
            Return New Bounds()
        Else
            Do
                With pointEnumerator.Current
                    If maxX < .X Then maxX = .X
                    If maxY < .Y Then maxY = .Y
                    If .X < minX Then minX = .X
                    If .Y < minY Then minY = .Y
                End With
            Loop While pointEnumerator.MoveNext()
        End If

        Return New Bounds((minX, minY), (maxX, maxY))
    End Function

    <Pure>
    Public Shared Function AreDisjoint(first As Bounds, second As Bounds) As Boolean
        Return Not first.Intersects(second)
    End Function

    Public Function LongestAxis() As Axis
        Return If(Width() > Height(), Axis.Horizontal, Axis.Vertical)
    End Function

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso TypeOf obj Is Bounds AndAlso Equals(DirectCast(obj, Bounds))
    End Function

    Public Overloads Function Equals(other As Bounds) As Boolean Implements IEquatable(Of Bounds).Equals
        Return BottomLeft = other.BottomLeft AndAlso TopRight = other.TopRight
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (BottomLeft, TopRight).GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return $"{BottomLeft} : {TopRight} "
    End Function

    Public Shared Narrowing Operator CType(bounds As Bounds) As Rectangle
        Return New Rectangle(CInt(bounds.BottomLeft.X), CInt(bounds.BottomLeft.Y), CInt(bounds.Width()), CInt(bounds.Height()))
    End Operator

    Public Shared Widening Operator CType(bounds As Bounds) As RectangleF
        Return New RectangleF(bounds.BottomLeft.X, bounds.BottomLeft.Y, bounds.Width(), bounds.Height())
    End Operator

    Public Shared Widening Operator CType(rect As Rectangle) As Bounds
        Return New Bounds(rect.Location, (rect.Width, rect.Height))
    End Operator

    Private Function Center() As Point Implements IFinite.Center
        Return BottomLeft.Lerp(TopRight, 1/2)
    End Function

    Private Function GetBounds() As Bounds Implements IFinite.Bounds
        Return Me
    End Function

End Structure