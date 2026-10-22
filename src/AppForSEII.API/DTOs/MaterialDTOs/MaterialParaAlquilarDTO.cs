using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class MaterialParaAlquilarDTO
    {
        
        public MaterialParaAlquilarDTO()
        {
        }

        
        public MaterialParaAlquilarDTO(int idMaterial, string nombre, decimal precio, int cantidad, string tipoMaterial, string tipoDeporte)
        {
            IdMaterial = idMaterial;
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
            TipoMaterial = tipoMaterial;
            TipoDeporte = tipoDeporte;
        }

        public int IdMaterial { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string Nombre { get; set; } = "Nombre";

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal Precio { get; set; } = 1m;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public int Cantidad { get; set; } = 1;

        [StringLength(50, ErrorMessage = "El tipo de material no puede tener más de 50 caracteres")]
        public string TipoMaterial { get; set; } = "General";

        [StringLength(50, ErrorMessage = "El tipo de deporte no puede tener más de 50 caracteres")]
        public string TipoDeporte { get; set; } = "General";

        
        public override bool Equals(object? obj)
        {
            return obj is MaterialParaAlquilarDTO dTO &&
                   IdMaterial == dTO.IdMaterial &&
                   Nombre == dTO.Nombre &&
                   Precio == dTO.Precio &&
                   Cantidad == dTO.Cantidad &&
                   TipoMaterial == dTO.TipoMaterial &&
                   TipoDeporte == dTO.TipoDeporte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMaterial, Nombre, Precio, Cantidad, TipoMaterial, TipoDeporte);
        }
    }
}