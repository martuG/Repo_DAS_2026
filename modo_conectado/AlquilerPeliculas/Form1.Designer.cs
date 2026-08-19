namespace AlquilerPeliculas;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private GroupBox grpDatos;
    private Label lblId;
    private Label lblNombre;
    private Label lblDirector;
    private Label lblDuracion;
    private TextBox txtId;
    private TextBox txtNombre;
    private TextBox txtDirector;
    private TextBox txtDuracion;
    private Button btnAgregar;
    private Button btnModificar;
    private Button btnEliminar;
    private Button btnListar;
    private Button btnLimpiar;
    private DataGridView dgvPeliculas;

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
        lblDirector = new Label();
        txtDirector = new TextBox();
        lblDuracion = new Label();
        txtDuracion = new TextBox();
        btnAgregar = new Button();
        btnModificar = new Button();
        btnEliminar = new Button();
        btnListar = new Button();
        btnLimpiar = new Button();
        dgvPeliculas = new DataGridView();
        grpDatos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPeliculas).BeginInit();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(280, 30);
        lblTitulo.Text = "Alquiler de películas";

        grpDatos.Controls.Add(lblId);
        grpDatos.Controls.Add(txtId);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblDirector);
        grpDatos.Controls.Add(txtDirector);
        grpDatos.Controls.Add(lblDuracion);
        grpDatos.Controls.Add(txtDuracion);
        grpDatos.Location = new Point(24, 62);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(752, 118);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la película";

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
        txtNombre.Size = new Size(250, 23);
        txtNombre.TabIndex = 1;

        lblDirector.AutoSize = true;
        lblDirector.Location = new Point(380, 32);
        lblDirector.Name = "lblDirector";
        lblDirector.Size = new Size(51, 15);
        lblDirector.Text = "Director";

        txtDirector.Location = new Point(380, 50);
        txtDirector.Name = "txtDirector";
        txtDirector.Size = new Size(220, 23);
        txtDirector.TabIndex = 2;

        lblDuracion.AutoSize = true;
        lblDuracion.Location = new Point(620, 32);
        lblDuracion.Name = "lblDuracion";
        lblDuracion.Size = new Size(108, 15);
        lblDuracion.Text = "Duración (minutos)";

        txtDuracion.Location = new Point(620, 50);
        txtDuracion.Name = "txtDuracion";
        txtDuracion.Size = new Size(110, 23);
        txtDuracion.TabIndex = 3;

        btnAgregar.Location = new Point(24, 198);
        btnAgregar.Name = "btnAgregar";
        btnAgregar.Size = new Size(110, 32);
        btnAgregar.TabIndex = 1;
        btnAgregar.Text = "Agregar";
        btnAgregar.UseVisualStyleBackColor = true;
        btnAgregar.Click += btnAgregar_Click;

        btnModificar.Location = new Point(148, 198);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(110, 32);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = true;
        btnModificar.Click += btnModificar_Click;

        btnEliminar.Location = new Point(272, 198);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(110, 32);
        btnEliminar.TabIndex = 3;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;

        btnListar.Location = new Point(396, 198);
        btnListar.Name = "btnListar";
        btnListar.Size = new Size(110, 32);
        btnListar.TabIndex = 4;
        btnListar.Text = "Listar";
        btnListar.UseVisualStyleBackColor = true;
        btnListar.Click += btnListar_Click;

        btnLimpiar.Location = new Point(520, 198);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(110, 32);
        btnLimpiar.TabIndex = 5;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = true;
        btnLimpiar.Click += btnLimpiar_Click;

        dgvPeliculas.AllowUserToAddRows = false;
        dgvPeliculas.AllowUserToDeleteRows = false;
        dgvPeliculas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPeliculas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPeliculas.Location = new Point(24, 248);
        dgvPeliculas.MultiSelect = false;
        dgvPeliculas.Name = "dgvPeliculas";
        dgvPeliculas.ReadOnly = true;
        dgvPeliculas.RowHeadersVisible = false;
        dgvPeliculas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPeliculas.Size = new Size(752, 280);
        dgvPeliculas.TabIndex = 6;
        dgvPeliculas.SelectionChanged += dgvPeliculas_SelectionChanged;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 552);
        Controls.Add(lblTitulo);
        Controls.Add(grpDatos);
        Controls.Add(btnAgregar);
        Controls.Add(btnModificar);
        Controls.Add(btnEliminar);
        Controls.Add(btnListar);
        Controls.Add(btnLimpiar);
        Controls.Add(dgvPeliculas);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Modo conectado - Alquiler de películas";
        Load += Form1_Load;
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPeliculas).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
