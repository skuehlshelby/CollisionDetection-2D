using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CollisionDetection.View.Web
{
    public class ResizeHelper :IDisposable
    {
        private readonly DotNetObjectReference<ResizeHelper> objectReference;
        private readonly ElementReference elementReference;
        private readonly IJSRuntime jsRuntime;
        private readonly double heightRatio;
        private readonly double widthRatio;

        private ResizeHelper(Size elementSize, Size initialWindowSize, ElementReference elementReference, IJSRuntime jsRuntime)
        {
            objectReference = DotNetObjectReference.Create(this);
            this.jsRuntime = jsRuntime;
            this.elementReference = elementReference;
            heightRatio = (double)elementSize.Height / initialWindowSize.Height;
            widthRatio = (double)elementSize.Width / initialWindowSize.Width;
        }

        public event EventHandler<Size> OnResize;

        [JSInvokable]
        public void OnWindowResize(int windowHeight, int windowWidth)
        {
            var size = new Size((int)(windowWidth * widthRatio), (int)(windowHeight * heightRatio));
            //SetSize(size);
            OnResize?.Invoke(this, size);
        }

        public void Dispose()
        {
            objectReference.Dispose();
        }

        public static async Task<Size> GetSize(IJSRuntime jsRuntime, ElementReference element)
        {
            return new Size(
                await jsRuntime.InvokeAsync<int>("getWidth", element),
                await jsRuntime.InvokeAsync<int>("getHeight", element));
        }

        public static async Task<Size> GetWindowSize(IJSRuntime jsRuntime)
        {
            return new Size(
                await jsRuntime.InvokeAsync<int>("getWindowWidth"),
                await jsRuntime.InvokeAsync<int>("getWindowHeight"));
        }

        private async void SetSize(Size size)
        {
            await jsRuntime.InvokeVoidAsync("setDimensions", elementReference, size.Height, size.Width);
        }

        public static async void SetSize(IJSRuntime jsRuntime, ElementReference element, Size size)
        {
            await jsRuntime.InvokeVoidAsync("setDimensions", element, size.Height, size.Width);
        }

        public static async Task<ResizeHelper> Create(IJSRuntime jsRuntime, ElementReference element, Size initialSize)
        {
            var windowSize = await GetWindowSize(jsRuntime);
            //SetSize(jsRuntime, element, initialSize);
            var helper = new ResizeHelper(initialSize, windowSize, element, jsRuntime);
            helper.RegisterCallback(jsRuntime, element);
            return helper;
        }

        private async void RegisterCallback(IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("registerResizeCallback", objectReference, element);
        }
    }
}