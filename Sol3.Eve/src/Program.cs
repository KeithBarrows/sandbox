using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Sol3.Eve.Services;
using Sol3.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace Sol3.Eve
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = SerilogSetup.Setup("Sol3.Eve");
            logger.Information("Initialize application");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //
            //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            builder.RootComponents.Add<App>("app");

            //TODO: --> builder.Services.AddSingleton<ILogger>(logger);

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth

                // EVE ONLINE //
                options.ProviderOptions.Authority = "https://login.eveonline.com"; // /oauth/authorize";
                options.ProviderOptions.ClientId = "da968792109f42afb459efd51a0655c4";
                if (options.ProviderOptions.DefaultScopes.Count > 0)
                {
                    options.ProviderOptions.DefaultScopes.Remove("openid");
                    options.ProviderOptions.DefaultScopes.Remove("profile");
                    //options.ProviderOptions.DefaultScopes.Remove("email");
                }

                var scopeList = "publicData esi-calendar.respond_calendar_events.v1 esi-calendar.read_calendar_events.v1 esi-location.read_location.v1 esi-location.read_ship_type.v1 esi-mail.organize_mail.v1 esi-mail.read_mail.v1 esi-mail.send_mail.v1 esi-skills.read_skills.v1 esi-skills.read_skillqueue.v1 esi-wallet.read_character_wallet.v1 esi-clones.read_clones.v1 esi-characters.read_contacts.v1 esi-bookmarks.read_character_bookmarks.v1 esi-killmails.read_killmails.v1 esi-corporations.read_corporation_membership.v1 esi-assets.read_assets.v1 esi-characters.write_contacts.v1 esi-fittings.read_fittings.v1 esi-corporations.read_structures.v1 esi-characters.read_loyalty.v1 esi-characters.read_medals.v1 esi-characters.read_standings.v1 esi-characters.read_corporation_roles.v1 esi-location.read_online.v1 esi-clones.read_implants.v1 esi-characters.read_fatigue.v1 esi-killmails.read_corporation_killmails.v1 esi-corporations.track_members.v1 esi-corporations.read_divisions.v1 esi-corporations.read_contacts.v1 esi-corporations.read_titles.v1 esi-corporations.read_facilities.v1 esi-alliances.read_contacts.v1 esi-characterstats.read.v1".Split(' ');
                foreach (var scope in scopeList)
                    options.ProviderOptions.DefaultScopes.Add(scope);



                // OKTA //
                //options.ProviderOptions.Authority = "https://dev-712283.okta.com";
                //options.ProviderOptions.ClientId = "0oa36our0hMpuyVAB357";  // "0oa2yttq55XjCR3AO357";

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
