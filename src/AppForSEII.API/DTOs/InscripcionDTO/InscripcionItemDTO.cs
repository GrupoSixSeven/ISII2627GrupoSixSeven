namespace AppForSEII.API.DTOs.InscripcionDTO
{
    public class InscripcionItemDTO
    {
        public InscripcionItemDTO(int claseDeportivaId, string descripcionClase, string tipoDeporte, string monitor, DateTime fechaHora, int plazasReservadas, decimal precio, string? observaciones = null)
        {
            ClaseDeportivaId = claseDeportivaId;
            DescripcionClase = descripcionClase;
            TipoDeporte = tipoDeporte;
            Monitor = monitor;
            FechaHora = fechaHora;
            PlazasReservadas = plazasReservadas;
            Precio = precio;
            Observaciones = observaciones;
        }

        public int ClaseDeportivaId { get; set; }
        public string DescripcionClase { get; set; } = "DescripcionClase";
        public string TipoDeporte { get; set; } = "TipoDeporte";
        public string Monitor { get; set; } = "Monitor";
        public DateTime FechaHora { get; set; }
        public int PlazasReservadas { get; set; } = 1;
        public decimal Precio { get; set; } = 1;
        public string? Observaciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionItemDTO dTO &&
                   ClaseDeportivaId == dTO.ClaseDeportivaId &&
                   DescripcionClase == dTO.DescripcionClase &&
                   TipoDeporte == dTO.TipoDeporte &&
                   Monitor == dTO.Monitor &&
                   FechaHora == dTO.FechaHora &&
                   PlazasReservadas == dTO.PlazasReservadas &&
                   Precio == dTO.Precio &&
                   Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ClaseDeportivaId, DescripcionClase, TipoDeporte, Monitor, FechaHora, PlazasReservadas, Precio, Observaciones);
        }
    }
}