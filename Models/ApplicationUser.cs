using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassageStudio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName  { get; set; }
        public string? City      { get; set; }

    
        [NotMapped]
        public string? Phone
        {
            get => PhoneNumber;
            set => PhoneNumber = value;
        }
    }
}
