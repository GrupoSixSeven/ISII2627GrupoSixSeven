using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaForCreateDTO
    {
        public ReservaForCreateDTO()
        {
            Pistas = new List<ReservaItemDTO>();
        }

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

        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string NombreCliente { get; set; } = "";

        [StringLength(100, ErrorMessage = "Los apellidos no pueden tener más de 100 caracteres")]
        public string Apellidos { get; set; } = "";

        [StringLength(9, ErrorMessage = "El DNI debe tener 9 caracteres")]
        public string Dni { get; set; } = "";

        [StringLength(50, ErrorMessage = "El método de pago no puede tener más de 50 caracteres")]
        public string MetodoPago { get; set; } = "";

        [Required]
        public DateTime FechaReserva { get; set; } = DateTime.Now;

        [Required]
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