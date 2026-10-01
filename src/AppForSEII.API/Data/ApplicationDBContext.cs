using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


    }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }



    public DbSet<TipoDeporte> TiposDeportes { get; set; }
    public DbSet<TipoMaterial> TiposMateriales { get; set; }

    // Diagrama: Pistas y Reservas
    public DbSet<Pista> Pistas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<PistaReservada> PistasReservadas { get; set; }

    // Diagrama: Materiales y Alquileres
    public DbSet<Material> Materiales { get; set; }
    public DbSet<Alquiler> Alquileres { get; set; }
    public DbSet<MaterialAlquilado> MaterialesAlquilados { get; set; }

    // Diagrama: Competiciones, Clases Deportivas e Inscripciones
    public DbSet<Competicion> Competiciones { get; set; }
    public DbSet<ClaseDeportiva> ClasesDeportivas { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }
    
    // Tablas intermedias para las inscripciones
    public DbSet<CompeticionInscripcion> CompeticionesInscripciones { get; set; } 
    public DbSet<ClaseInscrita> ClasesInscritas { get; set; }
}