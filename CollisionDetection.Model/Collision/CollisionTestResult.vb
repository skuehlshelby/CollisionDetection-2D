Namespace Collision
    Public NotInheritable Class CollisionTestResult
        Private Sub New()
            AreIntersecting = False
            WillIntersect = False
            AreIntersectingOrWillIntersect = False
            TimeUntilIntersection = TimeSpan.Zero
        End Sub

        Private Sub New(timeUntilIntersection As TimeSpan)
            Me.TimeUntilIntersection = timeUntilIntersection
            AreIntersecting = timeUntilIntersection = TimeSpan.Zero
            WillIntersect = TimeSpan.Zero < timeUntilIntersection
            AreIntersectingOrWillIntersect = AreIntersecting OrElse WillIntersect
        End Sub

        Public Readonly Property AreIntersecting As Boolean

        Public Readonly Property WillIntersect As Boolean

        Public Readonly Property AreIntersectingOrWillIntersect As Boolean

        Public ReadOnly Property TimeUntilIntersection As TimeSpan

        Public Shared Function NoIntersection() As CollisionTestResult
            Return New CollisionTestResult()
        End Function

        Public Shared Function CurrentlyIntersecting() As CollisionTestResult
            Return New CollisionTestResult(TimeSpan.Zero)
        End Function

        Public Shared Function WillIntersectAfter(timeUntilIntersection As TimeSpan) As CollisionTestResult
            Return New CollisionTestResult(timeUntilIntersection)
        End Function
    End Class
End NameSpace