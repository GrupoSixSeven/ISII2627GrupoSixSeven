using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models
{
    public class ClaseInscrita
    {
        public ClaseInscrita()
        {
        }

        public ClaseInscrita(int claseDeportivaId, int inscripcionId, int plazasReservadas, decimal precio, string? observaciones)
        {
            
            PlazasReservadas = plazasReservadas;
            Precio = precio;
            Observaciones = observaciones;
        }

        [Key]
        public int Id { get; set; } // Implementado tal y como marca el diagrama UML

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

            return otra_clase.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }

    
}