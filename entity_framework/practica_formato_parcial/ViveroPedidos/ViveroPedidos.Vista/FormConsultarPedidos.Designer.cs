namespace ViveroPedidos.Vista;

partial class FormConsultarPedidos
{
    private System.ComponentModel.IContainer components = null!;
    private DateTimePicker dtpDesde = null!;
    private DateTimePicker dtpHasta = null!;
    private TextBox txtSolicitante = null!;
    private Button btnBuscar = null!;
    private DataGridView dgvPedidos = null!;
    private DataGridView dgvDetalle = null!;
    private Label lblCabecera = null!;
    private Label lblDesde = null!;
    private Label lblHasta = null!;
    private Label lblSolicitante = null!;

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
        txtSolicitante = new TextBox();
        btnBuscar = new Button();
        dgvPedidos = new DataGridView();
        dgvDetalle = new DataGridView();
        lblCabecera = new Label();
        lblDesde = new Label();
        lblHasta = new Label();
        lblSolicitante = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
        SuspendLayout();

        lblDesde.AutoSize = true;
        lblDesde.Location = new Point(20, 20);
        lblDesde.Text = "Desde";
        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(70, 16);
        dtpDesde.Size = new Size(110, 23);

        lblHasta.AutoSize = true;
        lblHasta.Location = new Point(200, 20);
        lblHasta.Text = "Hasta";
        dtpHasta.Format = DateTimePickerFormat.Short;
        dtpHasta.Location = new Point(250, 16);
        dtpHasta.Size = new Size(110, 23);

        lblSolicitante.AutoSize = true;
        lblSolicitante.Location = new Point(380, 20);
        lblSolicitante.Text = "Solicitante";
        txtSolicitante.Location = new Point(455, 16);
        txtSolicitante.Size = new Size(200, 23);

        btnBuscar.Location = new Point(670, 14);
        btnBuscar.Size = new Size(100, 27);
        btnBuscar.Text = "Buscar";
        btnBuscar.Click += btnBuscar_Click;

        dgvPedidos.AllowUserToAddRows = false;
        dgvPedidos.AllowUserToDeleteRows = false;
        dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPedidos.Location = new Point(20, 55);
        dgvPedidos.MultiSelect = false;
        dgvPedidos.ReadOnly = true;
        dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPedidos.Size = new Size(800, 180);
        dgvPedidos.SelectionChanged += dgvPedidos_SelectionChanged;

        lblCabecera.Location = new Point(20, 250);
        lblCabecera.Size = new Size(800, 40);
        lblCabecera.Text = "Seleccione un pedido";

        dgvDetalle.AllowUserToAddRows = false;
        dgvDetalle.AllowUserToDeleteRows = false;
        dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDetalle.Location = new Point(20, 295);
        dgvDetalle.MultiSelect = false;
        dgvDetalle.ReadOnly = true;
        dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDetalle.Size = new Size(800, 160);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(840, 480);
        Controls.AddRange(new Control[]
        {
            lblDesde, dtpDesde, lblHasta, dtpHasta, lblSolicitante, txtSolicitante, btnBuscar,
            dgvPedidos, lblCabecera, dgvDetalle
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Consultar pedidos";
        ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
