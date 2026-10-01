using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class PistaReservada
{
    [Key]
    public int ID { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Cantidad { get; set; }


    public string? Observaciones { get; set; }

    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Introduzca un precio válido")]
    public decimal Precio { get; set; }

    //RELACIONES
    //Relación con Pista (N - 1)
    public int IdPista { get; set; }
    public Pista? Pista { get; set; }

    //Relación con Reserva (N - 1)
    public int IdReserva { get; set; }
    public Reserva? Reserva { get; set; }

    // Constructor vacío
    public PistaReservada()
    {
    }

    // Constructor con los atributos principales
    public PistaReservada(int id, int cantidad, string? observaciones, decimal precio, int idPista, int idReserva)
    {
        this.ID = id;
        this.Cantidad = cantidad;
        this.Observaciones = observaciones;
        this.Precio = precio;
        this.IdPista = idPista;
        this.IdReserva = idReserva;
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