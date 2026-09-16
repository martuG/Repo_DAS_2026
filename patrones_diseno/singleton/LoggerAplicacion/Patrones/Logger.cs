namespace LoggerAplicacion.Patrones;

/// <summary>
/// Patrón Singleton: garantiza que exista una sola instancia y ofrece un
/// punto de acceso global (Logger.Instancia). El constructor es privado
/// para impedir "new Logger()" desde afuera.
/// </summary>
public sealed class Logger
{
    private static Logger? _instancia;
    private static readonly object _candado = new();
    private readonly List<string> _registros = new();

    public Guid IdInstancia { get; } = Guid.NewGuid();

    public event Action? Cambio;

    private Logger()
    {
    }

    public static Logger Instancia
    {
        get
        {
            lock (_candado)
            {
                return _instancia ??= new Logger();
            }
        }
    }

    public IReadOnlyList<string> Registros => _registros;

    public void Registrar(string origen, string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
        {
            return;
        }

        _registros.Add($"[{DateTime.Now:HH:mm:ss}] [{origen}] {mensaje.Trim()}");
        Cambio?.Invoke();
    }
}
