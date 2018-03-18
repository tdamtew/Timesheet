using Microsoft.AspNetCore.Identity;

namespace sbpc.Timesheet.Data.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public bool TempPassword { get; set; }
        public string Role { get; set; }
    }
}
