using FacturacionSimple.Controladora;
using FacturacionSimple.Modelo.Entidades;

namespace FacturacionSimple.Vista;

public partial class FormProductos : Form
{
    private readonly ProductoControladora _controladora = new();
    private int? _idSeleccionado;

    public FormProductos()
    {
        InitializeComponent();
        CargarGrilla();
    }

    private void CargarGrilla()
    {
        dgv.DataSource = null;
        dgv.DataSource = _controladora.Listar(txtBuscar.Text);
        if (dgv.Columns.Contains("Detalles"))
            dgv.Columns["Detalles"]!.Visible = false;
        LimpiarCampos();
    }

    private void LimpiarCampos()
    {
        _idSeleccionado = null;
        txtCodigo.Clear();
        txtNombre.Clear();
        numPrecio.Value = 0;
        chkActivo.Checked = true;
        btnGuardar.Text = "Agregar";
    }

    private void btnBuscar_Click(object? sender, EventArgs e) => CargarGrilla();

    private void btnNuevo_Click(object? sender, EventArgs e) => LimpiarCampos();

    private void dgv_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow?.DataBoundItem is not Producto p)
            return;

        _idSeleccionado = p.Id;
        txtCodigo.Text = p.Codigo;
        txtNombre.Text = p.Nombre;
        numPrecio.Value = p.Precio;
        chkActivo.Checked = p.Activo;
        btnGuardar.Text = "Modificar";
    }

    private void btnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_idSeleccionado is null)
            {
                _controladora.GuardarNuevo(txtCodigo.Text, txtNombre.Text, numPrecio.Value);
                MessageBox.Show("Producto agregado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _controladora.Modificar(
                    _idSeleccionado.Value,
                    txtCodigo.Text,
                    txtNombre.Text,
                    numPrecio.Value,
                    chkActivo.Checked);
                MessageBox.Show("Producto modificado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CargarGrilla();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnBaja_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show("Seleccione un producto.", "Aviso");
            return;
        }

        try
        {
            _controladora.DarDeBaja(_idSeleccionado.Value);
            CargarGrilla();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnEliminar_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show("Seleccione un producto.", "Aviso");
            return;
        }

        if (MessageBox.Show("¿Eliminar físicamente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            _controladora.Eliminar(_idSeleccionado.Value);
            CargarGrilla();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
