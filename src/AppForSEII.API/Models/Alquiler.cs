namespace AppForSEII.API.Models;

public class Alquiler{
    [Key]
    public int IdAlquiler { get; set; }
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
