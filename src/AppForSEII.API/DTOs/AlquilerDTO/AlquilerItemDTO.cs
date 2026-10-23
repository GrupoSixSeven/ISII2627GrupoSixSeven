namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerItemDTO
    {
        public AlquilerItemDTO(int idMaterial, string nombreMaterial, string tipoMaterial, string tipoDeporte, int cantidad, decimal precio, string? descripcion = "")
        {
            IdMaterial = idMaterial;
            NombreMaterial = nombreMaterial;
            TipoMaterial = tipoMaterial;
            TipoDeporte = tipoDeporte;
            Cantidad = cantidad;
            Precio = precio;
            Descripcion = descripcion;
        }

        public  int IdMaterial { get; set; }
        public  string NombreMaterial { get; set; }= "NombreMaterial";
        public  string TipoMaterial { get; set; }= "TipoMaterial";
        public string TipoDeporte { get; set; }= "TipoDeporte";
        public int Cantidad { get; set; }=1;
        public decimal Precio { get; set; }=1;
        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerItemDTO dTO &&
                   IdMaterial == dTO.IdMaterial &&
                   NombreMaterial == dTO.NombreMaterial &&
                   TipoMaterial == dTO.TipoMaterial &&
                   TipoDeporte == dTO.TipoDeporte &&
                   Cantidad == dTO.Cantidad &&
                   Precio == dTO.Precio &&
                   Descripcion == dTO.Descripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMaterial, NombreMaterial, TipoMaterial, TipoDeporte, Cantidad, Precio, Descripcion);
        }
    }
}
