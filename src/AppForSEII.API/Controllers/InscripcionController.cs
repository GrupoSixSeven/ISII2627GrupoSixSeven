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
            var inscripcion = await _context.Inscripciones
                .Where(i => i.Id == id)
                    .Include(i => i.Cliente) 
                    .Include(i => i.CompeticionInscrita) 
                        .ThenInclude(ci => ci.Competicion) 
                            .ThenInclude(competicion => competicion!.TipoDeporte) 
                .Select(i => new InscripcionDetailDTO(
                    i.Id, 
                    i.Cliente!.Name ?? "",
                    i.Cliente.Surname ?? "",
                    i.Cliente.DNI ?? "",
                    i.Cliente.Email ?? "",
                    i.Cliente.PhoneNumber ?? "",
                    i.MetodoPago, 
                    i.CompeticionInscrita.Select(ci => new CompeticionItemDTO(
                        ci.Competicion!.Id, 
                        ci.Competicion.Nombre, 
                        ci.Competicion.TipoDeporte!.Nombre,
                        ci.Competicion.Fecha, 
                        ci.Competicion.Precio, 
                        ci.ProblemasFisicos)).ToList<CompeticionItemDTO>(),
                    i.DatosPago ?? "", 
                    i.FechaInscripcion))
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: La inscripción con id {id} no existe");
                return NotFound();
            }

            return Ok(inscripcion);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateInscripcion(InscripcionForCreateDTO inscripcionForCreate)
        {
            if (inscripcionForCreate.FechaInscripcion.Date < DateTime.Today)
                ModelState.AddModelError("FechaInscripcion", "Error! La fecha de su inscripción no puede ser anterior a hoy");

            if (inscripcionForCreate.Competiciones.Count == 0)
                ModelState.AddModelError("Competiciones", "Error! Debe incluir al menos una competición para inscribirse");

            if (inscripcionForCreate.Competiciones.GroupBy(c => c.Id).Any(g => g.Count() > 1))
                ModelState.AddModelError("Competiciones", "Error! Hay competiciones repetidas en el carrito");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(au => au.Email == inscripcionForCreate.Correo);
            if (user == null)
                ModelState.AddModelError("InscripcionApplicationUser", "Error! No hay ningún usuario registrado con ese email");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var competicionIds = inscripcionForCreate.Competiciones.Select(c => c.Id).ToList<int>();

            var competiciones = (await _context.Competiciones
                .Where(c => competicionIds.Contains(c.Id))
                .Select(c => new {
                    c.Id,
                    c.Nombre,
                    NombreTipoDeporte = c.TipoDeporte!.Nombre,
                    c.Fecha,
                    c.Precio,
                    c.Plazas,
                    NumberOfInscribedUsers = _context.Inscripciones
                        .SelectMany(i => i.CompeticionInscrita)
                        .Where(ci => ci.CompeticionId == c.Id)
                        .Count()
                })
                .ToListAsync());

            Inscripcion inscripcion = new Inscripcion
            {
                FechaInscripcion = inscripcionForCreate.FechaInscripcion,
                MetodoPago = inscripcionForCreate.MetodoPago,
                DatosPago = inscripcionForCreate.DatosPago,
                PrecioTotal = 0,
                Cliente = user,
                ClienteId = user!.Id
            };

            foreach (var item in inscripcionForCreate.Competiciones)
            {
                var competicion = competiciones.FirstOrDefault(c => c.Id == item.Id);

                if ((competicion == null) || (competicion.NumberOfInscribedUsers + 1 > competicion.Plazas))
                {
                    ModelState.AddModelError("Competiciones", $"Error! La competición con id '{item.Id}' no tiene plazas disponibles[cite: 5].");
                }
                else
                {
                    inscripcion.CompeticionInscrita.Add(new CompeticionInscrita
                    {
                        CompeticionId = competicion.Id,
                        ProblemasFisicos = item.Observaciones
                    });
                    item.Nombre = competicion.Nombre;
                    item.TipoDeporte = competicion.NombreTipoDeporte;
                    item.Fecha = competicion.Fecha;
                    item.Precio = competicion.Precio;
                }
            }
            
            inscripcion.PrecioTotal = inscripcion.CompeticionInscrita.Sum(ci => competiciones.First(c => c.Id == ci.CompeticionId).Precio);

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

            var inscripcionDetail = new InscripcionDetailDTO(inscripcion.Id,
                inscripcionForCreate.NombreUsuario, inscripcionForCreate.ApellidosUsuario,
                inscripcionForCreate.Dni, inscripcionForCreate.Correo, inscripcionForCreate.Telefono,
                inscripcion.MetodoPago, inscripcionForCreate.Competiciones, inscripcionForCreate.DatosPago, inscripcion.FechaInscripcion);

            return CreatedAtAction("GetInscripcion", new { id = inscripcion.Id }, inscripcionDetail);
        }
    }
}