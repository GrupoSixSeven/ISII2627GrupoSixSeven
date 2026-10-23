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
        public async Task<ActionResult> GetClasesDeportivas(int? idTipoDeporte, string? monitor, DateTime? fecha)
        {
            var query = _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .AsQueryable();

            if (idTipoDeporte.HasValue)
            {
                query = query.Where(c => c.TipoDeporteId == idTipoDeporte.Value);
            }

            if (!string.IsNullOrEmpty(monitor))
            {
                query = query.Where(c => c.Monitor != null && c.Monitor.Contains(monitor));
            }

            if (fecha.HasValue)
            {
                query = query.Where(c => c.FechaHora.Date == fecha.Value.Date);
            }

            var selectClases = await query
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
                    TipoDeporte = c.TipoDeporte != null ? c.TipoDeporte.Nombre : string.Empty
                })
                .ToListAsync();

            return Ok(selectClases);
        }
    }
}