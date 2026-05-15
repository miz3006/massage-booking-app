using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassageStudio.Models
{
    [Table("obvestilo")]
    public class Obvestilo
    {
        [Key]
        [Column("idObvestila")]
        public int IdObvestila { get; set; }

        [Required]
        [Column("naslov")]
        public string Naslov { get; set; } = "";

        [Required]
        [Column("vsebina", TypeName = "text")]
        public string Vsebina { get; set; } = "";

        [Required]
        [Column("datumObjave")]
        public DateTime DatumObjave { get; set; }

        [Column("UserId")]
        public string? UserId { get; set; } // id uporabnika, ki je poslal obvestilo
        public ApplicationUser? User { get; set; }
    }
}
