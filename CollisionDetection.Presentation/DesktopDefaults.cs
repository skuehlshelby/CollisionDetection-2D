using System;
using System.Collections.Generic;
using System.Drawing;
using CollisionDetection.Model;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public sealed class DesktopDefaults : DefaultProviderBase
    {
        public DesktopDefaults() : base(GetDefaults())
        {

        }

        private static IDictionary<MonitoredProperty, IList<object>> GetDefaults()
        {
            return new Dictionary<MonitoredProperty, IList<object>>()
            {
                { MonitoredProperty.FrameRate, new List<object> { 40, "40", TimeSpan.FromSeconds(1.0 / 40.0) } },
                { MonitoredProperty.AvailableRenderers, new List<object> { new[] { SolidCircle.AsFactory(), RedDashedBounds.AsFactory(), RedDashedBVH.AsFactory(), SolidMonoSpaceText.AsFactory(12.0F, Color.FromArgb(218, 0, 55), (5.0F, 5.0F)) } }}
            };
        }
    }
}
