namespace Catalogo.Modelo;

/// <summary>
/// Fila plana leída de la base. El árbol se arma después, en memoria.
/// </summary>
public sealed class RegistroComponente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool EsCaja { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal CostoEmpaque { get; set; }
    public int? PadreId { get; set; }
}
