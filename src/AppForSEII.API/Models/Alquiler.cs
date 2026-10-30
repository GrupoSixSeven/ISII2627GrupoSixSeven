namespace AppForSEII.API.Models;

public class Alquiler{
    [Key]
    public int IdAlquiler { get; set; }
    
    [Required(ErrorMessage = "Error, introduzca una fecha válida")]
    public required DateTime FechaAlquiler { get; set; }
    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public required MetodoPago MetodoPago { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio total debe ser un número positivo")]
    public required decimal PrecioTotal { get; set; }

    public string? ClienteId { get; set; } 
    public ApplicationUser? Cliente { get; set; }

    public virtual ICollection<MaterialAlquilado> MaterialesAlquilados { get; set; } = new List<MaterialAlquilado>();
    //Constructor vacío
    public Alquiler()
    {
        
    }    
    //Constructor con parámetros
    //Añadir parametros
    public Alquiler(int idAlquiler, DateTime fechaAlquiler, MetodoPago metodoPago, decimal precioTotal){
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
