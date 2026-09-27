namespace AppForSEII.API.Models;

public class Material{
    public Material()
    {
    }
    public Material(int idMaterial, string nombre, int cantidad, int precio)
    {
        IdMaterial = idMaterial;
        Nombre = nombre;
        Cantidad = cantidad;
        Precio = precio;
    }
    public int IdMaterial { get; set; }
    public string? Nombre { get; set; }
    public int Cantidad { get; set; }
    public int Precio { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Material otroMaterial = (Material)obj;
        return IdMaterial == otroMaterial.IdMaterial;

    }

    public override int GetHashCode()
    {
        return IdMaterial.GetHashCode();
    }


}