using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO(
            int idReserva,
            MetodoPago metodoPago,
            DateTime fechaReserva,
            decimal precioTotal,
            IList<ReservaItemDTO> pistas,
            string nombreUsuario,
            string apellidosUsuario,
            string dni,
            string email,
            string telefono,
            string? datosPago)
            : base(
                metodoPago,
                precioTotal,
                pistas,
                fechaReserva,
                email,
                datosPago)
        {
            IdReserva = idReserva;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Dni = dni;
            Telefono = telefono;
        }

        public int IdReserva { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string ApellidosUsuario { get; set; } = string.Empty;

        public string Dni { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is ReservaDetailDTO dto &&
                   base.Equals(dto) &&
                   IdReserva == dto.IdReserva &&
                   NombreUsuario == dto.NombreUsuario &&
                   ApellidosUsuario == dto.ApellidosUsuario &&
                   Dni == dto.Dni &&
                   Telefono == dto.Telefono;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                base.GetHashCode(),
                IdReserva,
                NombreUsuario,
                ApellidosUsuario,
                Dni,
                Telefono
            );
        }
    }
}