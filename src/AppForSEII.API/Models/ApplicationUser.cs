using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity; // Necesario para IdentityUser

namespace AppForSEII.API.Models
{
    // La clase hereda de IdentityUser como marca el diagrama
    public class ApplicationUser : IdentityUser
    {
        // 1. Constructor vacío 
        public ApplicationUser()
        {
        }

        // 2. Sobrecarga del constructor con los datos principales
        public ApplicationUser(string name, string surname, string dni, int age, string sex, string userName, string email, string phoneNumber)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            DNI = dni ?? throw new ArgumentNullException(nameof(dni));
            Age = age;
            Sex = sex ?? throw new ArgumentNullException(nameof(sex));
            
            // Estas propiedades vienen heredadas de IdentityUser
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        // Propiedades específicas del modelo (las heredadas no se declaran de nuevo)
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100)]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(20)]
        public string DNI { get; set; } = null!;

        [Range(0, 120, ErrorMessage = "La edad debe ser un valor válido.")]
        public int Age { get; set; }

        [Required]
        [StringLength(20)]
        public string Sex { get; set; } = null!;

        // Métodos Equals y GetHashCode
        public override bool Equals(object? obj)
        {
            if (obj is not ApplicationUser item)
                return false;

            // Al heredar de IdentityUser, el Id es de tipo string
            return Id == item.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}