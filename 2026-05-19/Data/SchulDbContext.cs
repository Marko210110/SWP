using Microsoft.EntityFrameworkCore;
using Schulverwaltung.Models;

namespace Schulverwaltung.Data;

public class SchulDbContext : DbContext
{
    public DbSet<Schueler> Schueler { get; set; }
    public DbSet<Kurs> Kurse { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=schulverwaltung.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schueler>(entity =>
        {
            entity.ToTable("Schueler");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Vorname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Nachname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Kurs>(entity =>
        {
            entity.ToTable("Kurse");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Fach).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Raumnummer).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Schueler>()
            .HasMany(s => s.Kurse)
            .WithMany(k => k.Schueler)
            .UsingEntity(j => j.ToTable("SchuelerKurse"));
    }
}
