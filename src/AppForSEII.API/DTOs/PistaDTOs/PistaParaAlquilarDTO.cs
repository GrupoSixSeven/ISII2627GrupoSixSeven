using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class PistaParaAlquilarDTO
    {
        public PistaParaAlquilarDTO()
        {
        }

        public PistaParaAlquilarDTO(int idPista, string nombrePista, int nPersonas, decimal precio, int stock, string nombreTipoDeporte)
        {
            IdPista = idPista;
            NombrePista = nombrePista;
            NPersonas = nPersonas;
            Precio = precio;
            Stock = stock;
            NombreTipoDeporte = nombreTipoDeporte;
        }

        public int IdPista { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string NombrePista { get; set; } = "Pista";

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El aforo mínimo es 1")]
        public int NPersonas { get; set; } = 1;

        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal Precio { get; set; } = 1m;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; } = 0;

        [StringLength(50, ErrorMessage = "El tipo de deporte no puede tener más de 50 caracteres")]
        public string NombreTipoDeporte { get; set; } = "General";

        public override bool Equals(object? obj)
        {
            return obj is PistaParaAlquilarDTO dto &&
                   IdPista == dto.IdPista &&
                   NombrePista == dto.NombrePista &&
                   NPersonas == dto.NPersonas &&
                   Precio == dto.Precio &&
                   Stock == dto.Stock &&
                   NombreTipoDeporte == dto.NombreTipoDeporte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, NPersonas, Precio, Stock, NombreTipoDeporte);
        }
    }
}