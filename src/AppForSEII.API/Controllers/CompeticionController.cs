using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.CompeticionParaInscribirseDTO;

namespace AppForSEII.API.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class CompeticionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompeticionController> _logger;

        public CompeticionController(ApplicationDbContext context, ILogger<CompeticionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CompeticionesParaInscribirseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetCompeticionesParaInscribirse(string? lugar, string? tipoDeporte)
        {
            IList<CompeticionesParaInscribirseDTO> competicionesDisponibles = await _context.Competiciones
                .Where(c => c.Plazas > 0 
                            && (string.IsNullOrEmpty(lugar) || c.Lugar.Contains(lugar)) 
                            && (string.IsNullOrEmpty(tipoDeporte) || c.TipoDeporte.Nombre.Contains(tipoDeporte)))
                .OrderBy(c => c.Fecha)
                .Select(c => new CompeticionesParaInscribirseDTO 
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    TipoDeporte = c.TipoDeporte.Nombre,
                    Fecha = c.Fecha,
                    Lugar = c.Lugar,
                    Precio = c.Precio
                })
                .ToListAsync();

            if (!competicionesDisponibles.Any())
            {
                return NotFound("No hay competiciones con plazas disponibles que coincidan con los criterios.");
            }

            return Ok(competicionesDisponibles);
        }
    }
