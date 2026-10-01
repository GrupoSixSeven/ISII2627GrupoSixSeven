using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class TipoDeporte
{
    [Key] //esto es para decir que el "id" es la clave primaria
    
    //getters y setters
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca el nombre de un tipo de deporte válido")]
    public required string Nombre { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca el nombre de un tipo de deporte válido")] 
    public required string NombreTipoDeporte { get; set; }


[Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca una descripcion valida")] 
 public required string Descripcion { get; set; }

    //getters y setters de la CU-3
    [Required(ErrorMessage = "Introduzca una competición válida")]
    public required Competicion Competicion { get; set; } 

    [Required(ErrorMessage = "Introduzca una pista válida")]
    public required Pista Pista { get; set; }
    
    //RELACIÓN CU-4
    //Relación con Pista (1 - N)
    public ICollection<Pista> Pistas { get; set; } = new List<Pista>();
    
    //RELACIONES CU-1
    // Relación con Competicion (1 - N) (Un tipo de deporte puede tener muchas competiciones asociadas, pero una competición es solo de un deporte)
    public ICollection<Competicion> Competiciones { get; set; } = new List<Competicion>();   
    
    //RELACIONES CU-3
    // Relación con Materiales  (1 - N) (Un tipo de deporte puede tener varios materiales asociados, pero un material es solo de un deporte)
    public ICollection<Material> Materiales { get; set; } = new List<Material>();

    // ==========================================
    // RELACIONES CU-2 (Clases Deportivas)
    // ==========================================
    // Relación con ClaseDeportiva (1 - N)
    public ICollection<ClaseDeportiva> ClasesDeportivas { get; set; } = new List<ClaseDeportiva>();

    //Constructor vacio 
    public TipoDeporte(){
    
    }

    public TipoDeporte(int Id, string Nombre, string nombreTipoDeporte, string descripcion )
    {
        this.Id = Id;
        this.Nombre = Nombre;
        this.NombreTipoDeporte = nombreTipoDeporte;
        this.Descripcion = descripcion;
    }
    
    //equals, q comprueba q el otro no sea null, que sea del mismo tipo, y q los ids sean los mismos
    public override bool Equals(object? otro)
    { 
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false; 
        
        TipoDeporte otro_tipodeporte = (TipoDeporte) otro;

        if (otro_tipodeporte.Id != this.Id) return false;
        return true;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}