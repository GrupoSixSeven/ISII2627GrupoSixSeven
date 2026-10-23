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
    public class PistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PistasController> _logger;

        public PistasController(ApplicationDbContext context, ILogger<PistasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PistaParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPistasParaAlquilar(string? tipoDeporte, DateTime? fecha)
        {
            DateTime fechaFiltro = fecha ?? DateTime.Today;

            if (fechaFiltro.Date < DateTime.Today)
            {
                ModelState.AddModelError("Fecha", "¡Error! La fecha de consulta no puede ser anterior a la fecha actual.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var pistas = await _context.Pistas
                .Include(p => p.TipoDeporte)
                .Include(p => p.PistasReservadas)
                    .ThenInclude(pr => pr.Reserva)
                .Where(p => tipoDeporte == null || p.TipoDeporte!.Nombre == tipoDeporte)
                .OrderBy(p => p.NombrePista)
                .Select(p => new PistaParaAlquilarDTO
                {
                    IdPista = p.IdPista,
                    NombrePista = p.NombrePista,
                    NPersonas = p.NPersonas,
                    Precio = p.Precio,
                    Stock = p.Stock - p.PistasReservadas
                        .Where(pr => pr.Reserva != null && pr.Reserva.FechaReserva.Date == fechaFiltro.Date)
                        .Sum(pr => pr.Cantidad),
                    NombreTipoDeporte = p.TipoDeporte!.Nombre
                })
                .ToListAsync();

            if (pistas == null || pistas.Count == 0)
            {
                return NotFound();
            }

            return Ok(pistas);
        }
    }
}