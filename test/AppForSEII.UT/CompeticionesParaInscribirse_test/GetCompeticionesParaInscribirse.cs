using AppForSEII.API.Controllers;
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
 
namespace AppForSEII.UT.CompeticionesParaInscribirse_test
{
    public class GetCompeticionesParaInscribirse_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime _fechaTenis = new DateTime(2026, 12, 10, 10, 0, 0);
        private static readonly DateTime _fechaPadel = new DateTime(2026, 12, 5, 17, 0, 0);
        private static readonly DateTime _fechaSinPlazas = new DateTime(2026, 12, 1, 9, 0, 0);
 
        public GetCompeticionesParaInscribirse_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Tenis", NombreTipoDeporte = "Competiciones", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Competiciones", Descripcion = "" }
            };
 
            var competiciones = new List<Competicion>() {
                new Competicion { Id = 1, Nombre = "Torneo de tenis", TipoDeporte = tiposDeporte[0],
                    Fecha = _fechaTenis, Lugar = "Ciudad Real", Precio = 15m, Plazas = 10 },
                new Competicion { Id = 2, Nombre = "Liga de pádel", TipoDeporte = tiposDeporte[1],
                    Fecha = _fechaPadel, Lugar = "Toledo", Precio = 20m, Plazas = 5 },
                new Competicion { Id = 3, Nombre = "Open de tenis", TipoDeporte = tiposDeporte[0],
                    Fecha = _fechaSinPlazas, Lugar = "Toledo", Precio = 25m, Plazas = 0 }
            };
 
            _context.AddRange(tiposDeporte);
            _context.AddRange(competiciones);
            _context.SaveChanges();
        }
 
        public static IEnumerable<object?[]> TestCasesFor_GetCompeticionesParaInscribirse_OK()
        {
            var competicionPadel = new CompeticionesParaInscribirseDTO(2, "Liga de pádel", "Pádel", _fechaPadel, "Toledo", 20m);
            var competicionTenis = new CompeticionesParaInscribirseDTO(1, "Torneo de tenis", "Tenis", _fechaTenis, "Ciudad Real", 15m);
 
            var allTests = new List<object?[]>
            {
                new object?[] { null, null, new List<CompeticionesParaInscribirseDTO>() { competicionPadel, competicionTenis } },
                new object?[] { "Ciudad Real", null, new List<CompeticionesParaInscribirseDTO>() { competicionTenis } },
                new object?[] { null, "Pádel", new List<CompeticionesParaInscribirseDTO>() { competicionPadel } },
                new object?[] { "Toledo", "Pádel", new List<CompeticionesParaInscribirseDTO>() { competicionPadel } },
            };
 
            return allTests;
        }
 
        [Theory]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetCompeticionesParaInscribirse_OK))]
        public async Task GetCompeticionesParaInscribirse_OK_test(string? lugar, string? tipoDeporte,
            IList<CompeticionesParaInscribirseDTO> expectedCompeticiones)
        {
            var mock = new Mock<ILogger<CompeticionController>>();
            ILogger<CompeticionController> logger = mock.Object;
            var controller = new CompeticionController(_context, logger);
 
            var result = await controller.GetCompeticionesParaInscribirse(lugar, tipoDeporte);
 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var competicionesActual = Assert.IsAssignableFrom<IList<CompeticionesParaInscribirseDTO>>(okResult.Value);
            Assert.Equal(expectedCompeticiones, competicionesActual);
        }
 
        public static IEnumerable<object?[]> TestCasesFor_GetCompeticionesParaInscribirse_NotFound()
        {
            var allTests = new List<object?[]>
            {
                new object?[] { "Madrid", null },
                new object?[] { null, "Fútbol" },
                new object?[] { "Toledo", "Tenis" },
            };
 
            return allTests;
        }
 
        [Theory]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetCompeticionesParaInscribirse_NotFound))]
        public async Task GetCompeticionesParaInscribirse_NotFound_test(string? lugar, string? tipoDeporte)
        {
            var mock = new Mock<ILogger<CompeticionController>>();
            ILogger<CompeticionController> logger = mock.Object;
            var controller = new CompeticionController(_context, logger);
 
            var result = await controller.GetCompeticionesParaInscribirse(lugar, tipoDeporte);
 
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No hay competiciones con plazas disponibles que coincidan con los criterios.", notFoundResult.Value);
        }
    }
}
 