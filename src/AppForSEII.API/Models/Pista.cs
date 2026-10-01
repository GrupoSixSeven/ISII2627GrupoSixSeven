using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Pista
{
    [Key]
    public int IdPista { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un nombre de pista válido")]
    public required string NombrePista { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El número de personas debe ser al menos 1")]
    public int NPersonas { get; set; }

    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Introduzca un precio válido")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    //RELACIONES
    //Relación con TipoDeporte (N - 1)
    public int IdTipoDeporte { get; set; }
    public TipoDeporte? TipoDeporte { get; set; }

    //Relación con PistaReservada (1 - N)
    public ICollection<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

    // Constructor vacío
    public Pista()
    {
    }

    // Constructor con los atributos principales
    public Pista(int idPista, string nombrePista, int nPersonas, decimal precio, int stock, int idTipoDeporte)
    {
        IdPista = idPista;
        NombrePista = nombrePista;
        NPersonas = nPersonas;
        Precio = precio;
        Stock = stock;
        IdTipoDeporte = idTipoDeporte;
    }

    // Comprueba que el otro objeto no sea null, que sea del mismo tipo y que sus Ids coincidan
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        Pista otra_pista = (Pista)otro;

        return otra_pista.IdPista == this.IdPista;
    }

    public override int GetHashCode()
    {
        return IdPista.GetHashCode();
    }
}