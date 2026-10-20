using Microsoft.Data.SqlClient;
using Catalogo.Modelo;
using Catalogo.Modelo.Datos;

namespace Catalogo.Modelo.Repositorios;

public class ComponenteRepositorio : IComponenteRepositorio
{
    public void Inicializar()
    {
        Conexion.AsegurarBaseDeDatos();
    }

    public IReadOnlyList<IComponente> ListarRaices()
    {
        List<RegistroComponente> registros = ListarRegistros();
        return EnsambladorCatalogo.Armar(registros);
    }

    public int Agregar(string nombre, bool esCaja, decimal? precioUnitario, decimal? costoEmpaque, int? padreId)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (@Nombre, @EsCaja, @PrecioUnitario, @CostoEmpaque, @PadreId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            conexion);

        comando.Parameters.AddWithValue("@Nombre", nombre);
        comando.Parameters.AddWithValue("@EsCaja", esCaja);
        comando.Parameters.AddWithValue("@PrecioUnitario", (object?)precioUnitario ?? DBNull.Value);
        comando.Parameters.AddWithValue("@CostoEmpaque", (object?)costoEmpaque ?? DBNull.Value);
        comando.Parameters.AddWithValue("@PadreId", (object?)padreId ?? DBNull.Value);

        return (int)comando.ExecuteScalar();
    }

    public bool? EsCaja(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            "SELECT EsCaja FROM Componentes WHERE Id = @Id",
            conexion);
        comando.Parameters.AddWithValue("@Id", id);

        object? resultado = comando.ExecuteScalar();
        if (resultado is null || resultado is DBNull)
        {
            return null;
        }

        return (bool)resultado;
    }

    public int ContarHijos(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            "SELECT COUNT(*) FROM Componentes WHERE PadreId = @Id",
            conexion);
        comando.Parameters.AddWithValue("@Id", id);

        return (int)comando.ExecuteScalar();
    }

    public int Eliminar(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            "DELETE FROM Componentes WHERE Id = @Id",
            conexion);
        comando.Parameters.AddWithValue("@Id", id);

        return comando.ExecuteNonQuery();
    }

    private static List<RegistroComponente> ListarRegistros()
    {
        List<RegistroComponente> registros = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            SELECT Id, Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId
            FROM Componentes
            ORDER BY Id
            """,
            conexion);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            registros.Add(new RegistroComponente
            {
                Id = (int)lector["Id"],
                Nombre = lector["Nombre"].ToString() ?? string.Empty,
                EsCaja = (bool)lector["EsCaja"],
                PrecioUnitario = lector["PrecioUnitario"] is DBNull ? 0 : Convert.ToDecimal(lector["PrecioUnitario"]),
                CostoEmpaque = lector["CostoEmpaque"] is DBNull ? 0 : Convert.ToDecimal(lector["CostoEmpaque"]),
                PadreId = lector["PadreId"] is DBNull ? null : (int)lector["PadreId"]
            });
        }

        return registros;
    }
}
