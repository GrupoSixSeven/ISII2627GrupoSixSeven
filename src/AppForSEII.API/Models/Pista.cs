using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Pista
{
    [Key]
    public int IdPista { get; set; }

    [Required]
    public string? NombrePista { get; set; }

    public int NPersonas { get; set; }

    public double Precio { get; set; }

    public int Stock { get; set; }

    // Constructor vacío
    public Pista()
    {
    }

    // Constructor con los atributos principales
    public Pista(int idPista, string? nombrePista, int nPersonas, double precio, int stock)
    {
        IdPista = idPista;
        NombrePista = nombrePista;
        NPersonas = nPersonas;
        Precio = precio;
        Stock = stock;
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