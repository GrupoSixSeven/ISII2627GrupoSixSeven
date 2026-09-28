namespace AppForSEII.API.Models;

public class Material{

    [Key] 

    public int IdMaterial { get; set; }
    public string? Nombre { get; set; }
    public int Cantidad { get; set; }
    public int Precio { get; set; }
    public Material(){}
    public Material(int idMaterial, string nombre, int cantidad, int precio)
    {
        IdMaterial = idMaterial;
        Nombre = nombre;
        Cantidad = cantidad;
        Precio = precio;
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