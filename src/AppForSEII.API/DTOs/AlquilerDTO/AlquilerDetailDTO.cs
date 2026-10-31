using System;
using System.Collections.Generic;
using AppForSEII.API.Models;
namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
    public AlquilerDetailDTO(int idAlquiler, string nombreUsuario, string apellidosUsuario, string dni,
        string email, string telefono, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales,
        DateTime fechaAlquiler)
        : base(nombreUsuario,
               apellidosUsuario,
               dni,
               email,
               telefono,
               metodoPago,
               materiales,
               fechaAlquiler)
    {
        IdAlquiler = idAlquiler;
    }
 
    public int IdAlquiler { get; set; }
 
    public override bool Equals(object? obj)
    {
        return obj is AlquilerDetailDTO dTO &&
               base.Equals(obj) &&
               PrecioTotal == dTO.PrecioTotal &&
               IdAlquiler == dTO.IdAlquiler;
    }
 
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IdAlquiler);
    }
    }
}