using System; 
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

[PrimaryKey(nameof(CompeticionId), nameof (InscripcionId))]
public class CompeticionInscripcion{

    [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range (0,int.MaxValue, ErrorMessage = "Introduzca un valor válido para el id de competicion")] 
    public int CompeticionId {get;set;}

     [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range (0,int.MaxValue, ErrorMessage = "Introduzca un valor válido para el id de inscripción")] 
    public int InscripcionId {get;set;}

    public String? ProblemasFisicos {get;set;} 

    //RELACIONES
    public Competicion? Competicion { get; set; }
    public Inscripcion? Inscripcion { get; set; }
    public CompeticionInscripcion(){
        
    }

    public CompeticionInscripcion (int competicionId, int inscripcionId, String? problemasFisicos){
        this.CompeticionId = competicionId;
        this.InscripcionId = inscripcionId;
        this.ProblemasFisicos =problemasFisicos;
    }

    public override bool Equals (object? otro){
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;
        CompeticionInscripcion otro_CompeticionInscripcion = (CompeticionInscripcion) otro;
        if (otro_CompeticionInscripcion.CompeticionId != this.CompeticionId || otro_CompeticionInscripcion.InscripcionId != this.InscripcionId) return false;
        return true;
    }

    public override int GetHashCode(){
        return HashCode.Combine(CompeticionId, InscripcionId);   
         }
}
