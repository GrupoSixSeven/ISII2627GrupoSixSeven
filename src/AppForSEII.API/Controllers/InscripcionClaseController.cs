using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionClaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionClaseController> _logger;

        public InscripcionClaseController(ApplicationDbContext context, ILogger<InscripcionClaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

       
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionClaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetInscripcion(int id)
        {
            if (_context.Inscripciones == null)
            {
                _logger.LogError("Error: La tabla Inscripciones no existe");
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones
                .Where(i => i.Id == id)
                .Include(i => i.Cliente)
                .Include(i => i.ClasesInscritas)
                    .ThenInclude(ci => ci.ClaseDeportiva)
                        .ThenInclude(clase => clase!.TipoDeporte)
                .Select(i => new InscripcionClaseDetailDTO(
                    i.Id,
                    i.MetodoPago,
                    i.FechaInscripcion,
                    i.PrecioTotal,
                    i.ClasesInscritas
                        .Select(ci => new InscripcionItemDTO(
                            ci.ClaseDeportiva!.Id, //EL ! SIRVE PARA DECIR AL PROGRAMA QUE NO ENTRA NULL 100%, AL HACER INCLUDE NUNCA LLEGARA NULL
                            ci.ClaseDeportiva.Descripcion,
                            ci.ClaseDeportiva.TipoDeporte!.Nombre,
                            ci.ClaseDeportiva.Monitor,
                            ci.ClaseDeportiva.FechaHora,
                            ci.PlazasReservadas,
                            ci.Precio,
                            ci.Observaciones)).ToList<InscripcionItemDTO>(),
                    "", 
                    i.Cliente!.Name ?? "",
                    i.Cliente.Surname ?? "",
                    i.Cliente.PhoneNumber ?? "",
                    i.Cliente.DNI ?? "",
                    i.Cliente.Email ?? ""))
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: La inscripción con id {id} no existe");
                return NotFound();
            }

            return Ok(inscripcion);
        }

        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<InscripcionItemDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetClasesDisponibles(string? tipoDeporte, string? monitor, DateTime? fecha)
        {
            if (_context.ClasesDeportivas == null)
            {
                _logger.LogError("Error: La tabla ClasesDeportivas no existe");
                return NotFound();
            }

            var clases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c => c.PlazasDisponibles > 0
                    && (tipoDeporte == null || c.TipoDeporte!.Nombre.Contains(tipoDeporte))
                    && (monitor == null || c.Monitor.Contains(monitor))
                    && (fecha == null || c.FechaHora.Date == fecha.Value.Date))
                .OrderBy(c => c.FechaHora)
                .Select(c => new InscripcionItemDTO(
                    c.Id,
                    c.Descripcion,
                    c.TipoDeporte!.Nombre,
                    c.Monitor,
                    c.FechaHora,
                    c.PlazasDisponibles, // aquí van las plazas libres de la clase
                    c.PrecioUnitario,
                    null))
                .ToListAsync();

            // Flujo alternativo 0: si la lista viene vacía, no hay clases con plazas disponibles
            return Ok(clases);
        }
    }
}