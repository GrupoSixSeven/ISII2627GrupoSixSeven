using System;
using System.Collections.Generic;

namespace AppForSEII.API.DTOs
{
    public class ReservaForCreateDTO
    {
        public ReservaForCreateDTO(string nombreCliente, string apellidos, string dni, string metodoPago, decimal precioTotal, IList<ReservaItemDTO> pistas)
        {
            NombreCliente = nombreCliente;
            Apellidos = apellidos;
            Dni = dni;
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Pistas = pistas;
        }

        public ReservaForCreateDTO(string nombreCliente, string apellidos, string dni, string metodoPago, DateTime fechaReserva, decimal precioTotal, IList<ReservaItemDTO> pistas)
        {
            NombreCliente = nombreCliente;
            Apellidos = apellidos;
            Dni = dni;
            MetodoPago = metodoPago;
            FechaReserva = fechaReserva;
            PrecioTotal = precioTotal;
            Pistas = pistas;
        }

        public string NombreCliente { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public DateTime FechaReserva { get; set; }
        public decimal PrecioTotal { get; set; }
        public IList<ReservaItemDTO> Pistas { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dTO &&
                   NombreCliente == dTO.NombreCliente &&
                   Apellidos == dTO.Apellidos &&
                   Dni == dTO.Dni &&
                   MetodoPago == dTO.MetodoPago &&
                   FechaReserva == dTO.FechaReserva &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<IList<ReservaItemDTO>>.Default.Equals(Pistas, dTO.Pistas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, Apellidos, Dni, MetodoPago, FechaReserva, PrecioTotal, Pistas);
        }
    }
}