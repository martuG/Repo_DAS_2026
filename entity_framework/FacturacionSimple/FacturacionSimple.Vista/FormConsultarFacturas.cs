using FacturacionSimple.Controladora;

namespace FacturacionSimple.Vista;

public partial class FormConsultarFacturas : Form
{
    private readonly FacturaControladora _controladora = new();

    public FormConsultarFacturas()
    {
        InitializeComponent();
        dtpDesde.Value = DateTime.Today.AddMonths(-1);
        dtpHasta.Value = DateTime.Today;
        Buscar();
    }

    private void Buscar()
    {
        var lista = _controladora.Listar(dtpDesde.Value, dtpHasta.Value, txtCliente.Text);
        dgvFacturas.DataSource = null;
        dgvFacturas.DataSource = lista.Select(f => new
        {
            f.Id,
            f.Numero,
            f.Fecha,
            f.ClienteNombre,
            f.ClienteDocumento,
            f.Total
        }).ToList();

        dgvDetalle.DataSource = null;
        lblCabecera.Text = "Seleccione una factura";
    }

    private void btnBuscar_Click(object? sender, EventArgs e) => Buscar();

    private void dgvFacturas_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvFacturas.CurrentRow is null)
            return;

        var idObj = dgvFacturas.CurrentRow.Cells["Id"].Value;
        if (idObj is null || !int.TryParse(idObj.ToString(), out var id))
            return;

        var factura = _controladora.ObtenerConDetalle(id);
        if (factura is null)
            return;

        lblCabecera.Text =
            $"Factura N° {factura.Numero} — {factura.Fecha:d} — {factura.ClienteNombre} ({factura.ClienteDocumento}) — Total {factura.Total:C2}";

        dgvDetalle.DataSource = factura.Detalles.Select(d => new
        {
            Producto = d.Producto?.Nombre ?? d.ProductoId.ToString(),
            d.Cantidad,
            d.PrecioUnitario,
            d.Subtotal
        }).ToList();
    }
}
