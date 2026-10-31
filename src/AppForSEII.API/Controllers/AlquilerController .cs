using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AlquilerDTOs;
using AppForSEII.API.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {
            var alquiler = await _context.Alquileres
                .Where(a => a.IdAlquiler == id)
                    .Include(a => a.Cliente) 
                    .Include(a => a.MaterialesAlquilados) 
                        .ThenInclude(ma => ma.Material) 
                            .ThenInclude(material => material!.TipoMaterial) 
                .Select(a => new AlquilerDetailDTO(
                    a.IdAlquiler,
                    a.Cliente!.Name ?? "",
                    a.Cliente.Surname ?? "",
                    a.Cliente.DNI ?? "",
                    a.Cliente.Email ?? "",
                    a.Cliente.PhoneNumber ?? "",
                    a.MetodoPago,
                    a.MaterialesAlquilados.Select(ma => new AlquilerItemDTO(
                        ma.IdMaterial,
                        ma.Material!.Nombre,
                        ma.Material.TipoMaterial!.NombreTipoMaterial,
                        ma.Material.TipoDeporte!.Nombre,     
                        ma.Cantidad,                         
                        ma.Precio,                            
                        ma.Descripcion)).ToList<AlquilerItemDTO>(),
                    a.FechaAlquiler))
                .FirstOrDefaultAsync();
 
            if (alquiler == null)
            {
                _logger.LogError($"Error: El alquiler con id {id} no existe");
                return NotFound();
            }
 
            return Ok(alquiler);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilerForCreateDTO alquilerForCreate)
        {
           
            if (alquilerForCreate.FechaAlquiler.Date < DateTime.Today)
                ModelState.AddModelError("FechaAlquiler", "Error! La fecha de su alquiler no puede ser anterior a hoy");
 
            if (alquilerForCreate.Materiales.Count == 0)
                ModelState.AddModelError("Materiales", "Error! Debe incluir al menos un material para alquilar");
 
            if (alquilerForCreate.Materiales.Any(m => m.Cantidad < 1))
                ModelState.AddModelError("Materiales", "Error! La cantidad de cada material debe ser al menos 1");
 
            if (alquilerForCreate.Materiales.GroupBy(m => m.IdMaterial).Any(g => g.Count() > 1))
                ModelState.AddModelError("Materiales", "Error! Hay materiales repetidos en el carrito");
 
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.Email == alquilerForCreate.Email);
            if (user == null)
                ModelState.AddModelError("AlquilerApplicationUser", "Error! No hay ningún usuario registrado con ese email");
 
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
 
 
            var materialIds = alquilerForCreate.Materiales.Select(am => am.IdMaterial).ToList<int>();
            var fecha = alquilerForCreate.FechaAlquiler.Date;
            var fechaSiguiente = fecha.AddDays(1);
 
            var materiales = (await _context.Materiales
                .Where(m => materialIds.Contains(m.IdMaterial))
 
                .Select(m => new {
                    m.IdMaterial,
                    m.Nombre,
                    NombreTipoMaterial = m.TipoMaterial!.NombreTipoMaterial,
                    NombreTipoDeporte = m.TipoDeporte!.Nombre,     
                    m.Cantidad,
                    m.Precio,
                    NumberOfRentedItems = _context.Alquileres
                        .Where(a => a.FechaAlquiler >= fecha && a.FechaAlquiler < fechaSiguiente)
                        .SelectMany(a => a.MaterialesAlquilados)
                        .Where(ma => ma.IdMaterial == m.IdMaterial)
                        .Sum(ma => (int?)ma.Cantidad) ?? 0
                })
                .ToListAsync());
 
 
            Alquiler alquiler = new Alquiler
            {
                FechaAlquiler = alquilerForCreate.FechaAlquiler,
                MetodoPago = alquilerForCreate.MetodoPago,
                PrecioTotal = 0,
                Cliente = user,
                ClienteId = user!.Id
            };
 
 
            foreach (var item in alquilerForCreate.Materiales)
            {
                var material = materiales.FirstOrDefault(m => m.IdMaterial == item.IdMaterial);
 
                if ((material == null) || (material.NumberOfRentedItems + item.Cantidad > material.Cantidad))
                {
                    ModelState.AddModelError("Materiales", $"Error! El material con id '{item.IdMaterial}' no está disponible en la cantidad solicitada para el {fecha.ToShortDateString()}");
                }
                else
                {
                    alquiler.MaterialesAlquilados.Add(new MaterialAlquilado
                    {
                        IdMaterial = material.IdMaterial,
                        Cantidad = item.Cantidad,
                        Precio = material.Precio,
                        Descripcion = item.Descripcion
                    });
                    item.NombreMaterial = material.Nombre;
                    item.TipoMaterial = material.NombreTipoMaterial;
                    item.TipoDeporte = material.NombreTipoDeporte;
                    item.Precio = material.Precio;      
                }
            }
            alquiler.PrecioTotal = alquiler.MaterialesAlquilados.Sum(ma => ma.Precio * ma.Cantidad);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
 
            _context.Add(alquiler);
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Alquiler", $"Error! Se produjo un error al guardar su alquiler, inténtelo de nuevo más tarde");
                return Conflict("Error" + ex.Message);
            }
 
            var alquilerDetail = new AlquilerDetailDTO(alquiler.IdAlquiler,
                alquilerForCreate.NombreUsuario, alquilerForCreate.ApellidosUsuario,
                alquilerForCreate.Dni, alquilerForCreate.Email, alquilerForCreate.Telefono,
                alquiler.MetodoPago, alquilerForCreate.Materiales, alquiler.FechaAlquiler);
 
            return CreatedAtAction("GetAlquiler", new { id = alquiler.IdAlquiler }, alquilerDetail);
        }
    
    }
        
        
}