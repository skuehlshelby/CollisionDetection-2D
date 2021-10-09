Namespace BVH
    Public NotInheritable Class SplitResult(Of T As IFinite)

        Private Sub New()
            FirstHalf = Nothing
            SecondHalf = Nothing
            SplitAxis = Nothing
            Failed = True
        End Sub

        Private Sub New(firstHalf As T(), secondHalf As T(), splitAxis As Axis)
            Me.FirstHalf = firstHalf
            Me.SecondHalf = secondHalf
            Me.SplitAxis = splitAxis
            Failed = False
        End Sub

        Public ReadOnly Property FirstHalf As T()

        Public ReadOnly Property SecondHalf As T()

        Public ReadOnly Property SplitAxis As Axis

        Public ReadOnly Property Failed As Boolean

        Public Shared Function Failure() As SplitResult(Of T)
            Return New SplitResult(Of T)()
        End Function

        Public Shared Function Success(firstHalf As T(), secondHalf As T(), splitAxis As Axis) As SplitResult(Of T)
            Return New SplitResult(Of T)(firstHalf, secondHalf, splitAxis)
        End Function

    End Class
End NameSpace