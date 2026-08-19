namespace AlquilerPeliculas.Entidades;

public class Pelicula
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public int Duracion { get; set; }
}
