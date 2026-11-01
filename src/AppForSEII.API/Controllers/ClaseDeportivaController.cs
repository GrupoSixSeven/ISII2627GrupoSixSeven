using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseDeportivaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClaseDeportivaController> _logger;

        public ClaseDeportivaController(ApplicationDbContext context, ILogger<ClaseDeportivaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClaseDeportivaParaReservarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClasesDeportivas(int? idTipoDeporte, string? monitor, DateTime? fecha)
        {
            var query = _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .AsQueryable();

            if (idTipoDeporte.HasValue)
            {
                query = query.Where(c => c.TipoDeporteId == idTipoDeporte.Value);
            }

            if (!string.IsNullOrEmpty(monitor))
            {
                query = query.Where(c => c.Monitor != null && c.Monitor.Contains(monitor));
            }

            if (fecha.HasValue)
            {
                query = query.Where(c => c.FechaHora.Date == fecha.Value.Date);
            }

            var selectClases = await query
                .OrderBy(c => c.FechaHora)
                .Select(c => new ClaseDeportivaParaReservarDTO
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    FechaHora = c.FechaHora,
                    Lugar = c.Lugar,
                    Monitor = c.Monitor,
                    Nivel = c.Nivel,
                    PlazasDisponibles = c.PlazasDisponibles,
                    PrecioUnitario = c.PrecioUnitario,
                    TipoDeporte = c.TipoDeporte != null ? c.TipoDeporte.Nombre : string.Empty
                })
                .ToListAsync();

            return Ok(selectClases);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionClaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateInscripcion(InscripcionClaseForCreateDTO inscripcionForCreate)
        {
            if (inscripcionForCreate.ClasesInscritas.Count == 0)
                ModelState.AddModelError("ClasesInscritas", "Error! Debe incluir al menos una clase deportiva");

            // Paso 3: plaza del cliente + 2 acompañantes como máximo
            if (inscripcionForCreate.ClasesInscritas.Any(c => c.PlazasReservadas < 1 || c.PlazasReservadas > 3))
                ModelState.AddModelError("ClasesInscritas", "Error! Puede reservar entre 1 y 3 plazas por clase (usted y 2 acompañantes como máximo)");

            if (inscripcionForCreate.ClasesInscritas.GroupBy(c => c.ClaseDeportivaId).Any(g => g.Count() > 1))
                ModelState.AddModelError("ClasesInscritas", "Error! Hay clases repetidas en el carrito");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(au => au.Email == inscripcionForCreate.Email);
            if (user == null)
                ModelState.AddModelError("InscripcionApplicationUser", "Error! No hay ningún usuario registrado con ese email");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var clasesIds = inscripcionForCreate.ClasesInscritas.Select(c => c.ClaseDeportivaId).ToList<int>();

            // Necesitamos las entidades para poder descontar las plazas disponibles
            var clases = await _context.ClasesDeportivas
                .Include(c => c.TipoDeporte)
                .Where(c => clasesIds.Contains(c.Id))
                .ToListAsync();

            Inscripcion inscripcion = new Inscripcion
            {
                FechaInscripcion = DateTime.Now,
                MetodoPago = inscripcionForCreate.MetodoPago,
                DatosPago = inscripcionForCreate.DatosPago,
                PrecioTotal = 0,
                Cliente = user,
                ClienteId = user!.Id
            };

            foreach (var item in inscripcionForCreate.ClasesInscritas)
            {
                var clase = clases.FirstOrDefault(c => c.Id == item.ClaseDeportivaId);

                if ((clase == null) || (clase.FechaHora < DateTime.Now) || (clase.PlazasDisponibles < item.PlazasReservadas))
                {
                    ModelState.AddModelError("ClasesInscritas", $"Error! La clase con id '{item.ClaseDeportivaId}' no está disponible o no tiene plazas suficientes");
                }
                else
                {
                    inscripcion.ClasesInscritas.Add(new ClaseInscrita
                    {
                        ClaseDeportiva = clase,
                        ClaseDeportivaId = clase.Id,
                        PlazasReservadas = item.PlazasReservadas,
                        Precio = clase.PrecioUnitario,
                        Observaciones = item.Observaciones
                    });

                    // Paso 7: se descuentan las plazas reservadas
                    clase.PlazasDisponibles -= item.PlazasReservadas;

                    item.Descripcion = clase.Descripcion;
                    item.TipoDeporte = clase.TipoDeporte!.Nombre;
                    item.Monitor = clase.Monitor;
                    item.FechaHora = clase.FechaHora;
                    item.Precio = clase.PrecioUnitario;
                }
            }
            inscripcion.PrecioTotal = inscripcion.ClasesInscritas.Sum(ci => ci.Precio * ci.PlazasReservadas);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(inscripcion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Inscripcion", $"Error! Se produjo un error al guardar su inscripción, inténtelo de nuevo más tarde");
                return Conflict("Error" + ex.Message);
            }

            // Los datos del cliente se recuperan de su perfil (paso 5 del caso de uso)
            var inscripcionDetail = new InscripcionClaseDetailDTO(inscripcion.Id,
                inscripcion.MetodoPago, inscripcion.FechaInscripcion, inscripcion.PrecioTotal,
                inscripcionForCreate.ClasesInscritas, inscripcion.DatosPago,
                user.Name ?? "", user.Surname ?? "", user.PhoneNumber ?? "",
                user.DNI ?? "", user.Email ?? "");

            // El GET del details está en InscripcionClaseController
            return CreatedAtAction("GetInscripcion", "InscripcionClase", new { id = inscripcion.Id }, inscripcionDetail);
        }
    }
}