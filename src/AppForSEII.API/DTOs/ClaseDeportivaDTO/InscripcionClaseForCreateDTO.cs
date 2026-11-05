using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ClaseDeportivaDTO
{
    public class InscripcionClaseForCreateDTO
    {
        public InscripcionClaseForCreateDTO()
        {
            ClasesInscritas = new List<InscripcionItemDTO>();
        }

        public InscripcionClaseForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string correo,
            string telefono, MetodoPago metodoPago, decimal precioTotal, IList<InscripcionItemDTO> clasesInscritas,
            string datosPago, DateTime fechaInscripcion)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            Dni = dni ?? throw new ArgumentNullException(nameof(dni));
            Correo = correo ?? throw new ArgumentNullException(nameof(correo));
            Telefono = telefono ?? throw new ArgumentNullException(nameof(telefono));
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            ClasesInscritas = clasesInscritas ?? throw new ArgumentNullException(nameof(clasesInscritas));
            DatosPago = datosPago ?? throw new ArgumentNullException(nameof(datosPago));
            FechaInscripcion = fechaInscripcion;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres")]
        public string ApellidosUsuario { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El DNI tiene que tener una longitud de 9 caracteres")]
        public string Dni { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono de contacto es obligatorio")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        public string Telefono { get; set; } = string.Empty;

        // También identifica al cliente en la BD (el controlador lo busca por correo)
        [Required(AllowEmptyStrings = false, ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(50, ErrorMessage = "El email no puede superar los 50 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Los datos del pago son obligatorios")]
        [StringLength(50, ErrorMessage = "Los datos del pago no pueden superar los 50 caracteres")]
        public string DatosPago { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inscripción es obligatoria")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe seleccionar al menos una clase deportiva")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una clase deportiva")]
        public IList<InscripcionItemDTO> ClasesInscritas { get; set; } = new List<InscripcionItemDTO>();

       
        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return Math.Abs((date1 - date2).TotalMinutes) < 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseForCreateDTO dTO &&
                   CompareDate(FechaInscripcion, dTO.FechaInscripcion) &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Correo == dTO.Correo &&
                   Telefono == dTO.Telefono &&
                   DatosPago == dTO.DatosPago &&
                   MetodoPago == dTO.MetodoPago &&
                   ClasesInscritas.SequenceEqual(dTO.ClasesInscritas) &&
                   PrecioTotal == dTO.PrecioTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dni, Correo, MetodoPago, DatosPago, PrecioTotal);
        }
    }
}