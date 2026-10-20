namespace MenuRestaurante.Patrones;

public class Edamame : IEntrada
{
    public string Nombre => "Edamame";
    public string Descripcion => "Vainas de soja al vapor con sal marina.";
}

public class RamenTonkotsu : IPrincipal
{
    public string Nombre => "Ramen tonkotsu";
    public string Descripcion => "Caldo de cerdo, fideos, chashu y huevo marinato.";
}

public class Mochi : IPostre
{
    public string Nombre => "Mochi";
    public string Descripcion => "Pasta de arroz glutinoso con relleno de helado.";
}

public class FabricaMenuJapones : IFabricaMenu
{
    public string Cocina => "Japonesa";

    public IEntrada CrearEntrada() => new Edamame();

    public IPrincipal CrearPrincipal() => new RamenTonkotsu();

    public IPostre CrearPostre() => new Mochi();
}
