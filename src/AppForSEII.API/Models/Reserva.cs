using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Reserva
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un nombre de cliente válido")]
    public required string NombreCliente { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca los apellidos válidos")]
    public required string Apellidos { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca un DNI válido")]
    public required string Dni { get; set; }

    [Required(ErrorMessage = "Introduzca una fecha de reserva válida")]
    public DateTime FechaReserva { get; set; }

    [Required(ErrorMessage = "Introduzca un método de pago válido")]
    public MetodoPago MetodoPago { get; set; }

    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Introduzca un precio total válido")]
    public decimal PrecioTotal { get; set; }
 
    //RELACIÓN
    //Relación con PistaReservada (1 - N)
    public ICollection<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

    // Constructor vacío
    public Reserva()
    {
    }

    // Constructor con los atributos principales
    public Reserva(int id, string nombreCliente, string apellidos, string dni, DateTime fechaReserva, MetodoPago metodoPago, decimal precioTotal)
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