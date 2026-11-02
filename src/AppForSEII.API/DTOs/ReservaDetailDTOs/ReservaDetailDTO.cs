using System;
using System.Collections.Generic;
using AppForSEII.API.Models;
 
namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO(int idReserva, MetodoPago metodoPago, DateTime fechaReserva, decimal precioTotal, IList<ReservaItemDTO> pistas, string nombreUsuario, string apellidosUsuario, string dni)
            : base(metodoPago, precioTotal, pistas, fechaReserva)
        {
            IdReserva = idReserva;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Dni = dni;
        }
 
        public int IdReserva { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string Dni { get; set; }
 
        public override bool Equals(object? obj)
        {
            return obj is ReservaDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdReserva == dTO.IdReserva;
        }
 
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdReserva);
        }
    }
}