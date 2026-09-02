using Microsoft.Data.SqlClient;
using Taller.Entidades;

namespace Taller.Datos;

internal class OrdenDao
{
    public List<Orden> Listar()
    {
        List<Orden> ordenes = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            SELECT o.Id,
                   o.ClienteId,
                   c.Nombre,
                   c.Telefono,
                   o.Fecha,
                   o.Estado,
                   ISNULL((SELECT SUM(i.Subtotal) FROM OrdenItems i WHERE i.OrdenId = o.Id), 0) AS Total
            FROM Ordenes o
            INNER JOIN Clientes c ON c.Id = o.ClienteId
            ORDER BY o.Id DESC
            """,
            conexion);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            ordenes.Add(new Orden
            {
                Id = (int)lector["Id"],
                Cliente = new Cliente
                {
                    Id = (int)lector["ClienteId"],
                    Nombre = lector["Nombre"].ToString() ?? string.Empty,
                    Telefono = lector["Telefono"].ToString() ?? string.Empty
                },
                Fecha = (DateTime)lector["Fecha"],
                Estado = lector["Estado"].ToString() ?? string.Empty,
                Total = Convert.ToDecimal(lector["Total"])
            });
        }

        return ordenes;
    }

    public List<OrdenItem> ListarItems(int ordenId)
    {
        List<OrdenItem> items = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            SELECT Id, OrdenId, Descripcion, Cantidad, PrecioUnitario, Subtotal
            FROM OrdenItems
            WHERE OrdenId = @OrdenId
            ORDER BY Id
            """,
            conexion);
        comando.Parameters.AddWithValue("@OrdenId", ordenId);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            items.Add(MapearItem(lector));
        }

        return items;
    }

    public int Insertar(Orden orden, IReadOnlyList<OrdenItem> items)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("La orden debe tener al menos un ítem.");
        }

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            int ordenId;
            using (SqlCommand comando = new SqlCommand(
                """
                INSERT INTO Ordenes (ClienteId, Fecha, Estado)
                VALUES (@ClienteId, @Fecha, @Estado);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """,
                conexion,
                transaccion))
            {
                comando.Parameters.AddWithValue("@ClienteId", orden.Cliente.Id);
                comando.Parameters.AddWithValue("@Fecha", orden.Fecha.Date);
                comando.Parameters.AddWithValue("@Estado", orden.Estado);
                ordenId = (int)comando.ExecuteScalar();
            }

            foreach (OrdenItem item in items)
            {
                decimal subtotal = item.Cantidad * item.PrecioUnitario;

                using SqlCommand comando = new SqlCommand(
                    """
                    INSERT INTO OrdenItems (OrdenId, Descripcion, Cantidad, PrecioUnitario, Subtotal)
                    VALUES (@OrdenId, @Descripcion, @Cantidad, @PrecioUnitario, @Subtotal)
                    """,
                    conexion,
                    transaccion);

                comando.Parameters.AddWithValue("@OrdenId", ordenId);
                comando.Parameters.AddWithValue("@Descripcion", item.Descripcion);
                comando.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                comando.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario);
                comando.Parameters.AddWithValue("@Subtotal", subtotal);
                comando.ExecuteNonQuery();
            }

            transaccion.Commit();
            return ordenId;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    public int Cerrar(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand consultarEstado = new SqlCommand(
            "SELECT Estado FROM Ordenes WHERE Id = @Id",
            conexion);
        consultarEstado.Parameters.AddWithValue("@Id", id);

        object? resultado = consultarEstado.ExecuteScalar();
        if (resultado is null || resultado is DBNull)
        {
            throw new InvalidOperationException($"No existe la orden {id}.");
        }

        if (string.Equals((string)resultado, "Cerrada", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("La orden ya está cerrada. No se pueden editar sus ítems.");
        }

        using SqlCommand comando = new SqlCommand(
            "UPDATE Ordenes SET Estado = @Estado WHERE Id = @Id",
            conexion);
        comando.Parameters.AddWithValue("@Estado", "Cerrada");
        comando.Parameters.AddWithValue("@Id", id);
        return comando.ExecuteNonQuery();
    }

    private static OrdenItem MapearItem(SqlDataReader lector)
    {
        return new OrdenItem
        {
            Id = (int)lector["Id"],
            OrdenId = (int)lector["OrdenId"],
            Descripcion = lector["Descripcion"].ToString() ?? string.Empty,
            Cantidad = (int)lector["Cantidad"],
            PrecioUnitario = Convert.ToDecimal(lector["PrecioUnitario"]),
            Subtotal = Convert.ToDecimal(lector["Subtotal"])
        };
    }
}
