Imports CollisionDetection.Model.BVH
Imports CollisionDetection.Model.Collision
Imports CollisionDetection.Model.Rendering.Drawables

Namespace Rendering
    Public NotInheritable Class FrameOptionsBuilder
        
        Private ReadOnly _options As FrameOptions
        
        Public Sub New(defaults As IDefaultProvider)
            _options = New FrameOptions()

            With _options
                .SetCollisionHandler(defaults.GetDefault(Of CollisionHandler)(MonitoredProperty.CollisionHandler))
                .SetFrameDuration(defaults.GetDefault(Of TimeSpan)(MonitoredProperty.FrameRate))
                .SetShowBoundingVolumes(defaults.GetDefault(Of Boolean)(MonitoredProperty.BoundingVolumeVisibility))
                .SetSplitMethod(defaults.GetDefault(Of SplitMethod)(MonitoredProperty.SplitMethod))
                .SetPaused(defaults.GetDefault(Of Boolean)(MonitoredProperty.Paused))
                .AddDrawingFactories(defaults.GetDefault(Of IDrawableFactory())(MonitoredProperty.AvailableRenderers))
            End With
        End Sub

        Public Sub New(oldOptions As IFrameOptions)
            _options = New FrameOptions()

            With _options
                .SetCollisionHandler(oldOptions.GetCollisionHandler())
                .SetFrameDuration(oldOptions.GetFrameDuration())
                .SetPaused(oldOptions.GetPaused())
                .SetShowAverageRenderTime(oldOptions.GetShowAverageRenderTime())
                .SetShowBoundingVolumes(oldOptions.GetShowBoundingVolumes())
                .SetSplitMethod(oldOptions.GetSplitMethod())
                .SetWorldBounds(oldOptions.GetWorldBounds())
            End With

            For Each factory As IDrawableFactory In oldOptions.GetDrawingFactories()
                _options.AddDrawingFactory(factory)
            Next
        End Sub

        Public Function WithAverageRenderTimeVisibility(visible As Boolean) As FrameOptionsBuilder
            _options.SetShowAverageRenderTime(visible)
            Return Me
        End Function

        Public Function WithBoundingVolumeVisibility(visible As Boolean) As FrameOptionsBuilder
            _options.SetShowBoundingVolumes(visible)
            Return Me
        End Function

        Public Function WithCollisionHandler(methodName As String) As FrameOptionsBuilder
            _options.SetCollisionHandler(methodName)
            Return Me
        End Function

        Public Function WithCollisionHandler(method As CollisionHandler) As FrameOptionsBuilder
            _options.SetCollisionHandler(method)
            Return Me
        End Function

        Public Function WithFrameDuration(duration As TimeSpan) As FrameOptionsBuilder
            _options.SetFrameDuration(duration)
            Return Me
        End Function

        Public Function WithPaused(paused As Boolean) As FrameOptionsBuilder
            _options.SetPaused(paused)
            Return Me
        End Function

        Public Function WithRenderer(renderer As IDrawableFactory) As FrameOptionsBuilder
            _options.AddDrawingFactory(renderer)
            Return Me
        End Function

        Public Function WithRenderers(ParamArray factories() As IDrawableFactory) As FrameOptionsBuilder
            For Each factory As IDrawableFactory In factories
                _options.AddDrawingFactory(factory)
            Next
            
            Return Me
        End Function

        Public Function WithSplitMethod(methodName As String) As FrameOptionsBuilder
            _options.SetSplitMethod(methodName)
            Return Me
        End Function

        Public Function WithSplitMethod(method As SplitMethod) As FrameOptionsBuilder
            _options.SetSplitMethod(method)
            Return Me
        End Function

        Public Function WithWorldBounds(worldBounds As Bounds) As FrameOptionsBuilder
            _options.SetWorldBounds(worldBounds)
            Return Me
        End Function

        Public Function Build() As IFrameOptions
            Return _options
        End Function

        Public Function Build(shapes As IEnumerable(Of IShape)) As IFrame
            Return New Frame(shapes, _options)
        End Function

    End Class
End NameSpace