using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassageStudio.Models
{
    [Table("maser")]
    public class Maser
    {
        [Key]
        [Column("idMaserja")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  
        public int IdMaserja { get; set; }

        [Column("ime")]
        public string imeMaserja { get; set; }

        [Column("Priimek")]
        public string priimekMaserja { get; set; }

        [Column("Email")]
        public string? eMail { get; set; }

        [Column("Opis")]
        public string? opis { get; set; }

        [Column("datumRojstva")]
        public DateTime datumRojstva { get; set; }
        [Column("slika")]
        public string? slika { get; set; } // shrani pot, npr. "/images/maser1.jpg"

    }
}
