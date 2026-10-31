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


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {
            var alquiler = await _context.Alquileres
                .Where(a => a.IdAlquiler == id)
                    .Include(a => a.Cliente) 
                    .Include(a => a.MaterialesAlquilados) 
                        .ThenInclude(ma => ma.Material) 
                            .ThenInclude(material => material!.TipoMaterial) 
                .Select(a => new AlquilerDetailDTO(
                    a.IdAlquiler,
                    a.Cliente!.Name ?? "",
                    a.Cliente.Surname ?? "",
                    a.Cliente.DNI ?? "",
                    a.Cliente.Email ?? "",
                    a.Cliente.PhoneNumber ?? "",
                    a.MetodoPago,
                    a.MaterialesAlquilados.Select(ma => new AlquilerItemDTO(
                        ma.IdMaterial,
                        ma.Material!.Nombre,
                        ma.Material.TipoMaterial!.NombreTipoMaterial,
                        ma.Material.TipoDeporte!.Nombre,     // verifica el nombre real de la propiedad
                        ma.Cantidad,                          // lo alquilado, no el stock
                        ma.Precio,                            // precio al alquilar
                        ma.Descripcion)).ToList<AlquilerItemDTO>(),
                    a.FechaAlquiler))
                .FirstOrDefaultAsync();
 
            if (alquiler == null)
            {
                _logger.LogError($"Error: El alquiler con id {id} no existe");
                return NotFound();
            }
 
            return Ok(alquiler);
        }
    }    
        
}