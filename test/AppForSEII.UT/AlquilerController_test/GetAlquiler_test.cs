using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AlquilerDTOs;
using AppForSEII.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII.UT.AlquilerController_test
{
    public class GetAlquiler_test : AppForSEII4SqliteUT
    {public GetAlquiler_test()
        {
            var tiposMaterial = new List<TipoMaterial>() {
                new TipoMaterial { IdTipoMaterial = 1, NombreTipoMaterial = "Balones" },
                new TipoMaterial { IdTipoMaterial = 2, NombreTipoMaterial = "Raquetas" }
            };

            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Fútbol", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Tenis", NombreTipoDeporte = "Liga Local", Descripcion = "" }
            };

            var materiales = new List<Material>(){
                new Material { IdMaterial = 1, Nombre = "Balón de Fútbol", Precio = 5m, Cantidad = 10, TipoMaterial = tiposMaterial[0], TipoDeporte = tiposDeporte[0], IdTipoMaterial = 1 },
                new Material { IdMaterial = 2, Nombre = "Raqueta", Precio = 10m, Cantidad = 0, TipoMaterial = tiposMaterial[1], TipoDeporte = tiposDeporte[1], IdTipoMaterial = 2 }
            };

            ApplicationUser user = new ApplicationUser { Id = "1", Name = "Elena", Surname = "Navarro Martínez", Email = "elena.navarro@uclm.es", UserName = "elena.navarro@uclm.es", DNI = "12345678A", PhoneNumber = "666555444" };

            var alquiler = new Alquiler 
            {
                ClienteId = user.Id,
                Cliente = user,
                FechaAlquiler = DateTime.Now.Date,
                MetodoPago = MetodoPago.Tarjeta,
                PrecioTotal = 5m,
                MaterialesAlquilados = new List<MaterialAlquilado>()
            };;

            alquiler.MaterialesAlquilados.Add(new MaterialAlquilado { IdMaterial = 1, Material = materiales[0], Alquiler = alquiler, Cantidad = 1, Precio = 5m, Descripcion = "Para el partido" });

            _context.ApplicationUsers.Add(user);
            _context.AddRange(tiposMaterial);
            _context.AddRange(tiposDeporte);
            _context.AddRange(materiales);
            _context.Add(alquiler);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAlquiler_NotFound_test()
        {

            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            var result = await controller.GetAlquiler(0);

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetAlquiler_Found_test()
        {
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;
            var controller = new AlquilerController(_context, logger);

            var expectedAlquiler = new AlquilerDetailDTO(1, "Elena", "Navarro Martínez", "12345678A", "elena.navarro@uclm.es", "666555444",
                        MetodoPago.Tarjeta, new List<AlquilerItemDTO>(), DateTime.Now.Date);
                        
            expectedAlquiler.Materiales.Add(new AlquilerItemDTO(1, "Balón de Fútbol", "Balones", "Fútbol", 1, 5m, "Para el partido"));

            var result = await controller.GetAlquiler(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var alquilerDTOActual = Assert.IsType<AlquilerDetailDTO>(okResult.Value);
            var eq = expectedAlquiler.Equals(alquilerDTOActual);

            Assert.Equal(expectedAlquiler, alquilerDTOActual);

        }
    }
}