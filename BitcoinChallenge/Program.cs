using LineDC.Liff;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinChallenge {
    public class Program {
        public static async Task Main(string[] args) {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            AppSettings appSettings = builder.Configuration.Get<AppSettings>();
            builder.Services.AddSingleton(new HttpClient {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            builder.Services.AddSingleton<ILiffClient>(new LiffClient(appSettings.LiffId));
            builder.Services.AddTransient(serviceType => new HttpClient {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            WebAssemblyHost host = builder.Build();
            await host.RunAsync();
        }

    }
}
