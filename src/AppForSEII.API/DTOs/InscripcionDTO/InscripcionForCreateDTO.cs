using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
using AppForSEII.API.DTOs.CompeticionDTOs;

namespace AppForSEII.API.DTOs.InscripcionDTO
{
    public class InscripcionForCreateDTO
    {
        public InscripcionForCreateDTO()
        {
            Competiciones = new List<CompeticionItemDTO>();
            ClasesInscritas = new List<InscripcionItemDTO>();
        }

        public InscripcionForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<CompeticionItemDTO> competiciones,
            string datosPago, DateTime fechaInscripcion, string nombreUsuario, string apellidosUsuario,
            string dni, string telefono, IList<InscripcionItemDTO> clasesInscritas)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Competiciones = competiciones;
            DatosPago = datosPago;
            FechaInscripcion = fechaInscripcion;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Dni = dni;
            Telefono = telefono;
            ClasesInscritas = clasesInscritas;
        }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;

        [Required(ErrorMessage = "El precio total es obligatorio.")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(ErrorMessage = "El campo DatosPago es obligatorio")]
        public string DatosPago { get; set; } = "";

        [Required(ErrorMessage = "El campo fecha de inscripcion es obligatorio")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public string NombreUsuario { get; set; } = "";
        public string ApellidosUsuario { get; set; } = "";
        public string Dni { get; set; } = "";
        public string Telefono { get; set; } = "";

        public IList<CompeticionItemDTO> Competiciones { get; set; } = new List<CompeticionItemDTO>();
        public IList<InscripcionItemDTO> ClasesInscritas { get; set; } = new List<InscripcionItemDTO>();

        public override bool Equals(object? obj)
        {
            return obj is InscripcionForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInscripcion == dTO.FechaInscripcion &&
                   DatosPago == dTO.DatosPago &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Telefono == dTO.Telefono &&
                   EqualityComparer<IList<CompeticionItemDTO>>.Default.Equals(Competiciones, dTO.Competiciones) &&
                   EqualityComparer<IList<InscripcionItemDTO>>.Default.Equals(ClasesInscritas, dTO.ClasesInscritas);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(MetodoPago);
            hash.Add(FechaInscripcion);
            hash.Add(PrecioTotal);
            hash.Add(DatosPago);
            hash.Add(NombreUsuario);
            hash.Add(ApellidosUsuario);
            hash.Add(Dni);
            hash.Add(Telefono);
            hash.Add(Competiciones);
            hash.Add(ClasesInscritas);
            return hash.ToHashCode();
        }
    }
}