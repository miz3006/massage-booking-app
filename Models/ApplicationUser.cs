using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassageStudio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName  { get; set; }
        public string? City      { get; set; }

        // Alias na PhoneNumber, da view-i lahko uporabljajo .Phone
        // in EF NE išče stolpca "Phone" v bazi
        [NotMapped]
        public string? Phone
        {
            get => PhoneNumber;
            set => PhoneNumber = value;
        }
    }
}
