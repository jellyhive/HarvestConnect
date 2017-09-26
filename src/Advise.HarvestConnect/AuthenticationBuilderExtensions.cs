using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Advise.HarvestConnect.Events;
using Advise.HarvestConnect.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Advise.HarvestConnect
{
    public static class AuthenticationBuilderExtensions
    {
        public static string AuthenticationScheme => "Harvest";

        private const string AuthorizationEndpointPattern =
                "https://api.harvestapp.com/oauth2/authorize?client_id={0}&redirect_uri={1}signin-harvest&state=optional-csrf-token&response_type=code";

        private const string TokenEndpoint = "https://api.harvestapp.com/oauth2/token";

        private const string UserInformationEndpointPattern = "https://api.harvestapp.com/account/who_am_i?access_token={0}";

        private static HttpClient _client;
        private static HttpClient Client
        {
            get
            {
                if (_client != null)
                {
                    return _client;
                }

                _client = new HttpClient();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return _client;
            }
        }

        public static AuthenticationBuilder AddHarvest(
            this AuthenticationBuilder builder,
            Action<HarvestOptions> configureOptions)
        {
            var harvestOptions = new HarvestOptions();
            configureOptions(harvestOptions);
            harvestOptions.Validate();

            return builder.AddOAuth(AuthenticationScheme, options =>
            {
                options.ClientId = harvestOptions.ClientId;
                options.ClientSecret = harvestOptions.ClientSecret;
                options.AuthorizationEndpoint = string.Format(AuthorizationEndpointPattern, harvestOptions.ClientId, AddTrailingSlashIfMissing(harvestOptions.BaseUrl));
                options.TokenEndpoint = TokenEndpoint;
                options.CallbackPath = new PathString("/signin-harvest");
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var response =
                            await Client.GetAsync(string.Format(UserInformationEndpointPattern, context.AccessToken));
                        var body = await response.Content.ReadAsStringAsync();
                        var whoAmI = JsonConvert.DeserializeObject<HarvestWhoAmIResponse>(body);
                        whoAmI.RawResponse = body;

                        var callback = harvestOptions.Events?.OnSuccessfulLogin;

                        if (callback != null)
                        {
                            await callback.Invoke(new HarvestSuccessfulLoginContext
                            {
                                AccessToken = context.AccessToken,
                                Identity = context.Identity,
                                RefreshToken = context.RefreshToken,
                                WhoAmI = whoAmI,
                                ExpiresIn = context.ExpiresIn
                            });
                        }
                    },
                    OnRemoteFailure = async context =>
                    {
                        var callback = harvestOptions.Events?.OnFailedLogin;

                        if (callback != null)
                        {
                            await callback.Invoke(new HarvestFailedLoginContext
                            {
                                HttpContext = context.HttpContext,
                                FailureMessage = context.Failure.Message
                            });

                            context.HandleResponse();
                        }
                    }
                };
            });
        }

        private static string AddTrailingSlashIfMissing(string url)
        {
            if (url.EndsWith("/"))
            {
                return url;
            }

            return url + "/";
        }
    }
}
