Imports CollisionDetection.Model.BVH
Imports CollisionDetection.Model.Collision
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Rendering
    Friend NotInheritable Class FrameOptions
        Implements IFrameOptions

        Private _splitMethod As SplitMethod
        Private _collisionHandler As CollisionHandler
        Private _frameDuration As TimeSpan
        Private _worldBounds As Bounds
        Private _showBoundingVolumes As Boolean
        Private _showAverageRenderTime As Boolean
        Private _paused As Boolean
        Private ReadOnly _drawingFactories As SpecializedSet(Of IDrawableFactory) = New SpecializedSet(Of IDrawableFactory)

        Public Function GetSplitMethod() As SplitMethod Implements IFrameOptions.GetSplitMethod
            Return _splitMethod
        End Function

        Public Sub SetSplitMethod(methodName As String)
            SetSplitMethod(SplitMethod.Parse(methodName))
        End Sub

        Public Sub SetSplitMethod(method As SplitMethod)
            _splitMethod = method
        End Sub

        Public Function GetCollisionHandler() As CollisionHandler Implements IFrameOptions.GetCollisionHandler
            Return _collisionHandler
        End Function

        Public Sub SetCollisionHandler(methodName As String)
            SetCollisionHandler(CollisionHandler.Parse(methodName))
        End Sub

        Public Sub SetCollisionHandler(collisionHandler As CollisionHandler)
            _collisionHandler = collisionHandler
        End Sub

        Public Function GetFrameDuration() As TimeSpan Implements IFrameOptions.GetFrameDuration
            return _frameDuration
        End Function

        Public Sub SetFrameDuration(framesPerSecond As Integer)
            SetFrameDuration(TimeSpan.FromSeconds(1.0 / framesPerSecond))
        End Sub

        Public Sub SetFrameDuration(duration As TimeSpan)
            _frameDuration = duration
        End Sub

        Public Function GetWorldBounds() As Bounds Implements IFrameOptions.GetWorldBounds
            Return _worldBounds
        End Function

        Public Sub SetWorldBounds(worldBounds As Bounds)
            _worldBounds = worldBounds
        End Sub

        Public Function GetShowBoundingVolumes() As Boolean Implements IFrameOptions.GetShowBoundingVolumes
            Return _showBoundingVolumes
        End Function

        Public Sub SetShowBoundingVolumes(showBoundingVolumes As Boolean)
            _showBoundingVolumes = showBoundingVolumes
        End Sub

        Public Function GetDrawingFactories() As IEnumerable(Of IDrawableFactory) Implements IFrameOptions.GetDrawingFactories
            Return _drawingFactories
        End Function

        Public Sub AddDrawingFactory(factory As IDrawableFactory)
            _drawingFactories.Add(factory)
        End Sub

        Public Sub AddDrawingFactories(ParamArray factories() As IDrawableFactory)
            _drawingFactories.Add(factories)
        End Sub

        Public Function GetPaused() As Boolean Implements IFrameOptions.GetPaused
            Return _paused
        End Function

        Public Sub SetPaused(paused As Boolean)
            _paused = paused
        End Sub

        Public Function GetShowAverageRenderTime() As Boolean Implements IFrameOptions.GetShowAverageRenderTime
            Return _showAverageRenderTime
        End Function

        Public Sub SetShowAverageRenderTime(showAverageRenderTime As Boolean)
            _showAverageRenderTime = showAverageRenderTime
        End Sub
    End Class
End Namespace
