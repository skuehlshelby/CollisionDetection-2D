Imports System.Drawing

Namespace Rendering

    Public Class ColorPalette
        Implements IEquatable(Of ColorPalette)

        Private ReadOnly _hashCode As Integer

        Public Sub New(background As Color, foreground As Color)
            Me.New(background, background, foreground, foreground)
        End Sub

        Public Sub New(background As Color, backgroundAccent As Color, foreground As Color, highContrast As Color)
            Me.Background = background
            Me.BackgroundAccent = backgroundAccent
            Me.Foreground = foreground
            Me.HighContrast = highContrast
            _hashCode = (Background, BackgroundAccent, Foreground, HighContrast).GetHashCode()
        End Sub

        Public ReadOnly Property Background As Color

        Public ReadOnly Property BackgroundAccent As Color

        Public ReadOnly Property Foreground As Color

        Public ReadOnly Property HighContrast As Color


        Public Function Values() As IEnumerable(Of Color)
            Return New Color(){Background, BackgroundAccent, Foreground, HighContrast}
        End Function

        Public Overrides Function ToString() As String
            Dim nl As String = Environment.NewLine

            Return $"--{NameOf(ColorPalette)}--
                 {nl}  {NameOf(Background)}: {Background}
                 {nl}  {NameOf(BackgroundAccent)}: {BackgroundAccent}
                 {nl}  {NameOf(Foreground)}: {Foreground}
                 {nl}  {NameOf(HighContrast)}: {HighContrast}"
        End Function

        Public Overloads Overrides Function Equals(obj As Object) As Boolean
            Return Equals(TryCast(obj, ColorPalette))
        End Function

        Public Overloads Function Equals(other As ColorPalette) As Boolean Implements IEquatable(Of ColorPalette).Equals
            Return other IsNot Nothing AndAlso Not other.Values().Except(Values()).Any()
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _hashCode
        End Function

        Public Shared Operator =(left as ColorPalette, right as ColorPalette) as Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(left as ColorPalette, right as ColorPalette) as Boolean
            Return Not Equals(left, right)
        End Operator
    End Class
End NameSpace