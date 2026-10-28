using ViveroPedidos.Controladora;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Vista;

public partial class FormPlantas : Form
{
    private readonly PlantaControladora _controladora = new();
    private int? _idSeleccionado;

    public FormPlantas()
    {
        InitializeComponent();
        CargarGrilla();
    }

    private void CargarGrilla()
    {
        try
        {
            dgv.DataSource = null;
            dgv.DataSource = _controladora.Listar(txtBuscar.Text);
            if (dgv.Columns.Contains("Detalles"))
                dgv.Columns["Detalles"]!.Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        LimpiarCampos();
    }

    private void LimpiarCampos()
    {
        _idSeleccionado = null;
        txtCodigo.Clear();
        txtNombreComun.Clear();
        txtEspecie.Clear();
        chkDisponible.Checked = true;
        chkDisponible.Enabled = false;
        btnGuardar.Text = "Agregar";
    }

    private void btnBuscar_Click(object? sender, EventArgs e) => CargarGrilla();

    private void btnNuevo_Click(object? sender, EventArgs e) => LimpiarCampos();

    private void dgv_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow?.DataBoundItem is not Planta planta)
            return;

        _idSeleccionado = planta.Id;
        txtCodigo.Text = planta.Codigo;
        txtNombreComun.Text = planta.NombreComun;
        txtEspecie.Text = planta.Especie;
        chkDisponible.Checked = planta.Disponible;
        chkDisponible.Enabled = true;
        btnGuardar.Text = "Modificar";
    }

    private void btnGuardar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_idSeleccionado is null)
            {
                _controladora.GuardarNuevo(txtCodigo.Text, txtNombreComun.Text, txtEspecie.Text);
                MessageBox.Show("Planta agregada.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _controladora.Modificar(
                    _idSeleccionado.Value,
                    txtCodigo.Text,
                    txtNombreComun.Text,
                    txtEspecie.Text,
                    chkDisponible.Checked);
                MessageBox.Show("Planta modificada.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CargarGrilla();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnNoDisponible_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show("Seleccione una planta.", "Aviso");
            return;
        }

        try
        {
            _controladora.MarcarNoDisponible(_idSeleccionado.Value);
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
            MessageBox.Show("Seleccione una planta.", "Aviso");
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
