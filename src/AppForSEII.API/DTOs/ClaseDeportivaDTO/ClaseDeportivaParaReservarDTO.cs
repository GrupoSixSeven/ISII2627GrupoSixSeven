using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class ClaseDeportivaParaReservarDTO
    {
        public ClaseDeportivaParaReservarDTO()
        {
        }

        public ClaseDeportivaParaReservarDTO(int id, string descripcion, DateTime fechaHora, string? lugar, string monitor, string nivel, int plazasDisponibles, decimal precioUnitario, string tipoDeporte)
        {
            Id = id;
            Descripcion = descripcion;
            FechaHora = fechaHora;
            Lugar = lugar;
            Monitor = monitor;
            Nivel = nivel;
            PlazasDisponibles = plazasDisponibles;
            PrecioUnitario = precioUnitario;
            TipoDeporte = tipoDeporte;
        }

        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "La descripción no puede tener más de 100 caracteres")]
        public string Descripcion { get; set; } = "Descripción";

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.Now;

        [StringLength(50, ErrorMessage = "El lugar no puede tener más de 50 caracteres")]
        public string? Lugar { get; set; }

        [StringLength(50, ErrorMessage = "El monitor no puede tener más de 50 caracteres")]
        public string Monitor { get; set; } = "Monitor";

        [StringLength(50, ErrorMessage = "El nivel no puede tener más de 50 caracteres")]
        public string Nivel { get; set; } = "General";

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Las plazas disponibles no pueden ser negativas")]
        public int PlazasDisponibles { get; set; } = 1;

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "El precio unitario no puede ser negativo")]
        public decimal PrecioUnitario { get; set; } = 1m;

        [StringLength(50, ErrorMessage = "El tipo de deporte no puede tener más de 50 caracteres")]
        public string TipoDeporte { get; set; } = "General";

        public override bool Equals(object? obj)
        {
            return obj is ClaseDeportivaParaReservarDTO dTO &&
                   Id == dTO.Id &&
                   Descripcion == dTO.Descripcion &&
                   FechaHora == dTO.FechaHora &&
                   Lugar == dTO.Lugar &&
                   Monitor == dTO.Monitor &&
                   Nivel == dTO.Nivel &&
                   PlazasDisponibles == dTO.PlazasDisponibles &&
                   PrecioUnitario == dTO.PrecioUnitario &&
                   TipoDeporte == dTO.TipoDeporte;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Descripcion);
            hash.Add(FechaHora);
            hash.Add(Lugar);
            hash.Add(Monitor);
            hash.Add(Nivel);
            hash.Add(PlazasDisponibles);
            hash.Add(PrecioUnitario);
            hash.Add(TipoDeporte);
            return hash.ToHashCode();
        }
    }
}