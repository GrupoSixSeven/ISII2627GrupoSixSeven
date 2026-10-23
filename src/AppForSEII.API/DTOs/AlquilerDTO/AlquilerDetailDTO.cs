
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
        public AlquilerDetailDTO(int idAlquiler, DateTime fechaAlquiler, MetodoPago metodoPago, 
            IList<AlquilerItemDTO> materiales)
            : base(metodoPago, fechaAlquiler, 0, materiales)
        {
            IdAlquiler = idAlquiler;
            FechaAlquiler = fechaAlquiler;
        }

        public int IdAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime FechaAlquiler { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dTO &&
                   base.Equals(obj) && // Llama a la comparación de las propiedades de la clase padre
                   IdAlquiler == dTO.IdAlquiler &&
                   FechaAlquiler == dTO.FechaAlquiler;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdAlquiler, FechaAlquiler);
        }
    }
}