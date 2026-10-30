using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.ClaseDeportivaDTO;
namespace AppForSEII.API.DTOs.InscripcionDTO
{
    public class InscripcionForCreateDTO
    {
        public InscripcionForCreateDTO(MetodoPago metodoPago, DateTime fechaInscripcion, decimal precioTotal, 
        string nombreUsuario, string apellidosUsuario, string dni, string telefono, string datosPago, 
        IList<InscripcionItemDTO> clasesInscritas)
    {
        MetodoPago = metodoPago;
        FechaInscripcion = fechaInscripcion;
        PrecioTotal = precioTotal;
        NombreUsuario = nombreUsuario;
        ApellidosUsuario = apellidosUsuario;
        Dni = dni;
        Telefono = telefono;
        DatosPago = datosPago;
        ClasesInscritas = clasesInscritas;
    }

        public MetodoPago MetodoPago { get; set; }
        public DateTime FechaInscripcion { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio.")]
        [Range(0, (double)double.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public decimal PrecioTotal { get; set; }

        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string DatosPago { get; set; }

        public IList<InscripcionItemDTO> ClasesInscritas { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionForCreateDTO dTO &&
                   MetodoPago == dTO.MetodoPago &&
                   FechaInscripcion == dTO.FechaInscripcion &&
                   PrecioTotal == dTO.PrecioTotal &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Telefono == dTO.Telefono &&
                   DatosPago == dTO.DatosPago &&
                   EqualityComparer<IList<InscripcionItemDTO>>.Default.Equals(ClasesInscritas, dTO.ClasesInscritas);
        }

       public override int GetHashCode()
{
    HashCode hash = new HashCode();
    hash.Add(MetodoPago);
    hash.Add(FechaInscripcion);
    hash.Add(PrecioTotal);
    hash.Add(NombreUsuario);
    hash.Add(ApellidosUsuario);
    hash.Add(Dni);
    hash.Add(Telefono);
    hash.Add(DatosPago);
    hash.Add(ClasesInscritas);
    return hash.ToHashCode();
}
    }
}