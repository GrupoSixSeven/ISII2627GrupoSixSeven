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
                            ci.Competicion!.Id, //EL ! SIRVE PARA DECIR AL PROGRAMA QUE NO ENTRA NULL 100%, AL HACER INCLUDE NUNCA LLEGARA NULL
                            ci.Competicion.Nombre, 
                            ci.Competicion.TipoDeporte!.Nombre,
                            ci.Competicion.Fecha, 
                            ci.Competicion.Precio, 
                            ci.ProblemasFisicos)).ToList<CompeticionItemDTO>(),
                    i.DatosPago ?? "",
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
    }
}