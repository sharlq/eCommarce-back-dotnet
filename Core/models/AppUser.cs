using Microsoft.AspNetCore.Identity;

namespace Core.models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
