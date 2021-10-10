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
        private bool lastDrewRectangle;
        private bool lastDrewEllipse;
        private bool fontSet;

        public GraphicsWrapper(Canvas2DContext context)
        {
            this.context = context;
        }

        public async void DrawDashedRectangle(Bounds bounds, Color color)
        {
            if (!lastDrewRectangle)
            {
                await context.SetLineDashAsync(new[] { 4.0f, 2.0f });
            }

            await context.SetStrokeStyleAsync(ColorToString(color));
            await context.StrokeRectAsync(bounds.BottomLeft.X, bounds.BottomLeft.Y, bounds.Width(), bounds.Height());
            lastDrewRectangle = true;
            lastDrewEllipse = false;
        }

        public async void FillEllipse(Bounds bounds, Color color)
        {
            var center = bounds.BottomLeft.Lerp(bounds.TopRight, 1.0F / 2.0F);

            if (!lastDrewEllipse)
            {
                await context.SetLineDashAsync(new[] { 1.0f, 0.0f });
            }
            
            await context.SetStrokeStyleAsync(ColorToString(color));
            await context.SetFillStyleAsync(ColorToString(color));
            await context.BeginPathAsync();
            await context.ArcAsync(center.X, center.Y, bounds.Width() / 2, 0, 2 * Math.PI, false);
            await context.FillAsync();
            await context.StrokeAsync();

            lastDrewRectangle = false;
            lastDrewEllipse = true;
        }

        public async void DrawText(string text, float fontSize, FontName fontName, Color color, Point location)
        {
            if (!fontSet)
            {
                await context.SetFontAsync(fontName.ToHtml(16.0F));
                fontSet = true;
            }
            
            await context.SetFillStyleAsync(ColorToString(color));
            await context.FillTextAsync(text, location.X, location.Y);
        }

        private static string ColorToString(Color color)
        {
            return color.IsNamedColor ? color.Name : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}