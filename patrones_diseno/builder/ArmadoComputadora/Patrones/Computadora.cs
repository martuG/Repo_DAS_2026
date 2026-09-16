namespace ArmadoComputadora.Patrones;

/// <summary>
/// Producto complejo que se construye paso a paso. No se arma con un
/// constructor de muchos parámetros: el Builder va completando cada parte.
/// </summary>
public class Computadora
{
    public string Perfil { get; set; } = "Personalizado";
    public string Cpu { get; set; } = string.Empty;
    public string Ram { get; set; } = string.Empty;
    public string Gpu { get; set; } = string.Empty;
    public string Disco { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
    public string Gabinete { get; set; } = string.Empty;

    public override string ToString()
    {
        return
            $"Perfil:    {Perfil}{Environment.NewLine}" +
            $"CPU:       {Cpu}{Environment.NewLine}" +
            $"RAM:       {Ram}{Environment.NewLine}" +
            $"GPU:       {Gpu}{Environment.NewLine}" +
            $"Disco:     {Disco}{Environment.NewLine}" +
            $"Fuente:    {Fuente}{Environment.NewLine}" +
            $"Gabinete:  {Gabinete}";
    }
}
