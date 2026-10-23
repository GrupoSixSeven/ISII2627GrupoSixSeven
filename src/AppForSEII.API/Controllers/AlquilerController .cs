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
        [Route("[action]")] // Nota: Es mejor dejar el /{id} en la ruta para que sea un endpoint REST válido (api/Alquiler/GetAlquiler/5)
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {
            if (_context.Alquileres == null)
            {
                _logger.LogError("Error: La tabla Alquileres no existe");
                return NotFound();
            }

            var alquiler = await _context.Alquileres
                .Where(a => a.IdAlquiler == id)
                .Select(a => new AlquilerDetailDTO(
                    a.IdAlquiler,
                    a.FechaAlquiler,
                    a.MetodoPago,
                    a.MaterialesAlquilados
                        .Select(ma => new AlquilerItemDTO(
                            ma.Material!.IdMaterial,
                            ma.Material.Nombre,
                            ma.Material.TipoMaterial!.NombreTipoMaterial,
                            ma.Material.TipoDeporte!.Nombre,
                            ma.Cantidad,
                            ma.Precio,
                            ma.Descripcion)).ToList<AlquilerItemDTO>()))
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