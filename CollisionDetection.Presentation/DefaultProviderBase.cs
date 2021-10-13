using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CollisionDetection.Model;
using CollisionDetection.Model.BVH;
using CollisionDetection.Model.Collision;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public abstract class DefaultProviderBase : IDefaultProvider
    {
        private readonly IDictionary<MonitoredProperty, IList<object>> valuesByName;

        protected DefaultProviderBase(IDictionary<MonitoredProperty, IList<object>> defaultOverrides)
        {
            valuesByName = defaultOverrides ?? throw new ArgumentNullException(nameof(defaultOverrides));

            foreach (var genericDefault in GetGenericDefaults())
            {
                if (!valuesByName.ContainsKey(genericDefault.Key))
                {
                    valuesByName.Add(genericDefault);
                }
            }

            var colorTracker = valuesByName[MonitoredProperty.ColorTracker].OfType<IColorTracker>().First();

            foreach (var color in valuesByName[MonitoredProperty.AvailableColors].OfType<Color[]>().First())
            {
                colorTracker.Add(color);
            }

            if (!valuesByName.ContainsKey(MonitoredProperty.ShapeFactory))
            {
                valuesByName.Add(MonitoredProperty.ShapeFactory, new List<object> { new CircleFactory(colorTracker) } );
            }
        }

        private static IDictionary<MonitoredProperty, IList<object>> GetGenericDefaults()
        {
            return new Dictionary<MonitoredProperty, IList<object>>
            {
                { MonitoredProperty.AvailableCollisionHandlers, new List<object> { CollisionHandler.Values(), CollisionHandler.Values().Select(method => method.Name).ToArray() } },
                { MonitoredProperty.AvailableColors, new List<object> { new[] { Color.CornflowerBlue, Color.Goldenrod, Color.DarkCyan } } },
                { MonitoredProperty.AvailableRenderers, new List<object> { new[] { SolidCircle.AsFactory(), RedDashedBounds.AsFactory(), GreenAndBlueBVH.AsFactory(), SolidMonoSpaceText.AsFactory(12.0F, Color.CornflowerBlue, (5.0F, 5.0F)) } } },
                { MonitoredProperty.AvailableSplitMethods, new List<object> { SplitMethod.Values(), SplitMethod.Values().Select(method => method.Name).ToArray() } },
                { MonitoredProperty.AverageRenderTimeVisibility, new List<object> { false } },
                { MonitoredProperty.BoundingVolumeVisibility, new List<object> { false } },
                { MonitoredProperty.CollisionHandler, new List<object> { CollisionHandler.Discrete, CollisionHandler.Discrete.Name } },
                { MonitoredProperty.ColorTracker, new List<object> { ColorTrackerSingleton.Instance } },
                { MonitoredProperty.FrameRate, new List<object> { 30, "30", TimeSpan.FromSeconds(1.0 / 30.0) } },
                { MonitoredProperty.MaximumFrameRate, new List<object> { 60, "60", TimeSpan.FromSeconds(1.0 / 60.0) } },
                { MonitoredProperty.MaximumShapeCount, new List<object> { 250, "250" } },
                { MonitoredProperty.Paused, new List<object> { false } },
                { MonitoredProperty.ShapeCount, new List<object> { 0, "0" } },
                { MonitoredProperty.SplitMethod, new List<object> { SplitMethod.Midpoint, SplitMethod.Midpoint.Name } }
            };
        }

        object IDefaultProvider.GetDefault(MonitoredProperty name)
        {
            return valuesByName[name].First();
        }

        bool IDefaultProvider.TryGetDefault(MonitoredProperty name, out object value)
        {
            try
            {
                value = ((IDefaultProvider)this).GetDefault(name);
                return true;
            }
            catch (KeyNotFoundException)
            {
                value = null;
                return false;
            }
        }

        T IDefaultProvider.GetDefault<T>(MonitoredProperty name)
        {
            return (T)valuesByName[name].First(value => value is T);
        }

        bool IDefaultProvider.TryGetDefault<T>(MonitoredProperty name, out T value)
        {
            try
            {
                value = ((IDefaultProvider)this).GetDefault<T>(name);
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