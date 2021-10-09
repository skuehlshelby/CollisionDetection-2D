Imports System.Reflection

Public NotInheritable Class MonitoredProperty
    Implements IEquatable(Of MonitoredProperty)

    Private Readonly _value As Integer
    Private Readonly _name As String

    Private Sub New(value As Integer, name As String)
        _value = value
        _name = name
    End Sub

    Public Shared Function Values() As IEnumerable(Of MonitoredProperty)
        return GetType(MonitoredProperty) _
            .GetProperties(BindingFlags.Static Or BindingFlags.Public Or BindingFlags.DeclaredOnly) _
            .Select(Function(prop) CType(prop.GetValue(Nothing), MonitoredProperty))
    End Function

    Public Shared Function Parse(name As String) As MonitoredProperty
        return Values().Single(Function(value) value._name.Equals(name, StringComparison.Ordinal))
    End Function

    Public Shared ReadOnly Property FrameRate As MonitoredProperty = new MonitoredProperty(0, "Frame Rate")
    
    Public Shared ReadOnly Property Paused As MonitoredProperty = new MonitoredProperty(1, "Paused")
    
    Public Shared ReadOnly Property ShapeCount As MonitoredProperty = new MonitoredProperty(2, "Shape Count")
    
    Public Shared ReadOnly Property BoundingVolumeVisibility As MonitoredProperty = new MonitoredProperty(3, "Bounding Volume Visibility")

    Public Shared ReadOnly Property AverageRenderTimeVisibility As MonitoredProperty = new MonitoredProperty(4, "Average Render Time Visibility")
    
    Public Shared ReadOnly Property SplitMethod As MonitoredProperty = new MonitoredProperty(5, "Split Method")
    
    Public Shared ReadOnly Property AvailableSplitMethods As MonitoredProperty = new MonitoredProperty(6, "Available Split Methods")
    
    Public Shared ReadOnly Property CollisionHandler As MonitoredProperty = new MonitoredProperty(7, "Collision Handler")

    Public Shared ReadOnly Property AvailableCollisionHandlers As MonitoredProperty = new MonitoredProperty(8, "Available Collision Handlers")

    Public Shared ReadOnly Property AvailableColors As MonitoredProperty = new MonitoredProperty(9, "Available Colors")
    
    Public Shared ReadOnly Property ColorAdded As MonitoredProperty = new MonitoredProperty(10, "Color Added")

    Public Shared ReadOnly Property ColorRemoved As MonitoredProperty = new MonitoredProperty(11, "Color Removed")

    Public Shared ReadOnly Property WorldBounds As MonitoredProperty = new MonitoredProperty(12, "World Bounds")
    
    Public Shared ReadOnly Property MousePosition As MonitoredProperty = new MonitoredProperty(13, "Mouse Position")

    Public Shared ReadOnly Property ShapeFactory As MonitoredProperty = new MonitoredProperty(14, "Shape Factory")

    Public Shared ReadOnly Property ColorTracker As MonitoredProperty = new MonitoredProperty(15, "Color Tracker")

    Public Shared ReadOnly Property AvailableRenderers As MonitoredProperty = New MonitoredProperty(16, "AvailableRenderers")

    Public Shared ReadOnly Property MaximumShapeCount As MonitoredProperty = New MonitoredProperty(17, "Maximum Shape Count")

    Public Shared ReadOnly Property MaximumFrameRate As MonitoredProperty = New MonitoredProperty(18, "Maximum Frame Rate")

#Region "Overrides And Equality Comparison"

    Public Overrides Function ToString() As String
        return _name
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        return Equals(TryCast(obj, MonitoredProperty))
    End Function
    
    Public Overloads Function Equals(other As MonitoredProperty) As Boolean Implements IEquatable(Of MonitoredProperty).Equals
        return other IsNot Nothing AndAlso _value = other._value
    End Function

    Public Overrides Function GetHashCode() As Integer
        return _value
    End Function

    public Shared Operator =(left As MonitoredProperty, right As MonitoredProperty) As Boolean
        return Equals(left, right)
    End Operator

    Public Shared Operator <>(left As MonitoredProperty, right As MonitoredProperty) As Boolean
        return Not Equals(left, right)
    End Operator

#End Region

End Class