namespace ArmadoComputadora;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblExplicacion;
    private GroupBox grpDirector;
    private Button btnOficina;
    private Button btnDesarrollo;
    private Button btnGamer;
    private GroupBox grpPersonalizado;
    private Label lblCpu;
    private ComboBox cmbCpu;
    private Label lblRam;
    private ComboBox cmbRam;
    private Label lblGpu;
    private ComboBox cmbGpu;
    private Label lblDisco;
    private ComboBox cmbDisco;
    private Label lblFuente;
    private ComboBox cmbFuente;
    private Label lblGabinete;
    private ComboBox cmbGabinete;
    private Button btnPersonalizado;
    private GroupBox grpResultado;
    private Label lblComo;
    private TextBox txtResultado;

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
        grpDirector = new GroupBox();
        btnOficina = new Button();
        btnDesarrollo = new Button();
        btnGamer = new Button();
        grpPersonalizado = new GroupBox();
        lblCpu = new Label();
        cmbCpu = new ComboBox();
        lblRam = new Label();
        cmbRam = new ComboBox();
        lblGpu = new Label();
        cmbGpu = new ComboBox();
        lblDisco = new Label();
        cmbDisco = new ComboBox();
        lblFuente = new Label();
        cmbFuente = new ComboBox();
        lblGabinete = new Label();
        cmbGabinete = new ComboBox();
        btnPersonalizado = new Button();
        grpResultado = new GroupBox();
        lblComo = new Label();
        txtResultado = new TextBox();
        grpDirector.SuspendLayout();
        grpPersonalizado.SuspendLayout();
        grpResultado.SuspendLayout();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(430, 30);
        lblTitulo.Text = "Patrón Builder — Armado de computadora";

        lblExplicacion.Location = new Point(24, 54);
        lblExplicacion.Name = "lblExplicacion";
        lblExplicacion.Size = new Size(752, 40);
        lblExplicacion.Text =
            "El producto (Computadora) se construye paso a paso. El Director aplica recetas listas (Oficina, Desarrollo, Gamer).\r\n" +
            "En modo personalizado el formulario usa el mismo Builder, sin Director, eligiendo cada componente.";

        grpDirector.Controls.Add(btnOficina);
        grpDirector.Controls.Add(btnDesarrollo);
        grpDirector.Controls.Add(btnGamer);
        grpDirector.Location = new Point(24, 104);
        grpDirector.Name = "grpDirector";
        grpDirector.Size = new Size(752, 78);
        grpDirector.TabIndex = 0;
        grpDirector.TabStop = false;
        grpDirector.Text = "Director — recetas predefinidas";

        btnOficina.Location = new Point(18, 28);
        btnOficina.Name = "btnOficina";
        btnOficina.Size = new Size(220, 32);
        btnOficina.TabIndex = 0;
        btnOficina.Text = "Armar Oficina";
        btnOficina.UseVisualStyleBackColor = true;
        btnOficina.Click += btnOficina_Click;

        btnDesarrollo.Location = new Point(258, 28);
        btnDesarrollo.Name = "btnDesarrollo";
        btnDesarrollo.Size = new Size(220, 32);
        btnDesarrollo.TabIndex = 1;
        btnDesarrollo.Text = "Armar Desarrollo";
        btnDesarrollo.UseVisualStyleBackColor = true;
        btnDesarrollo.Click += btnDesarrollo_Click;

        btnGamer.Location = new Point(498, 28);
        btnGamer.Name = "btnGamer";
        btnGamer.Size = new Size(232, 32);
        btnGamer.TabIndex = 2;
        btnGamer.Text = "Armar Gamer";
        btnGamer.UseVisualStyleBackColor = true;
        btnGamer.Click += btnGamer_Click;

        grpPersonalizado.Controls.Add(lblCpu);
        grpPersonalizado.Controls.Add(cmbCpu);
        grpPersonalizado.Controls.Add(lblRam);
        grpPersonalizado.Controls.Add(cmbRam);
        grpPersonalizado.Controls.Add(lblGpu);
        grpPersonalizado.Controls.Add(cmbGpu);
        grpPersonalizado.Controls.Add(lblDisco);
        grpPersonalizado.Controls.Add(cmbDisco);
        grpPersonalizado.Controls.Add(lblFuente);
        grpPersonalizado.Controls.Add(cmbFuente);
        grpPersonalizado.Controls.Add(lblGabinete);
        grpPersonalizado.Controls.Add(cmbGabinete);
        grpPersonalizado.Controls.Add(btnPersonalizado);
        grpPersonalizado.Location = new Point(24, 194);
        grpPersonalizado.Name = "grpPersonalizado";
        grpPersonalizado.Size = new Size(752, 168);
        grpPersonalizado.TabIndex = 1;
        grpPersonalizado.TabStop = false;
        grpPersonalizado.Text = "Builder directo — cada ConXxx() agrega una parte";

        lblCpu.AutoSize = true;
        lblCpu.Location = new Point(18, 28);
        lblCpu.Name = "lblCpu";
        lblCpu.Size = new Size(28, 15);
        lblCpu.Text = "CPU";

        cmbCpu.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCpu.Location = new Point(18, 46);
        cmbCpu.Name = "cmbCpu";
        cmbCpu.Size = new Size(220, 23);
        cmbCpu.TabIndex = 0;

        lblRam.AutoSize = true;
        lblRam.Location = new Point(258, 28);
        lblRam.Name = "lblRam";
        lblRam.Size = new Size(31, 15);
        lblRam.Text = "RAM";

        cmbRam.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRam.Location = new Point(258, 46);
        cmbRam.Name = "cmbRam";
        cmbRam.Size = new Size(220, 23);
        cmbRam.TabIndex = 1;

        lblGpu.AutoSize = true;
        lblGpu.Location = new Point(498, 28);
        lblGpu.Name = "lblGpu";
        lblGpu.Size = new Size(28, 15);
        lblGpu.Text = "GPU";

        cmbGpu.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbGpu.Location = new Point(498, 46);
        cmbGpu.Name = "cmbGpu";
        cmbGpu.Size = new Size(232, 23);
        cmbGpu.TabIndex = 2;

        lblDisco.AutoSize = true;
        lblDisco.Location = new Point(18, 80);
        lblDisco.Name = "lblDisco";
        lblDisco.Size = new Size(36, 15);
        lblDisco.Text = "Disco";

        cmbDisco.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDisco.Location = new Point(18, 98);
        cmbDisco.Name = "cmbDisco";
        cmbDisco.Size = new Size(220, 23);
        cmbDisco.TabIndex = 3;

        lblFuente.AutoSize = true;
        lblFuente.Location = new Point(258, 80);
        lblFuente.Name = "lblFuente";
        lblFuente.Size = new Size(43, 15);
        lblFuente.Text = "Fuente";

        cmbFuente.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFuente.Location = new Point(258, 98);
        cmbFuente.Name = "cmbFuente";
        cmbFuente.Size = new Size(220, 23);
        cmbFuente.TabIndex = 4;

        lblGabinete.AutoSize = true;
        lblGabinete.Location = new Point(498, 80);
        lblGabinete.Name = "lblGabinete";
        lblGabinete.Size = new Size(52, 15);
        lblGabinete.Text = "Gabinete";

        cmbGabinete.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbGabinete.Location = new Point(498, 98);
        cmbGabinete.Name = "cmbGabinete";
        cmbGabinete.Size = new Size(232, 23);
        cmbGabinete.TabIndex = 5;

        btnPersonalizado.Location = new Point(18, 128);
        btnPersonalizado.Name = "btnPersonalizado";
        btnPersonalizado.Size = new Size(220, 28);
        btnPersonalizado.TabIndex = 6;
        btnPersonalizado.Text = "Construir personalizada";
        btnPersonalizado.UseVisualStyleBackColor = true;
        btnPersonalizado.Click += btnPersonalizado_Click;

        grpResultado.Controls.Add(lblComo);
        grpResultado.Controls.Add(txtResultado);
        grpResultado.Location = new Point(24, 374);
        grpResultado.Name = "grpResultado";
        grpResultado.Size = new Size(752, 210);
        grpResultado.TabIndex = 2;
        grpResultado.TabStop = false;
        grpResultado.Text = "Producto construido";

        lblComo.AutoSize = true;
        lblComo.ForeColor = Color.FromArgb(0, 90, 158);
        lblComo.Location = new Point(18, 24);
        lblComo.Name = "lblComo";
        lblComo.Size = new Size(220, 15);
        lblComo.Text = "Elegí una receta o armá una personalizada.";

        txtResultado.Font = new Font("Consolas", 11F);
        txtResultado.Location = new Point(18, 46);
        txtResultado.Multiline = true;
        txtResultado.Name = "txtResultado";
        txtResultado.ReadOnly = true;
        txtResultado.ScrollBars = ScrollBars.Vertical;
        txtResultado.Size = new Size(712, 146);
        txtResultado.TabIndex = 0;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 604);
        Controls.Add(lblTitulo);
        Controls.Add(lblExplicacion);
        Controls.Add(grpDirector);
        Controls.Add(grpPersonalizado);
        Controls.Add(grpResultado);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Builder - Armado de computadora";
        Load += Form1_Load;
        grpDirector.ResumeLayout(false);
        grpPersonalizado.ResumeLayout(false);
        grpPersonalizado.PerformLayout();
        grpResultado.ResumeLayout(false);
        grpResultado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
