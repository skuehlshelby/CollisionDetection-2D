using System.Drawing;
using Microsoft.JSInterop;

namespace CollisionDetection.View.Web
{
    public class WindowResizeHelper
    {
        public Size CanvasSize { get; private set; }

        [JSInvokable]
        public void UpdateCanvasSize(int width, int height) => CanvasSize = new Size(width, height);
    }
}