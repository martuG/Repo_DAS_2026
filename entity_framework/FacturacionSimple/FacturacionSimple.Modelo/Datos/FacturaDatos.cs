using FacturacionSimple.Modelo.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FacturacionSimple.Modelo.Datos;

public class FacturaDatos
{
    public List<Factura> Listar(DateTime? desde = null, DateTime? hasta = null, string? cliente = null)
    {
        using var db = new FacturacionDbContext();
        var query = db.Facturas.AsNoTracking().AsQueryable();

        if (desde.HasValue)
            query = query.Where(f => f.Fecha >= desde.Value.Date);

        if (hasta.HasValue)
            query = query.Where(f => f.Fecha < hasta.Value.Date.AddDays(1));

        if (!string.IsNullOrWhiteSpace(cliente))
        {
            var c = cliente.Trim();
            query = query.Where(f =>
                f.ClienteNombre.Contains(c) || f.ClienteDocumento.Contains(c));
        }

        return query.OrderByDescending(f => f.Numero).ToList();
    }

    public Factura? ObtenerConDetalle(int id)
    {
        using var db = new FacturacionDbContext();
        return db.Facturas
            .AsNoTracking()
            .Include(f => f.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefault(f => f.Id == id);
    }

    public Factura Emitir(Factura cabecera, List<LineaFacturaDatos> lineas)
    {
        using var db = new FacturacionDbContext();
        using var tx = db.Database.BeginTransaction();

        try
        {
            var ultimoNumero = db.Facturas.Any()
                ? db.Facturas.Max(f => f.Numero)
                : 0;

            cabecera.Numero = ultimoNumero + 1;
            cabecera.Detalles = new List<DetalleFactura>();

            decimal total = 0;

            foreach (var linea in lineas)
            {
                var producto = db.Productos.FirstOrDefault(p => p.Id == linea.ProductoId && p.Activo)
                    ?? throw new InvalidOperationException($"Producto {linea.ProductoId} no disponible.");

                var subtotal = Math.Round(producto.Precio * linea.Cantidad, 2);
                total += subtotal;

                cabecera.Detalles.Add(new DetalleFactura
                {
                    ProductoId = producto.Id,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = producto.Precio,
                    Subtotal = subtotal
                });
            }

            cabecera.Total = total;
            db.Facturas.Add(cabecera);
            db.SaveChanges();
            tx.Commit();

            return cabecera;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}
