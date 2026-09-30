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

    //RELACIONES CU-1
    // Relación con Competicion (1 - N) (Un tipo de deporte puede tener muchas competiciones asociadas, pero una competición es solo de un deporte)
    public ICollection<Competicion> Competiciones { get; set; } = new List<Competicion>();    
    //Constructor vacio 
    public TipoDeporte(){
    
    }

    // Contructor con los atributos de la clase, con control de nulos sobre el atributo nombre
    public TipoDeporte(int Id, string Nombre)
    {
        this.Id = Id;
        this.Nombre = Nombre;
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