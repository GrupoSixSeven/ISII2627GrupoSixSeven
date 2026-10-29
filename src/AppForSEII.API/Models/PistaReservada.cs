using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(IdPista), nameof(IdReserva))]
public class PistaReservada
{
    [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Cantidad { get; set; }

    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range(typeof(decimal), "0", "1000", ErrorMessage = "Introduzca un precio válido")]
    public decimal Precio { get; set; }

    // RELACIONES / CLAVES FORÁNEAS (Clave primaria compuesta)
    [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range(0, int.MaxValue, ErrorMessage = "Introduzca un valor válido para el id de pista")]
    public int IdPista { get; set; }
    public Pista? Pista { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio, introduzca un valor")]
    [Range(0, int.MaxValue, ErrorMessage = "Introduzca un valor válido para el id de reserva")]
    public int IdReserva { get; set; }
    public Reserva? Reserva { get; set; }

    // Constructor vacío
    public PistaReservada()
    {
    }

    public PistaReservada(int cantidad, string? observaciones, decimal precio, int idPista, int idReserva)
    {
        this.Cantidad = cantidad;
        this.Observaciones = observaciones;
        this.Precio = precio;
        this.IdPista = idPista;
        this.IdReserva = idReserva;
    }

    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        PistaReservada otra_pistaReservada = (PistaReservada)otro;

        if (otra_pistaReservada.IdPista != this.IdPista || otra_pistaReservada.IdReserva != this.IdReserva) 
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdPista, IdReserva);
    }
}