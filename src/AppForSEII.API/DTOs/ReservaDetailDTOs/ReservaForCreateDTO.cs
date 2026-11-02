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

        public ReservaForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string email, string telefono, MetodoPago metodoPago, decimal precioTotal, IList<ReservaItemDTO> pistas, string datosPago, DateTime fechaReserva)
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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreUsuario { get; set; } = "";

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string ApellidosUsuario { get; set; } = "";

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required(ErrorMessage = "El precio total es obligatorio")]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(ErrorMessage = "Debe seleccionar al menos una pista")]
        public IList<ReservaItemDTO> Pistas { get; set; }

        [Required(ErrorMessage = "Los datos de pago son obligatorios")]
        public string DatosPago { get; set; } = "";

        [Required(ErrorMessage = "La fecha de reserva es obligatoria")]
        public DateTime FechaReserva { get; set; } = DateTime.Now;

        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Email == dTO.Email &&
                   Telefono == dTO.Telefono &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaReserva == dTO.FechaReserva &&
                   DatosPago == dTO.DatosPago;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(NombreUsuario);
            hash.Add(ApellidosUsuario);
            hash.Add(Dni);
            hash.Add(Email);
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