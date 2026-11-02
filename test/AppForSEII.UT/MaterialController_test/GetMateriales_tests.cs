using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.AlquilerDTOs; 
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.MaterialController_test
{
    public class GetMaterialesParaAlquilar_test : AppForSEII4SqliteUT
    {
        public GetMaterialesParaAlquilar_test()
        {
            var tiposMaterial = new List<TipoMaterial>() {
                new TipoMaterial { IdTipoMaterial = 1, NombreTipoMaterial = "Balones" },
                new TipoMaterial { IdTipoMaterial = 2, NombreTipoMaterial = "Raquetas" },
                new TipoMaterial { IdTipoMaterial = 3, NombreTipoMaterial = "Redes" }
            };

            var tiposDeporte = new List<TipoDeporte>() {
                new TipoDeporte { Id = 1, Nombre = "Fútbol", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 2, Nombre = "Tenis", NombreTipoDeporte = "Liga Local", Descripcion = "" },
                new TipoDeporte { Id = 3, Nombre = "Voleibol", NombreTipoDeporte = "Liga Local", Descripcion = "" }
            };

            var materiales = new List<Material>() {
                new Material { IdMaterial = 1, MaterialAlquilados = new List<MaterialAlquilado>(), Nombre = "Balón", Precio = 5m, Cantidad = 10, TipoMaterial = tiposMaterial[0], TipoDeporte = tiposDeporte[0], IdTipoMaterial = 1 },
                new Material { IdMaterial = 2, MaterialAlquilados = new List<MaterialAlquilado>(), Nombre = "Raqueta", Precio = 12m, Cantidad = 4, TipoMaterial = tiposMaterial[1], TipoDeporte = tiposDeporte[1], IdTipoMaterial = 2 },
                new Material { IdMaterial = 3, MaterialAlquilados = new List<MaterialAlquilado>(), Nombre = "Red", Precio = 20m, Cantidad = 2, TipoMaterial = tiposMaterial[2], TipoDeporte = tiposDeporte[2], IdTipoMaterial = 3 },
            };

            _context.AddRange(tiposMaterial);
            _context.AddRange(tiposDeporte);
            _context.AddRange(materiales);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetMaterialesParaAlquilar_OK()
        {

            var materialDTOs = new List<MaterialParaAlquilarDTO>() {
                new MaterialParaAlquilarDTO(1, "Balón", 5m, 10, "Balones", "Fútbol"),
                new MaterialParaAlquilarDTO(2, "Raqueta", 12m, 4, "Raquetas", "Tenis"),
                new MaterialParaAlquilarDTO(3, "Red", 20m, 2, "Redes", "Voleibol"),
            };

            var materialDTOsTC1 = new List<MaterialParaAlquilarDTO>() { materialDTOs[0], materialDTOs[1], materialDTOs[2] }
                    .OrderBy(m => m.Nombre).ToList();

            var materialDTOsTC2 = new List<MaterialParaAlquilarDTO>() { materialDTOs[0], materialDTOs[1] }
                    .OrderBy(m => m.Nombre).ToList();

            var materialDTOsTC3 = new List<MaterialParaAlquilarDTO>();

            var materialDTOsTC4 = new List<MaterialParaAlquilarDTO>() { materialDTOs[1] };

            var materialDTOsTC5 = new List<MaterialParaAlquilarDTO>();

            var allTests = new List<object?[]>
            {               

                new object?[] { (decimal?)null, (int?)null, materialDTOsTC1 },
                new object?[] { 12m, (int?)null, materialDTOsTC2 },
                new object?[] { 1m, (int?)null, materialDTOsTC3 },
                new object?[] { (decimal?)null, 2, materialDTOsTC4 },
                new object?[] { (decimal?)null, 9999, materialDTOsTC5 },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMaterialesParaAlquilar_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_OK_test(decimal? precioMaximo, int? tipoMaterialId, IList<MaterialParaAlquilarDTO> expectedMateriales)
        {

            var mockLogger = new Mock<ILogger<MaterialController>>();
            var controller = new MaterialController(_context, mockLogger.Object);

            var result = await controller.GetMaterialesParaAlquilar(precioMaximo, tipoMaterialId);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var materialDTOsActual = Assert.IsType<List<MaterialParaAlquilarDTO>>(okResult.Value);
            
            Assert.Equal(expectedMateriales, materialDTOsActual);
        }
        
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetMaterialesParaAlquilar_badrequest_test()
        {

            var mockLogger = new Mock<ILogger<MaterialController>>();
            var controller = new MaterialController(_context, mockLogger.Object);

            var result = await controller.GetMaterialesParaAlquilar(-5m, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("El precio máximo no puede ser negativo", problem);
        }
    }
}