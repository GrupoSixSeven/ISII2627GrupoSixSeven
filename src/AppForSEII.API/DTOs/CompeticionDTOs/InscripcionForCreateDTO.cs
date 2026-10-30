using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class InscripcionForCreateDTO
    {
        public InscripcionForCreateDTO()
        {
            Competiciones = new List<CompeticionItemDTO>();
            ClasesInscritas = new List<InscripcionItemDTO>();
        }

        public InscripcionForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<CompeticionItemDTO> competiciones, string datosPago, DateTime fechaInscripcion, IList<InscripcionItemDTO> clasesInscritas)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            Competiciones = competiciones;
            DatosPago = datosPago;
            FechaInscripcion = fechaInscripcion;
            ClasesInscritas = clasesInscritas;
        }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;
        
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required(ErrorMessage = "El campo DatosPago es obligatorio")]
        public string DatosPago { get; set; } = "";

        [Required(ErrorMessage = "El campo fecha de inscripcion es obligatorio")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Required]
        [MinLength(1, ErrorMessage = "Debe haber al menos 1 competición inscrita")]
        
        public IList<CompeticionItemDTO> Competiciones { get; set; } = new List<CompeticionItemDTO>();
    
        public IList<InscripcionItemDTO> ClasesInscritas { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is InscripcionForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInscripcion == dTO.FechaInscripcion &&
                   DatosPago == dTO.DatosPago &&
                   EqualityComparer<IList<InscripcionItemDTO>>.Default.Equals(ClasesInscritas, dTO.ClasesInscritas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaInscripcion, DatosPago, MetodoPago, PrecioTotal);
        }
    }
}