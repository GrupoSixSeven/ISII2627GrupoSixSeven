using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO(int idReserva, MetodoPago metodoPago, DateTime fechaReserva, decimal precioTotal,
            IList<ReservaItemDTO> pistas, string datosPago,
            string nombreUsuario, string apellidosUsuario, string telefono, string dni, string email)
            : base(nombreUsuario, apellidosUsuario, dni, email, telefono, metodoPago, precioTotal, pistas, datosPago, fechaReserva)
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