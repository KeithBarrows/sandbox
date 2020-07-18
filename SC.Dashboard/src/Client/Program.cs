using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SC.Dashboard.Shared.Auth;
using Sol3.Infrastructure.Configuration;

namespace SC.Dashboard.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var configurationRoot = ConfigurationManager.InitializeConfiguration();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddApiAuthorization();
            //builder.Services.AddOidcAuthentication(options => EsiAuthenticationMiddleware.SetOAuth2Options(options, configurationRoot));
            builder.Services.AddOidcAuthentication(options => 
            {
                options.ProviderOptions.Authority = "https://login.eveonline.com/oauth/authorize";
                options.ProviderOptions.ClientId = "da968792109f42afb459efd51a0655c4";
            });

            await builder.Build().RunAsync();
        }
    }
}
