using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }     
    }
}
