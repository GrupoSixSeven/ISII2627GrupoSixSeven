using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class Inscripcion{
   //getters y setters
   [Key]
   public int Id {get; set;}


    [Required(ErrorMessage = "Introduzca una fecha de inscripción válida")]
    public required DateTime FechaInscripcion {get; set;}

    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public required MetodoPago MetodoPago {get;set;} // da error ya que no está subida la enumeración MetodoPsgo, pero lo estará

    [Range (typeof(decimal), "0", "500", ErrorMessage = "Introduzca un precio total válido")] //he puesto de maximo 500€, para hacerlo realista
    public decimal PrecioTotal {get; set;}

    // Relación con ApplicationUser (Cliente)
    public string? ClienteId { get; set; } // Es string porque hereda de IdentityUser
    public ApplicationUser? Cliente { get; set; }
    
    //RELACIÓN
    //Relación con CompeticionInscripcion (1 - N), ya que podemos inscribirnos a varias competiciones, pero 1 CompeticionInscripcion solo pertenece a 1 inscripcion
    public ICollection<CompeticionInscrita> CompeticionInscrita { get; set; } = new List<CompeticionInscrita>(); //da error, pq no existe la clase CompeticionInscripcion

    // Relación con ClaseInscrita (1 - N)
    public ICollection<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

    //constructor vacio
    public Inscripcion(){
        
    }

    public Inscripcion (DateTime fechaInscripcion,int id, MetodoPago metodoPago, decimal precioTotal){
       this.FechaInscripcion = fechaInscripcion;
       this.Id = id;
       this.MetodoPago = metodoPago;
       this.PrecioTotal = precioTotal;
    }

    public override bool Equals(object? otro){
       if (otro ==  null) return false;
       if (otro.GetType() != this.GetType()) return false;
       Inscripcion otro_inscripcion = (Inscripcion) otro;
       if (otro_inscripcion.Id != this.Id) return false;
       return true;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}