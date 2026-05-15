using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hoca> Hocalar { get; set; }
    public DbSet<Salon> Salonlar { get; set; }
    public DbSet<Ogrenci> Ogrenciler { get; set; }
    public DbSet<Ders> Dersler { get; set; }
    public DbSet<OgrenciDersi> OgrenciDersleri { get; set; }
    public DbSet<Sinav> Sinavlar { get; set; }
    public DbSet<OgrenciSinavi> OgrenciSinavlari { get; set; }
    public DbSet<Soru> Sorular { get; set; }
    public DbSet<Kullanici> Kullanicilar { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // OGR - DERS UNIQUE
        // =========================
        modelBuilder.Entity<OgrenciDersi>()
            .HasIndex(x => new { x.OgrenciId, x.DersId })
            .IsUnique();

        // =========================
        // OGR - SINAV UNIQUE
        // =========================
        modelBuilder.Entity<OgrenciSinavi>()
            .HasIndex(x => new { x.OgrenciId, x.SinavId })
            .IsUnique();

        modelBuilder.Entity<OgrenciSinavi>()
            .Property(x => x.Notu)
            .HasPrecision(5, 2);

        // =========================
        // SINAV - ÖĞRENCİ SINAV
        // =========================
        modelBuilder.Entity<OgrenciSinavi>()
            .HasOne(x => x.Sinav)
            .WithMany(x => x.OgrenciSinavlari)
            .HasForeignKey(x => x.SinavId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // SINAV - SORU (DOĞRU RELATION)
        // =========================
        modelBuilder.Entity<Soru>()
            .HasOne(x => x.Sinav)
            .WithMany(x => x.Sorular)
            .HasForeignKey(x => x.SinavId)
            .OnDelete(DeleteBehavior.NoAction);



    }
}