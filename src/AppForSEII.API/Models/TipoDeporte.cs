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

 [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca la descripcion de un tipo de deporte válido")] 
    public required string Descripcion { get; set; }

   
    
    //RELACIÓN CU-4
    //Relación con Pista (1 - N)
    public ICollection<Pista> Pistas { get; set; } = new List<Pista>();
    
     public ICollection<Competicion> Competiciones { get; set; } = new List<Competicion>();   
    
    public ICollection<Material> Materiales { get; set; } = new List<Material>();

    public ICollection<ClaseDeportiva> ClasesDeportivas { get; set; } = new List<ClaseDeportiva>();

    //Constructor vacio 
    public TipoDeporte(){
    
    }

    // Contructor con los atributos de la clase, añadiendo descripcion como parametro opcional
    public TipoDeporte(int Id, string Nombre, string nombreTipoDeporte, string descripcion)
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