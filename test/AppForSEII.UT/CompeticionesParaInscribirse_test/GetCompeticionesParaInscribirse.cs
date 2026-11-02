using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.CompeticionParaInscribirseDTO;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.CompeticionController_test
{
    public class GetCompeticionesParaInscribirse_test : AppForSEII4SqliteUT
    {
        public GetCompeticionesParaInscribirse_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Fútbol", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Tenis", NombreTipoDeporte = "Torneo Local", Descripcion = "" },
                new TipoDeporte { Id = 3, Nombre = "Baloncesto", NombreTipoDeporte = "Torneo Local", Descripcion = "" }
            };

            var competiciones = new List<Competicion>() {
                new Competicion { Id = 1, Nombre = "Copa de Verano", Fecha = new DateTime(2027, 7, 15), Lugar = "Pabellón Norte", Plazas = 20, Precio = 15m, TipoDeporte = tiposDeporte[0] },
                new Competicion { Id = 2, Nombre = "Torneo de Tenis", Fecha = new DateTime(2027, 8, 10), Lugar = "Pista Central", Plazas = 10, Precio = 30m, TipoDeporte = tiposDeporte[1] },
                new Competicion { Id = 3, Nombre = "Liga Basket", Fecha = new DateTime(2027, 9, 5), Lugar = "Pabellón Sur", Plazas = 0, Precio = 25m, TipoDeporte = tiposDeporte[2] },
            };

            _context.AddRange(tiposDeporte);
            _context.AddRange(competiciones);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetCompeticionesParaInscribirse_OK()
        {
            var competicionesDTOs = new List<CompeticionesParaInscribirseDTO>() {
                new CompeticionesParaInscribirseDTO(1, "Copa de Verano", "Fútbol", new DateTime(2027, 7, 15), "Pabellón Norte", 15m),
                new CompeticionesParaInscribirseDTO(2, "Torneo de Tenis", "Tenis", new DateTime(2027, 8, 10), "Pista Central", 30m)
            };

            var competicionesDTOsTC1 = new List<CompeticionesParaInscribirseDTO>() { competicionesDTOs[0], competicionesDTOs[1] }
                    .OrderBy(c => c.Fecha).ToList();

            var competicionesDTOsTC2 = new List<CompeticionesParaInscribirseDTO>() { competicionesDTOs[0] }
                    .OrderBy(c => c.Fecha).ToList();

            var competicionesDTOsTC3 = new List<CompeticionesParaInscribirseDTO>() { competicionesDTOs[1] }
                    .OrderBy(c => c.Fecha).ToList();

            var allTests = new List<object?[]>
            {               
                new object?[] { null, null, competicionesDTOsTC1 },
                new object?[] { "Norte", null, competicionesDTOsTC2 },
                new object?[] { null, "Tenis", competicionesDTOsTC3 },
                new object?[] { "Pista", "Tenis", competicionesDTOsTC3 }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCompeticionesParaInscribirse_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompeticionesParaInscribirse_OK_test(string? lugar, string? tipoDeporte, IList<CompeticionesParaInscribirseDTO> expectedCompeticiones)
        {
            var mockLogger = new Mock<ILogger<CompeticionController>>();
            var controller = new CompeticionController(_context, mockLogger.Object);

            var result = await controller.GetCompeticionesParaInscribirse(lugar, tipoDeporte);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var competicionesDTOsActual = Assert.IsType<List<CompeticionesParaInscribirseDTO>>(okResult.Value);
            
            Assert.Equal(expectedCompeticiones, competicionesDTOsActual);
        }
        
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompeticionesParaInscribirse_notfound_test()
        {
            var mockLogger = new Mock<ILogger<CompeticionController>>();
            var controller = new CompeticionController(_context, mockLogger.Object);

            var result = await controller.GetCompeticionesParaInscribirse("Pabellón Este", null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var problem = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("No hay competiciones con plazas disponibles que coincidan con los criterios.", problem);
        }
    }
}