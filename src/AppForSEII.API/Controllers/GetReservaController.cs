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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReservaDetailDTO>> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.PistasReservadas)
                    .ThenInclude(pr => pr.Pista!)
                        .ThenInclude(p => p.TipoDeporte!)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            var reservaDetail = new ReservaDetailDTO(
                reserva.Id,
                reserva.NombreCliente,
                reserva.Apellidos,
                reserva.Dni,
                reserva.MetodoPago.ToString(),
                reserva.FechaReserva,
                reserva.PrecioTotal,
                reserva.PistasReservadas.Select(pr => new ReservaItemDTO(
                    pr.IdPista,
                    pr.Pista!.NombrePista,
                    pr.Pista.TipoDeporte!.Nombre,
                    pr.Pista.NPersonas,
                    pr.Cantidad,
                    pr.Precio,
                    pr.Observaciones
                )).ToList()
            );

            return Ok(reservaDetail);
        }
    }
}