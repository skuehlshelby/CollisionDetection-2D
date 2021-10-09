using System;
using System.Drawing;
using Blazor.Extensions.Canvas.Canvas2D;
using CollisionDetection.Model;
using CollisionDetection.Model.Rendering;
using CollisionDetection.Model.Rendering.Drawables;
using Point = CollisionDetection.Model.Point;

namespace CollisionDetection.View.Web
{
    public class GraphicsWrapper :IGraphics
    {
        private readonly Canvas2DContext context;

        public GraphicsWrapper(Canvas2DContext context)
        {
            this.context = context;
        }

        public async void DrawDashedRectangle(Bounds bounds, Color color)
        {
            await context.SetStrokeStyleAsync(ColorToString(color));
            await context.SetLineDashAsync(new[] { 4.0f, 2.0f });
            await context.StrokeRectAsync(bounds.BottomLeft.X, bounds.BottomLeft.Y, bounds.Width(), bounds.Height());
        }

        public async void FillEllipse(Bounds bounds, Color color)
        {
            var center = bounds.BottomLeft.Lerp(bounds.TopRight, 1.0F / 2.0F);

            await context.BeginPathAsync();
            await context.SetLineDashAsync(new[] { 1.0f, 0.0f });
            await context.SetStrokeStyleAsync(ColorToString(color));
            await context.SetFillStyleAsync(ColorToString(color));
            await context.ArcAsync(center.X, center.Y, bounds.Width() / 2, 0, 2 * Math.PI, false);
            await context.FillAsync();
            await context.StrokeAsync();
        }

        public async void DrawText(string text, float fontSize, FontName fontName, Color color, Point location)
        {
            await context.SetFontAsync(fontName.ToHtml(16.0F));
            await context.SetFillStyleAsync(ColorToString(color));
            await context.FillTextAsync(text, location.X, location.Y);
        }

        private static string ColorToString(Color color)
        {
            return color.IsNamedColor ? color.Name : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}