using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;
 
namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaForCreateDTO
    {
        public ReservaForCreateDTO()
        {
            Pistas = new List<ReservaItemDTO>();
        }
 
        public ReservaForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<ReservaItemDTO> pistas, DateTime fechaReserva)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Pistas = pistas;
            FechaReserva = fechaReserva;
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
        public IList<ReservaItemDTO> Pistas { get; set; } = new List<ReservaItemDTO>();
 
        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaReserva == dTO.FechaReserva &&
                   Pistas == dTO.Pistas;
        }
 
        public override int GetHashCode()
        {
            return HashCode.Combine(FechaReserva, MetodoPago, PrecioTotal, Pistas.Count);
        }
    }
}