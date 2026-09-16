namespace MenuRestaurante.Patrones;

public interface IPlato
{
    string Nombre { get; }
    string Descripcion { get; }
}

public interface IEntrada : IPlato
{
}

public interface IPrincipal : IPlato
{
}

public interface IPostre : IPlato
{
}

/// <summary>
/// Fábrica abstracta: una familia completa de productos que combinan entre sí
/// (entrada + principal + postre de la misma cocina). El cliente no conoce
/// las clases concretas (Bruschetta, Ramen, etc.).
/// </summary>
public interface IFabricaMenu
{
    string Cocina { get; }
    IEntrada CrearEntrada();
    IPrincipal CrearPrincipal();
    IPostre CrearPostre();
}
