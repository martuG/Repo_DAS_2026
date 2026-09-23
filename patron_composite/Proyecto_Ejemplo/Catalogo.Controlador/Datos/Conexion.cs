using Microsoft.Data.SqlClient;

namespace Catalogo.Controlador.Datos;

public static class Conexion
{
    public const string CadenaConexion =
        @"Server=(localdb)\mssqllocaldb;Database=CatalogoComposite;Integrated Security=true;TrustServerCertificate=true;";

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
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Componentes')
            BEGIN
                CREATE TABLE Componentes (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Nombre NVARCHAR(80) NOT NULL,
                    EsCaja BIT NOT NULL,
                    PrecioUnitario DECIMAL(18,2) NULL,
                    CostoEmpaque DECIMAL(18,2) NULL,
                    PadreId INT NULL,
                    CONSTRAINT FK_Componentes_Padre FOREIGN KEY (PadreId) REFERENCES Componentes(Id)
                )
            END
            """,
            conexion);
        crearTabla.ExecuteNonQuery();

        using SqlCommand contar = new SqlCommand("SELECT COUNT(*) FROM Componentes", conexion);
        int cantidad = (int)contar.ExecuteScalar();
        if (cantidad > 0)
        {
            return;
        }

        using SqlCommand semilla = new SqlCommand(
            """
            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Catálogo tienda', 1, NULL, 0, NULL);
            DECLARE @Catalogo INT = CAST(SCOPE_IDENTITY() AS INT);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Pedido de bienvenida', 1, NULL, 1500, @Catalogo);
            DECLARE @Pedido INT = CAST(SCOPE_IDENTITY() AS INT);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Remera', 0, 12000, NULL, @Pedido);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Kit escritorio', 1, NULL, 800, @Pedido);
            DECLARE @Kit INT = CAST(SCOPE_IDENTITY() AS INT);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES
                (N'Mouse', 0, 9000, NULL, @Kit),
                (N'Mousepad', 0, 4000, NULL, @Kit);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Pack cables', 1, NULL, 200, @Kit);
            DECLARE @Cables INT = CAST(SCOPE_IDENTITY() AS INT);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES
                (N'Cable HDMI', 0, 3500, NULL, @Cables),
                (N'Cable USB-C', 0, 2500, NULL, @Cables);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Taza', 0, 6000, NULL, @Pedido);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Libro de patrones', 0, 18000, NULL, @Catalogo);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES (N'Combo regalo', 1, NULL, 1000, @Catalogo);
            DECLARE @Combo INT = CAST(SCOPE_IDENTITY() AS INT);

            INSERT INTO Componentes (Nombre, EsCaja, PrecioUnitario, CostoEmpaque, PadreId)
            VALUES
                (N'Cuaderno', 0, 3000, NULL, @Combo),
                (N'Lapicera', 0, 1500, NULL, @Combo);
            """,
            conexion);
        semilla.ExecuteNonQuery();
    }
}
