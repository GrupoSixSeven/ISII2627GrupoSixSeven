namespace AppForSEII.API.Models;

public class Alquiler{
    [Key]
    public int IdAlquiler { get; set; }

    public required String NombreUsuario {get; set;}
    [Range (typeof(decimal), "0", "500", ErrorMessage = "Introduzca un precio total válido")]

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca apellidos del usuario que sean válidos")]
    public required String ApellidosUsuario {get;set;}

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un DNI válido")]
    public required String Dni {get; set;}

    [Required (ErrorMessage = "El campo de teléfono es obligatorio")]
    [Phone(ErrorMessage = "El formato del teléfono no es correcto")]
    public required String Telefono {get; set;}

    
    [Required(ErrorMessage = "Error, introduzca una fecha válida")]
    public required DateTime FechaAlquiler { get; set; }
    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public required MetodoPago MetodoPago { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo")]
    public required double PrecioTotal { get; set; }

    public virtual ICollection<MaterialAlquilado> MaterialesAlquilados { get; set; } = new List<MaterialAlquilado>();
    //Constructor vacío
    public Alquiler()
    {
        
    }    
    //Constructor con parámetros
    public Alquiler(int idAlquiler, DateTime fechaAlquiler, MetodoPago metodoPago, double precioTotal){
        this.IdAlquiler = idAlquiler;
        this.FechaAlquiler = fechaAlquiler;
        this.MetodoPago = metodoPago;
        this.PrecioTotal = precioTotal;
    }
    //Equals method
    public override bool Equals(object? otro)
    { 
       if (otro == null || otro.GetType() != this.GetType()) return false;
        Alquiler otro_alquiler = (Alquiler)otro;
        return otro_alquiler.IdAlquiler == this.IdAlquiler;
    }
    //GetHashCode method
    public override int GetHashCode()
    {
        return this.IdAlquiler.GetHashCode();
    }




}
