using System;

namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class CompeticionItemDTO
    {
        public CompeticionItemDTO(int id, string nombre, string tipoDeporte, DateTime fecha, decimal precio, string? observaciones = null)
        {
            Id = id;
            Nombre = nombre;
            TipoDeporte = tipoDeporte;
            Fecha = fecha;
            Precio = precio;
            Observaciones = observaciones;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TipoDeporte { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string? Observaciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompeticionItemDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   TipoDeporte == dTO.TipoDeporte &&
                   Fecha == dTO.Fecha &&
                   Precio == dTO.Precio &&
                   Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, TipoDeporte, Fecha, Precio, Observaciones);
        }
    }
}