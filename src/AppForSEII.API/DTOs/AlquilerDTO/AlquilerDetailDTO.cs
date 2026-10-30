using System;
using System.Collections.Generic;
using AppForSEII.API.Models;
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
        public AlquilerDetailDTO(int idAlquiler, DateTime fechaAlquiler, decimal precioTotal, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales, string nombreUsuario, string apellidoUsuario, string email, string telefono, string dni)
            : base(precioTotal, metodoPago, materiales, fechaAlquiler)
        {
            IdAlquiler = idAlquiler;
            NombreUsuario = nombreUsuario;
            ApellidoUsuario = apellidoUsuario;
            Email = email;
            Telefono = telefono;
            Dni = dni;
        }

        public int IdAlquiler { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public  string Dni { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dTO &&
                   base.Equals(obj) && 
                   IdAlquiler == dTO.IdAlquiler;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdAlquiler);
        }
    }
}