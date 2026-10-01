using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    public class ClaseDeportiva
    {

        // Únicamente constructor vacío
        public ClaseDeportiva()
        {
        }

        public ClaseDeportiva(string descripcion, DateTime fechaHora, string lugar, string monitor, string nivel, int plazasDisponibles, decimal precioUnitario, TipoDeporte tipoDeporte)
        {
            Descripcion = descripcion;
            FechaHora = fechaHora;
            Lugar = lugar;
            Monitor = monitor;
            Nivel = nivel;
            PlazasDisponibles = plazasDisponibles;
            PrecioUnitario = precioUnitario;
            
            TipoDeporte = tipoDeporte;
            if (tipoDeporte != null)
            {
                TipoDeporteId = tipoDeporte.Id;
            }
        }

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La descripción es obligatoria")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Descripción")]
        public required string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Fecha y Hora")]
        public required DateTime FechaHora { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El lugar es obligatorio.")]
        public required string Lugar { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del monitor es obligatorio.")]
        public required string Monitor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nivel es obligatorio.")]
        public required string Nivel { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las plazas disponibles no pueden ser negativas.")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Plazas Disponibles")]
        public int PlazasDisponibles { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Precision(10, 2)]
        [System.ComponentModel.DataAnnotations.Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        // Relación N a 1 con TipoDeporte
        public int TipoDeporteId { get; set; }
        public TipoDeporte? TipoDeporte { get; set; } 

        // Relación 1 a N con ClaseInscrita
        public IList<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

        // Método Equals modificado según la corrección del PR
        public override bool Equals(object? otro)
        {
            if (otro == null) return false;
            
            if (otro.GetType() != this.GetType()) return false;

            ClaseDeportiva otra_clase = (ClaseDeportiva)otro;

            return otra_clase.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
