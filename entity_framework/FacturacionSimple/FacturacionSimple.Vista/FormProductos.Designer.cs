namespace FacturacionSimple.Vista;

partial class FormProductos
{
    private System.ComponentModel.IContainer components = null!;
    private DataGridView dgv = null!;
    private TextBox txtBuscar = null!;
    private Button btnBuscar = null!;
    private TextBox txtCodigo = null!;
    private TextBox txtNombre = null!;
    private NumericUpDown numPrecio = null!;
    private CheckBox chkActivo = null!;
    private Button btnNuevo = null!;
    private Button btnGuardar = null!;
    private Button btnBaja = null!;
    private Button btnEliminar = null!;
    private Label lblBuscar = null!;
    private Label lblCodigo = null!;
    private Label lblNombre = null!;
    private Label lblPrecio = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        dgv = new DataGridView();
        txtBuscar = new TextBox();
        btnBuscar = new Button();
        txtCodigo = new TextBox();
        txtNombre = new TextBox();
        numPrecio = new NumericUpDown();
        chkActivo = new CheckBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnBaja = new Button();
        btnEliminar = new Button();
        lblBuscar = new Label();
        lblCodigo = new Label();
        lblNombre = new Label();
        lblPrecio = new Label();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
        SuspendLayout();

        lblBuscar.Location = new Point(20, 20);
        lblBuscar.Size = new Size(50, 23);
        lblBuscar.Text = "Buscar";

        txtBuscar.Location = new Point(75, 17);
        txtBuscar.Size = new Size(280, 23);

        btnBuscar.Location = new Point(370, 16);
        btnBuscar.Size = new Size(90, 25);
        btnBuscar.Text = "Buscar";
        btnBuscar.Click += btnBuscar_Click;

        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.Location = new Point(20, 55);
        dgv.MultiSelect = false;
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.Size = new Size(740, 250);
        dgv.SelectionChanged += dgv_SelectionChanged;

        lblCodigo.Location = new Point(20, 325);
        lblCodigo.Text = "Código";
        txtCodigo.Location = new Point(90, 322);
        txtCodigo.Size = new Size(140, 23);

        lblNombre.Location = new Point(250, 325);
        lblNombre.Text = "Nombre";
        txtNombre.Location = new Point(320, 322);
        txtNombre.Size = new Size(220, 23);

        lblPrecio.Location = new Point(560, 325);
        lblPrecio.Text = "Precio";
        numPrecio.DecimalPlaces = 2;
        numPrecio.Location = new Point(610, 322);
        numPrecio.Maximum = 1000000;
        numPrecio.Size = new Size(100, 23);

        chkActivo.Location = new Point(720, 324);
        chkActivo.Text = "Activo";
        chkActivo.Checked = true;

        btnNuevo.Location = new Point(20, 365);
        btnNuevo.Size = new Size(100, 30);
        btnNuevo.Text = "Nuevo";
        btnNuevo.Click += btnNuevo_Click;

        btnGuardar.Location = new Point(130, 365);
        btnGuardar.Size = new Size(100, 30);
        btnGuardar.Text = "Agregar";
        btnGuardar.Click += btnGuardar_Click;

        btnBaja.Location = new Point(240, 365);
        btnBaja.Size = new Size(120, 30);
        btnBaja.Text = "Baja lógica";
        btnBaja.Click += btnBaja_Click;

        btnEliminar.Location = new Point(370, 365);
        btnEliminar.Size = new Size(120, 30);
        btnEliminar.Text = "Eliminar físico";
        btnEliminar.Click += btnEliminar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(790, 420);
        Controls.AddRange(new Control[]
        {
            lblBuscar, txtBuscar, btnBuscar, dgv,
            lblCodigo, txtCodigo, lblNombre, txtNombre, lblPrecio, numPrecio, chkActivo,
            btnNuevo, btnGuardar, btnBaja, btnEliminar
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Productos";
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
