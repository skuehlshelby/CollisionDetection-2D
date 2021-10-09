using System;
using CollisionDetection.Model;

namespace CollisionDetection.Presentation
{
    public sealed class PropertyChangedEventArgs : EventArgs
    {

        public PropertyChangedEventArgs(MonitoredProperty monitoredProperty, object value)
        {
            MonitoredProperty = monitoredProperty;
            Value = value;
        }

        public MonitoredProperty MonitoredProperty { get; }
        
        public object Value { get; }
    }
}