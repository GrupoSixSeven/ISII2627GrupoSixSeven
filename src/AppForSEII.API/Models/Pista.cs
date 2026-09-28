using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Pista
{
    [Key]
    public int IdPista { get; set; }
    
    public string? NombrePista { get; set; }
    public int NPersonas { get; set; }
    public double Precio { get; set; }
    public int Stock { get; set; }

    public Pista()
    {
    }

    public Pista(int IdPista, string? NombrePista, int NPersonas, double Precio, int Stock)
    {
        this.IdPista = IdPista;
        if (NombrePista != null)
        {
            this.NombrePista = NombrePista;
        }
        this.NPersonas = NPersonas;
        this.Precio = Precio;
        this.Stock = Stock;
    }

    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        Pista otra_pista = (Pista)otro;

        if (otra_pista.IdPista != this.IdPista) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return IdPista.GetHashCode();
    }
}