using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class Inscripcion{
   //getters y setters
   [Key]
   public int Id {get; set;}

   [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca apellidos del usuario que sean válidos")]
   public required String ApellidosUsuario {get;set;}

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un DNI válido")]
   public required String Dni {get; set;}

    [Required(ErrorMessage = "Introduzca una fecha de inscripción válida")]
    public required DateTime FechaInscripcion {get; set;}

    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public required MetodoPago MetodoPago {get;set;} // da error ya que no está subida la enumeración MetodoPsgo, pero lo estará

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un nombre de usuario válido")]
    public required String NombreUsuario {get; set;}
    [Range (typeof(decimal), "0", "500", ErrorMessage = "Introduzca un precio total válido")] //he puesto de maximo 500€, para hacerlo realista
    public decimal PrecioTotal {get; set;}
    [Required (ErrorMessage = "El campo de teléfono es obligatorio")]
    [Phone(ErrorMessage = "El formato del teléfono no es correcto")]
    public required String Telefono {get; set;}
    
    // --- AÑADIDO SEGÚN EL DIAGRAMA UML (Sin quitar nada de lo anterior) ---
    [Required(AllowEmptyStrings = false, ErrorMessage = "Los datos de pago son obligatorios")]
    public required string DatosPago { get; set; }

    // Relación con ApplicationUser (Cliente)
    public string? ClienteId { get; set; } // Es string porque hereda de IdentityUser
    public ApplicationUser? Cliente { get; set; }
    
    //RELACIÓN
    //Relación con CompeticionInscripcion (1 - N), ya que podemos inscribirnos a varias competiciones, pero 1 CompeticionInscripcion solo pertenece a 1 inscripcion
    public ICollection<CompeticionInscripcion> CompeticionInscripciones { get; set; } = new List<CompeticionInscripcion>(); //da error, pq no existe la clase CompeticionInscripcion

    // --- AÑADIDO SEGÚN EL DIAGRAMA UML ---
    // Relación con ClaseInscrita (1 - N)
    public ICollection<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

    //constructor vacio
    public Inscripcion(){
        
    }

    //constructor con parámetros (Actualizado con datosPago)
    public Inscripcion (String apellidoUsuario, String dni, DateTime fechaInscripcion,int id, MetodoPago metodoPago, String nombreUsuario, decimal precioTotal, String telefono, string datosPago){
       this.ApellidosUsuario = apellidoUsuario;
       this.Dni = dni;
       this.FechaInscripcion = fechaInscripcion;
       this.Id = id;
       this.MetodoPago = metodoPago;
       this.NombreUsuario = nombreUsuario;
       this.PrecioTotal = precioTotal;
       this.Telefono = telefono;
       
       // Asignación de la propiedad añadida
       this.DatosPago = datosPago;
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