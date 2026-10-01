using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class ClaseDeportiva
    {
        public ClaseDeportiva()
        {
            ClasesInscritas = new List<ClaseInscrita>();
        }

        public ClaseDeportiva(string descripcion, DateTime fechaHora, string? lugar, string monitor, string nivel, int plazasDisponibles, decimal precioUnitario, TipoDeporte tipoDeporte)
        {
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            FechaHora = fechaHora;
            Lugar = lugar;
            Monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
            Nivel = nivel ?? throw new ArgumentNullException(nameof(nivel));
            PlazasDisponibles = plazasDisponibles;
            PrecioUnitario = precioUnitario;
            
            TipoDeporte = tipoDeporte ?? throw new ArgumentNullException(nameof(tipoDeporte));
            TipoDeporteId = tipoDeporte.Id;

            ClasesInscritas = new List<ClaseInscrita>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200)]
        [System.ComponentModel.DataAnnotations.Display(Name = "Descripción")]
        public string Descripcion { get; set; } = null!;

        [Required]
        [System.ComponentModel.DataAnnotations.Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        [StringLength(100)]
        public string? Lugar { get; set; }

        [Required(ErrorMessage = "El nombre del monitor es obligatorio.")]
        [StringLength(100)]
        public string Monitor { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Nivel { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Las plazas disponibles no pueden ser negativas.")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Plazas Disponibles")]
        public int PlazasDisponibles { get; set; }

        [Precision(10, 2)]
        [System.ComponentModel.DataAnnotations.Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        // Relación N a 1 con TipoDeporte
        public int TipoDeporteId { get; set; }
        
        [ForeignKey(nameof(TipoDeporteId))]
        public TipoDeporte? TipoDeporte { get; set; } // Corrección: añadido ? y eliminado = null!

        // Relación 1 a N con ClaseInscrita
        public IList<ClaseInscrita> ClasesInscritas { get; set; }

        // Corrección: añadidos Equals y GetHashCode
        public override bool Equals(object? obj)
        {
            if (obj is not ClaseDeportiva item)
                return false;

            // En entidades de base de datos, la igualdad suele medirse por su Clave Primaria (Id)
            return Id == item.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}