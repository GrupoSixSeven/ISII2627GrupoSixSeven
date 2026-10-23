using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.AlquilerDTOs;

public class AlquilerForCreateDTO
{
    public AlquilerForCreateDTO()
    {
        Materiales = new List<AlquilerItemDTO>();
    }

    public AlquilerForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string telefono, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales)
    {
        NombreUsuario = nombreUsuario;
        ApellidosUsuario = apellidosUsuario;
        DNI = dni;
        Telefono = telefono;
        MetodoPago = metodoPago;
        Materiales = materiales;
        
    }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres")]
        public string NombreUsuario { get; set; } = "Alejandro";
        
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres")]
        public string ApellidosUsuario { get; set; } = "Gonzalez";
        
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(9,ErrorMessage = "el DNI tiene que tener una longitud de 9 caracteres")]
        public string DNI { get; set; } = "12345678A";

        [Required]
        [Phone]
        public string Telefono { get; set; } = "600000000";

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;
        
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrecioTotal { get; set; } = 0;

        [Required]
        public IList<AlquilerItemDTO> Materiales { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is AlquilerForCreateDTO dTO &&

               NombreUsuario == dTO.NombreUsuario &&
                ApellidosUsuario == dTO.ApellidosUsuario &&
                DNI == dTO.DNI &&
                Telefono == dTO.Telefono &&
                MetodoPago == dTO.MetodoPago &&
                PrecioTotal == dTO.PrecioTotal;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine( NombreUsuario, ApellidosUsuario, DNI, Telefono, MetodoPago, PrecioTotal);
    }
}