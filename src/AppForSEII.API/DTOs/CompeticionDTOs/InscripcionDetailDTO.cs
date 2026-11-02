using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class InscripcionDetailDTO : InscripcionForCreateDTO
    {
        public InscripcionDetailDTO(int idInscripcion, string nombreUsuario, string apellidosUsuario, string dni, 
            string correo, string telefono, MetodoPago metodoPago, IList<CompeticionItemDTO> competiciones, 
            string datosPago, DateTime fechaInscripcion)
            : base(nombreUsuario,
                   apellidosUsuario,
                   dni,
                   correo,
                   telefono,
                   metodoPago,
                   competiciones,
                   datosPago,
                   fechaInscripcion)
        {
            IdInscripcion = idInscripcion;
        }

        public int IdInscripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionDetailDTO dTO &&
                   base.Equals(obj) && 
                   PrecioTotal == dTO.PrecioTotal &&
                   IdInscripcion == dTO.IdInscripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdInscripcion);
        }
    }
}