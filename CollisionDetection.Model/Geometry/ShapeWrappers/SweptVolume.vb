Namespace Geometry.ShapeWrappers
    Public NotInheritable Class SweptVolume(Of T As IFinite)
        Implements IFinite
        Implements IUnwrappable(Of T)
        Implements IEquatable(Of SweptVolume(Of T))

        Private Readonly _bounds As Bounds
        Private ReadOnly _center As Point

        Private Sub New(original As T, sweptBounds As Bounds)
            Me.Original = original
            _bounds = sweptBounds
            _center = _bounds.BottomLeft.Lerp(_bounds.TopRight, 1/2)
        End Sub

        Public ReadOnly Property Original As T

        Private Function Unwrap() As T Implements IUnwrappable(Of T).Unwrap
            Return Original
        End Function

        Public Function Center() As Point Implements IFinite.Center
            Return _center
        End Function

        Public Function Bounds() As Bounds Implements IFinite.Bounds
            Return _bounds
        End Function

#Region "Overrides and Equality Comparison"
        Public Overrides Function Equals(obj As Object) As Boolean
            Return Equals(DirectCast(obj, SweptVolume(Of T)))
        End Function

        Public Overloads Function Equals(other As SweptVolume(Of T)) As Boolean Implements IEquatable(Of SweptVolume(Of T)).Equals
            Return other IsNot Nothing AndAlso Original.Equals(other.Original)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Original.GetHashCode()
        End Function

#End Region

        Public Shared Function FromMovement(Of TMoveable As { IFinite, IMoveable, ICloneable })(item As TMoveable) As SweptVolume(Of TMoveable)
            Dim copy As TMoveable = CType(item.Clone(), TMoveable)
            copy.Move()
            Return New SweptVolume(Of TMoveable)(item, copy.Bounds().Union(item.Bounds()))
        End Function

        Public Shared Narrowing Operator CType(volume As SweptVolume(Of T)) As T
            Return volume.Original
        End Operator

    End Class
End NameSpace