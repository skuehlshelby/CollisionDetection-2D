Imports System.Drawing
Imports System.Runtime.CompilerServices

Public Module Extensions

    <Extension()>
    Public Function GetLeastCommonColorAndIncrementCount(tracker As IColorTracker) As Color
        Dim lc As Color = tracker.LeastCommon()
        tracker.Increment(lc)
        Return lc
    End Function

    <Extension()>
    Public Function GetMostCommonColorAndDecrementCount(tracker As IColorTracker) As Color
        Dim mc As Color = tracker.MostCommon()
        tracker.Decrement(mc)
        Return mc
    End Function

    <Extension()>
    Public Sub AddRange(tracker As IColorTracker, colors As IEnumerable(Of Color))
        For Each color As Color In colors
            tracker.Add(color)
        Next
    End Sub
End Module