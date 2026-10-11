using System; 
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

[PrimaryKey(nameof(CompeticionId), nameof (InscripcionId))]
public class CompeticionInscrita{

    public int CompeticionId {get;set;}
 
    public int InscripcionId {get;set;}

    public String? ProblemasFisicos {get;set;} 

    //RELACIONES
    public  Competicion Competicion { get; set; }
    public  Inscripcion Inscripcion { get; set; }
    public CompeticionInscrita(){
        
    }

    public CompeticionInscrita (int competicionId, int inscripcionId, String? problemasFisicos){
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
