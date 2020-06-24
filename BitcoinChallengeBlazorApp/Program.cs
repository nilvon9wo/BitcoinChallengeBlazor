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
            AppSettings appSettings = builder.Configuration.Get<AppSettings>();
            _ = builder.Services.AddSingleton<BitcoinChallengeSettings>(new BitcoinChallengeSettings(appSettings));
            _ = builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            _ = builder.Services.AddSingleton<PeriodicTimer>(new PeriodicTimer());
            await builder.Build().RunAsync();
        }
    }
}
