namespace Taller.Entidades;

internal class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Id} - {Nombre}";
    }
}
