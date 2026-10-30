using System;
using System.Collections.Generic;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
using AppForSEII.API.DTOs.CompeticionDTOs;

namespace AppForSEII.API.DTOs.InscripcionDTO
{
    public class InscripcionDetailDTO : InscripcionForCreateDTO
    {
        public InscripcionDetailDTO(
            int idInscripcion,
            MetodoPago metodoPago,
            DateTime fechaInscripcion,
            decimal precioTotal,
            IList<CompeticionItemDTO> competiciones,
            string datosPago,
            string nombreUsuario,
            string apellidosUsuario,
            string telefono,
            string dni,
            string correo,
            IList<InscripcionItemDTO> clasesInscritas)
            : base(metodoPago, precioTotal, competiciones, datosPago, fechaInscripcion,
                   nombreUsuario, apellidosUsuario, dni, telefono, clasesInscritas)
        {
            // Solo lo que el padre NO inicializa
            IdInscripcion = idInscripcion;
            Correo = correo;
        }

        public int IdInscripcion { get; set; }

        public string Correo { get; set; } = "";

        public override bool Equals(object? obj)
        {
            return obj is InscripcionDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdInscripcion == dTO.IdInscripcion &&
                   Correo == dTO.Correo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdInscripcion, Correo);
        }
    }
}