// ReSharper disable InconsistentNaming
namespace Advise.HarvestConnect.Options
{
    public class Modules
    {
        public bool expenses { get; set; }
        public bool invoices { get; set; }
        public bool estimates { get; set; }
        public bool approval { get; set; }
        public bool team { get; set; }
    }

    public class Company
    {
        public string base_uri { get; set; }
        public string full_domain { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public string week_start_day { get; set; }
        public string time_format { get; set; }
        public string clock { get; set; }
        public string decimal_symbol { get; set; }
        public string color_scheme { get; set; }
        public Modules modules { get; set; }
        public string thousands_separator { get; set; }
        public string plan_type { get; set; }
    }

    public class ProjectManager
    {
        public bool is_project_manager { get; set; }
        public bool can_see_rates { get; set; }
        public bool can_create_projects { get; set; }
        public bool can_create_invoices { get; set; }
    }

    public class User
    {
        public string timezone { get; set; }
        public string timezone_identifier { get; set; }
        public int timezone_utc_offset { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        public bool admin { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar_url { get; set; }
        public ProjectManager project_manager { get; set; }
        public bool timestamp_timers { get; set; }
    }

    public class HarvestWhoAmIResponse
    {
        public Company company { get; set; }
        public User user { get; set; }
    }
}
