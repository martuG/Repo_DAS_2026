namespace Catalogo.Modelo;

/// <summary>
/// Contenedor: una caja o paquete. Guarda productos y otras cajas,
/// y resume el precio de todo lo que tiene adentro.
/// </summary>
public sealed class Caja : IComponente
{
    private readonly List<IComponente> _hijos = new();

    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal CostoEmpaque { get; set; }
    public IReadOnlyList<IComponente> Hijos => _hijos;

    public void Agregar(IComponente hijo)
    {
        _hijos.Add(hijo);
    }

    public decimal Precio()
    {
        decimal suma = CostoEmpaque;
        foreach (IComponente hijo in _hijos)
        {
            suma += hijo.Precio();
        }

        return suma;
    }
}
