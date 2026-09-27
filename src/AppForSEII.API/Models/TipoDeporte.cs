using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace AppForSEII.API.Models;

public class TipoDeporte
{
    [Key] //esto es para decir que el "id" es la clave primaria
    
    //getters y setters
    public int Id { get; set; }
    public string? Nombre { get; set; }

    //Constructor vacio (lo hago, por si queremos hacer pruebas, pero creo que no será necesario)
    public TipoDeporte(){
    
    }

    // Contructor con los atributos de la clase, con control de nulos sobre el atributo nombre
    public TipoDeporte(int Id, string Nombre)
    {
        this.Id = Id;
        if (Nombre != null)
        {
              this.Nombre = Nombre;
        } 
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