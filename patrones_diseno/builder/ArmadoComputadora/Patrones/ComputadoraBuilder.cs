namespace ArmadoComputadora.Patrones;

public class ComputadoraBuilder : IComputadoraBuilder
{
    private Computadora _computadora = new();

    public IComputadoraBuilder ConPerfil(string perfil)
    {
        _computadora.Perfil = perfil;
        return this;
    }

    public IComputadoraBuilder ConCpu(string cpu)
    {
        _computadora.Cpu = cpu;
        return this;
    }

    public IComputadoraBuilder ConRam(string ram)
    {
        _computadora.Ram = ram;
        return this;
    }

    public IComputadoraBuilder ConGpu(string gpu)
    {
        _computadora.Gpu = gpu;
        return this;
    }

    public IComputadoraBuilder ConDisco(string disco)
    {
        _computadora.Disco = disco;
        return this;
    }

    public IComputadoraBuilder ConFuente(string fuente)
    {
        _computadora.Fuente = fuente;
        return this;
    }

    public IComputadoraBuilder ConGabinete(string gabinete)
    {
        _computadora.Gabinete = gabinete;
        return this;
    }

    public void Reiniciar()
    {
        _computadora = new Computadora();
    }

    public Computadora Construir()
    {
        Computadora resultado = _computadora;
        Reiniciar();
        return resultado;
    }
}
