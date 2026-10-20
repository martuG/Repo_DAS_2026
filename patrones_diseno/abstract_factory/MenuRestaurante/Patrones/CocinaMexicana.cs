namespace MenuRestaurante.Patrones;

public class Guacamole : IEntrada
{
    public string Nombre => "Guacamole";
    public string Descripcion => "Palta, lima, cilantro y nachos.";
}

public class TacosAlPastor : IPrincipal
{
    public string Nombre => "Tacos al pastor";
    public string Descripcion => "Cerdo adobado, piña, cebolla y salsa.";
}

public class Flan : IPostre
{
    public string Nombre => "Flan";
    public string Descripcion => "Flan de vainilla con caramelo.";
}

public class FabricaMenuMexicano : IFabricaMenu
{
    public string Cocina => "Mexicana";

    public IEntrada CrearEntrada() => new Guacamole();

    public IPrincipal CrearPrincipal() => new TacosAlPastor();

    public IPostre CrearPostre() => new Flan();
}
