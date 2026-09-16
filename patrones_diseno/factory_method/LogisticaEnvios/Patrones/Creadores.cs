namespace LogisticaEnvios.Patrones;

/// <summary>
/// Creador: declara el Factory Method (CrearTransporte). El algoritmo de
/// despacho usa el producto sin saber si es Camión, Barco o Avión.
/// Cada subclase redefine el método y elige el producto concreto.
/// </summary>
public abstract class CreadorEnvio
{
    public abstract string Canal { get; }

    public abstract ITransporte CrearTransporte();

    public string Despachar(string destino)
    {
        ITransporte transporte = CrearTransporte();
        return
            $"Creador: {GetType().Name}{Environment.NewLine}" +
            $"Factory Method devolvió: {transporte.GetType().Name} ({transporte.Nombre}){Environment.NewLine}" +
            transporte.Entregar(destino);
    }
}

public class CreadorTerrestre : CreadorEnvio
{
    public override string Canal => "Terrestre";

    public override ITransporte CrearTransporte() => new Camion();
}

public class CreadorMaritimo : CreadorEnvio
{
    public override string Canal => "Marítimo";

    public override ITransporte CrearTransporte() => new Barco();
}

public class CreadorAereo : CreadorEnvio
{
    public override string Canal => "Aéreo";

    public override ITransporte CrearTransporte() => new Avion();
}
