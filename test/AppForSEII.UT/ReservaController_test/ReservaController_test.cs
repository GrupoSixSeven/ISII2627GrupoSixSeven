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

namespace AppForSEII.UT
{
    public class ReservaController_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime _fechaReserva = new DateTime(2026, 11, 1, 12, 0, 0);

        public ReservaController_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Tenis", NombreTipoDeporte = "Pistas", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Pistas", Descripcion = "" }
            };

            var pistas = new List<Pista>() {
                new Pista { IdPista = 1, NombrePista = "Pista de tenis 1", NPersonas = 4, Precio = 10m, Stock = 3,
                    TipoDeporte = tiposDeporte[0], IdTipoDeporte = 1 },
                new Pista { IdPista = 2, NombrePista = "Pista de pádel 1", NPersonas = 4, Precio = 12m, Stock = 2,
                    TipoDeporte = tiposDeporte[1], IdTipoDeporte = 2 },
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = "elena.navarro@uclm.es",
                UserName = "elena.navarro@uclm.es", DNI = "12345678A", PhoneNumber = "666555444" };

            var reserva = new Reserva
            {
                ClienteId = user.Id,
                Cliente = user,
                FechaReserva = _fechaReserva,
                MetodoPago = MetodoPago.Tarjeta,
                PrecioTotal = 10m
            };

            reserva.PistasReservadas.Add(new PistaReservada { IdPista = 1, Pista = pistas[0], Reserva = reserva,
                Cantidad = 1, Precio = 10m, Observaciones = "Necesito iluminación" });

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.Add(reserva);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReserva_NotFound_test()
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var result = await controller.GetReserva(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReserva_Found_test()
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var expectedReserva = new ReservaDetailDTO(1, MetodoPago.Tarjeta, _fechaReserva, 10m,
                new List<ReservaItemDTO>(), "",
                "Elena", "Navarro Martínez", "666555444", "12345678A", "elena.navarro@uclm.es");

            expectedReserva.Pistas.Add(new ReservaItemDTO(1, "Pista de tenis 1", "Tenis", 4, 10m, "Necesito iluminación"));

            var result = await controller.GetReserva(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservaDTOActual = Assert.IsType<ReservaDetailDTO>(okResult.Value);
            Assert.Equal(expectedReserva, reservaDTOActual);
        }
    }
}