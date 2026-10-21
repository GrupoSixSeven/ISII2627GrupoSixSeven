namespace AppForSEII.API.DTOs
{
    public class MaterialParaAlquilarDTO
    {
        public int IdMaterial { get; set; }
        
        public string Nombre { get; set; }
        
        public decimal Precio { get; set; }
        
        public int Cantidad { get; set; }
        
        public string TipoMaterial { get; set; }
        
        public string TipoDeporte { get; set; }
    }
}