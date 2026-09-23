namespace Catalogo.Modelo;

/// <summary>
/// Hoja: un producto suelto. No contiene otros componentes.
/// </summary>
public sealed class Producto : IComponente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }

    public decimal Precio()
    {
        return PrecioUnitario;
    }
}
