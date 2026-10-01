namespace AppForSEII.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
public class Competicion {
    
//getters y setters
[Key]
public int Id { get; set; }

[Required(ErrorMessage = "Error, introduzca una fecha válida")]
public required DateTime Fecha {get; set;}

[Required(AllowEmptyStrings = false, ErrorMessage = "Error, introduzca un lugar válido")]
public required String Lugar {get; set;}

[Required(AllowEmptyStrings = false, ErrorMessage = "Error, introduzca un nombre válido")]
public required String Nombre {get; set;}

[Range (1,100, ErrorMessage = "Introduzca un número de plazas válido (1-100)")] //he puesto de maximo a 100 personas por competcion, para q intente ser un poco realista
public int Plazas {get; set;}
[Range (0,int.MaxValue, ErrorMessage = "Introduzca un valor váido para el precio")] //contemplo el 0, pq lo podemos hacer gratis
public decimal Precio {get; set;}

//RELACIONES
// Relación con TipoDeporte (1 - N) (Un Competición sólo puede ser de 1 deporte, pero un tipo de deporte puede tener más de 1 competición)
[Required(ErrorMessage = "Introduzca un tipo deporte válido")]
public required TipoDeporte TipoDeporte { get; set; } //da error en la rama, pq no existe la clase TipoDeporte

// Relación con CompeticiónInscripción (1 - N) (1 competición puede tener varias competiciones inscritas)

public ICollection<CompeticionInscripcion> CompeticionInscripciones { get; set; } = new List<CompeticionInscripcion>(); //pasa lo mismo que con la otra relación

//constructor vacío
public Competicion(){
        
    }
//constructor con parámetros
public Competicion (int id, DateTime fecha, String lugar, String nombre, int plazas, decimal precio){
        this.Id = id;
        this.Fecha = fecha;
        this.Lugar = lugar;
        this.Nombre = nombre;
        this.Plazas = plazas;
        this.Precio = precio;
    }

    public override bool Equals(object? otro){
       if (otro ==  null) return false;
       if (otro.GetType() != this.GetType()) return false;
        Competicion otro_competicion = (Competicion) otro;
        if (otro_competicion.Id != this.Id) return false;
        return true;

    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
