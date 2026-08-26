using Microsoft.Data.SqlClient;

namespace Banco.Datos;

internal static class Conexion
{
    public const string CadenaConexion =
        @"Server=(localdb)\mssqllocaldb;Database=BancoDAS;Integrated Security=true;TrustServerCertificate=true;";

    public static SqlConnection Crear()
    {
        return new SqlConnection(CadenaConexion);
    }

    public static void AsegurarBaseDeDatos()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(CadenaConexion);
        string nombreBase = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        using (SqlConnection master = new SqlConnection(builder.ConnectionString))
        {
            master.Open();
            using SqlCommand crearBase = new SqlCommand(
                $"IF DB_ID('{nombreBase}') IS NULL CREATE DATABASE [{nombreBase}]",
                master);
            crearBase.ExecuteNonQuery();
        }

        using SqlConnection conexion = Crear();
        conexion.Open();

        using SqlCommand crearTabla = new SqlCommand(
            """
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cuentas')
            BEGIN
                CREATE TABLE Cuentas (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Titular NVARCHAR(80) NOT NULL,
                    Balance DECIMAL(18,2) NOT NULL
                )
            END
            """,
            conexion);
        crearTabla.ExecuteNonQuery();

        using SqlCommand hayCuentas = new SqlCommand("SELECT COUNT(*) FROM Cuentas", conexion);
        int cantidad = (int)hayCuentas.ExecuteScalar();
        if (cantidad > 0)
        {
            return;
        }

        using SqlCommand seed = new SqlCommand(
            """
            INSERT INTO Cuentas (Titular, Balance) VALUES
            (N'Ana Perez', 5000),
            (N'Bruno Diaz', 2000),
            (N'Carla Gomez', 1500)
            """,
            conexion);
        seed.ExecuteNonQuery();
    }
}
