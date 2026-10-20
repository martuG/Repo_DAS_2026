namespace FacturacionSimple.Modelo.Entidades;

public class Factura
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string ClienteDocumento { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public ICollection<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
}
