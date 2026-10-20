namespace ArmadoComputadora.Patrones;

/// <summary>
/// Interfaz del Builder: cada método configura una parte y permite encadenar
/// llamadas (API fluida). Construir() devuelve el producto terminado.
/// </summary>
public interface IComputadoraBuilder
{
    IComputadoraBuilder ConPerfil(string perfil);
    IComputadoraBuilder ConCpu(string cpu);
    IComputadoraBuilder ConRam(string ram);
    IComputadoraBuilder ConGpu(string gpu);
    IComputadoraBuilder ConDisco(string disco);
    IComputadoraBuilder ConFuente(string fuente);
    IComputadoraBuilder ConGabinete(string gabinete);
    void Reiniciar();
    Computadora Construir();
}
