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
    public class GetReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetReservaController> _logger;

        public GetReservaController(ApplicationDbContext context, ILogger<GetReservaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReserva(ReservaForCreateDTO reservaForCreate)
        {
            if (reservaForCreate.FechaReserva.Date < DateTime.Today)
                ModelState.AddModelError("FechaReserva", "¡Error! La fecha de reserva no puede ser anterior a la fecha actual.");

            if (reservaForCreate.Pistas == null || reservaForCreate.Pistas.Count == 0)
                ModelState.AddModelError("Pistas", "¡Error! Debes incluir al menos una pista para reservar.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var idsPistas = reservaForCreate.Pistas!.Select(p => p.IdPista).ToList();

            var pistasDB = await _context.Pistas
                .Include(p => p.PistasReservadas)
                    .ThenInclude(pr => pr.Reserva)
                .Where(p => idsPistas.Contains(p.IdPista))
                .Select(p => new {
                    p.IdPista,
                    p.NombrePista,
                    p.Stock,
                    p.Precio,
                    CantidadYaReservada = p.PistasReservadas
                        .Where(pr => pr.Reserva != null && pr.Reserva.FechaReserva.Date == reservaForCreate.FechaReserva.Date)
                        .Sum(pr => pr.Cantidad)
                })
                .ToListAsync();

            Reserva reserva = new Reserva
            {
                NombreCliente = reservaForCreate.NombreCliente,
                Apellidos = reservaForCreate.Apellidos,
                Dni = reservaForCreate.Dni,
                MetodoPago = Enum.Parse<MetodoPago>(reservaForCreate.MetodoPago, true),
                FechaReserva = reservaForCreate.FechaReserva,
                PrecioTotal = 0,
                PistasReservadas = new List<PistaReservada>()
            };

            foreach (var item in reservaForCreate.Pistas!)
            {
                var pista = pistasDB.FirstOrDefault(p => p.IdPista == item.IdPista);

                if (pista == null)
                {
                    ModelState.AddModelError("Pistas", $"¡Error! La pista con ID '{item.IdPista}' no existe en el catálogo.");
                }
                else if ((pista.CantidadYaReservada + item.Cantidad) > pista.Stock)
                {
                    ModelState.AddModelError("Pistas", $"¡Error! No hay suficiente stock disponible para la pista '{pista.NombrePista}' en la fecha solicitada.");
                }
                else
                {
                    reserva.PistasReservadas.Add(new PistaReservada
                    {
                        IdPista = pista.IdPista,
                        Reserva = reserva,
                        Cantidad = item.Cantidad,
                        Precio = pista.Precio,
                        Observaciones = item.Observaciones
                    });

                    item.Precio = pista.Precio;
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            reserva.PrecioTotal = (decimal)reserva.PistasReservadas.Sum(pr => pr.Precio * pr.Cantidad);

            _context.Reservas.Add(reserva);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Reserva", "¡Error! Se produjo un problema al guardar la reserva. Inténtalo de nuevo más tarde.");
                return Conflict(new ValidationProblemDetails(ModelState));
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

            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reservaDetail);
        }
    }
}