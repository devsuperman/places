using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class Predicacion
{
    public int Id { get; set; }

    
    [Required, DataType(DataType.Date)]
    public DateTime Fecha { get; set; }


    [Required]
    public string Dirigente { get; set; } = string.Empty;


    [Required]
    public string Manzanas { get; set; } = string.Empty;


    public string Observacion { get; set; } = string.Empty;


    [Required, Display(Name="Territorio")]
    public int TerritorioId { get; set; }
    public Territorio Territorio { get; set; }
}