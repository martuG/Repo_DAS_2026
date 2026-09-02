using Microsoft.Data.SqlClient;

namespace Taller.Datos;

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

        using SqlCommand crearTablas = new SqlCommand(
            """
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
            BEGIN
                CREATE TABLE Clientes (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Nombre NVARCHAR(80) NOT NULL,
                    Telefono NVARCHAR(30) NOT NULL
                )
            END

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ordenes')
            BEGIN
                CREATE TABLE Ordenes (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ClienteId INT NOT NULL,
                    Fecha DATE NOT NULL,
                    Estado NVARCHAR(20) NOT NULL,
                    CONSTRAINT FK_Ordenes_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
                )
            END

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrdenItems')
            BEGIN
                CREATE TABLE OrdenItems (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    OrdenId INT NOT NULL,
                    Descripcion NVARCHAR(120) NOT NULL,
                    Cantidad INT NOT NULL,
                    PrecioUnitario DECIMAL(18,2) NOT NULL,
                    Subtotal DECIMAL(18,2) NOT NULL,
                    CONSTRAINT FK_OrdenItems_Ordenes FOREIGN KEY (OrdenId) REFERENCES Ordenes(Id)
                )
            END
            """,
            conexion);
        crearTablas.ExecuteNonQuery();

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
