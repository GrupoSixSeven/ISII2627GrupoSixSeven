using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ClaseDeportivaDTO
{
    public class InscripcionClaseDetailDTO : InscripcionClaseForCreateDTO
    {
        public InscripcionClaseDetailDTO(int idInscripcion, MetodoPago metodoPago, DateTime fechaInscripcion, decimal precioTotal, IList<InscripcionItemDTO> clasesInscritas, string datosPago, string nombreUsuario, string apellidosUsuario, string telefono, string dni, string correo)
            : base(nombreUsuario, apellidosUsuario, dni, correo, telefono, metodoPago, precioTotal, clasesInscritas, datosPago, fechaInscripcion)
        {
            IdInscripcion = idInscripcion;
        }

        public int IdInscripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionClaseDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdInscripcion == dTO.IdInscripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdInscripcion);
        }
    }
}