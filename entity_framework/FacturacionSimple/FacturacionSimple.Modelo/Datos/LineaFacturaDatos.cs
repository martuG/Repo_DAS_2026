namespace FacturacionSimple.Modelo.Datos;

/// <summary>
/// Línea temporal para emitir una factura (antes de persistir).
/// El precio unitario se copia del producto al grabar.
/// </summary>
public class LineaFacturaDatos
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}
