using LoggerAplicacion.Patrones;

namespace LoggerAplicacion;

public partial class Form1 : Form
{
    private FormSecundario? _segundaVentana;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Logger.Instancia.Cambio += ActualizarRegistros;
        MostrarIdentidad();
        ActualizarRegistros();
        Logger.Instancia.Registrar("Principal", "Ventana principal abierta. Se obtuvo Logger.Instancia.");
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        Logger.Instancia.Cambio -= ActualizarRegistros;
    }

    private void btnRegistrar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMensaje.Text))
        {
            MessageBox.Show("Escribí un mensaje para registrar.", "Logger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Logger.Instancia.Registrar("Principal", txtMensaje.Text);
        txtMensaje.Clear();
        txtMensaje.Focus();
    }

    private void btnPedirInstancia_Click(object sender, EventArgs e)
    {
        Logger otraReferencia = Logger.Instancia;
        MostrarIdentidad();
        Logger.Instancia.Registrar(
            "Principal",
            $"Se pidió Instancia otra vez. Guid={otraReferencia.IdInstancia:N} Hash={otraReferencia.GetHashCode()} (es la misma).");
    }

    private void btnAbrirSegunda_Click(object sender, EventArgs e)
    {
        if (_segundaVentana is null || _segundaVentana.IsDisposed)
        {
            _segundaVentana = new FormSecundario();
            _segundaVentana.Show(this);
        }
        else
        {
            _segundaVentana.Focus();
        }
    }

    private void MostrarIdentidad()
    {
        Logger logger = Logger.Instancia;
        lblGuid.Text = logger.IdInstancia.ToString();
        lblHash.Text = logger.GetHashCode().ToString();
    }

    private void ActualizarRegistros()
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => ActualizarRegistros());
            return;
        }

        lstRegistros.Items.Clear();
        foreach (string linea in Logger.Instancia.Registros)
        {
            lstRegistros.Items.Add(linea);
        }

        if (lstRegistros.Items.Count > 0)
        {
            lstRegistros.SelectedIndex = lstRegistros.Items.Count - 1;
        }
    }
}
