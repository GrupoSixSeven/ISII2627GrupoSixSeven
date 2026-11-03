using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.PistasController_test
{
    public class GetPistas_test : AppForSEII4SqliteUT
    {
        public GetPistas_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Fútbol", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Tenis", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 3, Nombre = "Pádel", NombreTipoDeporte = "Liga Local", Descripcion = "" }
            };

            var pistas = new List<Pista>() {
                new Pista { IdPista = 1, NombrePista = "Pista Central", NPersonas = 22, Precio = 40m, Stock = 1, TipoDeporte = tiposDeporte[0] },
                new Pista { IdPista = 2, NombrePista = "Pista Tenis 1", NPersonas = 4, Precio = 15m, Stock = 1, TipoDeporte = tiposDeporte[1] },
                new Pista { IdPista = 3, NombrePista = "Pista Pádel 1", NPersonas = 4, Precio = 20m, Stock = 1, TipoDeporte = tiposDeporte[2] }
            };

            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetPistasParaAlquilar_OK()
        {
            var pistaDTOs = new List<PistaParaAlquilarDTO>() {
                new PistaParaAlquilarDTO(1, "Pista Central", 22, 40m, 1, "Fútbol"),
                new PistaParaAlquilarDTO(2, "Pista Tenis 1", 4, 15m, 1, "Tenis"),
                new PistaParaAlquilarDTO(3, "Pista Pádel 1", 4, 20m, 1, "Pádel")
            };

            var pistaDTOsTC1 = new List<PistaParaAlquilarDTO>() { pistaDTOs[0], pistaDTOs[1], pistaDTOs[2] }
                    .OrderBy(p => p.NombrePista).ToList();

            var pistaDTOsTC2 = new List<PistaParaAlquilarDTO>() { pistaDTOs[0], pistaDTOs[1], pistaDTOs[2] }
                    .OrderBy(p => p.NombrePista).ToList();

            var pistaDTOsTC3 = new List<PistaParaAlquilarDTO>() { pistaDTOs[0] };

            var pistaDTOsTC4 = new List<PistaParaAlquilarDTO>();

            var allTests = new List<object?[]>
            {
                new object?[] { (string?)null, (DateTime?)null, pistaDTOsTC1 },
                new object?[] { (string?)null, DateTime.Today.AddDays(1), pistaDTOsTC2 },
                new object?[] { "Fútbol", DateTime.Today, pistaDTOsTC3 },
                new object?[] { "Badminton", DateTime.Today, pistaDTOsTC4 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetPistasParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPistasParaAlquilar_OK_test(string? tipoDeporte, DateTime? fecha, IList<PistaParaAlquilarDTO> expectedPistas)
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.GetPistasParaAlquilar(tipoDeporte, fecha);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var pistaDTOsActual = Assert.IsType<List<PistaParaAlquilarDTO>>(okResult.Value);

            Assert.Equal(expectedPistas, pistaDTOsActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPistasParaAlquilar_badrequest_test()
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.GetPistasParaAlquilar(null, DateTime.Today.AddDays(-1));

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("La fecha no puede ser anterior a la actual", problem);
        }
    }
}