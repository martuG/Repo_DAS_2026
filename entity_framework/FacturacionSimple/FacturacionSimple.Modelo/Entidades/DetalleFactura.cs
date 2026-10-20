namespace FacturacionSimple.Modelo.Entidades;

public class DetalleFactura
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public Factura Factura { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
