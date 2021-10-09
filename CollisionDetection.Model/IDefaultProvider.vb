Imports System.Runtime.InteropServices

public interface IDefaultProvider

    Function GetDefault(name As MonitoredProperty) As Object

    Function TryGetDefault(name As MonitoredProperty, <Out> ByRef value As Object) As Boolean
    
    Function GetDefault(Of T)(name As MonitoredProperty) As T

    Function TryGetDefault(Of T)(name As MonitoredProperty, <Out> ByRef value As T) As Boolean

End Interface