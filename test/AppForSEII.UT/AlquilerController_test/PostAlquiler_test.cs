using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
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

namespace AppForSEII.UT.AlquilerController_test
{
    public class PostAlquiler_test : AppForSEII4SqliteUT
    {
        private const string _userName = "elena.navarro@uclm.es";
        private const string _name = "Elena";
        private const string _surname = "Navarro Martínez";
        private const string _dni = "12345678A";
        private const string _phone = "666555444";

        public PostAlquiler_test()
        {
            var tipoMaterial = new TipoMaterial { IdTipoMaterial = 1, NombreTipoMaterial = "Balones" };
            var tipoDeporte = new TipoDeporte { Id = 1, Nombre = "Fútbol", NombreTipoDeporte = "Liga Local", Descripcion = "" };

            var materiales = new List<Material>()
            {
                new Material { IdMaterial = 1, Nombre = "Balón Oficial", Precio = 10m, Cantidad = 5, TipoMaterial = tipoMaterial, TipoDeporte = tipoDeporte, IdTipoMaterial = 1 },
                new Material { IdMaterial = 2, Nombre = "Raqueta", Precio = 20m, Cantidad = 2, TipoMaterial = tipoMaterial, TipoDeporte = tipoDeporte, IdTipoMaterial = 1 }
            };

            var user = new ApplicationUser {
                Id = "1", Name = _name, Surname = _surname, 
                Email = _userName, DNI = _dni, PhoneNumber = _phone, UserName = _userName
            };

            _context.ApplicationUsers.Add(user);
            _context.Add(tipoMaterial);
            _context.Add(tipoDeporte);
            _context.AddRange(materiales);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateAlquiler()
        {
            var validItem = new AlquilerItemDTO(1, "Balón Oficial", "Balones", "Fútbol", 1, 10m, "Ok");
            var validItemsList = new List<AlquilerItemDTO> { validItem };

            var alquilerFechaPasada = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, validItemsList, DateTime.Today.AddDays(-1));
            var alquilerSinMateriales = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, new List<AlquilerItemDTO>(), DateTime.Today);
            var alquilerCantidadCero = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { new AlquilerItemDTO(1, "Balón", "Balones", "Fútbol", 0, 10m, "") }, DateTime.Today);
            var alquilerMaterialRepetido = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { validItem, validItem }, DateTime.Today);
            var alquilerUsuarioFalso = new AlquilerForCreateDTO(_name, _surname, _dni, "falso@uclm.es", _phone, MetodoPago.Tarjeta, validItemsList, DateTime.Today);
            var alquilerSinStock = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { new AlquilerItemDTO(1, "Balón", "Balones", "Fútbol", 10, 10m, "") }, DateTime.Today);

            var alquilerMaterialNoExiste = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { new AlquilerItemDTO(99, "Inexistente", "Balones", "Fútbol", 1, 10m, "") }, DateTime.Today);

            var allTests = new List<object[]>
            {
                new object[] { alquilerFechaPasada, "Error! La fecha de su alquiler no puede ser anterior a hoy" },
                new object[] { alquilerSinMateriales, "Error! Debe incluir al menos un material para alquilar" },
                new object[] { alquilerCantidadCero, "Error! La cantidad de cada material debe ser al menos 1" },
                new object[] { alquilerMaterialRepetido, "Error! Hay materiales repetidos en el carrito" },
                new object[] { alquilerUsuarioFalso, "Error! No hay ningún usuario registrado con ese email" },
                new object[] { alquilerSinStock, "Error! El material con id '1' no está disponible en la cantidad solicitada" },
                new object[] { alquilerMaterialNoExiste, "Error! El material con id '99' no está disponible" } // Mensaje esperado para el nuevo caso
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateAlquiler))]
        public async Task CreateAlquiler_Error_test(AlquilerForCreateDTO alquilerDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<AlquilerController>>();
            var controller = new AlquilerController(_context, mock.Object);

            var result = await controller.CreateAlquiler(alquilerDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateAlquiler_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            var controller = new AlquilerController(_context, mock.Object);

            var alquilerDTO = new AlquilerForCreateDTO(_name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { new AlquilerItemDTO(1, "Balón Oficial", "Balones", "Fútbol", 2, 10m, "Ok") }, DateTime.Today);

            var expectedAlquilerDetailDTO = new AlquilerDetailDTO(
                1, _name, _surname, _dni, _userName, _phone, MetodoPago.Tarjeta, 
                new List<AlquilerItemDTO> { new AlquilerItemDTO(1, "Balón Oficial", "Balones", "Fútbol", 2, 10m, "Ok") }, 
                DateTime.Today);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualAlquilerDetailDTO = Assert.IsType<AlquilerDetailDTO>(createdResult.Value);

            Assert.Equal(expectedAlquilerDetailDTO, actualAlquilerDetailDTO);
        }
    }
}