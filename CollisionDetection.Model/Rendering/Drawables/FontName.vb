Imports System.Reflection

Namespace Rendering.Drawables

    Public Class FontName
        Implements IEquatable(Of FontName)

        Private ReadOnly _value As Integer

        Private Sub New(value As Integer, name As String, fontFamily As String)
            _value = value
            Me.Name = name
            Me.FontFamily = fontFamily
        End Sub

        Public ReadOnly Property Name as String

        Public ReadOnly Property FontFamily As String

        Public Function ToHtml(fontSize As Single) As String
            Return $"{Math.Round(fontSize, 1)}px {Name} {FontFamily}"
        End Function

        Public Function Parse(fontName As String) As FontName
            Return Values().First(Function(value) value.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
        End Function

        Public Shared Function Values() As IEnumerable(Of FontName)
            Return _
                GetType(FontName).GetProperties(BindingFlags.Public Or BindingFlags.Static Or BindingFlags.GetProperty).
                    Select(Function(prop) CType(prop.GetValue(Nothing), FontName))
        End Function

#Region "Families"

        Private Shared ReadOnly Property Serif As String = "serif"

        Private Shared ReadOnly Property SansSerif As String = "sans-serif"

        Private Shared ReadOnly Property MonoSpace As String = "monospace"

#End Region

#Region "Fonts"
        Public Shared ReadOnly Property Arial As FontName = New FontName(0, NameOf(Arial), SansSerif)

        Public Shared ReadOnly Property Verdana As FontName = New FontName(1, NameOf(Verdana), SansSerif)

        Public Shared ReadOnly Property Helvetica As FontName = New FontName(2, NameOf(Helvetica), SansSerif)

        Public Shared ReadOnly Property Tahoma As FontName = New FontName(3, NameOf(Tahoma), SansSerif)

        Public Shared ReadOnly Property TrebuchetMs As FontName = New FontName(4, "Trebuchet MS", SansSerif)

        Public Shared ReadOnly Property TimesNewRoman As FontName = New FontName(5, "Times New Roman", Serif)

        Public Shared ReadOnly Property Georgia As FontName = New FontName(6, NameOf(Georgia), Serif)

        Public Shared ReadOnly Property Garamond As FontName = New FontName(7, NameOf(Garamond), Serif)

        Public Shared ReadOnly Property CourierNew As FontName = New FontName(8, "Courier New", MonoSpace)

#End Region

#Region "Overrides As Equality Comparison"

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Equals(TryCast(obj, FontName))
        End Function

        Public Overloads Function Equals(other As FontName) As Boolean Implements IEquatable(Of FontName).Equals
            Return other IsNot Nothing AndAlso other._value = _value
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _value
        End Function

#End Region

    End Class

End NameSpace