# Harvest connect
Helper for authenticating via Harvest (https://www.getharvest.com)

## How to use?

1. Register a OAuth client over at https://\<YOURACCOUNT\>.harvestapp.com/oauth2_clients
1. Install clone this repo and include it in your project.
1. Use the code below to set it up.

```csharp
 public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = HarvestOptions.Scheme;
                    options.DefaultChallengeScheme = HarvestOptions.Scheme;
                    options.DefaultSignInScheme = "auth";
                    options.DefaultSignOutScheme = "auth";
                })
                .AddCookie("auth", options =>
                {
                    options.LogoutPath = "/logout";
                })
                .AddHarvest(options =>
                {
                    options.ClientId = "<provided by Harvest>";
                    options.ClientSecret = "<provided by harvest>";
                    options.BaseUrl = "<base url of your site, e.g. http://localhost:13370>";
                    options.Events = new HarvestEvents
                    {
                        // Called on succesful login
                        OnSuccessfulLogin = context =>
                        {
                            // Add the claims your app requires...
                            context.Identity.AddClaim(new Claim(context.Identity.NameClaimType,
                                context.WhoAmI.user.first_name));

                            if (context.WhoAmI.user.admin)
                            {
                                context.Identity.AddClaim(new Claim(context.Identity.RoleClaimType, "Administrator"));
                            }

                            context.Identity.AddClaim(new Claim("Email", context.WhoAmI.user.email));

                            // ... or do other stuff with the data received, e.g.:
                            // await _db.UpdateAvatar(context.WhoAmI.user.id, context.WhoAmI.user.avatar_url);

                            return Task.CompletedTask;
                        },
                        // Called when login fails or the user denies the login attempt
                        OnFailedLogin = async context =>
                        {
                            await context.HttpContext.SignOutAsync();
                            context.HttpContext.Response.Redirect("/home/logout?message=" + WebUtility.UrlEncode(context.FailureMessage));
                        }
                    };
                });
            services.AddMvc();
        }

```