namespace FacturacionSimple.Vista;

partial class FormConsultarFacturas
{
    private System.ComponentModel.IContainer components = null!;
    private DateTimePicker dtpDesde = null!;
    private DateTimePicker dtpHasta = null!;
    private TextBox txtCliente = null!;
    private Button btnBuscar = null!;
    private DataGridView dgvFacturas = null!;
    private DataGridView dgvDetalle = null!;
    private Label lblCabecera = null!;
    private Label lblDesde = null!;
    private Label lblHasta = null!;
    private Label lblCliente = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        dtpDesde = new DateTimePicker();
        dtpHasta = new DateTimePicker();
        txtCliente = new TextBox();
        btnBuscar = new Button();
        dgvFacturas = new DataGridView();
        dgvDetalle = new DataGridView();
        lblCabecera = new Label();
        lblDesde = new Label();
        lblHasta = new Label();
        lblCliente = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvFacturas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
        SuspendLayout();

        lblDesde.Location = new Point(20, 20);
        lblDesde.Text = "Desde";
        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(70, 16);
        dtpDesde.Size = new Size(110, 23);

        lblHasta.Location = new Point(200, 20);
        lblHasta.Text = "Hasta";
        dtpHasta.Format = DateTimePickerFormat.Short;
        dtpHasta.Location = new Point(250, 16);
        dtpHasta.Size = new Size(110, 23);

        lblCliente.Location = new Point(380, 20);
        lblCliente.Text = "Cliente";
        txtCliente.Location = new Point(440, 16);
        txtCliente.Size = new Size(200, 23);

        btnBuscar.Location = new Point(660, 14);
        btnBuscar.Size = new Size(100, 27);
        btnBuscar.Text = "Buscar";
        btnBuscar.Click += btnBuscar_Click;

        dgvFacturas.AllowUserToAddRows = false;
        dgvFacturas.AllowUserToDeleteRows = false;
        dgvFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvFacturas.Location = new Point(20, 55);
        dgvFacturas.MultiSelect = false;
        dgvFacturas.ReadOnly = true;
        dgvFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFacturas.Size = new Size(740, 180);
        dgvFacturas.SelectionChanged += dgvFacturas_SelectionChanged;

        lblCabecera.Location = new Point(20, 250);
        lblCabecera.Size = new Size(740, 25);
        lblCabecera.Text = "Seleccione una factura";

        dgvDetalle.AllowUserToAddRows = false;
        dgvDetalle.AllowUserToDeleteRows = false;
        dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDetalle.Location = new Point(20, 285);
        dgvDetalle.MultiSelect = false;
        dgvDetalle.ReadOnly = true;
        dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDetalle.Size = new Size(740, 160);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(790, 470);
        Controls.AddRange(new Control[]
        {
            lblDesde, dtpDesde, lblHasta, dtpHasta, lblCliente, txtCliente, btnBuscar,
            dgvFacturas, lblCabecera, dgvDetalle
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Consultar facturas";
        ((System.ComponentModel.ISupportInitialize)dgvFacturas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
