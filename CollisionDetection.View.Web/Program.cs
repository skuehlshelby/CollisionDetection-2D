using System;
using System.Net.Http;
using System.Threading.Tasks;
using CollisionDetection.Model;
using CollisionDetection.Presentation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CollisionDetection.View.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IDefaultProvider>(new WebStart());

            await builder.Build().RunAsync();
        }
    }
}
