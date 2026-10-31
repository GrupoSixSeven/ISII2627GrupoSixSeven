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

        public InscripcionClaseForCreateDTO(MetodoPago metodoPago, decimal precioTotal, IList<InscripcionItemDTO> clasesInscritas, string datosPago, DateTime fechaInscripcion)
        {
            MetodoPago = metodoPago;
            PrecioTotal = precioTotal;
            ClasesInscritas = clasesInscritas;
            DatosPago = datosPago;
            FechaInscripcion = fechaInscripcion;
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
        [MinLength(1, ErrorMessage = "Debe haber al menos 1 clase deportiva inscrita")]
        public IList<InscripcionItemDTO> ClasesInscritas { get; set; } = new List<InscripcionItemDTO>();

      public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInscripcion == dTO.FechaInscripcion &&
                   DatosPago == dTO.DatosPago &&
                   EqualityComparer<IList<InscripcionItemDTO>>.Default.Equals(ClasesInscritas, dTO.ClasesInscritas);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(FechaInscripcion);
            hash.Add(DatosPago);
            hash.Add(MetodoPago);
            hash.Add(PrecioTotal);
            foreach (var clase in ClasesInscritas)
                hash.Add(clase);
            return hash.ToHashCode();
        }
    }
}