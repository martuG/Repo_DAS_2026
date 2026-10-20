using MenuRestaurante.Patrones;

namespace MenuRestaurante;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cmbCocina.Items.AddRange(new object[] { "Italiana", "Japonesa", "Mexicana" });
        cmbCocina.SelectedIndex = 0;
        GenerarMenu();
    }

    private void btnGenerar_Click(object sender, EventArgs e)
    {
        GenerarMenu();
    }

    private void cmbCocina_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerarMenu();
    }

    private void GenerarMenu()
    {
        // El formulario solo conoce IFabricaMenu. No instancia Bruschetta, Ramen, etc.
        IFabricaMenu fabrica = cmbCocina.SelectedItem?.ToString() switch
        {
            "Japonesa" => new FabricaMenuJapones(),
            "Mexicana" => new FabricaMenuMexicano(),
            _ => new FabricaMenuItaliano()
        };

        MostrarFamilia(fabrica);
    }

    private void MostrarFamilia(IFabricaMenu fabrica)
    {
        IEntrada entrada = fabrica.CrearEntrada();
        IPrincipal principal = fabrica.CrearPrincipal();
        IPostre postre = fabrica.CrearPostre();

        lblFabrica.Text = $"Fábrica concreta: {fabrica.GetType().Name}  —  cocina {fabrica.Cocina}";

        grpEntrada.Text = $"Entrada  ({entrada.GetType().Name})";
        lblEntradaNombre.Text = entrada.Nombre;
        lblEntradaDesc.Text = entrada.Descripcion;

        grpPrincipal.Text = $"Principal  ({principal.GetType().Name})";
        lblPrincipalNombre.Text = principal.Nombre;
        lblPrincipalDesc.Text = principal.Descripcion;

        grpPostre.Text = $"Postre  ({postre.GetType().Name})";
        lblPostreNombre.Text = postre.Nombre;
        lblPostreDesc.Text = postre.Descripcion;
    }
}
