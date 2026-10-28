using ViveroPedidos.Controladora;
using ViveroPedidos.Modelo.Entidades;

namespace ViveroPedidos.Vista;

public partial class FormRegistrarPedido : Form
{
    private readonly PedidoControladora _pedidos = new();
    private readonly PlantaControladora _plantas = new();
    private readonly List<LineaUi> _lineas = new();

    private sealed class LineaUi
    {
        public int PlantaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string NombreComun { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public FormRegistrarPedido()
    {
        InitializeComponent();
        dtpFecha.Value = DateTime.Today;
        dtpRetiro.Value = DateTime.Today;
        CargarPlantas();
        RefrescarDetalle();
    }

    private void CargarPlantas()
    {
        try
        {
            var lista = _plantas.ListarDisponibles();
            cboPlanta.DisplayMember = nameof(Planta.NombreComun);
            cboPlanta.ValueMember = nameof(Planta.Id);
            cboPlanta.DataSource = lista;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefrescarDetalle()
    {
        dgvDetalle.DataSource = null;
        dgvDetalle.DataSource = _lineas.Select(l => new
        {
            l.Codigo,
            l.NombreComun,
            l.Especie,
            l.Cantidad
        }).ToList();

        lblItems.Text = $"Ítems: {_lineas.Sum(l => l.Cantidad)}";
    }

    private void btnAgregarLinea_Click(object? sender, EventArgs e)
    {
        if (cboPlanta.SelectedItem is not Planta planta)
        {
            MessageBox.Show("Seleccione una planta.", "Aviso");
            return;
        }

        if (!planta.Disponible)
        {
            MessageBox.Show("Solo se pueden pedir plantas disponibles.", "Aviso");
            return;
        }

        var cantidad = (int)numCantidad.Value;
        if (cantidad <= 0)
        {
            MessageBox.Show("La cantidad debe ser mayor que cero.", "Aviso");
            return;
        }

        if (_lineas.Any(l => l.PlantaId == planta.Id))
        {
            MessageBox.Show(
                "Esa planta ya está en el pedido. Quite la línea si quiere cambiar la cantidad.",
                "Aviso");
            return;
        }

        _lineas.Add(new LineaUi
        {
            PlantaId = planta.Id,
            Codigo = planta.Codigo,
            NombreComun = planta.NombreComun,
            Especie = planta.Especie,
            Cantidad = cantidad
        });

        RefrescarDetalle();
    }

    private void btnQuitarLinea_Click(object? sender, EventArgs e)
    {
        if (dgvDetalle.CurrentRow is null || dgvDetalle.CurrentRow.Index < 0
            || dgvDetalle.CurrentRow.Index >= _lineas.Count)
        {
            MessageBox.Show("Seleccione una línea.", "Aviso");
            return;
        }

        _lineas.RemoveAt(dgvDetalle.CurrentRow.Index);
        RefrescarDetalle();
    }

    private void btnGrabar_Click(object? sender, EventArgs e)
    {
        try
        {
            var pedido = _pedidos.Registrar(
                dtpFecha.Value.Date,
                txtSolicitante.Text,
                txtDocumento.Text,
                dtpRetiro.Value.Date,
                _lineas.Select(l => (l.PlantaId, l.Cantidad)).ToList());

            MessageBox.Show(
                $"Pedido registrado. Número: {pedido.Numero}\nÍtems: {pedido.CantidadItems}",
                "OK",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            _lineas.Clear();
            txtSolicitante.Clear();
            txtDocumento.Clear();
            dtpFecha.Value = DateTime.Today;
            dtpRetiro.Value = DateTime.Today;
            RefrescarDetalle();
            CargarPlantas();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
