using System;
using System.Threading.Tasks;

namespace Advise.HarvestConnect.Events
{
    public class HarvestEvents
    {
        public Func<HarvestSuccessfulLoginContext, Task> OnSuccessfulLogin { get; set; }
        public Func<HarvestFailedLoginContext, Task> OnFailedLogin { get; set; }
    }
}