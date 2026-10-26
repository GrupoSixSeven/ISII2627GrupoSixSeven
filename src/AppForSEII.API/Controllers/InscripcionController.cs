using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.InscripcionDTO;
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateInscripcion(InscripcionForCreateDTO inscripcionForCreate)
        {
            // 1. Validaciones iniciales del formulario de inscripción
            if (inscripcionForCreate.FechaInscripcion.Date < DateTime.Today)
                ModelState.AddModelError("FechaInscripcion", "¡Error! La fecha de inscripción no puede ser anterior a la fecha actual.");

            if (inscripcionForCreate.ClasesInscritas == null || inscripcionForCreate.ClasesInscritas.Count == 0)
                ModelState.AddModelError("ClasesInscritas", "¡Error! Debes incluir al menos una clase deportiva para inscribirte.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 2. Extraer los IDs de las clases deportivas solicitadas
            var idsClases = inscripcionForCreate.ClasesInscritas!.Select(c => c.ClaseDeportivaId).ToList();

            // 3. Consultar las clases en la base de datos junto con sus plazas reservadas actuales
            var clasesDB = await _context.ClasesDeportivas
                .Include(c => c.ClasesInscritas)
                .Where(c => idsClases.Contains(c.Id))
                .Select(c => new {
                    c.Id,
                    c.Descripcion,
                    c.PlazasDisponibles,
                    c.PrecioUnitario,
                    PlazasOcupadasActuales = c.ClasesInscritas.Sum(ci => ci.PlazasReservadas)
                })
                .ToListAsync();

            // 4. Instanciar la entidad principal de Inscripción
           // 4. Instanciar la entidad principal de Inscripción
Inscripcion inscripcion = new Inscripcion
{
    MetodoPago = inscripcionForCreate.MetodoPago,
    FechaInscripcion = inscripcionForCreate.FechaInscripcion,
    PrecioTotal = 0,
    ClasesInscritas = new List<ClaseInscrita>(),
    // Añade los campos requeridos mapeados desde el DTO:
    NombreUsuario = inscripcionForCreate.NombreUsuario,
    ApellidosUsuario = inscripcionForCreate.ApellidosUsuario,
    Dni = inscripcionForCreate.Dni,
    Telefono = inscripcionForCreate.Telefono,
    DatosPago = inscripcionForCreate.DatosPago
};

            // 5. Validar disponibilidad de plazas para cada clase solicitada
            foreach (var item in inscripcionForCreate.ClasesInscritas!)
            {
                var clase = clasesDB.FirstOrDefault(c => c.Id == item.ClaseDeportivaId);

                if (clase == null)
                {
                    ModelState.AddModelError("ClasesInscritas", $"¡Error! La clase deportiva con ID '{item.ClaseDeportivaId}' no existe.");
                }
                else if ((clase.PlazasOcupadasActuales + item.PlazasReservadas) > clase.PlazasDisponibles)
                {
                    ModelState.AddModelError("ClasesInscritas", $"¡Error! No hay suficientes plazas disponibles para la clase '{clase.Descripcion}'.");
                }
                else
                {
                    // Añadir la clase inscrita a la colección de la inscripción
                    inscripcion.ClasesInscritas.Add(new ClaseInscrita
                    {
                        ClaseDeportivaId = clase.Id,
                        Inscripcion = inscripcion,
                        PlazasReservadas = item.PlazasReservadas,
                        Precio = (clase.PrecioUnitario * item.PlazasReservadas),
                        Observaciones = item.Observaciones
                    });

                    // Actualizar el precio en el ítem del DTO
                    item.Precio = clase.PrecioUnitario;
                }
            }

            // Comprobar si hubo errores de validación de stock o existencia
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 6. Calcular el precio total de la inscripción
            inscripcion.PrecioTotal = inscripcion.ClasesInscritas.Sum(ci => ci.Precio);

            // 7. Añadir al contexto y guardar cambios en base de datos
            _context.Inscripciones.Add(inscripcion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Inscripcion", "¡Error! Se produjo un problema al guardar la inscripción. Inténtalo de nuevo más tarde.");
                return Conflict(new ValidationProblemDetails(ModelState));
            }

            // 8. Construir el DTO de detalle con la lista de `InscripcionItemDTO`
           // 8. Construir el DTO de detalle con todos los datos necesarios
var inscripcionDetail = new InscripcionDetailDTO(
    inscripcion.Id,
    inscripcion.FechaInscripcion,
    inscripcion.MetodoPago,
    inscripcion.PrecioTotal,
    inscripcionForCreate.NombreUsuario,
    inscripcionForCreate.ApellidosUsuario,
    inscripcionForCreate.Dni,
    inscripcionForCreate.Telefono,
    inscripcionForCreate.DatosPago,
    inscripcionForCreate.ClasesInscritas
);
            return CreatedAtAction("GetInscripcion", new { id = inscripcion.Id }, inscripcionDetail);
        }
    }
}