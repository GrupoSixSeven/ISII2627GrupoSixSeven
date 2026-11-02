using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class InscripcionForCreateDTO
    {
        public InscripcionForCreateDTO()
        {
            Competiciones = new List<CompeticionItemDTO>();
        }

        public InscripcionForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string correo,
            string telefono, MetodoPago metodoPago, IList<CompeticionItemDTO> competiciones, DateTime fechaInscripcion)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Dni = dni;
            Correo = correo;
            Telefono = telefono;
            MetodoPago = metodoPago;
            Competiciones = competiciones;
            FechaInscripcion = fechaInscripcion;
        }

        public string NombreUsuario { get; set; } = "";

        public string ApellidosUsuario { get; set; } = "";

        public string Dni { get; set; } = "";

        public string Telefono { get; set; } = "";
        
        public string Correo { get; set; } = "";

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required(ErrorMessage = "El campo fecha de inscripcion es obligatorio")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Required]
        [MinLength(1, ErrorMessage = "Debe haber al menos 1 competición inscrita")]
        public IList<CompeticionItemDTO> Competiciones { get; set; } = new List<CompeticionItemDTO>();

        public decimal PrecioTotal
        {
            get
            {
                return Competiciones.Sum(c => c.Precio);
            }
        }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return Math.Abs((date1 - date2).TotalMinutes) < 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionForCreateDTO dTO &&
                   CompareDate(FechaInscripcion, dTO.FechaInscripcion) &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Correo == dTO.Correo &&
                   Telefono == dTO.Telefono &&
                   MetodoPago == dTO.MetodoPago &&
                   Competiciones.SequenceEqual(dTO.Competiciones) &&
                   PrecioTotal == dTO.PrecioTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaInscripcion, Dni, Correo, MetodoPago);
        }
    }
}