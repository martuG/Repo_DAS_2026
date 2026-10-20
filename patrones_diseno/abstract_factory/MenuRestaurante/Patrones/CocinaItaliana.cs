namespace MenuRestaurante.Patrones;

public class Bruschetta : IEntrada
{
    public string Nombre => "Bruschetta";
    public string Descripcion => "Pan tostado con tomate, ajo y albahaca.";
}

public class FettuccineAlfredo : IPrincipal
{
    public string Nombre => "Fettuccine Alfredo";
    public string Descripcion => "Pasta fresca con salsa de crema y parmesano.";
}

public class Tiramisu : IPostre
{
    public string Nombre => "Tiramisú";
    public string Descripcion => "Bizcocho, café, mascarpone y cacao.";
}

public class FabricaMenuItaliano : IFabricaMenu
{
    public string Cocina => "Italiana";

    public IEntrada CrearEntrada() => new Bruschetta();

    public IPrincipal CrearPrincipal() => new FettuccineAlfredo();

    public IPostre CrearPostre() => new Tiramisu();
}
