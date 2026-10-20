using FacturacionSimple.Modelo.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FacturacionSimple.Modelo.Datos;

public class ProductoDatos
{
    public List<Producto> Listar(string? filtro = null)
    {
        using var db = new FacturacionDbContext();
        var query = db.Productos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var f = filtro.Trim();
            query = query.Where(p => p.Codigo.Contains(f) || p.Nombre.Contains(f));
        }

        return query.OrderBy(p => p.Codigo).ToList();
    }

    public List<Producto> ListarActivos()
    {
        using var db = new FacturacionDbContext();
        return db.Productos
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToList();
    }

    public Producto? ObtenerPorId(int id)
    {
        using var db = new FacturacionDbContext();
        return db.Productos.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public bool ExisteCodigo(string codigo, int? excluirId = null)
    {
        using var db = new FacturacionDbContext();
        return db.Productos.Any(p =>
            p.Codigo == codigo && (!excluirId.HasValue || p.Id != excluirId.Value));
    }

    public void Agregar(Producto producto)
    {
        using var db = new FacturacionDbContext();
        db.Productos.Add(producto);
        db.SaveChanges();
    }

    public void Modificar(Producto producto)
    {
        using var db = new FacturacionDbContext();
        var existente = db.Productos.FirstOrDefault(p => p.Id == producto.Id)
            ?? throw new InvalidOperationException("El producto no existe.");

        existente.Codigo = producto.Codigo;
        existente.Nombre = producto.Nombre;
        existente.Precio = producto.Precio;
        existente.Activo = producto.Activo;
        db.SaveChanges();
    }

    public void BajaLogica(int id)
    {
        using var db = new FacturacionDbContext();
        var producto = db.Productos.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("El producto no existe.");

        producto.Activo = false;
        db.SaveChanges();
    }

    public bool EstaReferenciado(int id)
    {
        using var db = new FacturacionDbContext();
        return db.DetallesFactura.Any(d => d.ProductoId == id);
    }

    public void EliminarFisico(int id)
    {
        using var db = new FacturacionDbContext();
        var producto = db.Productos.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("El producto no existe.");

        if (db.DetallesFactura.Any(d => d.ProductoId == id))
            throw new InvalidOperationException("No se puede eliminar: el producto está en facturas.");

        db.Productos.Remove(producto);
        db.SaveChanges();
    }
}
