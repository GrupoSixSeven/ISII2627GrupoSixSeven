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
            
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

            [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty; // Sin required y con valor por defecto

        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es obligatorio.")]
        public string Surname { get; set; } = string.Empty; // Sin required y con valor por defecto

        [Required(AllowEmptyStrings = false, ErrorMessage = "El DNI es obligatorio.")]
        public string DNI { get; set; } = string.Empty; // Sin required y con valor por defecto

        [Range(0, 120, ErrorMessage = "La edad debe ser un valor válido.")]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El sexo es obligatorio.")]
        public string Sex { get; set; } = string.Empty; // Sin required y con valor por defecto

        public override bool Equals(object? otro)
        {
            if (otro == null) return false;
            if (otro.GetType() != this.GetType()) return false;

            ApplicationUser otro_user = (ApplicationUser)otro;
            return otro_user.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}