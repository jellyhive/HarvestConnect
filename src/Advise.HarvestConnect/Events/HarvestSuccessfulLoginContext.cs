using System;
using System.Security.Claims;
using Advise.HarvestConnect.Options;

namespace Advise.HarvestConnect.Events
{
    public class HarvestSuccessfulLoginContext
    {
        public HarvestWhoAmIResponse WhoAmI { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public TimeSpan? ExpiresIn { get; set; }
    }
}