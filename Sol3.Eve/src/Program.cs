using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Sol3.Eve.Services;

namespace Sol3.Eve
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //
            //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth

                options.ProviderOptions.Authority = "https://dev-712283.okta.com";
                options.ProviderOptions.ClientId = "0oa36our0hMpuyVAB357";  // "0oa2yttq55XjCR3AO357";
                options.ProviderOptions.ResponseType = "code";
                //options.ProviderOptions.DefaultScopes.Add("api");
                //options.ProviderOptions.DefaultScopes.Add("openid");
                //options.ProviderOptions.DefaultScopes.Add("profile");
                //options.ProviderOptions.DefaultScopes.Add("email");

                options.AuthenticationPaths.LogInCallbackPath = "https://localhost:5001/authentication/login-callback";
                options.AuthenticationPaths.LogInFailedPath = "https://localhost:5001/authentication/login-failed";
                options.AuthenticationPaths.LogInPath = "https://localhost:5001/authentication/login";
                options.AuthenticationPaths.LogOutCallbackPath = "https://localhost:5001/authentication/logout-callback";
                options.AuthenticationPaths.LogOutFailedPath = "https://localhost:5001/authentication/logout-failed";
                options.AuthenticationPaths.LogOutPath = "https://localhost:5001/authentication/logout";
                options.AuthenticationPaths.LogOutSucceededPath = "https://localhost:5001/authentication/logged-out";
                //options.AuthenticationPaths.ProfilePath = "https://localhost:5001/authentication/profile";
                //options.AuthenticationPaths.RegisterPath = "https://localhost:5001/authentication/register";
                //options.AuthenticationPaths.RemoteProfilePath = "https://localhost:5001/authentication/login";
                //options.AuthenticationPaths.RemoteRegisterPath = "https://localhost:5001/authentication/login";
            });

            await builder.Build().RunAsync();
        }
    }
}
