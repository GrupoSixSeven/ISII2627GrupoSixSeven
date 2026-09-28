using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class PistaReservada
{
    [Key]
    public int ID { get; set; }

    public int Cantidad { get; set; }

    public int IdPista { get; set; }

    public int IdReserva { get; set; }

    public string? Observaciones { get; set; }

    public double Precio { get; set; }

    // Constructor vacío
    public PistaReservada()
    {
    }

    // Constructor con los atributos principales
    public PistaReservada(int id, int cantidad, int idPista, int idReserva, string? observaciones, double precio)
    {
        ID = id;
        Cantidad = cantidad;
        IdPista = idPista;
        IdReserva = idReserva;
        Observaciones = observaciones;
        Precio = precio;
    }

    // Comprueba que el otro objeto no sea null, que sea del mismo tipo y que sus IDs coincidan
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        PistaReservada otra_pistaReservada = (PistaReservada)otro;

        return otra_pistaReservada.ID == this.ID;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}