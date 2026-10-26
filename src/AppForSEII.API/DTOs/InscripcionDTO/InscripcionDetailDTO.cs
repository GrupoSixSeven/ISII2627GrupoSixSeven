using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.InscripcionDTO
{
    public class InscripcionDetailDTO : InscripcionForCreateDTO
    {
public InscripcionDetailDTO(int idInscripcion, DateTime fechaInscripcion, MetodoPago metodoPago, decimal precioTotal,
    string nombreUsuario, string apellidosUsuario, string dni, string telefono, string datosPago,
    IList<InscripcionItemDTO> clasesInscritas)
    : base(metodoPago, fechaInscripcion, precioTotal, nombreUsuario, apellidosUsuario, dni, telefono, datosPago, clasesInscritas)
{
    IdInscripcion = idInscripcion;
    FechaInscripcion = fechaInscripcion;
}

        public int IdInscripcion { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public new DateTime FechaInscripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdInscripcion == dTO.IdInscripcion &&
                   FechaInscripcion == dTO.FechaInscripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdInscripcion, FechaInscripcion);
        }
    }
}