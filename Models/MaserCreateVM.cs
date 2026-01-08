using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

public class MaserCreateVM
{
    [Required] public int IdMaserja { get; set; }
    [Required] public string imeMaserja { get; set; }
    [Required] public string priimekMaserja { get; set; }
    [EmailAddress] public string? eMail { get; set; }
    public string? opis { get; set; }
    [Required] public DateTime datumRojstva { get; set; }

    public IFormFile? SlikaFile { get; set; } // upload
}
