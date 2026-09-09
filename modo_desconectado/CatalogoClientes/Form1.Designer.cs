namespace CatalogoClientes;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private GroupBox grpDatos;
    private Label lblId;
    private Label lblNombre;
    private Label lblTelefono;
    private TextBox txtId;
    private TextBox txtNombre;
    private TextBox txtTelefono;
    private Button btnAgregar;
    private Button btnModificar;
    private Button btnEliminar;
    private Button btnLimpiar;
    private Button btnRecargar;
    private Button btnGuardar;
    private GroupBox grpLote;
    private TextBox txtLote;
    private Button btnAgregarLote;
    private DataGridView dgvClientes;
    private GroupBox grpMemoria;
    private Label lblPersonas;
    private ComboBox cmbPersonas;
    private Label lblPersonaSeleccionada;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        grpDatos = new GroupBox();
        lblId = new Label();
        txtId = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        btnAgregar = new Button();
        btnModificar = new Button();
        btnEliminar = new Button();
        btnLimpiar = new Button();
        btnRecargar = new Button();
        btnGuardar = new Button();
        grpLote = new GroupBox();
        txtLote = new TextBox();
        btnAgregarLote = new Button();
        dgvClientes = new DataGridView();
        grpMemoria = new GroupBox();
        lblPersonas = new Label();
        cmbPersonas = new ComboBox();
        lblPersonaSeleccionada = new Label();
        grpDatos.SuspendLayout();
        grpLote.SuspendLayout();
        grpMemoria.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(430, 30);
        lblTitulo.Text = "Catálogo de clientes (modo desconectado)";

        grpDatos.Controls.Add(lblId);
        grpDatos.Controls.Add(txtId);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblTelefono);
        grpDatos.Controls.Add(txtTelefono);
        grpDatos.Location = new Point(24, 56);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(752, 90);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del cliente (cambios en memoria, sin SQL)";

        lblId.AutoSize = true;
        lblId.Location = new Point(18, 32);
        lblId.Name = "lblId";
        lblId.Size = new Size(18, 15);
        lblId.Text = "Id";

        txtId.Location = new Point(18, 50);
        txtId.Name = "txtId";
        txtId.ReadOnly = true;
        txtId.Size = new Size(70, 23);
        txtId.TabIndex = 0;

        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(110, 32);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(51, 15);
        lblNombre.Text = "Nombre";

        txtNombre.Location = new Point(110, 50);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(300, 23);
        txtNombre.TabIndex = 1;

        lblTelefono.AutoSize = true;
        lblTelefono.Location = new Point(430, 32);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(52, 15);
        lblTelefono.Text = "Teléfono";

        txtTelefono.Location = new Point(430, 50);
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(290, 23);
        txtTelefono.TabIndex = 2;

        btnAgregar.Location = new Point(24, 156);
        btnAgregar.Name = "btnAgregar";
        btnAgregar.Size = new Size(110, 32);
        btnAgregar.TabIndex = 1;
        btnAgregar.Text = "Agregar";
        btnAgregar.UseVisualStyleBackColor = true;
        btnAgregar.Click += btnAgregar_Click;

        btnModificar.Location = new Point(144, 156);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(110, 32);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = true;
        btnModificar.Click += btnModificar_Click;

        btnEliminar.Location = new Point(264, 156);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(110, 32);
        btnEliminar.TabIndex = 3;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;

        btnLimpiar.Location = new Point(384, 156);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(110, 32);
        btnLimpiar.TabIndex = 4;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = true;
        btnLimpiar.Click += btnLimpiar_Click;

        btnRecargar.Location = new Point(504, 156);
        btnRecargar.Name = "btnRecargar";
        btnRecargar.Size = new Size(120, 32);
        btnRecargar.TabIndex = 5;
        btnRecargar.Text = "Recargar (Fill)";
        btnRecargar.UseVisualStyleBackColor = true;
        btnRecargar.Click += btnRecargar_Click;

        btnGuardar.Location = new Point(634, 156);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(142, 32);
        btnGuardar.TabIndex = 6;
        btnGuardar.Text = "Guardar (Update)";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;

        grpLote.Controls.Add(txtLote);
        grpLote.Controls.Add(btnAgregarLote);
        grpLote.Location = new Point(24, 200);
        grpLote.Name = "grpLote";
        grpLote.Size = new Size(752, 92);
        grpLote.TabIndex = 7;
        grpLote.TabStop = false;
        grpLote.Text = "Alta en lote — una línea por cliente: Nombre;Telefono";

        txtLote.AcceptsReturn = true;
        txtLote.Location = new Point(18, 22);
        txtLote.Multiline = true;
        txtLote.Name = "txtLote";
        txtLote.PlaceholderText = "Dana Ruiz;11-5555-4444";
        txtLote.ScrollBars = ScrollBars.Vertical;
        txtLote.Size = new Size(560, 56);
        txtLote.TabIndex = 0;
        txtLote.Text = "Dana Ruiz;11-5555-4444\r\nElena Soto;11-5555-5555\r\nFranco Díaz;11-5555-6666";

        btnAgregarLote.Location = new Point(594, 22);
        btnAgregarLote.Name = "btnAgregarLote";
        btnAgregarLote.Size = new Size(140, 56);
        btnAgregarLote.TabIndex = 1;
        btnAgregarLote.Text = "Agregar lote";
        btnAgregarLote.UseVisualStyleBackColor = true;
        btnAgregarLote.Click += btnAgregarLote_Click;

        dgvClientes.AllowUserToAddRows = false;
        dgvClientes.AllowUserToDeleteRows = false;
        dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvClientes.Location = new Point(24, 306);
        dgvClientes.MultiSelect = false;
        dgvClientes.Name = "dgvClientes";
        dgvClientes.RowHeadersVisible = false;
        dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvClientes.Size = new Size(752, 200);
        dgvClientes.TabIndex = 8;
        dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

        grpMemoria.Controls.Add(lblPersonas);
        grpMemoria.Controls.Add(cmbPersonas);
        grpMemoria.Controls.Add(lblPersonaSeleccionada);
        grpMemoria.Location = new Point(24, 518);
        grpMemoria.Name = "grpMemoria";
        grpMemoria.Size = new Size(752, 86);
        grpMemoria.TabIndex = 9;
        grpMemoria.TabStop = false;
        grpMemoria.Text = "DataSet solo en memoria (sin SQL Server)";

        lblPersonas.AutoSize = true;
        lblPersonas.Location = new Point(18, 28);
        lblPersonas.Name = "lblPersonas";
        lblPersonas.Size = new Size(54, 15);
        lblPersonas.Text = "Personas";

        cmbPersonas.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPersonas.Location = new Point(18, 46);
        cmbPersonas.Name = "cmbPersonas";
        cmbPersonas.Size = new Size(340, 23);
        cmbPersonas.TabIndex = 0;
        cmbPersonas.SelectedIndexChanged += cmbPersonas_SelectedIndexChanged;

        lblPersonaSeleccionada.AutoSize = true;
        lblPersonaSeleccionada.Location = new Point(380, 49);
        lblPersonaSeleccionada.Name = "lblPersonaSeleccionada";
        lblPersonaSeleccionada.Size = new Size(120, 15);
        lblPersonaSeleccionada.Text = "ValueMember (Id): —";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 624);
        Controls.Add(lblTitulo);
        Controls.Add(grpDatos);
        Controls.Add(btnAgregar);
        Controls.Add(btnModificar);
        Controls.Add(btnEliminar);
        Controls.Add(btnLimpiar);
        Controls.Add(btnRecargar);
        Controls.Add(btnGuardar);
        Controls.Add(grpLote);
        Controls.Add(dgvClientes);
        Controls.Add(grpMemoria);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Modo desconectado - Catálogo de clientes";
        Load += Form1_Load;
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        grpLote.ResumeLayout(false);
        grpLote.PerformLayout();
        grpMemoria.ResumeLayout(false);
        grpMemoria.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
