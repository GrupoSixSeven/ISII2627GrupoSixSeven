using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(ApplicationDbContext context, ILogger<ReservaController> logger)
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
                    r.Cliente!.Name ?? "",
                    r.Cliente.Surname ?? "",
                    r.Cliente.DNI ?? "",
                    r.Cliente.Email ?? "",
                    r.Cliente.PhoneNumber ?? "",
                    r.MetodoPago,
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
                    "",
                    r.FechaReserva))
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                _logger.LogError($"Error: La reserva con id {id} no existe");
                return NotFound();
            }

            return Ok(reserva);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReserva(ReservaForCreateDTO reservaForCreate)
        {
            if (reservaForCreate.FechaReserva.Date < DateTime.Today)
                ModelState.AddModelError("FechaReserva", "Error! La fecha de su reserva no puede ser anterior a hoy");

            if (reservaForCreate.Pistas.Count == 0)
                ModelState.AddModelError("Pistas", "Error! Debe incluir al menos una pista para reservar");

            if (reservaForCreate.Pistas.GroupBy(p => p.IdPista).Any(g => g.Count() > 1))
                ModelState.AddModelError("Pistas", "Error! Hay pistas repetidas en el carrito");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(au => au.Email == reservaForCreate.Email);
            if (user == null)
                ModelState.AddModelError("ReservaApplicationUser", "Error! No hay ningún usuario registrado con ese email");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var pistaIds = reservaForCreate.Pistas.Select(p => p.IdPista).ToList<int>();
            var fecha = reservaForCreate.FechaReserva.Date;
            var fechaSiguiente = fecha.AddDays(1);

            var pistas = (await _context.Pistas
                .Where(p => pistaIds.Contains(p.IdPista))
                .Select(p => new {
                    p.IdPista,
                    p.NombrePista,
                    NombreTipoDeporte = p.TipoDeporte!.Nombre,
                    p.NPersonas,
                    p.Precio,
                    p.Stock,
                    NumberOfReservas = _context.Reservas
                        .Where(r => r.FechaReserva >= fecha && r.FechaReserva < fechaSiguiente)
                        .SelectMany(r => r.PistasReservadas)
                        .Where(pr => pr.IdPista == p.IdPista)
                        .Sum(pr => (int?)pr.Cantidad) ?? 0
                })
                .ToListAsync());

            Reserva reserva = new Reserva
            {
                FechaReserva = reservaForCreate.FechaReserva,
                MetodoPago = reservaForCreate.MetodoPago,
                PrecioTotal = 0,
                Cliente = user,
                ClienteId = user!.Id
            };

            foreach (var item in reservaForCreate.Pistas)
            {
                var pista = pistas.FirstOrDefault(p => p.IdPista == item.IdPista);

                if ((pista == null) || (pista.NumberOfReservas + 1 > pista.Stock))
                {
                    ModelState.AddModelError("Pistas", $"Error! La pista con id '{item.IdPista}' no está disponible para el {fecha.ToShortDateString()}");
                }
                else
                {
                    reserva.PistasReservadas.Add(new PistaReservada
                    {
                        IdPista = pista.IdPista,
                        Cantidad = 1,
                        Precio = pista.Precio,
                        Observaciones = item.Observaciones
                    });
                    item.NombrePista = pista.NombrePista;
                    item.TipoDeporte = pista.NombreTipoDeporte;
                    item.NPersonas = pista.NPersonas;
                    item.Precio = pista.Precio;
                }
            }
            reserva.PrecioTotal = reserva.PistasReservadas.Sum(pr => pr.Precio * pr.Cantidad);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(reserva);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Reserva", $"Error! Se produjo un error al guardar su reserva, inténtelo de nuevo más tarde");
                return Conflict("Error" + ex.Message);
            }

            var reservaDetail = new ReservaDetailDTO(reserva.Id,
                reservaForCreate.NombreUsuario, reservaForCreate.ApellidosUsuario,
                reservaForCreate.Dni, reservaForCreate.Email, reservaForCreate.Telefono,
                reserva.MetodoPago, reserva.PrecioTotal, reservaForCreate.Pistas,
                reservaForCreate.DatosPago, reserva.FechaReserva);

            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reservaDetail);
        }
    }
}