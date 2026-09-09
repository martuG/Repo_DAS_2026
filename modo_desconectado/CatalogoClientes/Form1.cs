using System.Data;
using CatalogoClientes.Datos;

namespace CatalogoClientes;

public partial class Form1 : Form
{
    private readonly ClienteDesconectadoDao _dao = new();
    private DataTable _clientes = new();

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
            EnlazarComboEnMemoria();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo conectar a SQL Server LocalDB.\n" +
                "Verificá que esté instalado o ajustá la cadena en Datos/Conexion.cs.\n\n" +
                ex.Message,
                "Error de conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnAgregar_Click(object sender, EventArgs e)
    {
        if (!TryObtenerDatos(requiereId: false, out _, out string nombre, out string telefono))
        {
            return;
        }

        _dao.Agregar(_clientes, nombre, telefono);
        LimpiarCampos();
        dgvClientes.ClearSelection();
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (!TryObtenerDatos(requiereId: true, out int id, out string nombre, out string telefono))
        {
            return;
        }

        if (!_dao.Modificar(_clientes, id, nombre, telefono))
        {
            MessageBox.Show("No hay un cliente activo con ese Id en memoria.", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show("Cliente modificado en memoria. Usá Guardar para persistir.", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (!TryObtenerId(out int id))
        {
            return;
        }

        DialogResult confirmacion = MessageBox.Show(
            $"¿Marcar de baja el cliente con Id {id}? El borrado se persiste al Guardar (Delete, no Remove).",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        if (!_dao.Eliminar(_clientes, id))
        {
            MessageBox.Show("No hay un cliente activo con ese Id en memoria.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        LimpiarCampos();
        dgvClientes.ClearSelection();
    }

    private void btnAgregarLote_Click(object sender, EventArgs e)
    {
        string[] lineas = txtLote.Lines;
        int cantidad = _dao.AgregarLoteDesdeLineas(_clientes, lineas);

        if (cantidad == 0)
        {
            MessageBox.Show(
                "No hay líneas válidas. Usá el formato Nombre;Telefono, una por renglón.",
                "Alta en lote",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        txtLote.Clear();
        MessageBox.Show(
            $"Se agregaron {cantidad} clientes en memoria. Un solo Update los persiste al Guardar.",
            "Alta en lote",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        ConfirmarEdicionDeGrilla();

        if (!_clientes.HasChanges())
        {
            MessageBox.Show("No hay cambios pendientes en memoria.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            int n = _dao.Guardar(_clientes);
            MessageBox.Show($"Se persistieron {n} cambios.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarGrilla();
            LimpiarCampos();
        }
        catch (DBConcurrencyException)
        {
            MessageBox.Show(
                "Otro usuario modificó el mismo registro. Vuelva a cargar los datos.",
                "Conflicto de concurrencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo guardar.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnRecargar_Click(object sender, EventArgs e)
    {
        try
        {
            CargarGrilla();
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo recargar.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
        dgvClientes.ClearSelection();
    }

    private void dgvClientes_SelectionChanged(object sender, EventArgs e)
    {
        DataRow? fila = FilaSeleccionada();
        if (fila is null)
        {
            return;
        }

        txtId.Text = fila["Id"].ToString();
        txtNombre.Text = fila["Nombre"].ToString();
        txtTelefono.Text = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString();
    }

    private void cmbPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbPersonas.SelectedValue is int id)
        {
            lblPersonaSeleccionada.Text = $"ValueMember (Id): {id}";
        }
        else if (cmbPersonas.SelectedValue is not null && int.TryParse(cmbPersonas.SelectedValue.ToString(), out int idParseado))
        {
            lblPersonaSeleccionada.Text = $"ValueMember (Id): {idParseado}";
        }
        else
        {
            lblPersonaSeleccionada.Text = "ValueMember (Id): —";
        }
    }

    private void CargarGrilla()
    {
        _clientes = _dao.Cargar();
        dgvClientes.DataSource = _clientes;

        if (dgvClientes.Columns["Id"] is not null)
        {
            dgvClientes.Columns["Id"].ReadOnly = true;
        }

        dgvClientes.ClearSelection();
    }

    private DataRow? FilaSeleccionada()
    {
        if (dgvClientes.CurrentRow?.DataBoundItem is not DataRowView vista)
        {
            return null;
        }

        DataRow fila = vista.Row;
        if (fila.RowState is DataRowState.Deleted or DataRowState.Detached)
        {
            return null;
        }

        return fila;
    }

    private bool TryObtenerDatos(bool requiereId, out int id, out string nombre, out string telefono)
    {
        id = 0;
        nombre = txtNombre.Text.Trim();
        telefono = txtTelefono.Text.Trim();

        if (requiereId && !TryObtenerId(out id))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private bool TryObtenerId(out int id)
    {
        if (!int.TryParse(txtId.Text, out id))
        {
            MessageBox.Show("Seleccioná un cliente de la grilla.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private void LimpiarCampos()
    {
        txtId.Clear();
        txtNombre.Clear();
        txtTelefono.Clear();
        txtNombre.Focus();
    }

    private void ConfirmarEdicionDeGrilla()
    {
        Validate();
        dgvClientes.EndEdit();
        if (BindingContext[_clientes] is CurrencyManager administrador)
        {
            administrador.EndCurrentEdit();
        }
    }

    private void EnlazarComboEnMemoria()
    {
        DataSet ds = new DataSet("Personas");
        DataTable dt = new DataTable("Persona");

        DataColumn colId = new DataColumn("Id", typeof(int));
        DataColumn colNombre = new DataColumn("Nombre", typeof(string));
        colNombre.MaxLength = 50;

        dt.Columns.Add(colId);
        dt.Columns.Add(colNombre);
        dt.PrimaryKey = new DataColumn[] { colId };

        DataRow dr = dt.NewRow();
        dr["Id"] = 1;
        dr["Nombre"] = "Juan";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Id"] = 2;
        dr["Nombre"] = "Pedro";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Id"] = 3;
        dr["Nombre"] = "Lucía";
        dt.Rows.Add(dr);

        ds.Tables.Add(dt);

        cmbPersonas.DisplayMember = "Nombre";
        cmbPersonas.ValueMember = "Id";
        cmbPersonas.DataSource = ds.Tables["Persona"];
    }
}
