using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionDetection.Model;
using CollisionDetection.Model.BVH;
using CollisionDetection.Model.Collision;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public class WebStart : IDefaultProvider
    {
        private readonly IDictionary<MonitoredProperty, IList<object>> valuesByName =
            new Dictionary<MonitoredProperty, IList<object>>();

        public WebStart()
        {
            valuesByName.Add(MonitoredProperty.AverageRenderTimeVisibility, new List<object> { false });
            valuesByName.Add(MonitoredProperty.BoundingVolumeVisibility, new List<object> { false });
            valuesByName.Add(MonitoredProperty.Paused, new List<object> {true});
            valuesByName.Add(MonitoredProperty.FrameRate, new List<object> {25, "25", TimeSpan.FromSeconds(1.0 / 25.0)});
            valuesByName.Add(MonitoredProperty.MaximumFrameRate, new List<object> {30, "30"});
            valuesByName.Add(MonitoredProperty.ShapeCount, new List<object> {10, "10"});
            valuesByName.Add(MonitoredProperty.MaximumShapeCount, new List<object> {60, "60"});
            valuesByName.Add(MonitoredProperty.AvailableSplitMethods, new List<object> {SplitMethod.Values(), SplitMethod.Values().Select(method => method.Name).ToArray() });
            valuesByName.Add(MonitoredProperty.SplitMethod, new List<object> { SplitMethod.Midpoint, SplitMethod.Midpoint.Name });
            valuesByName.Add(MonitoredProperty.AvailableCollisionHandlers, new List<object> { CollisionHandler.Values(), CollisionHandler.Values().Select(method => method.Name).ToArray() });
            valuesByName.Add(MonitoredProperty.CollisionHandler, new List<object> { CollisionHandler.Discrete, CollisionHandler.Discrete.Name });
            valuesByName.Add(MonitoredProperty.AvailableRenderers, new List<object> { new []{SolidCircle.AsFactory(), RedDashedBounds.AsFactory(), GreenAndBlueBVH.AsFactory(), SolidMonoSpaceText.AsFactory()}});

            ColorTrackerSingleton.Instance.Add(Color.CornflowerBlue);
            ColorTrackerSingleton.Instance.Add(Color.Goldenrod);
            ColorTrackerSingleton.Instance.Add(Color.DarkCyan);

            valuesByName.Add(MonitoredProperty.ColorTracker, new List<object> { ColorTrackerSingleton.Instance });
            valuesByName.Add(MonitoredProperty.AvailableColors, new List<object> { ColorTrackerSingleton.Instance.All().ToArray() });
            valuesByName.Add(MonitoredProperty.ShapeFactory, new List<object> { new CircleFactory(ColorTrackerSingleton.Instance) });
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
