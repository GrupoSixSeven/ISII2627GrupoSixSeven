using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

    // Relación con las inscripciones realizadas por el usuario 
    // lo comento porque da error ya que no esta la enumeracion imple
   // public IList<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();

    // Constructor vacío
    public ApplicationUser() : base()
    {
    }

    // Constructor con parámetros
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

    // Comprueba igualdad comparando tipos e Ids no nulos
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        ApplicationUser otro_usuario = (ApplicationUser)otro;

        // Si alguna Id es nula (objeto aún no persistido), se comparan por referencia en memoria
        if (this.Id == null || otro_usuario.Id == null)
        {
            return ReferenceEquals(this, otro_usuario);
        }

        return otro_usuario.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return Id != null ? Id.GetHashCode() : base.GetHashCode();
    }
}