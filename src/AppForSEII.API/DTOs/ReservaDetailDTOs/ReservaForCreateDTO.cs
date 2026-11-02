using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaForCreateDTO
    {
        public ReservaForCreateDTO()
        {
            Pistas = new List<ReservaItemDTO>();
            Email = string.Empty;
            DatosPago = string.Empty;
        }

        public ReservaForCreateDTO(
            MetodoPago metodoPago,
            decimal precioTotal,
            IList<ReservaItemDTO> pistas,
            DateTime fechaReserva,
            string email,
            string? datosPago)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Pistas = pistas;
            FechaReserva = fechaReserva;
            Email = email;
            DatosPago = datosPago;
        }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(ErrorMessage = "El campo fecha de reserva es obligatorio")]
        public DateTime FechaReserva { get; set; } = DateTime.Now;

        [Required]
        [MinLength(1, ErrorMessage = "Debe haber al menos 1 pista reservada")]
        public IList<ReservaItemDTO> Pistas { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; }

        public string? DatosPago { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dto &&
                   MetodoPago == dto.MetodoPago &&
                   PrecioTotal == dto.PrecioTotal &&
                   FechaReserva == dto.FechaReserva &&
                   Email == dto.Email &&
                   DatosPago == dto.DatosPago &&
                   Pistas.SequenceEqual(dto.Pistas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                FechaReserva,
                MetodoPago,
                PrecioTotal,
                Email,
                DatosPago,
                Pistas.Count
            );
        }
    }
}