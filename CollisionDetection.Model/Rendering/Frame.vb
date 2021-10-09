Imports System.Drawing
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Rendering

    Public Class Frame
        Implements IFrame

        Private Shared ReadOnly Stopwatch As Stopwatch = New Stopwatch()
        Private Shared ReadOnly StopwatchHistory As ICollection(Of TimeSpan) = New List(Of TimeSpan)()
        Private Shared _averageTime As TimeSpan
        Private ReadOnly Scene As ISet(Of IDrawable) = New HashSet(Of IDrawable)

        Public Sub New(shapes As IEnumerable(Of IShape), options As IFrameOptions)
            Stopwatch.Restart()

            If Not options.GetPaused() Then AllotFrameTime(shapes, options.GetFrameDuration())

            HandleCollisions(shapes, options)

            If Not options.GetPaused() Then MoveShapes(shapes)

            AddDrawablesToScene(shapes, options.GetDrawingFactories())

            If options.GetShowAverageRenderTime() Then AddAverageRenderTimeToScene(options)

            UpdateAverageTime(Stopwatch.Elapsed)

            Stopwatch.Stop()
        End Sub

        Private Shared Sub AllotFrameTime(shapes As IEnumerable(Of IShape), frameDuration As TimeSpan)
            For Each shape As IShape In shapes
                shape.AddTime(frameDuration)
            Next
        End Sub

        Private Sub HandleCollisions(shapes As IEnumerable(Of IShape), options As IFrameOptions)
            With options.GetCollisionHandler().DetectAndResolveCollisions(options.GetSplitMethod(), shapes, options.GetWorldBounds())
                If options.GetShowBoundingVolumes() Then
                    .Match(Sub(bvh) Scene.Add(options.GetDrawingFactories().First(Function(f) f.IsApplicable(bvh)).GetDrawable(bvh)))
                End If
            End With
        End Sub

        Private Shared Sub MoveShapes(shapes As IEnumerable(Of IShape))
            For Each shape As IShape In shapes
                shape.Move()
            Next
        End Sub

        Private Sub AddDrawablesToScene(shapes As IEnumerable(Of IShape), renderers As IEnumerable(Of IDrawableFactory))
            For Each shape As IShape In shapes
                Scene.Add(renderers.First(Function(f) f.IsApplicable(shape)).GetDrawable(shape))
            Next
        End Sub

        Private Sub AddAverageRenderTimeToScene(options As IFrameOptions)
            Dim averageTime As String = $"Average Render Time: {_averageTime:ffff}µs"
            Dim fontSize As Single = 16.0
            Dim color As Color = Color.CornflowerBlue
            Dim location As Point = (5, 20)

            Scene.Add(options.GetDrawingFactories().First(Function(f) f.IsApplicable(averageTime, fontSize, color, location)).GetDrawable(averageTime, fontSize, color, location))
        End Sub

        Private Shared Sub UpdateAverageTime(time As TimeSpan)
            If StopwatchHistory.Count = 30 Then
                _averageTime = TimeSpan.FromMilliseconds(StopwatchHistory.Average(Function(t) t.TotalMilliseconds))
                StopwatchHistory.Clear()
            End If

            StopwatchHistory.Add(time)
        End Sub

        Private Function GetScene() As ISet(Of IDrawable) Implements IFrame.GetScene
            Return Scene
        End Function

    End Class
End NameSpace