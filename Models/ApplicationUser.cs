using Microsoft.AspNetCore.Identity;

namespace HealthObserver.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Pesel { get; set; }

        public List<string>? Roles { get; set; }
	}
}
