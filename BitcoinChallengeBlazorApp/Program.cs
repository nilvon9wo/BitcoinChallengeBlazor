using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoinChallengeBlazorApp {
    public class Program {
        public static async Task Main(string[] args) {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            AppSettings appSettings = builder.Configuration.Get<AppSettings>();
            _ = builder.Services.AddSingleton<BitcoinChallengeSettings>(new BitcoinChallengeSettings(appSettings));
            _ = builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}
