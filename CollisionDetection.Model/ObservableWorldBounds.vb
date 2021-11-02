Imports System.Collections.ObjectModel

Public Class ObservableWorldBounds
    Implements IObservable(Of Bounds)

    Private ReadOnly _observers As ICollection(Of IObserver(Of Bounds)) = New Collection(Of IObserver(Of Bounds))
    Private _bounds As Bounds

    Public Sub New(bounds As Bounds)
        _bounds = bounds
    End Sub

    Public Property Bounds As Bounds
        Get
            Return _bounds
        End Get
        Set
            _bounds = value

            For Each subscriber As IObserver(Of Bounds) In _observers
                subscriber.OnNext(value)
            Next
        End Set
    End Property

    Public Function Subscribe(observer As IObserver(Of Bounds)) As IDisposable Implements IObservable(Of Bounds).Subscribe
        If Not _observers.Contains(observer) Then
            _observers.Add(observer)
        End If

        Return New Unsubscribe(_observers, observer)
    End Function

    Private Class Unsubscribe
        Implements IDisposable

        Private _observers As ICollection(Of IObserver(Of Bounds))
        Private _observer As IObserver(Of Bounds)

        Sub New(observers As ICollection(Of IObserver(Of Bounds)), observer As IObserver(Of Bounds))
            _observers = observers
            _observer = observer
        End Sub


        Public Sub Dispose() Implements IDisposable.Dispose
            If _observers IsNot Nothing AndAlso _observer IsNot Nothing AndAlso _observers.Contains(_observer) Then
                _observers.Remove(_observer)
            End If

            _observers = Nothing
            _observer = Nothing

            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Class