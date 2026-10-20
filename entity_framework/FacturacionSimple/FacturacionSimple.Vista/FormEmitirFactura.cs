using FacturacionSimple.Controladora;
using FacturacionSimple.Modelo.Entidades;

namespace FacturacionSimple.Vista;

public partial class FormEmitirFactura : Form
{
    private readonly FacturaControladora _facturas = new();
    private readonly ProductoControladora _productos = new();
    private readonly List<LineaUi> _lineas = new();

    private sealed class LineaUi
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Math.Round(Cantidad * PrecioUnitario, 2);
    }

    public FormEmitirFactura()
    {
        InitializeComponent();
        dtpFecha.Value = DateTime.Today;
        CargarProductos();
        RefrescarDetalle();
    }

    private void CargarProductos()
    {
        var lista = _productos.ListarActivos();
        cboProducto.DisplayMember = nameof(Producto.Nombre);
        cboProducto.ValueMember = nameof(Producto.Id);
        cboProducto.DataSource = lista;
    }

    private void RefrescarDetalle()
    {
        dgvDetalle.DataSource = null;
        dgvDetalle.DataSource = _lineas.Select(l => new
        {
            l.Codigo,
            l.Nombre,
            l.Cantidad,
            l.PrecioUnitario,
            l.Subtotal
        }).ToList();

        lblTotal.Text = $"Total: {_lineas.Sum(l => l.Subtotal):C2}";
    }

    private void btnAgregarLinea_Click(object? sender, EventArgs e)
    {
        if (cboProducto.SelectedItem is not Producto producto)
        {
            MessageBox.Show("Seleccione un producto.", "Aviso");
            return;
        }

        var cantidad = (int)numCantidad.Value;
        if (cantidad <= 0)
        {
            MessageBox.Show("La cantidad debe ser mayor que cero.", "Aviso");
            return;
        }

        var existente = _lineas.FirstOrDefault(l => l.ProductoId == producto.Id);
        if (existente is not null)
        {
            existente.Cantidad += cantidad;
        }
        else
        {
            _lineas.Add(new LineaUi
            {
                ProductoId = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio
            });
        }

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
            var emitida = _facturas.Emitir(
                dtpFecha.Value.Date,
                txtCliente.Text,
                txtDocumento.Text,
                _lineas.Select(l => (l.ProductoId, l.Cantidad)).ToList());

            MessageBox.Show(
                $"Factura emitida. Número: {emitida.Numero}\nTotal: {emitida.Total:C2}",
                "OK",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            _lineas.Clear();
            txtCliente.Clear();
            txtDocumento.Clear();
            RefrescarDetalle();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
