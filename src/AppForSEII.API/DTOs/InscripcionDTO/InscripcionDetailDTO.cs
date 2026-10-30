using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            : base(metodoPago, precioTotal, competiciones, datosPago, fechaInscripcion, nombreUsuario, apellidosUsuario, dni, telefono, clasesInscritas)
        {
            IdInscripcion = idInscripcion;
            FechaInscripcion = fechaInscripcion;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            Telefono = telefono;
            Dni = dni;
            correo = correo;
        }

        public int IdInscripcion { get; set; }

       [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public new DateTime FechaInscripcion { get; set; }

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