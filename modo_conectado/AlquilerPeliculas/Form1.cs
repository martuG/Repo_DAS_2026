using AlquilerPeliculas.Datos;
using AlquilerPeliculas.Entidades;

namespace AlquilerPeliculas;

public partial class Form1 : Form
{
    private readonly PeliculaDao _dao = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            Conexion.AsegurarBaseDeDatos();
            CargarGrilla();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo conectar a SQL Server LocalDB.\n" +
                "Verificá que esté instalado o ajustá la cadena de conexión en Datos/Conexion.cs.\n\n" +
                ex.Message,
                "Error de conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CargarGrilla()
    {
        dgvPeliculas.DataSource = null;
        dgvPeliculas.DataSource = _dao.Listar();
        dgvPeliculas.ClearSelection();
    }

    private void btnListar_Click(object sender, EventArgs e)
    {
        Ejecutar("listar las películas", CargarGrilla);
    }

    private void btnAgregar_Click(object sender, EventArgs e)
    {
        if (!TryObtenerPeliculaDesdeFormulario(requiereId: false, out Pelicula pelicula))
        {
            return;
        }

        Ejecutar("agregar la película", () =>
        {
            pelicula.Id = _dao.Insertar(pelicula);
            CargarGrilla();
            LimpiarCampos();
            MessageBox.Show($"Película agregada con Id {pelicula.Id}.", "Alta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (!TryObtenerPeliculaDesdeFormulario(requiereId: true, out Pelicula pelicula))
        {
            return;
        }

        Ejecutar("modificar la película", () =>
        {
            int filas = _dao.Modificar(pelicula);
            if (filas == 0)
            {
                MessageBox.Show("No existe una película con ese Id.", "Modificación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarGrilla();
            MessageBox.Show("Película modificada.", "Modificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtId.Text, out int id) || id <= 0)
        {
            MessageBox.Show("Seleccioná una película de la grilla o ingresá un Id válido.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult confirmacion = MessageBox.Show(
            $"¿Eliminar la película con Id {id}?",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        Ejecutar("eliminar la película", () =>
        {
            int filas = _dao.Eliminar(id);
            if (filas == 0)
            {
                MessageBox.Show("No existe una película con ese Id.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarGrilla();
            LimpiarCampos();
            MessageBox.Show("Película eliminada.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
        dgvPeliculas.ClearSelection();
    }

    private void dgvPeliculas_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvPeliculas.CurrentRow?.DataBoundItem is not Pelicula pelicula)
        {
            return;
        }

        txtId.Text = pelicula.Id.ToString();
        txtNombre.Text = pelicula.Nombre;
        txtDirector.Text = pelicula.Director;
        txtDuracion.Text = pelicula.Duracion.ToString();
    }

    private bool TryObtenerPeliculaDesdeFormulario(bool requiereId, out Pelicula pelicula)
    {
        pelicula = new Pelicula();

        if (requiereId)
        {
            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("Seleccioná una película de la grilla o ingresá un Id válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            pelicula.Id = id;
        }

        string nombre = txtNombre.Text.Trim();
        string director = txtDirector.Text.Trim();

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(director))
        {
            MessageBox.Show("El nombre y el director son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(txtDuracion.Text, out int duracion) || duracion <= 0)
        {
            MessageBox.Show("La duración debe ser un número entero de minutos mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        pelicula.Nombre = nombre;
        pelicula.Director = director;
        pelicula.Duracion = duracion;
        return true;
    }

    private void LimpiarCampos()
    {
        txtId.Clear();
        txtNombre.Clear();
        txtDirector.Clear();
        txtDuracion.Clear();
        txtNombre.Focus();
    }

    private void Ejecutar(string accion, Action operacion)
    {
        try
        {
            operacion();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo {accion}.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
