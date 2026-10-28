namespace ViveroPedidos.Vista;

partial class FormPlantas
{
    private System.ComponentModel.IContainer components = null!;
    private DataGridView dgv = null!;
    private TextBox txtBuscar = null!;
    private Button btnBuscar = null!;
    private TextBox txtCodigo = null!;
    private TextBox txtNombreComun = null!;
    private TextBox txtEspecie = null!;
    private CheckBox chkDisponible = null!;
    private Button btnNuevo = null!;
    private Button btnGuardar = null!;
    private Button btnNoDisponible = null!;
    private Button btnEliminar = null!;
    private Label lblBuscar = null!;
    private Label lblCodigo = null!;
    private Label lblNombreComun = null!;
    private Label lblEspecie = null!;

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
        txtNombreComun = new TextBox();
        txtEspecie = new TextBox();
        chkDisponible = new CheckBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnNoDisponible = new Button();
        btnEliminar = new Button();
        lblBuscar = new Label();
        lblCodigo = new Label();
        lblNombreComun = new Label();
        lblEspecie = new Label();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();

        lblBuscar.Location = new Point(20, 20);
        lblBuscar.Size = new Size(50, 23);
        lblBuscar.Text = "Buscar";

        txtBuscar.Location = new Point(75, 17);
        txtBuscar.Size = new Size(320, 23);

        btnBuscar.Location = new Point(410, 16);
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
        dgv.Size = new Size(820, 240);
        dgv.SelectionChanged += dgv_SelectionChanged;

        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(20, 318);
        lblCodigo.Text = "Código";
        txtCodigo.Location = new Point(75, 315);
        txtCodigo.MaxLength = 20;
        txtCodigo.Size = new Size(120, 23);

        lblNombreComun.AutoSize = true;
        lblNombreComun.Location = new Point(210, 318);
        lblNombreComun.Text = "Nombre común";
        txtNombreComun.Location = new Point(310, 315);
        txtNombreComun.MaxLength = 120;
        txtNombreComun.Size = new Size(200, 23);

        lblEspecie.AutoSize = true;
        lblEspecie.Location = new Point(530, 318);
        lblEspecie.Text = "Especie";
        txtEspecie.Location = new Point(590, 315);
        txtEspecie.MaxLength = 120;
        txtEspecie.Size = new Size(180, 23);

        chkDisponible.AutoSize = true;
        chkDisponible.Checked = true;
        chkDisponible.Enabled = false;
        chkDisponible.Location = new Point(20, 355);
        chkDisponible.Text = "Disponible";

        btnNuevo.Location = new Point(20, 395);
        btnNuevo.Size = new Size(110, 32);
        btnNuevo.Text = "Nuevo";
        btnNuevo.Click += btnNuevo_Click;

        btnGuardar.Location = new Point(140, 395);
        btnGuardar.Size = new Size(110, 32);
        btnGuardar.Text = "Agregar";
        btnGuardar.Click += btnGuardar_Click;

        btnNoDisponible.Location = new Point(260, 395);
        btnNoDisponible.Size = new Size(150, 32);
        btnNoDisponible.Text = "No disponible";
        btnNoDisponible.Click += btnNoDisponible_Click;

        btnEliminar.Location = new Point(420, 395);
        btnEliminar.Size = new Size(140, 32);
        btnEliminar.Text = "Eliminar físico";
        btnEliminar.Click += btnEliminar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(860, 450);
        Controls.AddRange(new Control[]
        {
            lblBuscar, txtBuscar, btnBuscar, dgv,
            lblCodigo, txtCodigo, lblNombreComun, txtNombreComun, lblEspecie, txtEspecie,
            chkDisponible, btnNuevo, btnGuardar, btnNoDisponible, btnEliminar
        });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Plantas";
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
