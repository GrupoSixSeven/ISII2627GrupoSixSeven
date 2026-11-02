using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AppForSEII.API.Models;

public class Reserva
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Introduzca una fecha de reserva válida")]
    public required DateTime FechaReserva { get; set; }

    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public required MetodoPago MetodoPago { get; set; }

    public string? DatosPago { get; set; }

    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Introduzca un precio total válido")]
    public decimal PrecioTotal { get; set; }

    // Relación con ApplicationUser (Cliente)
    public string? ClienteId { get; set; }

    public ApplicationUser? Cliente { get; set; }

    // Relación con PistaReservada (1 - N)
    public ICollection<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

    // Constructor vacío
    public Reserva()
    {
    }

    // Constructor con los atributos principales
    public Reserva(int id, DateTime fechaReserva, MetodoPago metodoPago, decimal precioTotal)
    {
        Id = id;
        FechaReserva = fechaReserva;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
    }

    // Constructor con cliente y pistas reservadas
    [SetsRequiredMembers]
    public Reserva(
        ApplicationUser? cliente,
        DateTime fechaReserva,
        MetodoPago metodoPago,
        ICollection<PistaReservada> pistasReservadas)
    {
        Cliente = cliente;
        FechaReserva = fechaReserva;
        MetodoPago = metodoPago;
        PistasReservadas = pistasReservadas;
    }

    public override bool Equals(object? otro)
    {
        if (otro == null)
            return false;

        if (otro.GetType() != GetType())
            return false;

        Reserva otra_reserva = (Reserva)otro;

        return otra_reserva.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}