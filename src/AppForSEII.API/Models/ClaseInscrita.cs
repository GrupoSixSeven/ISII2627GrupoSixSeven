using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    // Definimos la clave primaria compuesta como indica la guía del seminario
    [PrimaryKey(nameof(ClaseDeportivaId), nameof(InscripcionId))]
    public class ClaseInscrita
    {
        public ClaseInscrita()
        {
        }

        public ClaseInscrita(int claseDeportivaId, int inscripcionId, int plazasReservadas, decimal precio, string? observaciones)
        {
            ClaseDeportivaId = claseDeportivaId;
            InscripcionId = inscripcionId;
            PlazasReservadas = plazasReservadas;
            Precio = precio;
            Observaciones = observaciones;
        }

        // Relación con ClaseDeportiva
        public int ClaseDeportivaId { get; set; }
        public ClaseDeportiva? ClaseDeportiva { get; set; }

        // Relación con Inscripcion
        public int InscripcionId { get; set; }
        public Inscripcion? Inscripcion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe reservar al menos 1 plaza.")]
        public int PlazasReservadas { get; set;}

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Precision(10, 2)]
        public decimal Precio { get; set; }

        // Es nullable (?), por lo que no lleva 'required' ni validaciones de AllowEmptyStrings
        public string? Observaciones { get; set; }

        public override bool Equals(object? otro)
        {
            if (otro == null) return false;
            
            if (otro.GetType() != this.GetType()) return false;

            ClaseInscrita otra_clase = (ClaseInscrita)otro;

            // La igualdad ahora se basa en las dos claves foráneas
            return otra_clase.ClaseDeportivaId == this.ClaseDeportivaId &&
                   otra_clase.InscripcionId == this.InscripcionId;
        }

        public override int GetHashCode()
        {
            // El hash se combina usando las dos claves foráneas
            return HashCode.Combine(ClaseDeportivaId, InscripcionId);
        }
    }
}