using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.CompeticionParaInscribirseDTO
{
    public class CompeticionesParaInscribirseDTO
    {
        public CompeticionesParaInscribirseDTO()
        {
        }

        public CompeticionesParaInscribirseDTO(int id, string nombre, string tipoDeporte, DateTime fecha, string lugar, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            TipoDeporte = tipoDeporte;
            Fecha = fecha;
            Lugar = lugar;
            Precio = precio;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string Nombre { get; set; } = "Nombre";

        [StringLength(50, ErrorMessage = "El tipo de deporte no puede tener más de 50 caracteres")]
        public string TipoDeporte { get; set; } = "General";

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(100, ErrorMessage = "El lugar no puede tener más de 100 caracteres")]
        public string Lugar { get; set; } = "Lugar por defecto";

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal Precio { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            return obj is CompeticionesParaInscribirseDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   TipoDeporte == dTO.TipoDeporte &&
                   Fecha == dTO.Fecha &&
                   Lugar == dTO.Lugar &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, TipoDeporte, Fecha, Lugar, Precio);
        }
    }
}