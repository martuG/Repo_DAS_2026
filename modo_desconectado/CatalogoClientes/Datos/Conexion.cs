using Microsoft.Data.SqlClient;

namespace CatalogoClientes.Datos;

internal static class Conexion
{
    public const string CadenaConexion =
        @"Server=(localdb)\mssqllocaldb;Database=TallerDAS;Integrated Security=true;TrustServerCertificate=true;";

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
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
            BEGIN
                CREATE TABLE Clientes (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Nombre NVARCHAR(80) NOT NULL,
                    Telefono NVARCHAR(30) NULL
                )
            END
            """,
            conexion);
        crearTabla.ExecuteNonQuery();

        using SqlCommand hayClientes = new SqlCommand("SELECT COUNT(*) FROM Clientes", conexion);
        int cantidad = (int)hayClientes.ExecuteScalar();
        if (cantidad > 0)
        {
            return;
        }

        using SqlCommand seed = new SqlCommand(
            """
            INSERT INTO Clientes (Nombre, Telefono) VALUES
            (N'Ana Perez', N'11-5555-1111'),
            (N'Bruno Diaz', N'11-5555-2222'),
            (N'Carla Gomez', N'11-5555-3333')
            """,
            conexion);
        seed.ExecuteNonQuery();
    }
}
