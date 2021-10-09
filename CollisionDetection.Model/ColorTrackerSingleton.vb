Imports System.Drawing


Public Class ColorTrackerSingleton
    Implements IColorTracker

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance As IColorTracker = New ColorTrackerSingleton()

    Private ReadOnly _colors As IDictionary(Of Color, Integer) = New Dictionary(Of Color, Integer)
    Private ReadOnly _defunct As IDictionary(Of Color, Integer) = New Dictionary(Of Color, Integer)

    Private Sub Increment(color As Color) Implements IColorTracker.Increment
        If Not _colors.ContainsKey(color) Then
            Add(color)
        End If

        _colors(color) += 1
    End Sub

    Private Sub Decrement(color As Color) Implements IColorTracker.Decrement
        If _colors.ContainsKey(color) Then
            _colors(color) -= 1
        End If

        If _defunct.ContainsKey(color) Then
            _defunct(color) -= 1
        End If
    End Sub

    Private Sub Add(color As Color) Implements IColorTracker.Add
        If Not _colors.ContainsKey(color) Then
            _colors.Add(color, 0)
        Else
            Increment(color)
        End If
    End Sub

    Private Sub Remove(color As Color) Implements IColorTracker.Remove
        If _colors.Count = 1 Then Return
        If Not _colors.ContainsKey(color) Then Return

        _defunct.Add(color, _colors(color))
        _colors.Remove(color)
    End Sub

    Private Function LeastCommon() As Color Implements IColorTracker.LeastCommon
        LeastCommon = _colors.First().Key

        For Each color As Color In _colors.Keys
            If _colors(color) = 0 Then
                Return color
            ElseIf _colors(color) < _colors(LeastCommon)
                LeastCommon = color
            End If
        Next
    End Function

    Private Function MostCommon() As Color Implements IColorTracker.MostCommon
        CleanUpDefunctColors()

        If _defunct.Any() Then
            Return _defunct.Keys(0)
        Else
            MostCommon = _colors.Keys(0)

            For Each color As Color In _colors.Keys
                If _colors(MostCommon) < _colors(color) Then
                    MostCommon = color
                End If
            Next
        End If
    End Function

    Private Function All() As ISet(Of Color) Implements IColorTracker.All
        Return New HashSet(Of Color)(_colors.Keys)
    End Function

    Private Sub CleanUpDefunctColors()
        For index As Integer = _defunct.Count - 1 To 0 Step -1
            If _defunct.ElementAt(index).Value = 0 Then
                _defunct.Remove(_defunct.ElementAt(index))
            End If
        Next
    End Sub

End Class