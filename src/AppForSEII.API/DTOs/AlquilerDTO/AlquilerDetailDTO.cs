using System;
using System.Collections.Generic;
using AppForSEII.API.Models;
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
        public AlquilerDetailDTO(int idAlquiler, string nombreUsuario, string apellidosUsuario, string dni, string telefono, DateTime fechaAlquiler, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales)
            : base(nombreUsuario, apellidosUsuario, dni, telefono, metodoPago, materiales)
        {
            IdAlquiler = idAlquiler;
            FechaAlquiler = fechaAlquiler;
        }

        public int IdAlquiler { get; set; }
        public DateTime FechaAlquiler { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dTO &&
                   base.Equals(obj) && 
                   IdAlquiler == dTO.IdAlquiler &&
                   FechaAlquiler == dTO.FechaAlquiler;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdAlquiler, FechaAlquiler);
        }
    }
}