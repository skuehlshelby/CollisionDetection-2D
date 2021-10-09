Imports System.Drawing


Public Interface IColorTracker
    
    Sub Add(color As Color)

    Sub Remove(color As Color)

    Sub Increment(color As Color)

    Sub Decrement(color As Color)

    Function LeastCommon() As Color

    Function MostCommon() As Color

    Function All() As ISet(Of Color)

End Interface