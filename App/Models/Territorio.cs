using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class Territorio
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; }
}