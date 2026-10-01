using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppForSEII.API.Models
{
    public class TipoDeporte
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca el nombre de un tipo de deporte válido")]
        public required string Nombre { get; set; }

        public string? NombreTipoDeporte { get; set; }

        // Añadido según el diagrama (nullable, sin required)
        public string? Descripcion { get; set; }

        // RELACIONES (Inicializadas fuera del constructor, como pide Alejandro)
        public ICollection<Pista> Pistas { get; set; } = new List<Pista>();
        public ICollection<Competicion> Competiciones { get; set; } = new List<Competicion>();   
        public ICollection<Material> Materiales { get; set; } = new List<Material>();
        public ICollection<ClaseDeportiva> ClasesDeportivas { get; set; } = new List<ClaseDeportiva>();

        // Constructor vacio 
        public TipoDeporte()
        {
        }

        // Constructor con atributos actualizados
        public TipoDeporte(int id, string nombre, string? nombreTipoDeporte, string? descripcion)
        {
            Id = id;
            Nombre = nombre;
            NombreTipoDeporte = nombreTipoDeporte;
            Descripcion = descripcion;
        }
        
        // Estilo exacto del Equals que aprobó Alejandro
        public override bool Equals(object? otro)
        { 
            if (otro == null) return false;
            if (otro.GetType() != this.GetType()) return false; 
            
            TipoDeporte otro_tipodeporte = (TipoDeporte)otro;

            return otro_tipodeporte.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}