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
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.PistasReservadas)
                    .ThenInclude(pr => pr.Pista)
                        .ThenInclude(p => p!.TipoDeporte)
                .Where(r => r.Id == id)
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
                        .ToList(),
                    r.Cliente!.Name ?? "",
                    r.Cliente.Surname ?? "",
                    r.Cliente.DNI ?? "",
                    r.Cliente.Email ?? "",
                    r.Cliente.PhoneNumber ?? "",
                    r.DatosPago))
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
            if (reservaForCreate.Pistas.Count == 0)
                ModelState.AddModelError("Pistas", "Error! Debe incluir al menos una pista reservada");

            if (reservaForCreate.Pistas.GroupBy(p => p.IdPista).Any(g => g.Count() > 1))
                ModelState.AddModelError("Pistas", "Error! Hay pistas repetidas en la reserva");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(au => au.Email == reservaForCreate.Email);

            if (user == null)
                ModelState.AddModelError("ReservaApplicationUser", "Error! No hay ningún usuario registrado con ese email");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var pistasIds = reservaForCreate.Pistas.Select(p => p.IdPista).ToList();

            var pistas = await _context.Pistas
                .Include(p => p.TipoDeporte)
                .Where(p => pistasIds.Contains(p.IdPista))
                .ToListAsync();

            Reserva reserva = new Reserva
            {
                FechaReserva = reservaForCreate.FechaReserva,
                MetodoPago = reservaForCreate.MetodoPago,
                DatosPago = reservaForCreate.DatosPago,
                PrecioTotal = 0,
                Cliente = user,
                ClienteId = user!.Id
            };

            foreach (var item in reservaForCreate.Pistas)
            {
                var pista = pistas.FirstOrDefault(p => p.IdPista == item.IdPista);

                if ((pista == null) || (pista.Stock < 1))
                {
                    ModelState.AddModelError("Pistas", $"Error! La pista con id '{item.IdPista}' no está disponible o no tiene stock suficiente");
                }
                else
                {
                    reserva.PistasReservadas.Add(new PistaReservada
                    {
                        Pista = pista,
                        IdPista = pista.IdPista,
                        Reserva = reserva,
                        Cantidad = 1,
                        Precio = pista.Precio,
                        Observaciones = item.Observaciones
                    });

                    pista.Stock -= 1;

                    item.NombrePista = pista.NombrePista;
                    item.TipoDeporte = pista.TipoDeporte!.Nombre;
                    item.NPersonas = pista.NPersonas;
                    item.Precio = pista.Precio;
                }
            }

            reserva.PrecioTotal = reserva.PistasReservadas.Sum(pr => pr.Precio * pr.Cantidad);

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Reservas.Add(reserva);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Reserva", "Error! Se produjo un error al guardar su reserva, inténtelo de nuevo más tarde");
                return Conflict("Error " + ex.Message);
            }

            var reservaDetail = new ReservaDetailDTO(
                reserva.Id,
                reserva.MetodoPago,
                reserva.FechaReserva,
                reserva.PrecioTotal,
                reservaForCreate.Pistas,
                user!.Name ?? "",
                user.Surname ?? "",
                user.DNI ?? "",
                user.Email ?? "",
                user.PhoneNumber ?? "",
                reserva.DatosPago
            );

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reservaDetail);
        }
    }
}