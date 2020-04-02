using Microsoft.AspNetCore.Identity;

namespace VisualAlgorithms.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
    }
}
