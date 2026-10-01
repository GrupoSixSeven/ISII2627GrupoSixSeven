using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations; 

namespace AppForSEII.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }
    
    // AQUÍ ESTÁ EL CAMBIO: 'id' ha pasado a ser 'Id'
    public ApplicationUser(string Id, string name, string surname, string userName)
    {
        this.Id = Id; // Actualizado para usar el nuevo nombre con mayúscula
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
    }

    [StringLength(50)]
    public string? Name {get;set;}

    [StringLength(50)]
    public string? Surname {get;set;}
}