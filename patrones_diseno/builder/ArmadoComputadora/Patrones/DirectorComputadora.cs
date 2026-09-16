namespace ArmadoComputadora.Patrones;

/// <summary>
/// Director: conoce recetas de armado. Usa el Builder sin saber cómo
/// representa internamente el producto. El cliente puede omitir el Director
/// y configurar el Builder a mano (modo personalizado).
/// </summary>
public class DirectorComputadora
{
    public Computadora ArmarOficina(IComputadoraBuilder builder)
    {
        return builder
            .ConPerfil("Oficina")
            .ConCpu("Intel Core i5-12400")
            .ConRam("16 GB DDR4")
            .ConGpu("Integrada")
            .ConDisco("512 GB SSD")
            .ConFuente("500 W")
            .ConGabinete("Mini Tower")
            .Construir();
    }

    public Computadora ArmarDesarrollo(IComputadoraBuilder builder)
    {
        return builder
            .ConPerfil("Desarrollo")
            .ConCpu("AMD Ryzen 7 5800X")
            .ConRam("32 GB DDR4")
            .ConGpu("NVIDIA RTX 3060")
            .ConDisco("1 TB SSD")
            .ConFuente("650 W")
            .ConGabinete("Mid Tower")
            .Construir();
    }

    public Computadora ArmarGamer(IComputadoraBuilder builder)
    {
        return builder
            .ConPerfil("Gamer")
            .ConCpu("Intel Core i7-13700K")
            .ConRam("32 GB DDR5")
            .ConGpu("NVIDIA RTX 4080")
            .ConDisco("2 TB SSD")
            .ConFuente("850 W")
            .ConGabinete("Full Tower")
            .Construir();
    }
}
