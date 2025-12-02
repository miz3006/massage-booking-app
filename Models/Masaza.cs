using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassageStudio.Models
{
    [Table("masaze")]
    public class Masaza
    {
        [Key]
        [Column("idMasaze")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  
        public int IdMasaze { get; set; }

        [Column("nazivMasaze")]
        public string NazivMasaze { get; set; } = "";

        [Column("trajanjeMasaze")]
        public int? TrajanjeMasaze { get; set; }

        [Column("opis")]
        public string? Opis { get; set; }

        [Column("cena")]
        public decimal? Cena { get; set; }

        [Column("kratica")]
        public string? Kratica { get; set; }

        [Column("vrsta")]
        public string? Vrsta { get; set; }
    }
}
