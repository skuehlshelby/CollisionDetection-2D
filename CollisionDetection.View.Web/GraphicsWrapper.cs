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
        private readonly float[] dashStyle = {6.0f, 2.0f};
        private readonly float[] noDash = Array.Empty<float>();
        private float[] lastDashStyle;
        private Color lastColor;
        private string lastFont;

        public GraphicsWrapper(Canvas2DContext context)
        {
            this.context = context;
        }

        public async void DrawDashedRectangle(Bounds bounds, Color color)
        {
            if (lastDashStyle != dashStyle)
            {
                await context.SetLineDashAsync(dashStyle);
                lastDashStyle = dashStyle;
            }

            if (lastColor != color)
            {
                await context.SetStrokeStyleAsync(ColorToString(color));
                lastColor = color;
            }
                
            await context.StrokeRectAsync(bounds.BottomLeft.X, bounds.BottomLeft.Y, bounds.Width(), bounds.Height());
        }

        public async void FillEllipse(Bounds bounds, Color color)
        {
            var center = bounds.BottomLeft.Lerp(bounds.TopRight, 1.0F / 2.0F);

            if (lastDashStyle != noDash)
            {
                await context.SetLineDashAsync(noDash);
                lastDashStyle = noDash;
            }
            
            if (lastColor != color)
            {
                string colorAsString = ColorToString(color);
                await context.SetStrokeStyleAsync(colorAsString);
                await context.SetFillStyleAsync(colorAsString);
                lastColor = color;
            }

            await context.BeginPathAsync();
            await context.ArcAsync(center.X, center.Y, bounds.Width() / 2, 0, 2 * Math.PI, false);
            await context.FillAsync();
            await context.StrokeAsync();
        }

        public async void DrawText(string text, float fontSize, FontName fontName, Color color, Point location)
        {
            if (lastFont != fontName.Name)
            {
                await context.SetFontAsync(fontName.ToHtml(16.0F));
                lastFont = fontName.Name;
            }
            
            if (lastColor != color)
            {
                await context.SetFillStyleAsync(ColorToString(color));
                lastColor = color;
            }

            await context.FillTextAsync(text, location.X, location.Y);
        }

        private static string ColorToString(Color color)
        {
            return color.IsNamedColor ? color.Name : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}