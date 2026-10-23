using System;
using System.Collections.Generic;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO() : base()
        {
        }

        public ReservaDetailDTO(int idReserva, string nombreCliente, string apellidos, string dni, 
            string metodoPago, DateTime fechaReserva, decimal precioTotal, IList<ReservaItemDTO> pistas)
            : base(nombreCliente, apellidos, dni, metodoPago, fechaReserva, precioTotal, pistas)
        {
            IdReserva = idReserva;
        }

        public int IdReserva { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaDetailDTO dTO &&
                   base.Equals(obj) && 
                   IdReserva == dTO.IdReserva;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdReserva);
        }
    }
}