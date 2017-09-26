using Microsoft.AspNetCore.Http;

namespace Advise.HarvestConnect.Events
{
    public class HarvestFailedLoginContext
    {
        public string FailureMessage { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}