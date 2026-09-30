using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class TipoMaterial
{

    [Key]
    public int IdTipoMaterial {get;set;}

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del tipo de material es obligatorio")]
    public required string NombreTipoMaterial {get;set;}

    //Relación con Materiales (1-N), ya que un tipo de material puede estar asociado a varios materiales y un material solo puede estar asociado a un tipo de material
    public ICollection<Material> Materiales{get;set;} = new List<Material>();

    //Constructor Vacio
    public TipoMaterial(){}


    //Constructor con parámetros 
    public TipoMaterial(int idTipoMaterial, string nombreTipoMaterial)
    {
        this.IdTipoMaterial = idTipoMaterial;
        this.NombreTipoMaterial = nombreTipoMaterial;
    }


    //Metodo Equals y GetHashCode para comparar objetos de tipo TipoMaterial
    public override bool Equals(object? otro)
    { 
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false; 
        
        TipoMaterial otro_tipo_material = (TipoMaterial) otro;

        if (otro_tipo_material.IdTipoMaterial != this.IdTipoMaterial) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return IdTipoMaterial.GetHashCode();
    }
}