Imports System.Drawing
Imports CollisionDetection.Model.BVH

Namespace Rendering.Drawables

    Public NotInheritable Class GreenAndBlueBVH
        Implements IDrawable
        Implements IDrawableFactory

        Private ReadOnly _bvh As IBoundingVolumeHierarchy

        Private Sub New(bvh As IBoundingVolumeHierarchy)
            _bvh = bvh
        End Sub

        Private Sub New()
        End Sub

        Private Sub Draw(graphics As IGraphics) Implements IDrawable.Draw
            Dim boundsArray As Bounds() = _bvh.BoundingVolumes().ToArray()

            For index As Integer = LBound(boundsArray) To UBound(boundsArray)
                If index Mod 2 = 0 Then
                    graphics.DrawDashedRectangle(boundsArray(index), Color.FromArgb(95, 185, 176))
                Else
                    graphics.DrawDashedRectangle(boundsArray(index), Color.FromArgb(65, 61, 101))
                End If
            Next
        End Sub

        Private Function IsApplicable(Paramarray args() As Object) As Boolean Implements IDrawableFactory.IsApplicable
            Return TypeOf args(0) Is IBoundingVolumeHierarchy
        End Function

        Private Function GetDrawable(Paramarray args() As Object) As IDrawable Implements IDrawableFactory.GetDrawable
            Return New GreenAndBlueBVH(DirectCast(args(0), IBoundingVolumeHierarchy))
        End Function

        Public Shared Function AsFactory() As IDrawableFactory
            Return New GreenAndBlueBVH()
        End Function

    End Class

End NameSpace