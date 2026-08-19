using Microsoft.Data.SqlClient;
using AlquilerPeliculas.Entidades;

namespace AlquilerPeliculas.Datos;

internal class PeliculaDao
{
    public List<Pelicula> Listar()
    {
        List<Pelicula> peliculas = new();

        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand("SELECT Id, Nombre, Director, Duracion FROM Peliculas ORDER BY Id", conexion);

        using SqlDataReader lector = comando.ExecuteReader();
        while (lector.Read())
        {
            peliculas.Add(Mapear(lector));
        }

        return peliculas;
    }

    public Pelicula? ObtenerPorId(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand("SELECT Id, Nombre, Director, Duracion FROM Peliculas WHERE Id = @Id", conexion);
        comando.Parameters.AddWithValue("@Id", id);

        using SqlDataReader lector = comando.ExecuteReader();
        if (!lector.Read())
        {
            return null;
        }

        return Mapear(lector);
    }

    public int Insertar(Pelicula pelicula)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            INSERT INTO Peliculas (Nombre, Director, Duracion)
            VALUES (@Nombre, @Director, @Duracion);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            conexion);

        AgregarParametros(comando, pelicula);
        return (int)comando.ExecuteScalar();
    }

    public int Modificar(Pelicula pelicula)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand(
            """
            UPDATE Peliculas
            SET Nombre = @Nombre,
                Director = @Director,
                Duracion = @Duracion
            WHERE Id = @Id
            """,
            conexion);

        comando.Parameters.AddWithValue("@Id", pelicula.Id);
        AgregarParametros(comando, pelicula);
        return comando.ExecuteNonQuery();
    }

    public int Eliminar(int id)
    {
        using SqlConnection conexion = Conexion.Crear();
        conexion.Open();

        using SqlCommand comando = new SqlCommand("DELETE FROM Peliculas WHERE Id = @Id", conexion);
        comando.Parameters.AddWithValue("@Id", id);

        return comando.ExecuteNonQuery();
    }

    private static void AgregarParametros(SqlCommand comando, Pelicula pelicula)
    {
        comando.Parameters.AddWithValue("@Nombre", pelicula.Nombre);
        comando.Parameters.AddWithValue("@Director", pelicula.Director);
        comando.Parameters.AddWithValue("@Duracion", pelicula.Duracion);
    }

    private static Pelicula Mapear(SqlDataReader lector)
    {
        Pelicula pelicula = new Pelicula();

        pelicula.Id = (int)lector["Id"];
        pelicula.Nombre = lector["Nombre"].ToString();
        pelicula.Director = lector["Director"].ToString();
        pelicula.Duracion = (int)lector["Duracion"];

        return pelicula;
    }
}
