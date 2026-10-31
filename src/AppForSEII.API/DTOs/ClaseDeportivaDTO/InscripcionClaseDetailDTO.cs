using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ClaseDeportivaDTO
{
    public class InscripcionClaseDetailDTO : InscripcionClaseForCreateDTO
    {
        public InscripcionClaseDetailDTO(int idInscripcion, MetodoPago metodoPago, DateTime fechaInscripcion, decimal precioTotal, IList<InscripcionItemDTO> clasesInscritas, string datosPago, string nombreUsuario, string apellidosUsuario, string telefono, string dni, string correo)
            : base(metodoPago, precioTotal, clasesInscritas, datosPago, fechaInscripcion)
        {
            IdInscripcion = idInscripcion;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Telefono = telefono;
            Dni = dni;
            Correo = correo;
        }

        public int IdInscripcion { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string Telefono { get; set; }
        public string Dni { get; set; }
        public string Correo { get; set; }

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