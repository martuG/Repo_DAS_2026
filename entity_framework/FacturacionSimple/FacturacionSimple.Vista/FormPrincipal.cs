namespace FacturacionSimple.Vista;

public partial class FormPrincipal : Form
{
    public FormPrincipal()
    {
        InitializeComponent();
    }

    private void btnProductos_Click(object? sender, EventArgs e)
    {
        using var frm = new FormProductos();
        frm.ShowDialog(this);
    }

    private void btnEmitir_Click(object? sender, EventArgs e)
    {
        using var frm = new FormEmitirFactura();
        frm.ShowDialog(this);
    }

    private void btnConsultar_Click(object? sender, EventArgs e)
    {
        using var frm = new FormConsultarFacturas();
        frm.ShowDialog(this);
    }
}
