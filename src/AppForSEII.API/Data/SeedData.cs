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
        }
        

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {
            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            
            // USUARIO 1: ELENA
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                
                // Usamos el inicializador de objetos { } para cumplir con el modificador "required"
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
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            // USUARIO 2: PETER
            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                
                // Aplicamos la misma estructura rellenando los datos obligatorios que faltaban
                ApplicationUser user = new ApplicationUser() 
                {
                    Id = "3",
                    Name = "Peter",
                    Surname = "Jackson",
                    DNI = "87654321X", // Dato inventado para cumplir con la base de datos
                    Age = 40,          // Dato inventado
                    Sex = "Masculino", // Dato inventado
                    UserName = "peter@uclm.es",
                    Email = "peter@uclm.es",
                    PhoneNumber = "600987654", // Dato inventado
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "OtherPass12$");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }
    }
}