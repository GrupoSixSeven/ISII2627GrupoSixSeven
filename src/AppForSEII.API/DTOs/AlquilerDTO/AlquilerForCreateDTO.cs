using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.AlquilerDTOs;

public class AlquilerForCreateDTO
{
    public AlquilerForCreateDTO()
    {
        Materiales = new List<AlquilerItemDTO>();
    }

    public AlquilerForCreateDTO(decimal precioTotal, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales, DateTime fechaAlquiler)
    {

        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        Materiales = materiales;
        FechaAlquiler = fechaAlquiler;
        
    }


        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;
        
        [Required]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio total debe ser un número positivo")]
        public decimal PrecioTotal { get; set; } = 0;

        [Required (ErrorMessage = "La fecha de alquiler es obligatoria")]
        public DateTime FechaAlquiler { get; set; } = DateTime.Now;

        [Required]
        [MinLength(1)]
        public IList<AlquilerItemDTO> Materiales { get; set; }


    public override bool Equals(object? obj)
    {
        return obj is AlquilerForCreateDTO dTO &&

                Materiales == dTO.Materiales &&
                MetodoPago == dTO.MetodoPago &&
                PrecioTotal == dTO.PrecioTotal &&
                FechaAlquiler == dTO.FechaAlquiler;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine( Materiales, MetodoPago, PrecioTotal, FechaAlquiler);
    }
}