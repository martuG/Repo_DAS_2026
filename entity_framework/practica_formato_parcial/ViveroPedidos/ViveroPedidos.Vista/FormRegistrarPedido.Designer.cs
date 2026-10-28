namespace ViveroPedidos.Vista;

partial class FormRegistrarPedido
{
    private System.ComponentModel.IContainer components = null!;
    private DateTimePicker dtpFecha = null!;
    private DateTimePicker dtpRetiro = null!;
    private TextBox txtSolicitante = null!;
    private TextBox txtDocumento = null!;
    private ComboBox cboPlanta = null!;
    private NumericUpDown numCantidad = null!;
    private Button btnAgregarLinea = null!;
    private Button btnQuitarLinea = null!;
    private DataGridView dgvDetalle = null!;
    private Label lblItems = null!;
    private Button btnGrabar = null!;
    private Label lblFecha = null!;
    private Label lblRetiro = null!;
    private Label lblSolicitante = null!;
    private Label lblDocumento = null!;
    private Label lblPlanta = null!;
    private Label lblCantidad = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        dtpFecha = new DateTimePicker();
        dtpRetiro = new DateTimePicker();
        txtSolicitante = new TextBox();
        txtDocumento = new TextBox();
        cboPlanta = new ComboBox();
        numCantidad = new NumericUpDown();
        btnAgregarLinea = new Button();
        btnQuitarLinea = new Button();
        dgvDetalle = new DataGridView();
        lblItems = new Label();
        btnGrabar = new Button();
        lblFecha = new Label();
        lblRetiro = new Label();
        lblSolicitante = new Label();
        lblDocumento = new Label();
        lblPlanta = new Label();
        lblCantidad = new Label();
        ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
        SuspendLayout();

        lblFecha.AutoSize = true;
        lblFecha.Location = new Point(20, 20);
        lblFecha.Text = "Fecha";
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(70, 16);
        dtpFecha.Size = new Size(110, 23);

        lblSolicitante.AutoSize = true;
        lblSolicitante.Location = new Point(200, 20);
        lblSolicitante.Text = "Solicitante";
        txtSolicitante.Location = new Point(275, 16);
        txtSolicitante.MaxLength = 120;
        txtSolicitante.Size = new Size(220, 23);

        lblDocumento.AutoSize = true;
        lblDocumento.Location = new Point(510, 20);
        lblDocumento.Text = "Documento";
        txtDocumento.Location = new Point(590, 16);
        txtDocumento.MaxLength = 30;
        txtDocumento.Size = new Size(140, 23);

        lblRetiro.AutoSize = true;
        lblRetiro.Location = new Point(20, 58);
        lblRetiro.Text = "Fecha de retiro";
        dtpRetiro.Format = DateTimePickerFormat.Short;
        dtpRetiro.Location = new Point(120, 54);
        dtpRetiro.Size = new Size(110, 23);

        lblPlanta.AutoSize = true;
        lblPlanta.Location = new Point(20, 100);
        lblPlanta.Text = "Planta";
        cboPlanta.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPlanta.Location = new Point(70, 96);
        cboPlanta.Size = new Size(360, 23);

        lblCantidad.AutoSize = true;
        lblCantidad.Location = new Point(450, 100);
        lblCantidad.Text = "Cant.";
        numCantidad.Location = new Point(495, 96);
        numCantidad.Minimum = 1;
        numCantidad.Maximum = 10000;
        numCantidad.Value = 1;
        numCantidad.Size = new Size(70, 23);

        btnAgregarLinea.Location = new Point(580, 94);
        btnAgregarLinea.Size = new Size(90, 27);
        btnAgregarLinea.Text = "Agregar";
        btnAgregarLinea.Click += btnAgregarLinea_Click;

        btnQuitarLinea.Location = new Point(680, 94);
        btnQuitarLinea.Size = new Size(90, 27);
        btnQuitarLinea.Text = "Quitar";
        btnQuitarLinea.Click += btnQuitarLinea_Click;

        dgvDetalle.AllowUserToAddRows = false;
        dgvDetalle.AllowUserToDeleteRows = false;
        dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDetalle.Location = new Point(20, 140);
        dgvDetalle.MultiSelect = false;
        dgvDetalle.ReadOnly = true;
        dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDetalle.Size = new Size(800, 250);

        lblItems.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblItems.Location = new Point(20, 410);
        lblItems.Size = new Size(300, 25);
        lblItems.Text = "Ítems: 0";

        btnGrabar.Location = new Point(660, 404);
        btnGrabar.Size = new Size(160, 35);
        btnGrabar.Text = "Grabar pedido";
        btnGrabar.Click += btnGrabar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(840, 460);
        Controls.AddRange(new Control[]
        {
            lblFecha, dtpFecha, lblSolicitante, txtSolicitante, lblDocumento, txtDocumento,
            lblRetiro, dtpRetiro,
            lblPlanta, cboPlanta, lblCantidad, numCantidad, btnAgregarLinea, btnQuitarLinea,
            dgvDetalle, lblItems, btnGrabar
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Registrar pedido";
        ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
