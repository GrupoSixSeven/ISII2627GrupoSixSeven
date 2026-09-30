using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII.API.Models;
//Clave primaria compuesta por IdMaterial y IdAlquiler
[PrimaryKey(nameof(IdMaterial), nameof(IdAlquiler))]
public class MaterialAlquilado
{

    // Relación con Alquiler (N-1).
    public int IdAlquiler { get; set; }
    public Alquiler? Alquiler { get; set; }

    // Relación con Material (N-1)
    public int IdMaterial { get; set; }
    public Material? Material { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad minima debe ser 1")]
    public int Cantidad { get; set; }

    [DataType(DataType.Currency)]
    [Precision(18, 2)]
    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    //Constructor vacío 
    public MaterialAlquilado(){}

    //Constructor con parámetros
    public MaterialAlquilado(int idMaterial, int idAlquiler, int cantidad, decimal precio, string? descripcion)
    {
        this.IdMaterial = idMaterial;
        this.IdAlquiler = idAlquiler;
        this.Cantidad = cantidad;
        this.Precio = precio;
        this.Descripcion = descripcion;
    }
    



    public override bool Equals(object? otro)
    { 
       if (otro == null || otro.GetType() != this.GetType()) return false;
        MaterialAlquilado otro_material_alquilado = (MaterialAlquilado)otro;
        return otro_material_alquilado.IdMaterial == this.IdMaterial && otro_material_alquilado.IdAlquiler == this.IdAlquiler;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdMaterial, IdAlquiler);
    }
}
