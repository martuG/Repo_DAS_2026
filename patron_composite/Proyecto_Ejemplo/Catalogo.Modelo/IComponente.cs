namespace Catalogo.Modelo;

/// <summary>
/// Componente del patrón Composite. La hoja y la caja exponen la misma operación,
/// así el cliente calcula un precio sin preguntar de qué clase se trata.
/// </summary>
public interface IComponente
{
    int Id { get; }
    string Nombre { get; }
    decimal Precio();
}
