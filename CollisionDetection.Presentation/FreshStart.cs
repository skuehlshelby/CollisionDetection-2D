using System;
using System.Collections.Generic;
using CollisionDetection.Model.BVH;
using CollisionDetection.Model.Collision;
using System.Drawing;
using System.Linq;
using CollisionDetection.Model;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public sealed class FreshStart : IDefaultProvider
    {
        private readonly IDictionary<MonitoredProperty, IList<object>> valuesByName = new Dictionary<MonitoredProperty, IList<object>>()
        {
            {MonitoredProperty.AverageRenderTimeVisibility, new List<object> { false }},
            {MonitoredProperty.BoundingVolumeVisibility, new List<object> { false }},
            {MonitoredProperty.Paused, new List<object> { false }},
            {MonitoredProperty.AvailableSplitMethods, new List<object> {SplitMethod.Values(), SplitMethod.Values().Select(method => method.Name).ToArray()}},
            {MonitoredProperty.SplitMethod, new List<object> { SplitMethod.Midpoint, SplitMethod.Midpoint.Name }},
            {MonitoredProperty.AvailableCollisionHandlers, new List<object> {CollisionHandler.Values(), CollisionHandler.Values().Select(method => method.Name).ToArray()}},
            {MonitoredProperty.CollisionHandler, new List<object> {CollisionHandler.Discrete, CollisionHandler.Discrete.Name}},
            {MonitoredProperty.FrameRate, new List<object> { 30, TimeSpan.FromSeconds(1.0 / 30.0) }},
            {MonitoredProperty.ShapeCount, new List<object> { 0 }},
            {MonitoredProperty.AvailableRenderers, new List<object> { new []{SolidCircle.AsFactory(), RedDashedBounds.AsFactory(), GreenAndBlueBVH.AsFactory(), SolidMonoSpaceText.AsFactory()} }}
        };

        public FreshStart()
        {
            ColorTrackerSingleton.Instance.Add(Color.CornflowerBlue);
            ColorTrackerSingleton.Instance.Add(Color.Goldenrod);
            ColorTrackerSingleton.Instance.Add(Color.DarkCyan);

            valuesByName.Add(MonitoredProperty.ColorTracker, new List<object> { ColorTrackerSingleton.Instance });
            valuesByName.Add(MonitoredProperty.AvailableColors, new List<object> { ColorTrackerSingleton.Instance.All().ToArray() });
            valuesByName.Add(MonitoredProperty.ShapeFactory, new List<object> { new CircleFactory(ColorTrackerSingleton.Instance) });
        }

        public object GetDefault(MonitoredProperty name)
        {
            return valuesByName[name].First();
        }

        public bool TryGetDefault(MonitoredProperty name, out object value)
        {
            try
            {
                value = GetDefault(name);
                return true;
            }
            catch (KeyNotFoundException)
            {
                value = null;
                return false;
            }
        }

        public T GetDefault<T>(MonitoredProperty name)
        {
            return (T)valuesByName[name].First(value => value is T);
        }

        public bool TryGetDefault<T>(MonitoredProperty name, out T value)
        {
            try
            {
                value = GetDefault<T>(name);
                return true;
            }
            catch (Exception e) when (e is KeyNotFoundException or InvalidCastException)
            {
                value = default;
                return false;
            }
        }
    }
}
