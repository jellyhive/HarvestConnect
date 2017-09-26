using System;
using Advise.HarvestConnect.Events;

namespace Advise.HarvestConnect.Options
{
    public class HarvestOptions
    {
        public const string Scheme = "Harvest";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public HarvestEvents Events { get; set; }

        public void Validate()
        {
            if(string.IsNullOrWhiteSpace(ClientId))
            {
                throw new ArgumentNullException(nameof(ClientId));
            }

            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new ArgumentNullException(nameof(ClientSecret));
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new ArgumentNullException(nameof(BaseUrl));
            }
        }
    }
}
