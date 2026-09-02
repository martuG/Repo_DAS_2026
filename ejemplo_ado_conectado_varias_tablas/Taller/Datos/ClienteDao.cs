using Microsoft.Data.SqlClient;
using Taller.Entidades;

namespace Taller.Datos;

internal class ClienteDao
{
    public List<Cliente> Listar()
    {
        List<Cliente> clientes = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            "SELECT Id, Nombre, Telefono FROM Clientes ORDER BY Id",
            conexion);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            clientes.Add(Mapear(lector));
        }

        return clientes;
    }

    public int Insertar(Cliente cliente)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            INSERT INTO Clientes (Nombre, Telefono)
            VALUES (@Nombre, @Telefono);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            conexion);

        comando.Parameters.AddWithValue("@Nombre", cliente.Nombre);
        comando.Parameters.AddWithValue("@Telefono", cliente.Telefono);
        return (int)comando.ExecuteScalar();
    }

    public int EliminarSiNoTieneOrdenes(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand hayOrdenes = new SqlCommand(
            "SELECT COUNT(*) FROM Ordenes WHERE ClienteId = @Id",
            conexion);
        hayOrdenes.Parameters.AddWithValue("@Id", id);

        int cantidad = (int)hayOrdenes.ExecuteScalar();
        if (cantidad > 0)
        {
            throw new InvalidOperationException("No se puede eliminar: el cliente tiene órdenes asociadas.");
        }

        using SqlCommand comando = new SqlCommand("DELETE FROM Clientes WHERE Id = @Id", conexion);
        comando.Parameters.AddWithValue("@Id", id);
        return comando.ExecuteNonQuery();
    }

    private static Cliente Mapear(SqlDataReader lector)
    {
        return new Cliente
        {
            Id = (int)lector["Id"],
            Nombre = lector["Nombre"].ToString() ?? string.Empty,
            Telefono = lector["Telefono"].ToString() ?? string.Empty
        };
    }
}
