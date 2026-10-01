using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class TipoDeporte
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca el nombre de un tipo de deporte válido")]
    public required string Nombre { get; set; }

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Introduzca una competición válida")]
    public required Competicion Competicion { get; set; } 

    [Required(ErrorMessage = "Introduzca una pista válida")]
    public required Pista Pista { get; set; }

    public ICollection<Competicion> Competiciones { get; set; } = new List<Competicion>();   
    public ICollection<Material> Materiales { get; set; } = new List<Material>();
    public ICollection<ClaseDeportiva> ClasesDeportivas { get; set; } = new List<ClaseDeportiva>();

    public TipoDeporte()
    {
    }
    
    public override bool Equals(object? otro)
    { 
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false; 
        
        TipoDeporte otro_tipodeporte = (TipoDeporte)otro;

        if (otro_tipodeporte.Id != this.Id) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}