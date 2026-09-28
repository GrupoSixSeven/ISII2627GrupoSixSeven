using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Surname { get; set; }

    [Required]
    public string? DNI { get; set; }

    public int Age { get; set; }

    public string? Sex { get; set; }

    // Constructor vacío
    public ApplicationUser() : base()
    {
    }

    // Constructor con parámetros de la clase e Identity
    public ApplicationUser(
        string name, 
        string surname, 
        string dni, 
        int age, 
        string? sex, 
        string userName, 
        string email, 
        string phoneNumber) : base()
    {
        Name = name;
        Surname = surname;
        DNI = dni;
        Age = age;
        Sex = sex;
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    // Comprueba que el otro objeto no sea null, que sea del mismo tipo y que las claves coincidan
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        ApplicationUser otro_usuario = (ApplicationUser)otro;

        return otro_usuario.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return Id != null ? Id.GetHashCode() : 0;
    }
}