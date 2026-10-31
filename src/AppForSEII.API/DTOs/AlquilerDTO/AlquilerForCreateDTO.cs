using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AppForSEII.API.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AppForSEII.API.DTOs.AlquilerDTOs;

public class AlquilerForCreateDTO
{
    public AlquilerForCreateDTO()
    {
        Materiales = new List<AlquilerItemDTO>();

    }
 
    
    public AlquilerForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni, string email,
        string telefono, MetodoPago metodoPago, IList<AlquilerItemDTO> materiales, DateTime fechaAlquiler)
    {
        NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
        ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
        Dni = dni ?? throw new ArgumentNullException(nameof(dni));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Telefono = telefono ?? throw new ArgumentNullException(nameof(telefono));
        MetodoPago = metodoPago;
        Materiales = materiales ?? throw new ArgumentNullException(nameof(materiales));
        FechaAlquiler = fechaAlquiler;
    }
 
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
    public string NombreUsuario { get; set; } = string.Empty;
 
    [Required(AllowEmptyStrings = false, ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(100, ErrorMessage = "Los apellidos no pueden superar los 100 caracteres")]
    public string ApellidosUsuario { get; set; } = string.Empty;
 
    [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI es obligatorio")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "El DNI tiene que tener una longitud de 9 caracteres")]
    public string Dni { get; set; } = string.Empty;
 
    [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono de contacto es obligatorio")]
    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string Telefono { get; set; } = string.Empty;
 
    // También identifica al cliente en la BD (el controlador lo busca por email)
    [Required(AllowEmptyStrings = false, ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(50, ErrorMessage = "El email no puede superar los 50 caracteres")]
    public string Email { get; set; } = string.Empty;
 
    [Required(ErrorMessage = "El método de pago es obligatorio")]
    public MetodoPago MetodoPago { get; set; } = MetodoPago.Tarjeta;
 
    [Required(ErrorMessage = "La fecha de alquiler es obligatoria")]
    public DateTime FechaAlquiler { get; set; } = DateTime.Now;
 
    [Required(ErrorMessage = "Debe seleccionar al menos un material")]
    [MinLength(1, ErrorMessage = "Debe seleccionar al menos un material")]
    public IList<AlquilerItemDTO> Materiales { get; set; } = new List<AlquilerItemDTO>();
 
    
    public decimal PrecioTotal
    {
        get
        {
            return Materiales.Sum(m => m.Precio * m.Cantidad);
        }
    }
 
    protected bool CompareDate(DateTime date1, DateTime date2)
    {
        return Math.Abs((date1 - date2).TotalMinutes) < 1;
    }
 
    public override bool Equals(object? obj)
    {
        return obj is AlquilerForCreateDTO dTO &&
               CompareDate(FechaAlquiler, dTO.FechaAlquiler) &&
               NombreUsuario == dTO.NombreUsuario &&
               ApellidosUsuario == dTO.ApellidosUsuario &&
               Dni == dTO.Dni &&
               Email == dTO.Email &&
               Telefono == dTO.Telefono &&
               MetodoPago == dTO.MetodoPago &&
               Materiales.SequenceEqual(dTO.Materiales) &&
               PrecioTotal == dTO.PrecioTotal;
    }
 
    public override int GetHashCode()
    {
        return HashCode.Combine(FechaAlquiler, Dni, Email, MetodoPago);
    }
}