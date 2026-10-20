using FacturacionSimple.Modelo.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FacturacionSimple.Modelo.Datos;

public class FacturacionDbContext : DbContext
{
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<DetalleFactura> DetallesFactura => Set<DetalleFactura>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=FacturacionSimpleDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("Productos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Codigo).HasMaxLength(30).IsRequired();
            e.HasIndex(x => x.Codigo).IsUnique();
            e.Property(x => x.Nombre).HasMaxLength(120).IsRequired();
            e.Property(x => x.Precio).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Factura>(e =>
        {
            e.ToTable("Facturas");
            e.HasKey(x => x.Id);
            e.Property(x => x.Numero).IsRequired();
            e.HasIndex(x => x.Numero).IsUnique();
            e.Property(x => x.ClienteNombre).HasMaxLength(120).IsRequired();
            e.Property(x => x.ClienteDocumento).HasMaxLength(30).IsRequired();
            e.Property(x => x.Total).HasPrecision(18, 2);
            e.HasMany(x => x.Detalles)
                .WithOne(d => d.Factura)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DetalleFactura>(e =>
        {
            e.ToTable("DetallesFactura");
            e.HasKey(x => x.Id);
            e.Property(x => x.PrecioUnitario).HasPrecision(18, 2);
            e.Property(x => x.Subtotal).HasPrecision(18, 2);
            e.HasOne(x => x.Producto)
                .WithMany(p => p.Detalles)
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
