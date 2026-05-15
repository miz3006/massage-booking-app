using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace MassageStudio.Models
{
    [Table("Termin")]
    public class Termin
    {
        [Key]
        public int IdTermin { get; set; }

        [Column("Datum")]
        public DateTime Datum { get; set; }

        [Column("Cas_prihoda")]
        public TimeSpan Cas_prihoda { get; set; }

        [Column("Zaseden")]
        public bool Zaseden { get; set; }
        [Column("MaserID")]
        [Required(ErrorMessage = "Izberi maserja.")]
        public int? MaserID { get; set; }
        [ForeignKey("MaserID")]
        [ValidateNever]
        public virtual Maser Maser { get; set; }
    }
}
