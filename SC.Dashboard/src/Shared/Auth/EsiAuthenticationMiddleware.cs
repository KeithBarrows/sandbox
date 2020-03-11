using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using SC.Dashboard.Shared.Config;
using Microsoft.Extensions.Configuration;

/*********************************************************************************************************************
 * https://docs.microsoft.com/en-us/aspnet/core/security/blazor/webassembly/?view=aspnetcore-3.1                     *
 * https://docs.microsoft.com/en-us/aspnet/core/security/blazor/webassembly/additional-scenarios?view=aspnetcore-3.1 *
 ********************************************************************************************************************/


namespace SC.Dashboard.Shared.Auth
{
    public class EsiAuthenticationMiddleware
    {
        public static void SetOAuth2Options(OAuthOptions options, IConfiguration configuration)
        {
            var config = new EveAuth(configuration);

            // TODO:  Move to azure safe...
            options.ClientId = "da968792109f42afb459efd51a0655c4";
            options.ClientSecret = "vbrchRcfef3ttXPK0RLjTTlwKDfiuy6s68TZm9Z2";

            options.AuthorizationEndpoint = config.AuthorizationEndpoint;
            options.TokenEndpoint = config.TokenEndpoint;
            options.UserInformationEndpoint = config.UserInformationEndpoint;
            options.CallbackPath = new PathString(config.CallbackPath);

            // Set the scopes you want to request
            var scopeList = config.Scopes.Split(' ');
            foreach(var scope in scopeList)
                options.Scope.Add(scope);

            // Define how to map returned user data to claims
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            //options.ClaimActions.MapJsonKey(ClaimTypes.Email, "EmailAddress", ClaimValueTypes.Email);  // TODO: does this exist?

            // Register to events
            options.Events = new OAuthEvents
            {
                // After OAuth2 has authenticated the user
                OnCreatingTicket = async context =>
                {
                    // Create the request message to get user data via the backchannel
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Query for user data via backchannel
                    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    // Parse user data into an object
                    var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Store the received authentication token somewhere. In a cookie for example
                    context.HttpContext.Response.Cookies.Append("token", context.AccessToken);

                    // Execute defined mapping action to create the claims from the received user object
                    context.RunClaimActions(JObject.FromObject(user));
                },
                OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
                    return Task.FromResult(0);
                }
            };
        }
    }
}