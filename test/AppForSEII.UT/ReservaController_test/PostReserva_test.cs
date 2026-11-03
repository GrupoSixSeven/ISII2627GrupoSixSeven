using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.ReservaController_test
{
    public class PostReserva_test : AppForSEII4SqliteUT
    {
        private const string _email = "elena.navarro@uclm.es";
        private const string _datosPago = "1234-5678-9012-3456";

        private const string _pista1Nombre = "Pista de tenis 1";
        private const string _pista1TipoDeporte = "Tenis";
        private static readonly DateTime _fechaReserva = DateTime.Today.AddDays(1);

        public PostReserva_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = _pista1TipoDeporte, NombreTipoDeporte = "Pistas", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Pistas", Descripcion = "" }
            };

            var pistas = new List<Pista>() {
                new Pista { IdPista = 1, NombrePista = _pista1Nombre, NPersonas = 4, Precio = 10m, Stock = 3,
                    TipoDeporte = tiposDeporte[0], IdTipoDeporte = 1 },
                new Pista { IdPista = 2, NombrePista = "Pista de pádel 1", NPersonas = 4, Precio = 12m, Stock = 1,
                    TipoDeporte = tiposDeporte[1], IdTipoDeporte = 2 },
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = _email,
                UserName = _email, DNI = "12345678A", PhoneNumber = "666555444" };

            var reserva = new Reserva
            {
                ClienteId = user.Id,
                Cliente = user,
                FechaReserva = _fechaReserva,
                MetodoPago = MetodoPago.Tarjeta,
                PrecioTotal = 12m
            };

            reserva.PistasReservadas.Add(new PistaReservada { IdPista = 2, Pista = pistas[1], Reserva = reserva,
                Cantidad = 1, Precio = 12m, Observaciones = null });

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.Add(reserva);
            _context.SaveChanges();
        }

        private static ReservaForCreateDTO CrearReserva(string email, List<ReservaItemDTO> pistas, DateTime fechaReserva)
        {
            return new ReservaForCreateDTO("Elena", "Navarro Martínez", "12345678A", email, "666555444",
                MetodoPago.Tarjeta, 0m, pistas, _datosPago, fechaReserva);
        }

        private static ReservaItemDTO CrearItem(int idPista, string? observaciones = null)
        {
            return new ReservaItemDTO(idPista, "", "", 0, 0m, observaciones);
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReserva()
        {
            var reservaSinPistas = CrearReserva(_email, new List<ReservaItemDTO>(), _fechaReserva);
            var reservaFechaPasada = CrearReserva(_email, new List<ReservaItemDTO>() { CrearItem(1) }, DateTime.Today.AddDays(-1));
            var reservaPistaRepetida = CrearReserva(_email, new List<ReservaItemDTO>() { CrearItem(1), CrearItem(1) }, _fechaReserva);
            var reservaUsuarioNoRegistrado = CrearReserva("victor.lopez@uclm.es", new List<ReservaItemDTO>() { CrearItem(1) }, _fechaReserva);
            var reservaPistaNoExiste = CrearReserva(_email, new List<ReservaItemDTO>() { CrearItem(99) }, _fechaReserva);
            var reservaPistaSinDisponibilidad = CrearReserva(_email, new List<ReservaItemDTO>() { CrearItem(2) }, _fechaReserva);

            var allTests = new List<object[]>
            {
                new object[] { reservaSinPistas, "Error! Debe incluir al menos una pista para reservar" },
                new object[] { reservaFechaPasada, "Error! La fecha de su reserva no puede ser anterior a hoy" },
                new object[] { reservaPistaRepetida, "Error! Hay pistas repetidas en el carrito" },
                new object[] { reservaUsuarioNoRegistrado, "Error! No hay ningún usuario registrado con ese email" },
                new object[] { reservaPistaNoExiste, "Error! La pista con id '99' no está disponible" },
                new object[] { reservaPistaSinDisponibilidad, "Error! La pista con id '2' no está disponible" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReserva))]
        public async Task CreateReserva_Error_test(ReservaForCreateDTO reservaDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var result = await controller.CreateReserva(reservaDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReserva_Success_test()
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var reservaDTO = CrearReserva(_email, new List<ReservaItemDTO>() { CrearItem(1, "Necesito iluminación") }, _fechaReserva);

            var expectedReservaDetailDTO = new ReservaDetailDTO(2,
                "Elena", "Navarro Martínez", "12345678A", _email, "666555444",
                MetodoPago.Tarjeta, 10m,
                new List<ReservaItemDTO>() {
                    new ReservaItemDTO(1, _pista1Nombre, _pista1TipoDeporte, 4, 10m, "Necesito iluminación")
                },
                _datosPago, _fechaReserva);

            var result = await controller.CreateReserva(reservaDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualReservaDetailDTO = Assert.IsType<ReservaDetailDTO>(createdResult.Value);
            Assert.Equal(expectedReservaDetailDTO, actualReservaDetailDTO);
            Assert.Equal(2, _context.Reservas.Count());
        }
    }
}