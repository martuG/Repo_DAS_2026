using LogisticaEnvios.Patrones;

namespace LogisticaEnvios;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cmbCanal.Items.AddRange(new object[] { "Terrestre", "Marítimo", "Aéreo" });
        cmbCanal.SelectedIndex = 0;
        txtDestino.Text = "Rosario";
    }

    private void btnDespachar_Click(object sender, EventArgs e)
    {
        string destino = txtDestino.Text.Trim();
        if (string.IsNullOrWhiteSpace(destino))
        {
            MessageBox.Show("Ingresá un destino.", "Envío", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // El formulario elige el CREADOR. El producto (Camion/Barco/Avion)
        // lo instancia el Factory Method CrearTransporte(), no este código.
        CreadorEnvio creador = cmbCanal.SelectedItem?.ToString() switch
        {
            "Marítimo" => new CreadorMaritimo(),
            "Aéreo" => new CreadorAereo(),
            _ => new CreadorTerrestre()
        };

        ITransporte transporte = creador.CrearTransporte();
        lblProducto.Text =
            $"Factory Method → {transporte.GetType().Name}  |  costo ${transporte.CostoBase:N0}  |  {transporte.TiempoEstimado}";

        string detalle = creador.Despachar(destino);
        txtResultado.Text = detalle;

        lstHistorial.Items.Add($"[{DateTime.Now:HH:mm:ss}] {creador.Canal} → {destino} ({transporte.Nombre})");
        lstHistorial.SelectedIndex = lstHistorial.Items.Count - 1;
    }
}
