using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CollisionDetection.View.Web
{
    public class ScaleChildWithParent
    {
        private readonly ElementReference child;
        private readonly IJSRuntime jsRuntime;
        private readonly DotNetObjectReference<ScaleChildWithParent> objectReference;

        private ScaleChildWithParent(ElementReference child, IJSRuntime jsRuntime)
        {
            this.child = child;
            this.jsRuntime = jsRuntime;
            objectReference = DotNetObjectReference.Create(this);
        }

        public static async Task<ScaleChildWithParent> Create(ElementReference child, ElementReference parent, IJSRuntime jsRuntime)
        {
            var instance = new ScaleChildWithParent(child, jsRuntime);
            var size = await instance.GetSize(parent);
            instance.SetSize(child, size.Height, size.Width);
            instance.AddResizeListener(parent);
            return instance;
        }

        public event EventHandler<Size> OnResize;

        [JSInvokable]
        public async void Resized(int height, int width)
        {
            await SetWidth(child, width);

            OnResize?.Invoke(this, await GetSize(child));
        }

        private async void AddResizeListener(ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("addResizeListener", objectReference, element);
        }

        private async void SetSize(ElementReference element, int height, int width )
        {
            await jsRuntime.InvokeVoidAsync("setSize", element, height, width);
        }

        private async Task<Size> GetSize(ElementReference element)
        {
            return new Size(await GetWidth(element), await GetHeight(element));
        }

        private async Task<int> GetHeight(ElementReference element)
        {
            return await jsRuntime.InvokeAsync<int>("getHeight", element);
        }

        private async Task<int> GetWidth(ElementReference element)
        {
            return await jsRuntime.InvokeAsync<int>("getWidth", element);
        }

        private async ValueTask SetHeight(ElementReference element, int height)
        {
            await jsRuntime.InvokeVoidAsync("setHeight", element, height);
        }

        private async ValueTask SetWidth(ElementReference element, int width)
        {
            await jsRuntime.InvokeVoidAsync("setWidth", element, width);
        }
    }
}