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
    public class CreateInscripcion_test : AppForSEII4SqliteUT
    {
        private const string _email = "elena.navarro@uclm.es";
        private const string _datosPago = "1234-5678-9012-3456";

        private const string _clase1Descripcion = "Yoga para principiantes";
        private const string _clase1TipoDeporte = "Yoga";
        private const string _clase1Monitor = "Ana López";
        private static readonly DateTime _clase1Fecha = new DateTime(2027, 1, 10, 10, 0, 0);

        public CreateInscripcion_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = _clase1TipoDeporte, NombreTipoDeporte = "Clases", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Clases", Descripcion = "" }
            };

            var clases = new List<ClaseDeportiva>() {
                // Clase normal: con plazas y en el futuro
                new ClaseDeportiva { Id = 1, Descripcion = _clase1Descripcion, TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = _clase1Monitor, FechaHora = _clase1Fecha, Lugar = "Sala 1", Nivel = "Principiante",
                    PlazasDisponibles = 10, PrecioUnitario = 5m },
                // Clase con solo 2 plazas libres, para probar que no se pueden reservar más
                new ClaseDeportiva { Id = 2, Descripcion = "Pádel intermedio", TipoDeporte = tiposDeporte[1], TipoDeporteId = 2,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2027, 1, 11, 18, 0, 0), Lugar = "Pista 2", Nivel = "Intermedio",
                    PlazasDisponibles = 2, PrecioUnitario = 8m },
                // Clase que ya se ha impartido
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

        // Crea el DTO de entrada. Los datos de cada clase (descripción, monitor...) los rellena el POST,
        // así que aquí solo importan el id, las plazas y las observaciones
        private static InscripcionClaseForCreateDTO CrearInscripcion(string email, List<InscripcionItemDTO> clases)
        {
            return new InscripcionClaseForCreateDTO(MetodoPago.Tarjeta, 0m, clases, _datosPago, DateTime.Now) { Email = email };
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
            // Arrange
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            // Act
            var result = await controller.CreateInscripcion(inscripcionDTO);

            // Assert
            // comprobamos que la respuesta es BadRequest y obtenemos el error devuelto
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            // comprobamos que el mensaje de error es el esperado
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateInscripcion_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ClaseDeportivaController>>();
            ILogger<ClaseDeportivaController> logger = mock.Object;
            var controller = new ClaseDeportivaController(_context, logger);

            // 2 plazas en la clase de Yoga (5€ cada una)
            var inscripcionDTO = CrearInscripcion(_email, new List<InscripcionItemDTO>() { CrearItem(1, 2, "Voy con una amiga") });

            var expectedInscripcionDetailDTO = new InscripcionClaseDetailDTO(1, MetodoPago.Tarjeta, DateTime.Now, 10m,
                new List<InscripcionItemDTO>() {
                    new InscripcionItemDTO(1, _clase1Descripcion, _clase1TipoDeporte, _clase1Monitor, _clase1Fecha, 2, 5m, "Voy con una amiga")
                },
                _datosPago, "Elena", "Navarro Martínez", "666555444", "12345678A", _email);

            // Act
            var result = await controller.CreateInscripcion(inscripcionDTO);

            // Assert
            // comprobamos que la respuesta es Created y obtenemos la inscripción creada
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualInscripcionDetailDTO = Assert.IsType<InscripcionClaseDetailDTO>(createdResult.Value);
            // comprobamos que la inscripción esperada y la obtenida son la misma
            Assert.Equal(expectedInscripcionDetailDTO, actualInscripcionDetailDTO);
            // comprobamos que se han descontado las plazas reservadas (paso 7): 10 - 2 = 8
            Assert.Equal(8, _context.ClasesDeportivas.First(c => c.Id == 1).PlazasDisponibles);
        }
    }
}