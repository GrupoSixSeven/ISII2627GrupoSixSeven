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

namespace AppForSEII.UT.ClaseDeportivaController_test
{
    public class GetClasesDeportivas_test : AppForSEII4SqliteUT
    {
        public GetClasesDeportivas_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Yoga", NombreTipoDeporte = "Clases", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Clases", Descripcion = "" }
            };

            var clases = new List<ClaseDeportiva>() {
                new ClaseDeportiva { Id = 1, Descripcion = "Yoga para principiantes", TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = "Ana López", FechaHora = new DateTime(2027, 1, 10, 10, 0, 0), Lugar = "Sala 1", Nivel = "Principiante",
                    PlazasDisponibles = 10, PrecioUnitario = 5m },
                new ClaseDeportiva { Id = 2, Descripcion = "Pádel intermedio", TipoDeporte = tiposDeporte[1], TipoDeporteId = 2,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2027, 1, 11, 18, 0, 0), Lugar = "Pista 2", Nivel = "Intermedio",
                    PlazasDisponibles = 4, PrecioUnitario = 8m },
                new ClaseDeportiva { Id = 3, Descripcion = "Yoga avanzado", TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2027, 1, 12, 9, 0, 0), Lugar = "Sala 1", Nivel = "Avanzado",
                    PlazasDisponibles = 6, PrecioUnitario = 6m },

                // Esta clase no tiene plazas libres, así que el select nunca debe devolverla (paso 2 del caso de uso)
                new ClaseDeportiva { Id = 4, Descripcion = "Pádel avanzado", TipoDeporte = tiposDeporte[1], TipoDeporteId = 2,
                    Monitor = "Ana López", FechaHora = new DateTime(2027, 1, 11, 20, 0, 0), Lugar = "Pista 1", Nivel = "Avanzado",
                    PlazasDisponibles = 0, PrecioUnitario = 8m },
            };

            _context.AddRange(tiposDeporte);
            _context.AddRange(clases);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetClasesDeportivas_OK()
        {
            var claseDTOs = new List<ClaseDeportivaParaReservarDTO>() {
                new ClaseDeportivaParaReservarDTO { Id = 1, Descripcion = "Yoga para principiantes", FechaHora = new DateTime(2027, 1, 10, 10, 0, 0),
                    Lugar = "Sala 1", Monitor = "Ana López", Nivel = "Principiante", PlazasDisponibles = 10, PrecioUnitario = 5m, TipoDeporte = "Yoga" },
                new ClaseDeportivaParaReservarDTO { Id = 2, Descripcion = "Pádel intermedio", FechaHora = new DateTime(2027, 1, 11, 18, 0, 0),
                    Lugar = "Pista 2", Monitor = "Luis Pérez", Nivel = "Intermedio", PlazasDisponibles = 4, PrecioUnitario = 8m, TipoDeporte = "Pádel" },
                new ClaseDeportivaParaReservarDTO { Id = 3, Descripcion = "Yoga avanzado", FechaHora = new DateTime(2027, 1, 12, 9, 0, 0),
                    Lugar = "Sala 1", Monitor = "Luis Pérez", Nivel = "Avanzado", PlazasDisponibles = 6, PrecioUnitario = 6m, TipoDeporte = "Yoga" },
            };

            // El select ordena por FechaHora, así que las listas esperadas van en ese orden
            var clasesTC1 = new List<ClaseDeportivaParaReservarDTO>() { claseDTOs[0], claseDTOs[1], claseDTOs[2] }; // sin filtros
            var clasesTC2 = new List<ClaseDeportivaParaReservarDTO>() { claseDTOs[0], claseDTOs[2] };               // tipo de deporte Yoga
            var clasesTC3 = new List<ClaseDeportivaParaReservarDTO>();                                              // tipo de deporte que no existe
            var clasesTC4 = new List<ClaseDeportivaParaReservarDTO>() { claseDTOs[1], claseDTOs[2] };               // monitor Luis
            var clasesTC5 = new List<ClaseDeportivaParaReservarDTO>();                                              // monitor que no existe
            var clasesTC6 = new List<ClaseDeportivaParaReservarDTO>() { claseDTOs[1] };                             // fecha 11/01 (la de 0 plazas no sale)
            var clasesTC7 = new List<ClaseDeportivaParaReservarDTO>();                                              // fecha sin clases

            var allTests = new List<object?[]>
            {   //idTipoDeporte - monitor - fecha - clases esperadas
                new object?[] { null, null, null, clasesTC1 },
                new object?[] { 1, null, null, clasesTC2 },
                new object?[] { 99, null, null, clasesTC3 },
                new object?[] { null, "Luis", null, clasesTC4 },
                new object?[] { null, "Marta", null, clasesTC5 },
                new object?[] { null, null, new DateTime(2027, 1, 11), clasesTC6 },
                new object?[] { null, null, new DateTime(2027, 2, 1), clasesTC7 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetClasesDeportivas_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClasesDeportivas_OK_test(int? idTipoDeporte, string? monitor, DateTime? fecha, IList<ClaseDeportivaParaReservarDTO> expectedClases)
        {
            // Arrange
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            // Act
            var result = await controller.GetClasesDeportivas(idTipoDeporte, monitor, fecha);

            // Assert
            // comprobamos que la respuesta es OK y obtenemos la lista de clases
            var okResult = Assert.IsType<OkObjectResult>(result);
            var clasesDTOsActual = Assert.IsType<List<ClaseDeportivaParaReservarDTO>>(okResult.Value);
            // comprobamos que las clases esperadas y las obtenidas son las mismas
            Assert.Equal(expectedClases, clasesDTOsActual);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClasesDeportivas_NoDevuelveClasesSinPlazas_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            // Act
            var result = await controller.GetClasesDeportivas(null, null, null);

            // Assert
            // comprobamos que la respuesta es OK y que ninguna clase devuelta tiene 0 plazas
            var okResult = Assert.IsType<OkObjectResult>(result);
            var clasesDTOsActual = Assert.IsType<List<ClaseDeportivaParaReservarDTO>>(okResult.Value);
            Assert.DoesNotContain(clasesDTOsActual, c => c.PlazasDisponibles == 0);
        }
    }
}