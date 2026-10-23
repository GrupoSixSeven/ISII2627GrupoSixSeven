using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AlquilerDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilerForCreateDTO alquilerForCreate)
        {
            
            if (alquilerForCreate.FechaAlquiler.Date < DateTime.Today)
                ModelState.AddModelError("FechaAlquiler", "¡Error! La fecha de alquiler no puede ser anterior a la fecha actual.");

            if (alquilerForCreate.Materiales == null || alquilerForCreate.Materiales.Count == 0)
                ModelState.AddModelError("Materiales", "¡Error! Debes incluir al menos un material para alquilar.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
            var nombresMateriales = alquilerForCreate.Materiales!.Select(m => m.NombreMaterial).ToList();

            
            var materialesDB = _context.Materiales
                .Include(m => m.MaterialAlquilados)
                    .ThenInclude(ma => ma.Alquiler)
                .Where(m => nombresMateriales.Contains(m.Nombre))
                .Select(m => new {
                    m.IdMaterial,
                    m.Nombre,
                    m.Cantidad,
                    m.Precio,
                   
                    CantidadYaAlquilada = m.MaterialAlquilados
                        .Where(ma => ma.Alquiler != null && ma.Alquiler.FechaAlquiler.Date == alquilerForCreate.FechaAlquiler.Date)
                        .Sum(ma => ma.Cantidad)
                })
                .ToList();

            // Instanciamos la entidad principal Alquiler (Corregidas las mayúsculas)
            Alquiler alquiler = new Alquiler
            {
            
                MetodoPago = alquilerForCreate.MetodoPago,
                FechaAlquiler = alquilerForCreate.FechaAlquiler,
                PrecioTotal = 0,
                MaterialesAlquilados = new List<MaterialAlquilado>()
            };
            

            
            foreach (var item in alquilerForCreate.Materiales!)
            {
                var material = materialesDB.FirstOrDefault(m => m.Nombre == item.NombreMaterial);
                
                
                if (material == null)
                {
                    ModelState.AddModelError("Materiales", $"¡Error! El material '{item.NombreMaterial}' no existe en el catálogo.");
                }
                else if ((material.CantidadYaAlquilada + item.Cantidad) > material.Cantidad)
                {
                    ModelState.AddModelError("Materiales", $"¡Error! No hay suficiente stock disponible para '{item.NombreMaterial}' en la fecha solicitada.");
                }
                else
                {
                   
                    alquiler.MaterialesAlquilados.Add(new MaterialAlquilado
                    {
                        IdMaterial = material.IdMaterial,
                        Alquiler = alquiler,
                        Cantidad = item.Cantidad,
                        Precio = material.Precio,
                        Descripcion = item.Descripcion
                    });
                    
                    
                    item.Precio = material.Precio;
                }
            }

            
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            
            alquiler.PrecioTotal = (double)alquiler.MaterialesAlquilados.Sum(ma => ma.Precio * ma.Cantidad);

            _context.Alquileres.Add(alquiler);

            try
            {
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Alquiler", "¡Error! Se produjo un problema al guardar el alquiler. Inténtalo de nuevo más tarde.");
                return Conflict(new ValidationProblemDetails(ModelState));
            }

            
            var alquilerDetail = new AlquilerDetailDTO(
                alquiler.IdAlquiler, 
                alquiler.FechaAlquiler,
                alquiler.MetodoPago,
                alquilerForCreate.Materiales // El DTO ya tiene los precios reales actualizados
            );

            
            return CreatedAtAction("GetAlquiler", new { id = alquiler.IdAlquiler }, alquilerDetail);
        }
    }
}