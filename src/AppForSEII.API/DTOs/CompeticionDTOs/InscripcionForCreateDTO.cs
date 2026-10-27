using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;
using Humanizer;

namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class InscripcionForCreateDTO
    {
        public InscripcionForCreateDTO()
        {
            Competiciones = new List<CompeticionItemDTO>();
        }

        public InscripcionForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<CompeticionItemDTO> competiciones, string datosPago, DateTime fechaInscripcion)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Competiciones = competiciones;
            DatosPago = datosPago;
            FechaInscripcion = fechaInscripcion;
        }


        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;
        
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required (ErrorMessage = "El campo DatosPago es obligatorio")]
        public string DatosPago {get; set; }

        [Required (ErrorMessage = "El campo fecha de inscripcion es obligatorio")]
        public DateTime FechaInscripcion {get; set;}


        [Required]
        public IList<CompeticionItemDTO> Competiciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInscripcion == dTO.FechaInscripcion &&
                   DatosPago == dTO.DatosPago;
                   
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaInscripcion,DatosPago, MetodoPago, PrecioTotal);
        }
    }
}