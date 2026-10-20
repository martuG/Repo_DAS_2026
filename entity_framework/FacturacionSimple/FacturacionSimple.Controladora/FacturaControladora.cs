using FacturacionSimple.Modelo.Datos;
using FacturacionSimple.Modelo.Entidades;

namespace FacturacionSimple.Controladora;

public class FacturaControladora
{
    private readonly FacturaDatos _datos = new();
    private readonly ProductoDatos _productos = new();

    public List<Factura> Listar(DateTime? desde = null, DateTime? hasta = null, string? cliente = null)
        => _datos.Listar(desde, hasta, cliente);

    public Factura? ObtenerConDetalle(int id) => _datos.ObtenerConDetalle(id);

    public Factura Emitir(
        DateTime fecha,
        string clienteNombre,
        string clienteDocumento,
        List<(int ProductoId, int Cantidad)> lineas)
    {
        if (string.IsNullOrWhiteSpace(clienteNombre))
            throw new InvalidOperationException("El nombre del cliente es obligatorio.");

        if (string.IsNullOrWhiteSpace(clienteDocumento))
            throw new InvalidOperationException("El documento del cliente es obligatorio.");

        if (lineas is null || lineas.Count == 0)
            throw new InvalidOperationException("La factura debe tener al menos una línea.");

        if (lineas.GroupBy(l => l.ProductoId).Any(g => g.Count() > 1))
            throw new InvalidOperationException("No se permiten dos líneas con el mismo producto.");

        var lineasDatos = new List<LineaFacturaDatos>();

        foreach (var (productoId, cantidad) in lineas)
        {
            if (cantidad <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor que cero.");

            var producto = _productos.ObtenerPorId(productoId);
            if (producto is null || !producto.Activo)
                throw new InvalidOperationException("Solo se pueden facturar productos activos.");

            lineasDatos.Add(new LineaFacturaDatos
            {
                ProductoId = productoId,
                Cantidad = cantidad
            });
        }

        var factura = new Factura
        {
            Fecha = fecha,
            ClienteNombre = clienteNombre.Trim(),
            ClienteDocumento = clienteDocumento.Trim()
        };

        return _datos.Emitir(factura, lineasDatos);
    }
}
