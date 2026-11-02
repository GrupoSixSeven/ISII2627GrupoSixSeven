using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO(int idReserva, string nombreUsuario, string apellidosUsuario, string dni, string email,
            string telefono, MetodoPago metodoPago, IList<ReservaItemDTO> pistas, string datosPago, DateTime fechaReserva)
            : base(nombreUsuario, apellidosUsuario, dni, email, telefono, metodoPago, pistas, datosPago, fechaReserva)
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