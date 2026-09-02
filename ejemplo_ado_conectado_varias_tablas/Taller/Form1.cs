using Taller.Datos;
using Taller.Entidades;

namespace Taller;

public partial class Form1 : Form
{
    private readonly ClienteDao _clienteDao = new();
    private readonly OrdenDao _ordenDao = new();
    private readonly List<OrdenItem> _itemsNuevos = new();
    private bool _cargando;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            Conexion.AsegurarBaseDeDatos();
            lblFechaValor.Text = DateTime.Today.ToString("d");
            CargarClientes();
            CargarOrdenes();
            ActualizarGrillaItemsNuevos();
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

    private void tabPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabPrincipal.SelectedTab == tabNuevaOrden)
        {
            CargarComboClientes();
        }
        else if (tabPrincipal.SelectedTab == tabConsulta)
        {
            CargarOrdenes();
        }
        else if (tabPrincipal.SelectedTab == tabClientes)
        {
            CargarClientes();
        }
    }

    private void btnAgregarCliente_Click(object sender, EventArgs e)
    {
        string nombre = txtNombreCliente.Text.Trim();
        string telefono = txtTelefono.Text.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(telefono))
        {
            MessageBox.Show("El teléfono es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Ejecutar("agregar el cliente", () =>
        {
            Cliente cliente = new Cliente { Nombre = nombre, Telefono = telefono };
            cliente.Id = _clienteDao.Insertar(cliente);
            CargarClientes();
            LimpiarCamposCliente();
            MessageBox.Show($"Cliente agregado con Id {cliente.Id}.", "Alta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnEliminarCliente_Click(object sender, EventArgs e)
    {
        if (dgvClientes.CurrentRow?.DataBoundItem is not Cliente cliente)
        {
            MessageBox.Show("Seleccioná un cliente de la grilla.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult confirmacion = MessageBox.Show(
            $"¿Eliminar al cliente {cliente.Nombre}?",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        Ejecutar("eliminar el cliente", () =>
        {
            int filas = _clienteDao.EliminarSiNoTieneOrdenes(cliente.Id);
            if (filas == 0)
            {
                MessageBox.Show("No existe un cliente con ese Id.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarClientes();
            LimpiarCamposCliente();
            MessageBox.Show("Cliente eliminado.", "Eliminación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnListarClientes_Click(object sender, EventArgs e)
    {
        Ejecutar("listar los clientes", CargarClientes);
    }

    private void btnLimpiarCliente_Click(object sender, EventArgs e)
    {
        LimpiarCamposCliente();
        dgvClientes.ClearSelection();
    }

    private void dgvClientes_SelectionChanged(object sender, EventArgs e)
    {
        if (_cargando || dgvClientes.CurrentRow?.DataBoundItem is not Cliente cliente)
        {
            return;
        }

        txtNombreCliente.Text = cliente.Nombre;
        txtTelefono.Text = cliente.Telefono;
    }

    private void btnAgregarItem_Click(object sender, EventArgs e)
    {
        string descripcion = txtDescripcion.Text.Trim();
        if (string.IsNullOrWhiteSpace(descripcion))
        {
            MessageBox.Show("La descripción es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
        {
            MessageBox.Show("La cantidad debe ser un entero mayor a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
        {
            MessageBox.Show("El precio unitario debe ser un número mayor o igual a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _itemsNuevos.Add(new OrdenItem
        {
            Descripcion = descripcion,
            Cantidad = cantidad,
            PrecioUnitario = precio,
            Subtotal = cantidad * precio
        });

        ActualizarGrillaItemsNuevos();
        txtDescripcion.Clear();
        txtCantidad.Clear();
        txtPrecio.Clear();
        txtDescripcion.Focus();
    }

    private void btnQuitarItem_Click(object sender, EventArgs e)
    {
        if (dgvItemsNuevos.CurrentRow?.DataBoundItem is not OrdenItem item)
        {
            MessageBox.Show("Seleccioná un ítem de la grilla.", "Ítems", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _itemsNuevos.Remove(item);
        ActualizarGrillaItemsNuevos();
    }

    private void btnGuardarOrden_Click(object sender, EventArgs e)
    {
        if (cmbCliente.SelectedItem is not Cliente cliente)
        {
            MessageBox.Show("Elegí un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_itemsNuevos.Count == 0)
        {
            MessageBox.Show("La orden debe tener al menos un ítem.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Ejecutar("guardar la orden", () =>
        {
            Orden orden = new Orden
            {
                Cliente = cliente,
                Fecha = DateTime.Today,
                Estado = "Abierta"
            };
            int ordenId = _ordenDao.Insertar(orden, _itemsNuevos);
            _itemsNuevos.Clear();
            ActualizarGrillaItemsNuevos();
            CargarOrdenes();
            MessageBox.Show($"Orden guardada con Id {ordenId}.", "Alta de orden", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void btnActualizarConsulta_Click(object sender, EventArgs e)
    {
        Ejecutar("listar las órdenes", CargarOrdenes);
    }

    private void btnCerrarOrden_Click(object sender, EventArgs e)
    {
        if (dgvOrdenes.CurrentRow?.DataBoundItem is not Orden orden)
        {
            MessageBox.Show("Seleccioná una orden.", "Cerrar orden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.Equals(orden.Estado, "Cerrada", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("La orden ya está cerrada. No se pueden editar sus ítems.", "Cerrar orden", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        DialogResult confirmacion = MessageBox.Show(
            $"¿Cerrar la orden {orden.Id} de {orden.Cliente.Nombre}?",
            "Confirmar cierre",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        Ejecutar("cerrar la orden", () =>
        {
            _ordenDao.Cerrar(orden.Id);
            CargarOrdenes();
            MessageBox.Show("Orden cerrada.", "Cerrar orden", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
    {
        if (_cargando)
        {
            return;
        }

        CargarItemsConsulta();
    }

    private void CargarClientes()
    {
        _cargando = true;
        try
        {
            List<Cliente> clientes = _clienteDao.Listar();
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = clientes;
            if (dgvClientes.Columns["Telefono"] is DataGridViewColumn telefono)
            {
                telefono.HeaderText = "Teléfono";
            }

            dgvClientes.ClearSelection();
            CargarComboClientes(clientes);
        }
        finally
        {
            _cargando = false;
        }
    }

    private void CargarComboClientes(List<Cliente>? clientes = null)
    {
        clientes ??= _clienteDao.Listar();
        int seleccionado = (cmbCliente.SelectedItem as Cliente)?.Id ?? 0;

        cmbCliente.DataSource = null;
        cmbCliente.DataSource = clientes.ToList();

        if (seleccionado == 0 || cmbCliente.Items.Count == 0)
        {
            return;
        }

        foreach (Cliente cliente in cmbCliente.Items)
        {
            if (cliente.Id == seleccionado)
            {
                cmbCliente.SelectedItem = cliente;
                return;
            }
        }
    }

    private void CargarOrdenes()
    {
        _cargando = true;
        try
        {
            int seleccion = (dgvOrdenes.CurrentRow?.DataBoundItem as Orden)?.Id ?? 0;

            dgvOrdenes.DataSource = null;
            dgvOrdenes.DataSource = _ordenDao.Listar();
            ConfigurarGrillaOrdenes();

            if (seleccion > 0)
            {
                foreach (DataGridViewRow fila in dgvOrdenes.Rows)
                {
                    if (fila.DataBoundItem is Orden orden && orden.Id == seleccion)
                    {
                        fila.Selected = true;
                        dgvOrdenes.CurrentCell = fila.Cells[0];
                        break;
                    }
                }
            }
            else
            {
                dgvOrdenes.ClearSelection();
            }
        }
        finally
        {
            _cargando = false;
        }

        CargarItemsConsulta();
    }

    private void CargarItemsConsulta()
    {
        if (dgvOrdenes.CurrentRow?.DataBoundItem is not Orden orden)
        {
            dgvItemsConsulta.DataSource = null;
            return;
        }

        dgvItemsConsulta.DataSource = null;
        dgvItemsConsulta.DataSource = _ordenDao.ListarItems(orden.Id);
        ConfigurarGrillaItems(dgvItemsConsulta);
    }

    private void ActualizarGrillaItemsNuevos()
    {
        dgvItemsNuevos.DataSource = null;
        dgvItemsNuevos.DataSource = _itemsNuevos.ToList();
        ConfigurarGrillaItems(dgvItemsNuevos);
        decimal total = _itemsNuevos.Sum(item => item.Subtotal);
        lblTotalOrden.Text = $"Total: {total:N2}";
    }

    private void LimpiarCamposCliente()
    {
        txtNombreCliente.Clear();
        txtTelefono.Clear();
        txtNombreCliente.Focus();
    }

    private void ConfigurarGrillaOrdenes()
    {
        if (dgvOrdenes.Columns["Fecha"] is DataGridViewColumn fecha)
        {
            fecha.DefaultCellStyle.Format = "d";
        }

        if (dgvOrdenes.Columns["Total"] is DataGridViewColumn total)
        {
            total.DefaultCellStyle.Format = "N2";
            total.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }

    private static void ConfigurarGrillaItems(DataGridView grilla)
    {
        OcultarColumna(grilla, "Id");
        OcultarColumna(grilla, "OrdenId");

        if (grilla.Columns["Descripcion"] is DataGridViewColumn descripcion)
        {
            descripcion.HeaderText = "Descripción";
        }

        if (grilla.Columns["PrecioUnitario"] is DataGridViewColumn precio)
        {
            precio.HeaderText = "Precio unitario";
            precio.DefaultCellStyle.Format = "N2";
            precio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        if (grilla.Columns["Subtotal"] is DataGridViewColumn subtotal)
        {
            subtotal.DefaultCellStyle.Format = "N2";
            subtotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }

    private static void OcultarColumna(DataGridView grilla, string nombre)
    {
        if (grilla.Columns[nombre] is DataGridViewColumn columna)
        {
            columna.Visible = false;
        }
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
