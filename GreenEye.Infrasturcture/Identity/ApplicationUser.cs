using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GreenEye.Infrasturcture.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Address { get; set; }
    }
}
