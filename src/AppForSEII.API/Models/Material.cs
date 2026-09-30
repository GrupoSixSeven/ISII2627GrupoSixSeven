using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII.API.Models;

public class Material{

    [Key]

    public int IdMaterial { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
    public required string Nombre { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad minima debe ser 1")]
    public int Cantidad { get; set; }
    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Precio { get; set; }

    //Relaciones
    //Relacion TipoMaterial (N-1)
    public int IdTipoMaterial { get; set; }
    public TipoMaterial? TipoMaterial { get; set; }

    //Relacion TipoDeporte (N - 1)
    public int IdTipoDeporte { get; set; }
    public TipoDeporte? TipoDeporte { get; set; }
    
    //Relacion MaterialAlquilado (1-N)
    public ICollection<MaterialAlquilado> MaterialAlquilados { get; set; } = new List<MaterialAlquilado>();
    public Material(){}
    public Material(int idMaterial, string nombre, int cantidad, decimal precio)
    {
        this.IdMaterial = idMaterial;
        this.Nombre = nombre;
        this.Cantidad = cantidad;
        this.Precio = precio;
    }


   public override bool Equals(object? otro)
    { 
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false; 
        
        Material otro_material = (Material) otro;

        if (otro_material.IdMaterial != this.IdMaterial) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return IdMaterial.GetHashCode();
    }


}