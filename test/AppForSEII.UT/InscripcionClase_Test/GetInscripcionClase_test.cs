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

namespace AppForSEII.UT.InscripcionClaseController_test
{
    public class GetInscripcion_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime _fechaInscripcion = new DateTime(2026, 11, 1, 12, 0, 0);
        private static readonly DateTime _fechaClase = new DateTime(2027, 1, 10, 10, 0, 0);

        public GetInscripcion_test()
        {
            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Yoga", NombreTipoDeporte = "Clases", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Pádel", NombreTipoDeporte = "Clases", Descripcion = "" }
            };

            var clases = new List<ClaseDeportiva>() {
                new ClaseDeportiva { Id = 1, Descripcion = "Yoga para principiantes", TipoDeporte = tiposDeporte[0], TipoDeporteId = 1,
                    Monitor = "Ana López", FechaHora = _fechaClase, Lugar = "Sala 1", Nivel = "Principiante",
                    PlazasDisponibles = 8, PrecioUnitario = 5m },
                new ClaseDeportiva { Id = 2, Descripcion = "Pádel intermedio", TipoDeporte = tiposDeporte[1], TipoDeporteId = 2,
                    Monitor = "Luis Pérez", FechaHora = new DateTime(2027, 1, 11, 18, 0, 0), Lugar = "Pista 2", Nivel = "Intermedio",
                    PlazasDisponibles = 4, PrecioUnitario = 8m },
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = "elena.navarro@uclm.es",
                UserName = "elena.navarro@uclm.es", DNI = "12345678A", PhoneNumber = "666555444" };

            var inscripcion = new Inscripcion
            {
                ClienteId = user.Id,
                Cliente = user,
                FechaInscripcion = _fechaInscripcion,
                MetodoPago = MetodoPago.Tarjeta,
                DatosPago = "1234-5678-9012-3456",
                PrecioTotal = 10m
            };

            inscripcion.ClasesInscritas.Add(new ClaseInscrita { ClaseDeportivaId = 1, ClaseDeportiva = clases[0], Inscripcion = inscripcion,
                PlazasReservadas = 2, Precio = 5m, Observaciones = "Voy con una amiga" });

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposDeporte);
            _context.AddRange(clases);
            _context.Add(inscripcion);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetInscripcion_NotFound_test()
        {
            var mock = new Mock<ILogger<InscripcionClaseController>>();
            ILogger<InscripcionClaseController> logger = mock.Object;
            var controller = new InscripcionClaseController(_context, logger);

            var result = await controller.GetInscripcion(0);

           
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetInscripcion_Found_test()
        {
            var mock = new Mock<ILogger<InscripcionClaseController>>();
            ILogger<InscripcionClaseController> logger = mock.Object;
            var controller = new InscripcionClaseController(_context, logger);

            var expectedInscripcion = new InscripcionClaseDetailDTO(1, MetodoPago.Tarjeta, _fechaInscripcion, 10m,
                new List<InscripcionItemDTO>(), "1234-5678-9012-3456",
                "Elena", "Navarro Martínez", "666555444", "12345678A", "elena.navarro@uclm.es");

            expectedInscripcion.ClasesInscritas.Add(new InscripcionItemDTO(1, "Yoga para principiantes", "Yoga", "Ana López",
                _fechaClase, 2, 5m, "Voy con una amiga"));

            var result = await controller.GetInscripcion(1);

        
            var okResult = Assert.IsType<OkObjectResult>(result);
            var inscripcionDTOActual = Assert.IsType<InscripcionClaseDetailDTO>(okResult.Value);
            Assert.Equal(expectedInscripcion, inscripcionDTOActual);
        }
    }
}