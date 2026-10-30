using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.CompeticionDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
using AppForSEII.API.DTOs.InscripcionDTO;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionController> _logger;

        public InscripcionController(ApplicationDbContext context, ILogger<InscripcionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CASO DE USO: Inscribirse a competición (versión de Alejandro, sin clases)
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionDetailDTO), (int)HttpStatusCode.OK)]
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
                .Include(i => i.CompeticionInscrita)
                    .ThenInclude(ci => ci.Competicion)
                        .ThenInclude(competicion => competicion!.TipoDeporte)
                .Select(i => new InscripcionDetailDTO(
                    i.Id,
                    i.MetodoPago,
                    i.FechaInscripcion,
                    i.PrecioTotal,
                    i.CompeticionInscrita
                        .Select(ci => new CompeticionItemDTO(
                            ci.Competicion!.Id, // El ! indica que, al hacer Include, nunca llegará null
                            ci.Competicion.Nombre,
                            ci.Competicion.TipoDeporte!.Nombre,
                            ci.Competicion.Fecha,
                            ci.Competicion.Precio,
                            ci.ProblemasFisicos)).ToList<CompeticionItemDTO>(),
                    "", /* i.DatosPago // descomentar cuando exista la propiedad, y quitar el "" */
                    i.Cliente!.Name ?? "",
                    i.Cliente.Surname ?? "",
                    i.Cliente.PhoneNumber ?? "",
                    i.Cliente.DNI ?? "",
                    i.Cliente.Email ?? "",
                    new List<InscripcionItemDTO>()))
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: La inscripción con id {id} no existe");
                return NotFound();
            }

            return Ok(inscripcion);
        }

        // CASO DE USO: Inscribirse a clases deportivas (sin competiciones)
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetInscripcionClases(int id)
        {
            if (_context.Inscripciones == null)
            {
                _logger.LogError("Error: La tabla Inscripciones no existe");
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones
                .Where(i => i.Id == id)
                .Select(i => new InscripcionDetailDTO(
                    i.Id,
                    i.MetodoPago,
                    i.FechaInscripcion,
                    i.PrecioTotal,
                    new List<CompeticionItemDTO>(),
                    i.MetodoPago.ToString(), // sustituir por i.DatosPago ?? "" cuando exista la propiedad
                    i.Cliente!.Name ?? "",
                    i.Cliente.Surname ?? "",
                    i.Cliente.PhoneNumber ?? "",
                    i.Cliente.DNI ?? "",
                    i.Cliente.Email ?? "",
                    i.ClasesInscritas
                        .Select(ci => new InscripcionItemDTO(
                            ci.ClaseDeportivaId,
                            ci.ClaseDeportiva!.Descripcion,
                            ci.ClaseDeportiva.TipoDeporte!.Nombre,
                            ci.ClaseDeportiva.Monitor,
                            ci.ClaseDeportiva.FechaHora,
                            ci.PlazasReservadas,
                            ci.Precio,
                            ci.Observaciones ?? "")).ToList<InscripcionItemDTO>()
                ))
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: La inscripción con id {id} no existe");
                return NotFound();
            }

            return Ok(inscripcion);
        }
    }
}