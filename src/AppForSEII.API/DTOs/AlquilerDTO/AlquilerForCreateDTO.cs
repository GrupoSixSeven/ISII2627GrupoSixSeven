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


    public MetodoPago MetodoPago { get; set; }
    public DateTime FechaAlquiler { get; set; }
    public decimal PrecioTotal { get; set; }
    public IList<AlquilerItemDTO> Materiales { get; set; }

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