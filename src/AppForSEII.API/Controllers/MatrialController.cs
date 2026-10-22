using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaterialController> _logger;

        public MaterialController(ApplicationDbContext context, ILogger<MaterialController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MaterialParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetMaterialesParaAlquilar(decimal? precioMaximo, int? idTipoMaterial)
        {
            IList<MaterialParaAlquilarDTO> selectMateriales = await _context.Materiales
                
                .Include(m => m.TipoMaterial)
                .Include(m => m.TipoDeporte)
                
                
                .Where(m => 
                    (precioMaximo == null || m.Precio <= precioMaximo) 
                    && (idTipoMaterial == null || m.IdTipoMaterial == idTipoMaterial)
                )
                
                
                .OrderBy(m => m.Nombre)
                
                
                .Select(m => new MaterialParaAlquilarDTO
                {
                    IdMaterial = m.IdMaterial,
                    Nombre = m.Nombre,
                    Precio = m.Precio,
                    Cantidad = m.Cantidad,
                    TipoMaterial = m.TipoMaterial!.NombreTipoMaterial,
                    TipoDeporte = m.TipoDeporte!.Nombre
                })
                .ToListAsync();

            return Ok(selectMateriales);
        }
    }
}