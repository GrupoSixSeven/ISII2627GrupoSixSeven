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
        [ProducesResponseType(typeof(IList<AlquilerDetailDTO>), (int)HttpStatusCode.OK)]
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
                    .Include(a => a.Cliente)
                    .Include(a => a.MaterialesAlquilados)
                        .ThenInclude(ma => ma.Material)
                            .ThenInclude(material => material!.TipoMaterial)
                    .Select(i => new AlquilerDetailDTO(
                        i.IdAlquiler,
                        i.FechaAlquiler,
                        i.PrecioTotal,
                        i.MetodoPago,
                        i.MaterialesAlquilados.Select(ma => new AlquilerItemDTO(
                            ma.Material!.IdMaterial,
                            ma.Material.Nombre,
                            ma.Material.TipoMaterial!.NombreTipoMaterial,
                            ma.Material.TipoDeporte!.Nombre,
                            ma.Material.Cantidad,
                            ma.Material.Precio,
                            ma.Descripcion
                        )).ToList<AlquilerItemDTO>(),
                        i.Cliente!.Name?? "",
                        i.Cliente!.Surname?? "",
                        i.Cliente.Email ?? "",
                        i.Cliente.PhoneNumber ?? "",
                        i.Cliente.DNI?? ""
                    ))
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