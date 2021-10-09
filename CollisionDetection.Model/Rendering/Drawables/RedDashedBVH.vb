Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CollisionDetection.Model.BVH

Namespace Rendering.Drawables

    Public NotInheritable Class RedDashedBVH
        Inherits Specialized
        Implements IDrawable
        Implements IDrawableFactory

        Private ReadOnly _bvh As IBoundingVolumeHierarchy

        Private Sub New(bvh As IBoundingVolumeHierarchy)
            _bvh = bvh
        End Sub

        Private Sub New()
        End Sub

        Protected Overrides Sub SetRequirements()
            MyBase.SetRequirements()
            Require(0, Function(arg) TypeOf arg Is IBoundingVolumeHierarchy)
        End Sub

        Private Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            For Each b As Bounds In _bvh.BoundingVolumes()
                graphics.DrawDashedRectangle(b, Color.FromArgb(218, 0, 55))
            Next
        End Sub

        Private Function GetDrawable(Paramarray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New RedDashedBVH(DirectCast(args(0), IBoundingVolumeHierarchy))
        End Function

        Public Shared Function AsFactory() As IDrawableFactory
            Return New RedDashedBVH()
        End Function
    End Class

End NameSpace