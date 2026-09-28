
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class ClaseDeportiva
{
    [Key]
    public int Id { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaHora { get; set; }
    public string? Lugar { get; set; }
    public string? Monitor { get; set; }
    public string? Nivel { get; set; }
    public int PlazasDisponibles { get; set; }
    public decimal PrecioUnitario { get; set; }

    public int TipoDeporteId { get; set; }
   //lo comento porque da error ya que no esta la clase implementada 
   //  public TipoDeporte? TipoDeporte { get; set; }




    // Constructor vacío
    public ClaseDeportiva(){
    }

    // Constructor con los atributos principales
    public ClaseDeportiva(int id, string? descripcion,  DateTime fechaHora, string? lugar, string? monitor, string? nivel,  int plazasDisponibles, decimal precioUnitario, int tipoDeporteId) // falta TipoDeporte? tipoDeporte)
    {
        Id = id;
        Descripcion = descripcion;
        FechaHora = fechaHora;
        Lugar = lugar;
        Monitor = monitor;
        Nivel = nivel;
        PlazasDisponibles = plazasDisponibles;
        PrecioUnitario = precioUnitario;
        TipoDeporteId = tipoDeporteId;
        //falta implementacion TipoDeporte = tipoDeporte;
    }

    // Comprueba que el otro objeto no sea null, que sea del mismo tipo y que sus Ids coincidan
    public override bool Equals(object? otro)
    {
        if (otro == null) return false;
        if (otro.GetType() != this.GetType()) return false;

        ClaseDeportiva otra_clase = (ClaseDeportiva)otro;

        return otra_clase.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}