namespace AppForSEII.API.DTOs
{
    public class ReservaItemDTO
    {
        public ReservaItemDTO(int idPista, string nombrePista, string nombreTipoDeporte, int nPersonas, int cantidad, decimal precio, string? observaciones = "")
        {
            IdPista = idPista;
            NombrePista = nombrePista;
            NombreTipoDeporte = nombreTipoDeporte;
            NPersonas = nPersonas;
            Cantidad = cantidad;
            Precio = precio;
            Observaciones = observaciones;
        }

        public int IdPista { get; set; }
        public string NombrePista { get; set; }
        public string NombreTipoDeporte { get; set; }
        public int NPersonas { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string? Observaciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaItemDTO dTO &&
                   IdPista == dTO.IdPista &&
                   NombrePista == dTO.NombrePista &&
                   NombreTipoDeporte == dTO.NombreTipoDeporte &&
                   NPersonas == dTO.NPersonas &&
                   Cantidad == dTO.Cantidad &&
                   Precio == dTO.Precio &&
                   Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, NombreTipoDeporte, NPersonas, Cantidad, Precio, Observaciones);
        }
    }
}