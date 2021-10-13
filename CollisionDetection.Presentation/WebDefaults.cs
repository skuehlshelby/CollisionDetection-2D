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
    public class WebDefaults : DefaultProviderBase
    {
        public WebDefaults() : base(GetDefaults())
        {

        }

        private static IDictionary<MonitoredProperty, IList<object>> GetDefaults()
        {
            return new Dictionary<MonitoredProperty, IList<object>>()
            {
                { MonitoredProperty.Paused, new List<object> { true } },
                { MonitoredProperty.FrameRate, new List<object> { 25, "25", TimeSpan.FromSeconds(1.0 / 25.0) } },
                { MonitoredProperty.MaximumFrameRate, new List<object> { 30, "30" } },
                { MonitoredProperty.ShapeCount, new List<object> { 10, "10" } },
                { MonitoredProperty.MaximumShapeCount, new List<object> { 60, "60" } },
                { MonitoredProperty.AvailableRenderers, new List<object> { new[] { SolidCircle.AsFactory(), RedDashedBounds.AsFactory(), GreenAndBlueBVH.AsFactory(), SolidMonoSpaceText.AsFactory(16.0F, Color.CornflowerBlue, (5.0F, 20.0F)) } } }
            };
        }
    }
}
