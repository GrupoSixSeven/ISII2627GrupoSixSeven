using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string name, string surname, string dni, int age, string sex, string userName, string email, string phoneNumber)
        {
            Name = name;
            Surname = surname;
            DNI = dni;
            Age = age;
            Sex = sex;
            
            // Estas propiedades se heredan de IdentityUser
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio.")]
        public required string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es obligatorio.")]
        public required string Surname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI es obligatorio.")]
        public required string DNI { get; set; }

        [Range(0, 120, ErrorMessage = "La edad debe ser un valor válido.")]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El sexo es obligatorio.")]
        public required string Sex { get; set; }

        public override bool Equals(object? otro)
        {
            if (otro == null) return false;
            
            if (otro.GetType() != this.GetType()) return false;

            ApplicationUser otro_user = (ApplicationUser)otro;

            // Al heredar de IdentityUser, el Id es de tipo string por defecto
            return otro_user.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}