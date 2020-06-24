using BitcoinChallengeBlazorApp.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinChallengeBlazorApp {
    public class Program {
        public static async Task Main(string[] args) {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("app");
            _ = builder.Services.AddTransient(sp => new HttpClient { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });
            _ = builder.Services.AddSingleton<PeriodicTimer>(
                new PeriodicTimer(builder.Configuration.Get<AppSettings>())
                );
            
            await builder.Build()
                .RunAsync();
        }
    }
}
