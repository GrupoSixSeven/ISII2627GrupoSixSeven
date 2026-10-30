using System;

namespace AppForSEII.API.DTOs.ClaseDeportivaDTO
{
    public class InscripcionItemDTO
    {
        public InscripcionItemDTO(int claseDeportivaId, string descripcion, string tipoDeporte, string monitor, DateTime fechaHora, int plazasReservadas, decimal precio, string? observaciones = null)
        {
            ClaseDeportivaId = claseDeportivaId;
            Descripcion = descripcion;
            TipoDeporte = tipoDeporte;
            Monitor = monitor;
            FechaHora = fechaHora;
            PlazasReservadas = plazasReservadas;
            Precio = precio;
            Observaciones = observaciones;
        }

        public int ClaseDeportivaId { get; set; }
        public string Descripcion { get; set; } 
        public string TipoDeporte { get; set; } 
        public string Monitor { get; set; } 
        public DateTime FechaHora { get; set; }
        public int PlazasReservadas { get; set; } 
        public decimal Precio { get; set; } 
        public string? Observaciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionItemDTO dTO &&
                   ClaseDeportivaId == dTO.ClaseDeportivaId &&
                   Descripcion == dTO.Descripcion &&
                   TipoDeporte == dTO.TipoDeporte &&
                   Monitor == dTO.Monitor &&
                   FechaHora == dTO.FechaHora &&
                   PlazasReservadas == dTO.PlazasReservadas &&
                   Precio == dTO.Precio &&
                   Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ClaseDeportivaId, Descripcion, TipoDeporte, Monitor, FechaHora, PlazasReservadas, Precio, Observaciones);
        }
    }
}