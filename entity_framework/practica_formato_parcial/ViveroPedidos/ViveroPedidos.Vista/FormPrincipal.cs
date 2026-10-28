namespace ViveroPedidos.Vista;

public partial class FormPrincipal : Form
{
    public FormPrincipal()
    {
        InitializeComponent();
    }

    private void btnPlantas_Click(object? sender, EventArgs e)
    {
        using var frm = new FormPlantas();
        frm.ShowDialog(this);
    }

    private void btnRegistrar_Click(object? sender, EventArgs e)
    {
        using var frm = new FormRegistrarPedido();
        frm.ShowDialog(this);
    }

    private void btnConsultar_Click(object? sender, EventArgs e)
    {
        using var frm = new FormConsultarPedidos();
        frm.ShowDialog(this);
    }
}
