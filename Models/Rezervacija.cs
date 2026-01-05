using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace MassageStudio.Models
{
    [Table("rezervacija")]
    public class Rezervacija
    {
        [Key]
        [Column("idRez")]
        public int IdRez { get; set; }

        [Column("ime")]
        public string? Ime { get; set; }

        [Column("priimek")]
        public string? Priimek { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("telSt")]
        public string? Telefon { get; set; }

        [Column("datum")]
        public DateTime Datum { get; set; }

        [Column("cas_prihoda")]
        public TimeSpan CasPrihoda { get; set; }

        [Column("maserID")]
        public int? MaserID { get; set; }

        [Column("masazaID")]
        public int MasazaID { get; set; }

        [Column("trajanje")]
        public int? Trajanje { get; set; }

        [Column("stRacuna")]
        public int? StRacuna { get; set; }
        [NotMapped]
        public string UserId { get; set; }
        [ForeignKey("MaserID")]
        [ValidateNever] 
        public virtual Maser? Maser { get; set; }

        [ForeignKey("MasazaID")]
        [ValidateNever] 
        public virtual Masaza? Masaza { get; set; }



    }
}
