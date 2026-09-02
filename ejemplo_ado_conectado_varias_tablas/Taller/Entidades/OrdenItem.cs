namespace Taller.Entidades;

internal class OrdenItem
{
    public int Id { get; set; }
    public int OrdenId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
