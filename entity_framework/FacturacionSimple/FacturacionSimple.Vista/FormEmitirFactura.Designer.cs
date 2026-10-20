namespace FacturacionSimple.Vista;

partial class FormEmitirFactura
{
    private System.ComponentModel.IContainer components = null!;
    private DateTimePicker dtpFecha = null!;
    private TextBox txtCliente = null!;
    private TextBox txtDocumento = null!;
    private ComboBox cboProducto = null!;
    private NumericUpDown numCantidad = null!;
    private Button btnAgregarLinea = null!;
    private Button btnQuitarLinea = null!;
    private DataGridView dgvDetalle = null!;
    private Label lblTotal = null!;
    private Button btnGrabar = null!;
    private Label lblFecha = null!;
    private Label lblCliente = null!;
    private Label lblDocumento = null!;
    private Label lblProducto = null!;
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
        txtCliente = new TextBox();
        txtDocumento = new TextBox();
        cboProducto = new ComboBox();
        numCantidad = new NumericUpDown();
        btnAgregarLinea = new Button();
        btnQuitarLinea = new Button();
        dgvDetalle = new DataGridView();
        lblTotal = new Label();
        btnGrabar = new Button();
        lblFecha = new Label();
        lblCliente = new Label();
        lblDocumento = new Label();
        lblProducto = new Label();
        lblCantidad = new Label();
        ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).BeginInit();
        SuspendLayout();

        lblFecha.Location = new Point(20, 20);
        lblFecha.Text = "Fecha";
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(90, 16);
        dtpFecha.Size = new Size(120, 23);

        lblCliente.Location = new Point(230, 20);
        lblCliente.Text = "Cliente";
        txtCliente.Location = new Point(290, 16);
        txtCliente.Size = new Size(220, 23);

        lblDocumento.Location = new Point(530, 20);
        lblDocumento.Text = "Documento";
        txtDocumento.Location = new Point(620, 16);
        txtDocumento.Size = new Size(140, 23);

        lblProducto.Location = new Point(20, 60);
        lblProducto.Text = "Producto";
        cboProducto.DropDownStyle = ComboBoxStyle.DropDownList;
        cboProducto.Location = new Point(90, 56);
        cboProducto.Size = new Size(320, 23);

        lblCantidad.Location = new Point(430, 60);
        lblCantidad.Text = "Cant.";
        numCantidad.Location = new Point(480, 56);
        numCantidad.Minimum = 1;
        numCantidad.Maximum = 10000;
        numCantidad.Value = 1;
        numCantidad.Size = new Size(70, 23);

        btnAgregarLinea.Location = new Point(570, 54);
        btnAgregarLinea.Size = new Size(90, 27);
        btnAgregarLinea.Text = "Agregar";
        btnAgregarLinea.Click += btnAgregarLinea_Click;

        btnQuitarLinea.Location = new Point(670, 54);
        btnQuitarLinea.Size = new Size(90, 27);
        btnQuitarLinea.Text = "Quitar";
        btnQuitarLinea.Click += btnQuitarLinea_Click;

        dgvDetalle.AllowUserToAddRows = false;
        dgvDetalle.AllowUserToDeleteRows = false;
        dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDetalle.Location = new Point(20, 100);
        dgvDetalle.MultiSelect = false;
        dgvDetalle.ReadOnly = true;
        dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDetalle.Size = new Size(740, 260);

        lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTotal.Location = new Point(20, 380);
        lblTotal.Size = new Size(300, 25);
        lblTotal.Text = "Total: $0,00";

        btnGrabar.Location = new Point(620, 375);
        btnGrabar.Size = new Size(140, 35);
        btnGrabar.Text = "Grabar factura";
        btnGrabar.Click += btnGrabar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(790, 430);
        Controls.AddRange(new Control[]
        {
            lblFecha, dtpFecha, lblCliente, txtCliente, lblDocumento, txtDocumento,
            lblProducto, cboProducto, lblCantidad, numCantidad, btnAgregarLinea, btnQuitarLinea,
            dgvDetalle, lblTotal, btnGrabar
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Emitir factura";
        ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDetalle).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
