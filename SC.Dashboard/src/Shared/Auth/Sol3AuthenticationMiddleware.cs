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

namespace SC.Dashboard.Shared.Auth
{
    public class Sol3AuthenticationMiddleware
    {
        public static void SetOAuth2Options(OAuthOptions options, IConfiguration configuration)
        {
            var config = new EveAuth(configuration);

            // TODO:  Move to azure safe...
            options.ClientId = "xxxxxxxxxxxxxxxxxx";
            options.ClientSecret = "xxxxxxxxxxxxxxxxxx";

            options.AuthorizationEndpoint = config.AuthorizationEndpoint;
            options.TokenEndpoint = config.TokenEndpoint;
            options.UserInformationEndpoint = config.UserInformationEndpoint;
            options.CallbackPath = new PathString(config.CallbackPath);

            // Set the scopes you want to request
            var scopes = config.Scopes;
            var scopeList = scopes.Split(' ');
            foreach(var scope in scopeList)
                options.Scope.Add(scope);

            //options.Scope.Add("user-read");
            //options.Scope.Add("user-write");

            // Define how to map returned user data to claims
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "EmailAddress", ClaimValueTypes.Email);  // TODO: does this exist?

            // Register to events
            options.Events = new OAuthEvents
            {
                // After OAuth2 has authenticated the user
                OnCreatingTicket = async context =>
                {
                    // Create the request message to get user data via the backchannel
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    //// Additional header if needed. Here's an example to go through Azure API Management 
                    //request.Headers.Add("Ocp-Apim-Subscription-Key", "<given key>");

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