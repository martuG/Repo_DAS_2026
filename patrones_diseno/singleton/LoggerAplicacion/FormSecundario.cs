using LoggerAplicacion.Patrones;

namespace LoggerAplicacion;

public partial class FormSecundario : Form
{
    public FormSecundario()
    {
        InitializeComponent();
    }

    private void FormSecundario_Load(object sender, EventArgs e)
    {
        Logger.Instancia.Cambio += ActualizarRegistros;
        MostrarIdentidad();
        ActualizarRegistros();
        Logger.Instancia.Registrar("Secundaria", "Segunda ventana abierta. Compará el Guid con la ventana principal.");
    }

    private void FormSecundario_FormClosed(object sender, FormClosedEventArgs e)
    {
        Logger.Instancia.Cambio -= ActualizarRegistros;
        Logger.Instancia.Registrar("Secundaria", "Segunda ventana cerrada.");
    }

    private void btnRegistrar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMensaje.Text))
        {
            MessageBox.Show("Escribí un mensaje para registrar.", "Logger", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Logger.Instancia.Registrar("Secundaria", txtMensaje.Text);
        txtMensaje.Clear();
        txtMensaje.Focus();
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
