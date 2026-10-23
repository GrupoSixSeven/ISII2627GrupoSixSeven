using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompeticionDTOs
{
    public class InscripcionDetailDTO : InscripcionForCreateDTO
    {
        public InscripcionDetailDTO(int idInscripcion, string nombreUsuario, string apellidosUsuario, string dni, string telefono, MetodoPago metodoPago, DateTime fechaInscripcion, decimal precioTotal, IList<CompeticionItemDTO> competiciones)
            : base(nombreUsuario, apellidosUsuario, dni, telefono, metodoPago, precioTotal, competiciones)
        {
            IdInscripcion = idInscripcion;
            FechaInscripcion = fechaInscripcion;
        }

        public int IdInscripcion { get; set; }
        public DateTime FechaInscripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionDetailDTO dTO &&
                   base.Equals(obj) && 
                   IdInscripcion == dTO.IdInscripcion &&
                   FechaInscripcion == dTO.FechaInscripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdInscripcion, FechaInscripcion);
        }
    }
}