using ArmadoComputadora.Patrones;

namespace ArmadoComputadora;

public partial class Form1 : Form
{
    private readonly IComputadoraBuilder _builder = new ComputadoraBuilder();
    private readonly DirectorComputadora _director = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cmbCpu.Items.AddRange(new object[]
        {
            "Intel Core i5-12400",
            "Intel Core i7-13700K",
            "AMD Ryzen 5 5600",
            "AMD Ryzen 7 5800X"
        });
        cmbRam.Items.AddRange(new object[] { "8 GB DDR4", "16 GB DDR4", "32 GB DDR4", "32 GB DDR5", "64 GB DDR5" });
        cmbGpu.Items.AddRange(new object[] { "Integrada", "NVIDIA RTX 3060", "NVIDIA RTX 4070", "NVIDIA RTX 4080" });
        cmbDisco.Items.AddRange(new object[] { "512 GB SSD", "1 TB SSD", "2 TB SSD" });
        cmbFuente.Items.AddRange(new object[] { "500 W", "650 W", "750 W", "850 W" });
        cmbGabinete.Items.AddRange(new object[] { "Mini Tower", "Mid Tower", "Full Tower" });

        cmbCpu.SelectedIndex = 0;
        cmbRam.SelectedIndex = 1;
        cmbGpu.SelectedIndex = 0;
        cmbDisco.SelectedIndex = 0;
        cmbFuente.SelectedIndex = 0;
        cmbGabinete.SelectedIndex = 1;
    }

    private void btnOficina_Click(object sender, EventArgs e)
    {
        Mostrar(_director.ArmarOficina(_builder), "Director.ArmarOficina(builder)");
    }

    private void btnDesarrollo_Click(object sender, EventArgs e)
    {
        Mostrar(_director.ArmarDesarrollo(_builder), "Director.ArmarDesarrollo(builder)");
    }

    private void btnGamer_Click(object sender, EventArgs e)
    {
        Mostrar(_director.ArmarGamer(_builder), "Director.ArmarGamer(builder)");
    }

    private void btnPersonalizado_Click(object sender, EventArgs e)
    {
        Computadora pc = _builder
            .ConPerfil("Personalizado")
            .ConCpu(cmbCpu.Text)
            .ConRam(cmbRam.Text)
            .ConGpu(cmbGpu.Text)
            .ConDisco(cmbDisco.Text)
            .ConFuente(cmbFuente.Text)
            .ConGabinete(cmbGabinete.Text)
            .Construir();

        Mostrar(pc, "Cliente usa el Builder directo (sin Director)");
    }

    private void Mostrar(Computadora pc, string comoSeConstruyo)
    {
        txtResultado.Text = pc.ToString();
        lblComo.Text = comoSeConstruyo;
    }
}
