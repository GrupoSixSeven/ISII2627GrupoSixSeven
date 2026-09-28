using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Reserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? NombreCliente { get; set; }

    [Required]
    public string? Apellidos { get; set; }

    [Required]
    public string? Dni { get; set; }

    public DateTime FechaReserva { get; set; }

    public string? MetodoPago { get; set; }

    public double PrecioTotal { get; set; }

    // Constructor vacío
    public Reserva()
    {
    }

    // Constructor con los atributos principales
    public Reserva(int id, string? nombreCliente, string? apellidos, string? dni, DateTime fechaReserva, string? metodoPago, double precioTotal)
    {
        Id = id;
        NombreCliente = nombreCliente;
        Apellidos = apellidos;
        Dni = dni;
        FechaReserva = fechaReserva;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
    }

    // Comprueba que el otro objeto no sea null, que sea del mismo tipo y que sus Ids coincidan
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        Reserva otra_reserva = (Reserva)otro;

        return otra_reserva.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}