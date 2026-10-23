using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.CompeticionDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                .Select(i => new InscripcionDetailDTO(
                    i.Id, 
                    i.NombreUsuario, 
                    i.ApellidosUsuario,
                    i.Dni, 
                    i.Telefono,
                    i.MetodoPago,
                    i.FechaInscripcion, 
                    i.PrecioTotal,
                    i.CompeticionInscrita
                        .Select(ci => new CompeticionItemDTO(
                            ci.Competicion!.Id,
                            ci.Competicion.Nombre, 
                            ci.Competicion.TipoDeporte!.Nombre,
                            ci.Competicion.Fecha, 
                            ci.Competicion.Precio, 
                            ci.ProblemasFisicos)).ToList<CompeticionItemDTO>()))
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