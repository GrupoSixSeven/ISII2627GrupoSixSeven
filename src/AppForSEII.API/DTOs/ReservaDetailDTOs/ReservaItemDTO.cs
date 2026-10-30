using System;
using System.ComponentModel.DataAnnotations;
 
namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    // Pista dentro de una reserva (paso 7: nombre, deporte, nº de personas y observaciones)
    public class ReservaItemDTO
    {
        public ReservaItemDTO()
        {
            NombrePista = string.Empty;
            TipoDeporte = string.Empty;
        }
 
        public ReservaItemDTO(int idPista, string nombrePista, string tipoDeporte, int nPersonas, decimal precio, string? observaciones)
        {
            IdPista = idPista;
            NombrePista = nombrePista;
            TipoDeporte = tipoDeporte;
            NPersonas = nPersonas;
            Precio = precio;
            Observaciones = observaciones;
        }
 
        public int IdPista { get; set; }
        public string NombrePista { get; set; }
        public string TipoDeporte { get; set; }
        public int NPersonas { get; set; }
 
        [Range(0, (double)decimal.MaxValue)]
        public decimal Precio { get; set; }
 
        // Opcional, una por pista (paso 5)
        [StringLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres")]
        public string? Observaciones { get; set; }
 
        public override bool Equals(object? obj)
        {
            return obj is ReservaItemDTO dto &&
                   IdPista == dto.IdPista &&
                   NombrePista == dto.NombrePista &&
                   TipoDeporte == dto.TipoDeporte &&
                   NPersonas == dto.NPersonas &&
                   Precio == dto.Precio &&
                   Observaciones == dto.Observaciones;
        }
 
        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, TipoDeporte, NPersonas, Precio, Observaciones);
        }
    }
}