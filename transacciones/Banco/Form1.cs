using Banco.Datos;
using Banco.Entidades;

namespace Banco;

public partial class Form1 : Form
{
    private readonly BancoDao _dao = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            Conexion.AsegurarBaseDeDatos();
            CargarCuentas();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo conectar a SQL Server LocalDB.\n" +
                "Verificá que esté instalado o ajustá la cadena en Datos/Conexion.cs.\n\n" +
                ex.Message,
                "Error de conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnActualizar_Click(object sender, EventArgs e)
    {
        CargarCuentas();
    }

    private void btnTransferir_Click(object sender, EventArgs e)
    {
        if (cmbOrigen.SelectedItem is not Cuenta origen || cmbDestino.SelectedItem is not Cuenta destino)
        {
            MessageBox.Show("Elegí cuenta origen y destino.", "Transferencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (origen.Id == destino.Id)
        {
            MessageBox.Show("La cuenta origen y destino deben ser distintas.", "Transferencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
        {
            MessageBox.Show("Ingresá un monto mayor a 0.", "Transferencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _dao.Transferir(origen.Id, destino.Id, monto);
            CargarCuentas();
            txtMonto.Clear();
            MessageBox.Show("Transferencia realizada. Las dos actualizaciones se confirmaron juntas (Commit).", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "La transacción se revirtió (Rollback). No cambió ningún saldo.\n\n" + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CargarCuentas()
    {
        List<Cuenta> cuentas = _dao.Listar();

        dgvCuentas.DataSource = null;
        dgvCuentas.DataSource = cuentas;
        dgvCuentas.ClearSelection();

        int origenId = (cmbOrigen.SelectedItem as Cuenta)?.Id ?? 0;
        int destinoId = (cmbDestino.SelectedItem as Cuenta)?.Id ?? 0;

        cmbOrigen.DataSource = cuentas.ToList();
        cmbDestino.DataSource = cuentas.ToList();

        SeleccionarPorId(cmbOrigen, origenId);
        SeleccionarPorId(cmbDestino, destinoId);
    }

    private static void SeleccionarPorId(ComboBox combo, int id)
    {
        if (id == 0 || combo.Items.Count == 0)
        {
            return;
        }

        foreach (Cuenta cuenta in combo.Items)
        {
            if (cuenta.Id == id)
            {
                combo.SelectedItem = cuenta;
                return;
            }
        }
    }
}
