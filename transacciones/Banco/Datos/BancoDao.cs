using Banco.Entidades;
using Microsoft.Data.SqlClient;

namespace Banco.Datos;

internal class BancoDao
{
    public List<Cuenta> Listar()
    {
        List<Cuenta> cuentas = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            "SELECT Id, Titular, Balance FROM Cuentas ORDER BY Id",
            conexion);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            cuentas.Add(new Cuenta
            {
                Id = (int)lector["Id"],
                Titular = lector["Titular"].ToString() ?? string.Empty,
                Saldo = (decimal)lector["Balance"]
            });
        }

        return cuentas;
    }

    public void Transferir(int desde, int hacia, decimal monto)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            decimal saldoOrigen = ObtenerSaldo(conexion, transaccion, desde);

            if (saldoOrigen < monto)
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }

            using (SqlCommand comando = new SqlCommand(
                "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @id",
                conexion,
                transaccion))
            {
                comando.Parameters.AddWithValue("@monto", monto);
                comando.Parameters.AddWithValue("@id", desde);
                comando.ExecuteNonQuery();
            }

            using (SqlCommand comando = new SqlCommand(
                "UPDATE Cuentas SET Balance = Balance + @monto WHERE Id = @id",
                conexion,
                transaccion))
            {
                comando.Parameters.AddWithValue("@monto", monto);
                comando.Parameters.AddWithValue("@id", hacia);
                comando.ExecuteNonQuery();
            }

            transaccion.Commit();
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    private static decimal ObtenerSaldo(SqlConnection conexion, SqlTransaction transaccion, int id)
    {
        using SqlCommand comando = new SqlCommand(
            "SELECT Balance FROM Cuentas WHERE Id = @id",
            conexion,
            transaccion);
        comando.Parameters.AddWithValue("@id", id);

        object? resultado = comando.ExecuteScalar();
        if (resultado is null || resultado is DBNull)
        {
            throw new InvalidOperationException($"No existe la cuenta {id}.");
        }

        return (decimal)resultado;
    }
}
