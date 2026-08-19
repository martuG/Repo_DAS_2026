using Microsoft.Data.SqlClient;

namespace AlquilerPeliculas.Datos;

internal static class Conexion
{
    public static string CadenaConexion => $"...";

    public static SqlConnection Crear()
    {
        return new SqlConnection(CadenaConexion);
    }

}
