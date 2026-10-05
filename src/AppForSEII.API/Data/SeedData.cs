using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII.API.Data {
    public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

            try {
                SeedTiposDeporte(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the TiposDeporte in the Database.");
            }
 
            try {
                SeedTiposMaterial(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the TiposMaterial in the Database.");
            }
 
            try {
                SeedMateriales(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Materiales in the Database.");
            }

            try {
                SeedCompeticiones(dbContext);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Competiciones in the Database.");
            }
            
            try
            {
                SeedClasesDeportivas(dbContext);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "An error occurred seeding the ClasesDeportivas in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {
            foreach (string roleName in roles) {
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser() 
                {
                    Id = "1",
                    Name = "Elena",
                    Surname = "Navarro Martínez",
                    DNI = "12345678Z",
                    Age = 30,
                    Sex = "Femenino",
                    UserName = "elena@uclm.es",
                    Email = "elena@uclm.es",
                    PhoneNumber = "600123456",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                ApplicationUser user = new ApplicationUser() 
                {
                    Id = "3",
                    Name = "Peter",
                    Surname = "Jackson",
                    DNI = "87654321X",
                    Age = 40,
                    Sex = "Masculino",
                    UserName = "peter@uclm.es",
                    Email = "peter@uclm.es",
                    PhoneNumber = "600987654",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "OtherPass12$");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }

        public static void SeedTiposDeporte(ApplicationDbContext dbContext) {
            if (!dbContext.TiposDeportes.Any()) {
                dbContext.TiposDeportes.AddRange(
                    new TipoDeporte 
            { 
                Id = 0,
                Nombre = "Fútbol", 
                NombreTipoDeporte = "Liga Local", 
                Descripcion = "2" 
            },
            new TipoDeporte 
            { 
                Id = 0, 
                Nombre = "Baloncesto", 
                NombreTipoDeporte = "Liga Regional", 
                Descripcion = "1" 
            },
            new TipoDeporte 
            { 
                Id = 0, 
                Nombre = "Tenis", 
                NombreTipoDeporte = "Liga Local", 
                Descripcion = "3" 
            }
                );
                dbContext.SaveChanges();
            }
        }
 
        public static void SeedTiposMaterial(ApplicationDbContext dbContext) {
            if (!dbContext.TiposMateriales.Any()) {
                dbContext.TiposMateriales.AddRange(
                    new TipoMaterial 
            { 
                IdTipoMaterial = 0,
                NombreTipoMaterial = "Balón" 
            },
            new TipoMaterial 
            { 
                IdTipoMaterial = 0,
                NombreTipoMaterial = "Raqueta" 
            },
            new TipoMaterial 
            { 
                IdTipoMaterial = 0,
                NombreTipoMaterial = "Protección" 
            }
                );
                dbContext.SaveChanges();
            }
        }
 
        public static void SeedMateriales(ApplicationDbContext dbContext) {
            if (!dbContext.Materiales.Any()) {
                var futbol = dbContext.TiposDeportes.First(t => t.Nombre == "Fútbol");
                var baloncesto = dbContext.TiposDeportes.First(t => t.Nombre == "Baloncesto");
                var tenis = dbContext.TiposDeportes.First(t => t.Nombre == "Tenis");
 
                var balon = dbContext.TiposMateriales.First(t => t.NombreTipoMaterial == "Balón");
                var raqueta = dbContext.TiposMateriales.First(t => t.NombreTipoMaterial == "Raqueta");
 
                dbContext.Materiales.AddRange(
                    new Material 
            { 
                IdMaterial = 0,
                Nombre = "Balón de fútbol reglamentario",
                Precio = 3.50m,
                Cantidad = 10,
                TipoMaterial = balon,
                TipoDeporte = futbol,
                MaterialAlquilados = new List<MaterialAlquilado>()
            },
            new Material 
            { 
                IdMaterial = 0,
                Nombre = "Balón de baloncesto",
                Precio = 2.50m,
                Cantidad = 8,
                TipoMaterial = balon,
                TipoDeporte = baloncesto,
                MaterialAlquilados = new List<MaterialAlquilado>()
            },
            new Material 
            { 
                IdMaterial = 0,
                Nombre = "Raqueta de tenis",
                Precio = 4.00m,
                Cantidad = 5,
                TipoMaterial = raqueta,
                TipoDeporte = tenis,
                MaterialAlquilados = new List<MaterialAlquilado>()
            }
                );
                dbContext.SaveChanges();
            }
        }

        public static void SeedCompeticiones(ApplicationDbContext dbContext) {
            if (!dbContext.Competiciones.Any()) {
                var futbol = dbContext.TiposDeportes.First(t => t.Nombre == "Fútbol");
                var baloncesto = dbContext.TiposDeportes.First(t => t.Nombre == "Baloncesto");
                var tenis = dbContext.TiposDeportes.First(t => t.Nombre == "Tenis");

                dbContext.Competiciones.AddRange(
                    new Competicion 
            {
                Nombre = "Liga Local de Futbol Sala",
                TipoDeporte = futbol,
                Fecha = new DateTime(2026, 11, 15),
                Lugar = "Pabellon Municipal",
                Plazas = 20, 
                Precio = 15.00m
            },
            new Competicion 
            {
                Nombre = "Torneo 3x3 de Baloncesto",
                TipoDeporte = baloncesto,
                Fecha = new DateTime(2026, 11, 22),
                Lugar = "Pista Exterior Norte",
                Plazas = 12,
                Precio = 10.10m
            },
            new Competicion 
            {
                Nombre = "Open de Tenis de Otono",
                TipoDeporte = tenis,
                Fecha = new DateTime(2026, 12, 5),
                Lugar = "Pistas de Tenis",
                Plazas = 16,
                Precio = 20.50m
            },
            new Competicion 
            {
                Nombre = "Torneo Benefico de Futbol 7",
                TipoDeporte = futbol,
                Fecha = new DateTime(2026, 12, 13),
                Lugar = "Campo Anexo",
                Plazas = 0,
                Precio = 12.00m
            }
                );
                dbContext.SaveChanges();
            }
        }

        public static void SeedClasesDeportivas(ApplicationDbContext dbContext){
            if (!dbContext.ClasesDeportivas.Any())
            {
                var futbol = dbContext.TiposDeportes.First(t => t.Nombre == "Fútbol");
                var baloncesto = dbContext.TiposDeportes.First(t => t.Nombre == "Baloncesto");
                var tenis = dbContext.TiposDeportes.First(t => t.Nombre == "Tenis");

                dbContext.ClasesDeportivas.AddRange(
                    new ClaseDeportiva 
                        {
                        Descripcion = "Entrenamiento de técnica y posesión",
                        FechaHora = DateTime.Today.AddDays(1).AddHours(18), 
                        Lugar = "Pista 1",
                        Monitor = "Elena Navarro Martínez", 
                        Nivel = "Intermedio",
                        PlazasDisponibles = 15, 
                        PrecioUnitario = 7.50m,
                        TipoDeporte = futbol,
                        TipoDeporteId = futbol.Id
                        },
                    
                    new ClaseDeportiva 
                        {
                            Descripcion = "Iniciación al baloncesto",
                            FechaHora = DateTime.Today.AddDays(2).AddHours(19),
                            Lugar = "Pista 2",
                            Monitor = "Peter Jackson",
                            Nivel = "Iniciación",
                            PlazasDisponibles = 30,
                            PrecioUnitario = 6.00m,
                            TipoDeporte = baloncesto,
                            TipoDeporteId = baloncesto.Id
                        },
                    
                    new ClaseDeportiva 
                        {
                            Descripcion = "Perfeccionamiento de saque y volea",
                            FechaHora = DateTime.Today.AddDays(3).AddHours(17).AddMinutes(30),
                            Lugar = "Pista 2",
                            Monitor = "Elena Navarro Martínez",
                            Nivel = "Avanzado",
                            PlazasDisponibles = 8,
                            PrecioUnitario = 10.00m,
                            TipoDeporte = tenis,
                            TipoDeporteId = tenis.Id
                        }
                );
                dbContext.SaveChanges();
            }
        }
    }
}