using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
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
    public class PostInscripcionClase_test : AppForSEII4SqliteUT
    {
        private const string _email = "elena.navarro@uclm.es";
        private const string _datosPago = "1234-5678-9012-3456";

        private const string _clase1Descripcion = "Yoga para principiantes";
        private const string _clase1TipoDeporte = "Yoga";
        private const string _clase1Monitor = "Ana López";
        private static readonly DateTime _clase1Fecha = new DateTime(2027, 1, 10, 10, 0, 0);

        public PostInscripcionClase_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = _clase1TipoDeporte, NombreTipoDeporte = "Clases", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Clases", Descripcion = "" }
            };

            var clases = new List<ClaseDeportiva>() {
                new ClaseDeportiva { Id = 1, Descripcion = _clase1Descripcion, TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = _clase1Monitor, FechaHora = _clase1Fecha, Lugar = "Sala 1", Nivel = "Principiante",
                    PlazasDisponibles = 10, PrecioUnitario = 5m },
                new ClaseDeportiva { Id = 2, Descripcion = "Pádel intermedio", TipoDeporte = tiposDeporte[1], TipoDeporteId = 2,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2027, 1, 11, 18, 0, 0), Lugar = "Pista 2", Nivel = "Intermedio",
                    PlazasDisponibles = 2, PrecioUnitario = 8m },
                new ClaseDeportiva { Id = 3, Descripcion = "Yoga avanzado", TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2025, 1, 12, 9, 0, 0), Lugar = "Sala 1", Nivel = "Avanzado",
                    PlazasDisponibles = 10, PrecioUnitario = 6m },
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = _email,
                UserName = _email, DNI = "12345678A", PhoneNumber = "666555444" };

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposDeporte);
            _context.AddRange(clases);
            _context.SaveChanges();
        }

        
        private static InscripcionClaseForCreateDTO CrearInscripcion(string correo, List<InscripcionItemDTO> clases)
        {
            return new InscripcionClaseForCreateDTO("Elena", "Navarro Martínez", "12345678A", correo, "666555444",
                MetodoPago.Tarjeta, 0m, clases, _datosPago, DateTime.Now);
        }

        private static InscripcionItemDTO CrearItem(int claseId, int plazas, string? observaciones = null)
        {
            return new InscripcionItemDTO(claseId, "", "", "", DateTime.MinValue, plazas, 0m, observaciones);
        }

        public static IEnumerable<object[]> TestCasesFor_CreateInscripcion()
        {
            var inscripcionSinClases = CrearInscripcion(_email, new List<InscripcionItemDTO>());
            var inscripcionCeroPlazas = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(1, 0) });
            var inscripcionCuatroPlazas = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(1, 4) });
            var inscripcionClaseRepetida = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(1, 1), CrearItem(1, 1) });
            var inscripcionUsuarioNoRegistrado = CrearInscripcion("victor.lopez@uclm.es", new List<InscripcionItemDTO>() { CrearItem(1, 1) });
            var inscripcionClaseNoExiste = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(99, 1) });
            var inscripcionClasePasada = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(3, 1) });
            var inscripcionSinPlazasSuficientes = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(2, 3) });

            var allTests = new List<object[]>
            {   //inscripción a crear - error esperado
                new object[] { inscripcionSinClases, "Error! Debe incluir al menos una clase deportiva" },
                new object[] { inscripcionCeroPlazas, "Error! Puede reservar entre 1 y 3 plazas por clase" },
                new object[] { inscripcionCuatroPlazas, "Error! Puede reservar entre 1 y 3 plazas por clase" },
                new object[] { inscripcionClaseRepetida, "Error! Hay clases repetidas en el carrito" },
                new object[] { inscripcionUsuarioNoRegistrado, "Error! No hay ningún usuario registrado con ese email" },
                new object[] { inscripcionClaseNoExiste, "Error! La clase con id '99' no está disponible" },
                new object[] { inscripcionClasePasada, "Error! La clase con id '3' no está disponible" },
                new object[] { inscripcionSinPlazasSuficientes, "Error! La clase con id '2' no está disponible" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateInscripcion))]
        public async Task CreateInscripcion_Error_test(InscripcionClaseForCreateDTO inscripcionDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            var result = await controller.CreateInscripcion(inscripcionDTO);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateInscripcion_Success_test()
        {
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            var inscripcionDTO = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(1, 2, "Voy con una amiga") });

            var expectedInscripcionDetailDTO = new InscripcionClaseDetailDTO(1, MetodoPago.Tarjeta, DateTime.Now, 10m,
                new List<InscripcionItemDTO>() {
                    new InscripcionItemDTO(1, _clase1Descripcion, _clase1TipoDeporte, _clase1Monitor, _clase1Fecha, 2, 5m, "Voy con una amiga")
                },
                _datosPago, "Elena", "Navarro Martínez", "666555444", "12345678A", _email);

            var result = await controller.CreateInscripcion(inscripcionDTO);

  
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualInscripcionDetailDTO = Assert.IsType<InscripcionClaseDetailDTO>(createdResult.Value);
            Assert.Equal(expectedInscripcionDetailDTO, actualInscripcionDetailDTO);
            Assert.Equal(8, _context.ClasesDeportivas.First(c => c.Id == 1).PlazasDisponibles);
        }
    }
}