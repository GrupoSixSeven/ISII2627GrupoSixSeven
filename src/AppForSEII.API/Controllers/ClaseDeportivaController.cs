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
    public class ClaseDeportivaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClaseDeportivaController> _logger;

        public ClaseDeportivaController(ApplicationDbContext context, ILogger<ClaseDeportivaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClaseDeportivaParaReservarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClasesDeportivas(int? idTipoDeporte, string? monitor)
        {
            IList<ClaseDeportivaParaReservarDTO> selectClases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c => 
                    (idTipoDeporte == null || c.TipoDeporteId == idTipoDeporte)
                    && (string.IsNullOrEmpty(monitor) || c.Monitor.Contains(monitor))
                )
                .OrderBy(c => c.FechaHora)
                .Select(c => new ClaseDeportivaParaReservarDTO
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    FechaHora = c.FechaHora,
                    Lugar = c.Lugar,
                    Monitor = c.Monitor,
                    Nivel = c.Nivel,
                    PlazasDisponibles = c.PlazasDisponibles,
                    PrecioUnitario = c.PrecioUnitario,
                    TipoDeporte = c.TipoDeporte!.Nombre
                })
                .ToListAsync();

            return Ok(selectClases);
        }
    }
}