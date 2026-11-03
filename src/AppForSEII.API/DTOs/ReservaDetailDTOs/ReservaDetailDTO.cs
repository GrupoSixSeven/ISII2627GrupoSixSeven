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
            : base(email, metodoPago, precioTotal, pistas, datosPago, fechaReserva)
        {
            IdReserva = idReserva;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Telefono = telefono;
            Dni = dni;
        }

        public int IdReserva { get; set; }

        public string NombreUsuario { get; set; } = "";

        public string ApellidosUsuario { get; set; } = "";

        public string Telefono { get; set; } = "";

        public string Dni { get; set; } = "";

        public override bool Equals(object? obj)
        {
            return obj is ReservaDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdReserva == dTO.IdReserva &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Telefono == dTO.Telefono &&
                   Dni == dTO.Dni;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdReserva, Dni);
        }
    }
}