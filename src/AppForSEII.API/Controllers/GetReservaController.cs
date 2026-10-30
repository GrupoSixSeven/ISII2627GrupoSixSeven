using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetReservaController> _logger;
 
        public GetReservaController(ApplicationDbContext context, ILogger<GetReservaController> logger)
        {
            _context = context;
            _logger = logger;
        }
 
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReserva(int id)
        {
            if (_context.Reservas == null)
            {
                _logger.LogError("Error: La tabla Reservas no existe");
                return NotFound();
            }
 
            var reserva = await _context.Reservas
                .Where(r => r.Id == id)
                .Include(r => r.Cliente)
                .Include(r => r.PistasReservadas)
                    .ThenInclude(pr => pr.Pista)
                        .ThenInclude(p => p!.TipoDeporte)
                .Select(r => new ReservaDetailDTO(
                    r.Id,
                    r.MetodoPago,
                    r.FechaReserva,
                    r.PrecioTotal,
                    r.PistasReservadas
                        .Select(pr => new ReservaItemDTO(
                            pr.IdPista,
                            pr.Pista!.NombrePista,
                            pr.Pista.TipoDeporte!.Nombre,
                            pr.Pista.NPersonas,
                            pr.Precio,
                            pr.Observaciones))
                        .ToList<ReservaItemDTO>(),
                    r.Cliente!.Name ?? "",
                    r.Cliente.Surname ?? "",
                    r.Cliente.DNI ?? ""))
                .FirstOrDefaultAsync();
 
            if (reserva == null)
            {
                _logger.LogError($"Error: La reserva con id {id} no existe");
                return NotFound();
            }
 
            return Ok(reserva);
        }
    }
}