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

        public ReservaForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string email,
            string telefono, MetodoPago metodoPago, decimal precioTotal, IList<ReservaItemDTO> pistas,
            string datosPago, DateTime fechaReserva)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Dni = dni;
            Email = email;
            Telefono = telefono;
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Pistas = pistas;
            DatosPago = datosPago;
            FechaReserva = fechaReserva;
        }

        public string NombreUsuario { get; set; } = "";

        public string ApellidosUsuario { get; set; } = "";

        public string Dni { get; set; } = "";

        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required(ErrorMessage = "El precio total es obligatorio")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(ErrorMessage = "La fecha de reserva es obligatoria")]
        public DateTime FechaReserva { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Los datos de pago son obligatorios")]
        public string DatosPago { get; set; } = "";

        [Required(ErrorMessage = "Debe seleccionar al menos una pista")]
        [MinLength(1, ErrorMessage = "Debe haber al menos 1 pista reservada")]
        public IList<ReservaItemDTO> Pistas { get; set; } = new List<ReservaItemDTO>();

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return Math.Abs((date1 - date2).TotalMinutes) < 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Telefono == dTO.Telefono &&
                   Email == dTO.Email &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   CompareDate(FechaReserva, dTO.FechaReserva) &&
                   DatosPago == dTO.DatosPago &&
                   Pistas.SequenceEqual(dTO.Pistas);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Email);
            hash.Add(Dni);
            hash.Add(FechaReserva);
            hash.Add(DatosPago);
            hash.Add(MetodoPago);
            hash.Add(PrecioTotal);
            foreach (var pista in Pistas)
                hash.Add(pista);
            return hash.ToHashCode();
        }
    }
}