namespace LogisticaEnvios.Patrones;

/// <summary>
/// Producto: el medio de transporte concreto lo decide la subclase del creador.
/// </summary>
public interface ITransporte
{
    string Nombre { get; }
    decimal CostoBase { get; }
    string TiempoEstimado { get; }
    string Entregar(string destino);
}

public class Camion : ITransporte
{
    public string Nombre => "Camión";
    public decimal CostoBase => 15000m;
    public string TiempoEstimado => "2 a 4 días";

    public string Entregar(string destino) =>
        $"El camión sale por ruta hacia {destino}. Costo ${CostoBase:N0}. Tiempo: {TiempoEstimado}.";
}

public class Barco : ITransporte
{
    public string Nombre => "Barco";
    public decimal CostoBase => 28000m;
    public string TiempoEstimado => "8 a 15 días";

    public string Entregar(string destino) =>
        $"El barco zarpa con contenedor hacia {destino}. Costo ${CostoBase:N0}. Tiempo: {TiempoEstimado}.";
}

public class Avion : ITransporte
{
    public string Nombre => "Avión";
    public decimal CostoBase => 45000m;
    public string TiempoEstimado => "24 a 48 horas";

    public string Entregar(string destino) =>
        $"El avión despega con carga hacia {destino}. Costo ${CostoBase:N0}. Tiempo: {TiempoEstimado}.";
}
