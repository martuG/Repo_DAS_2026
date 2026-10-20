namespace FacturacionSimple.Modelo.Entidades;

public class Producto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
}
