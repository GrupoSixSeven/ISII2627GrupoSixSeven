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
        public async Task<ActionResult> GetPistasParaAlquilar(string? tipoDeporte, int? nPersonas)
        {
            IList<PistaParaAlquilarDTO> selectPistas = await _context.Pistas
                .Include(p => p.TipoDeporte)
                .Where(p => 
                    (tipoDeporte == null || p.TipoDeporte!.Nombre == tipoDeporte) 
                    && (nPersonas == null || p.NPersonas >= nPersonas)
                )
                .OrderBy(p => p.NombrePista)
                .Select(p => new PistaParaAlquilarDTO
                {
                    IdPista = p.IdPista,
                    NombrePista = p.NombrePista,
                    NPersonas = p.NPersonas,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    NombreTipoDeporte = p.TipoDeporte!.Nombre
                })
                .ToListAsync();

            return Ok(selectPistas);
        }
    }
}