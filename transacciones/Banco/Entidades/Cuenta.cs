namespace Banco.Entidades;

internal class Cuenta
{
    public int Id { get; set; }
    public string Titular { get; set; } = string.Empty;
    public decimal Saldo { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Titular} (${Saldo:N2})";
    }
}
