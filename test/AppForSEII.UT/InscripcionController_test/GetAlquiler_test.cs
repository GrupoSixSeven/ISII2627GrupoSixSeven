using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.CompeticionDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.InscripcionController_test
{
    public class GetInscripcion_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime _fechaCompeticion = new DateTime(2026, 12, 10, 10, 0, 0);

        public GetInscripcion_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Tenis", NombreTipoDeporte = "Competiciones", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Competiciones", Descripcion = "" }
            };

            var competiciones = new List<Competicion>() {
                new Competicion { Id = 1, Nombre = "Torneo de tenis", TipoDeporte = tiposDeporte[0],
                    Fecha = _fechaCompeticion, Lugar = "Ciudad Real", Precio = 15m, Plazas = 10 },
                new Competicion { Id = 2, Nombre = "Liga de pádel", TipoDeporte = tiposDeporte[1],
                    Fecha = _fechaCompeticion, Lugar = "Toledo", Precio = 20m, Plazas = 5 }
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = "elena.navarro@uclm.es", UserName = "elena.navarro@uclm.es", DNI = "12345678A", PhoneNumber = "666555444" };

            var inscripcion = new Inscripcion
            {
                ClienteId = user.Id,
                Cliente = user,
                FechaInscripcion = DateTime.Now.Date,
                MetodoPago = MetodoPago.Tarjeta,
                DatosPago = "1234-5678-9012-3456",
                PrecioTotal = 15m,
                CompeticionInscrita = new List<CompeticionInscrita>()
            };

            inscripcion.CompeticionInscrita.Add(new CompeticionInscrita { CompeticionId = 1, Competicion = competiciones[0], ProblemasFisicos = "Lesión de rodilla" });

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposDeporte);
            _context.AddRange(competiciones);
            _context.Add(inscripcion);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetInscripcion_NotFound_test()
        {
            var mock = new Mock<ILogger<InscripcionController>>();
            ILogger<InscripcionController> logger = mock.Object;

            var controller = new InscripcionController(_context, logger);

            var result = await controller.GetInscripcion(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetInscripcion_Found_test()
        {
            var mock = new Mock<ILogger<InscripcionController>>();
            ILogger<InscripcionController> logger = mock.Object;
            var controller = new InscripcionController(_context, logger);

            var expectedInscripcion = new InscripcionDetailDTO(1, "Elena", "Navarro Martínez", "12345678A", "elena.navarro@uclm.es", "666555444",
                        MetodoPago.Tarjeta, new List<CompeticionItemDTO>(), "1234-5678-9012-3456", DateTime.Now.Date);

            expectedInscripcion.Competiciones.Add(new CompeticionItemDTO(1, "Torneo de tenis", "Tenis", _fechaCompeticion, 15m, "Lesión de rodilla"));

            var result = await controller.GetInscripcion(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var inscripcionDTOActual = Assert.IsType<InscripcionDetailDTO>(okResult.Value);

            Assert.Equal(expectedInscripcion, inscripcionDTOActual);
        }
    }
}