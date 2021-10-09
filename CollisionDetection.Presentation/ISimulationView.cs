using System;
using CollisionDetection.Model;

namespace CollisionDetection.Presentation
{
    public interface ISimulationView
    {
        event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        object QueryProperty(MonitoredProperty monitoredProperty);

        bool TryQueryProperty(MonitoredProperty monitoredProperty, out object value);
    }
}


