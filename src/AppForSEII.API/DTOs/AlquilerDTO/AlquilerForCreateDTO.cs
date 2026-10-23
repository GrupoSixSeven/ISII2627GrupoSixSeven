namespace AppForSEII.API.DTOs.AlquilerDTOs;

public class AlquilerForCreateDTO
{
    public AlquilerForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<AlquilerItemDTO> materiales)
    {

        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
        Materiales = materiales;
    }

    public AlquilerForCreateDTO(MetodoPago metodoPago, DateTime fechaAlquiler, decimal precioTotal, IList<AlquilerItemDTO> materiales)
    {

        MetodoPago = metodoPago;
        FechaAlquiler = fechaAlquiler;
        PrecioTotal = precioTotal;
        Materiales = materiales;
    }


    public MetodoPago MetodoPago { get; set; }= MetodoPago.Efectivo;
    public DateTime FechaAlquiler { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El precio total es obligatorio.")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
    public decimal PrecioTotal { get; set; }= 1;

    [Required(ErrorMessage = "Los materiales son obligatorios.")]
    [StringLength(500, ErrorMessage = "Los materiales no pueden exceder los 500 caracteres.")]
    public IList<AlquilerItemDTO> Materiales { get; set; } = new List<AlquilerItemDTO>();

    public override bool Equals(object? obj)
    {
        return obj is AlquilerForCreateDTO dTO &&

               MetodoPago == dTO.MetodoPago &&
               FechaAlquiler == dTO.FechaAlquiler &&
               PrecioTotal == dTO.PrecioTotal &&
               EqualityComparer<IList<AlquilerItemDTO>>.Default.Equals(Materiales, dTO.Materiales);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine( MetodoPago, FechaAlquiler, PrecioTotal, Materiales);
    }
}