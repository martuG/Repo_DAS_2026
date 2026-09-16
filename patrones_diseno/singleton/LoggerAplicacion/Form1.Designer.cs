namespace LoggerAplicacion;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblExplicacion;
    private GroupBox grpIdentidad;
    private Label lblGuidTitulo;
    private Label lblGuid;
    private Label lblHashTitulo;
    private Label lblHash;
    private Button btnPedirInstancia;
    private GroupBox grpNuevo;
    private TextBox txtMensaje;
    private Button btnRegistrar;
    private Button btnAbrirSegunda;
    private Label lblRegistros;
    private ListBox lstRegistros;

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
        lblExplicacion = new Label();
        grpIdentidad = new GroupBox();
        lblGuidTitulo = new Label();
        lblGuid = new Label();
        lblHashTitulo = new Label();
        lblHash = new Label();
        btnPedirInstancia = new Button();
        grpNuevo = new GroupBox();
        txtMensaje = new TextBox();
        btnRegistrar = new Button();
        btnAbrirSegunda = new Button();
        lblRegistros = new Label();
        lstRegistros = new ListBox();
        grpIdentidad.SuspendLayout();
        grpNuevo.SuspendLayout();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(360, 30);
        lblTitulo.Text = "Patrón Singleton — Logger único";

        lblExplicacion.Location = new Point(24, 54);
        lblExplicacion.Name = "lblExplicacion";
        lblExplicacion.Size = new Size(752, 40);
        lblExplicacion.Text =
            "Solo puede existir una instancia de Logger. El constructor es privado y se accede con Logger.Instancia.\r\n" +
            "Abrí la segunda ventana: el Guid y el HashCode coinciden, y ambos formularios escriben en la misma lista.";

        grpIdentidad.Controls.Add(lblGuidTitulo);
        grpIdentidad.Controls.Add(lblGuid);
        grpIdentidad.Controls.Add(lblHashTitulo);
        grpIdentidad.Controls.Add(lblHash);
        grpIdentidad.Controls.Add(btnPedirInstancia);
        grpIdentidad.Location = new Point(24, 104);
        grpIdentidad.Name = "grpIdentidad";
        grpIdentidad.Size = new Size(752, 100);
        grpIdentidad.TabIndex = 0;
        grpIdentidad.TabStop = false;
        grpIdentidad.Text = "Identidad de Logger.Instancia (si cambia, habría más de un objeto)";

        lblGuidTitulo.AutoSize = true;
        lblGuidTitulo.Location = new Point(18, 28);
        lblGuidTitulo.Name = "lblGuidTitulo";
        lblGuidTitulo.Size = new Size(90, 15);
        lblGuidTitulo.Text = "IdInstancia (Guid)";

        lblGuid.AutoSize = true;
        lblGuid.Font = new Font("Consolas", 11F, FontStyle.Bold);
        lblGuid.ForeColor = Color.FromArgb(0, 90, 158);
        lblGuid.Location = new Point(18, 48);
        lblGuid.Name = "lblGuid";
        lblGuid.Size = new Size(80, 18);
        lblGuid.Text = "—";

        lblHashTitulo.AutoSize = true;
        lblHashTitulo.Location = new Point(430, 28);
        lblHashTitulo.Name = "lblHashTitulo";
        lblHashTitulo.Size = new Size(90, 15);
        lblHashTitulo.Text = "GetHashCode()";

        lblHash.AutoSize = true;
        lblHash.Font = new Font("Consolas", 11F, FontStyle.Bold);
        lblHash.ForeColor = Color.FromArgb(0, 90, 158);
        lblHash.Location = new Point(430, 48);
        lblHash.Name = "lblHash";
        lblHash.Size = new Size(20, 18);
        lblHash.Text = "—";

        btnPedirInstancia.Location = new Point(560, 42);
        btnPedirInstancia.Name = "btnPedirInstancia";
        btnPedirInstancia.Size = new Size(172, 32);
        btnPedirInstancia.TabIndex = 0;
        btnPedirInstancia.Text = "Pedir Instancia otra vez";
        btnPedirInstancia.UseVisualStyleBackColor = true;
        btnPedirInstancia.Click += btnPedirInstancia_Click;

        grpNuevo.Controls.Add(txtMensaje);
        grpNuevo.Controls.Add(btnRegistrar);
        grpNuevo.Controls.Add(btnAbrirSegunda);
        grpNuevo.Location = new Point(24, 216);
        grpNuevo.Name = "grpNuevo";
        grpNuevo.Size = new Size(752, 86);
        grpNuevo.TabIndex = 1;
        grpNuevo.TabStop = false;
        grpNuevo.Text = "Registrar un mensaje en el logger compartido";

        txtMensaje.Location = new Point(18, 32);
        txtMensaje.Name = "txtMensaje";
        txtMensaje.PlaceholderText = "Ej.: Usuario inició sesión";
        txtMensaje.Size = new Size(400, 23);
        txtMensaje.TabIndex = 0;

        btnRegistrar.Location = new Point(430, 28);
        btnRegistrar.Name = "btnRegistrar";
        btnRegistrar.Size = new Size(140, 32);
        btnRegistrar.TabIndex = 1;
        btnRegistrar.Text = "Registrar";
        btnRegistrar.UseVisualStyleBackColor = true;
        btnRegistrar.Click += btnRegistrar_Click;

        btnAbrirSegunda.Location = new Point(580, 28);
        btnAbrirSegunda.Name = "btnAbrirSegunda";
        btnAbrirSegunda.Size = new Size(152, 32);
        btnAbrirSegunda.TabIndex = 2;
        btnAbrirSegunda.Text = "Abrir 2.ª ventana";
        btnAbrirSegunda.UseVisualStyleBackColor = true;
        btnAbrirSegunda.Click += btnAbrirSegunda_Click;

        lblRegistros.AutoSize = true;
        lblRegistros.Location = new Point(24, 316);
        lblRegistros.Name = "lblRegistros";
        lblRegistros.Size = new Size(220, 15);
        lblRegistros.Text = "Historial único (compartido por toda la app)";

        lstRegistros.Font = new Font("Consolas", 9.75F);
        lstRegistros.FormattingEnabled = true;
        lstRegistros.ItemHeight = 15;
        lstRegistros.Location = new Point(24, 336);
        lstRegistros.Name = "lstRegistros";
        lstRegistros.Size = new Size(752, 244);
        lstRegistros.TabIndex = 2;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 604);
        Controls.Add(lblTitulo);
        Controls.Add(lblExplicacion);
        Controls.Add(grpIdentidad);
        Controls.Add(grpNuevo);
        Controls.Add(lblRegistros);
        Controls.Add(lstRegistros);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Singleton - Logger de la aplicación";
        Load += Form1_Load;
        FormClosed += Form1_FormClosed;
        AcceptButton = btnRegistrar;
        grpIdentidad.ResumeLayout(false);
        grpIdentidad.PerformLayout();
        grpNuevo.ResumeLayout(false);
        grpNuevo.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
