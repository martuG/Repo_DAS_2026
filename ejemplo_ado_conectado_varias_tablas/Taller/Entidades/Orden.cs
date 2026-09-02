namespace Taller.Entidades;

internal class Orden
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; } = new();
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
