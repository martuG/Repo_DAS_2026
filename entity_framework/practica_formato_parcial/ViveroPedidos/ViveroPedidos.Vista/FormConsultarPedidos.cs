using ViveroPedidos.Controladora;

namespace ViveroPedidos.Vista;

public partial class FormConsultarPedidos : Form
{
    private readonly PedidoControladora _controladora = new();

    public FormConsultarPedidos()
    {
        InitializeComponent();
        dtpDesde.Value = DateTime.Today.AddMonths(-1);
        dtpHasta.Value = DateTime.Today;
        Buscar();
    }

    private void Buscar()
    {
        dgvDetalle.DataSource = null;
        lblCabecera.Text = "Seleccione un pedido";

        try
        {
            var lista = _controladora.Listar(dtpDesde.Value.Date, dtpHasta.Value.Date, txtSolicitante.Text) ?? [];
            dgvPedidos.DataSource = null;
            dgvPedidos.DataSource = lista.Select(p => new
            {
                p.Id,
                p.Numero,
                p.Fecha,
                p.SolicitanteNombre,
                p.SolicitanteDocumento,
                p.FechaRetiro,
                p.CantidadItems
            }).ToList();
        }
        catch (Exception ex)
        {
            dgvPedidos.DataSource = null;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnBuscar_Click(object? sender, EventArgs e) => Buscar();

    private void dgvPedidos_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvPedidos.CurrentRow is null)
            return;

        var idObj = dgvPedidos.CurrentRow.Cells["Id"].Value;
        if (idObj is null || !int.TryParse(idObj.ToString(), out var id))
            return;

        try
        {
            var pedido = _controladora.ObtenerConDetalle(id);
            if (pedido is null)
                return;

            lblCabecera.Text =
                $"Pedido N° {pedido.Numero} — {pedido.Fecha:d} — {pedido.SolicitanteNombre} ({pedido.SolicitanteDocumento}) — Retiro {pedido.FechaRetiro:d} — Ítems {pedido.CantidadItems}";

            dgvDetalle.DataSource = (pedido.Detalles ?? []).Select(d => new
            {
                d.PlantaId,
                d.NombreHistorico,
                d.Cantidad
            }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
