Imports CollisionDetection.Model.BVH
Imports CollisionDetection.Model.Collision
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Rendering

    Public Interface IFrameOptions
    
        Function GetSplitMethod() As SplitMethod

        Function GetCollisionHandler() As CollisionHandler

        Function GetFrameDuration() As TimeSpan

        Function GetWorldBounds() As Bounds

        Function GetShowBoundingVolumes() As Boolean

        Function GetPaused() As Boolean

        Function GetDrawingFactories() As IEnumerable(Of IDrawableFactory)

        Function GetShowAverageRenderTime() As Boolean

    End Interface

End NameSpace