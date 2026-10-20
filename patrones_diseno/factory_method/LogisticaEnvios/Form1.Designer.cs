namespace LogisticaEnvios;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblExplicacion;
    private GroupBox grpPedido;
    private Label lblCanal;
    private ComboBox cmbCanal;
    private Label lblDestino;
    private TextBox txtDestino;
    private Button btnDespachar;
    private Label lblProducto;
    private GroupBox grpResultado;
    private TextBox txtResultado;
    private Label lblHistorial;
    private ListBox lstHistorial;

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
        grpPedido = new GroupBox();
        lblCanal = new Label();
        cmbCanal = new ComboBox();
        lblDestino = new Label();
        txtDestino = new TextBox();
        btnDespachar = new Button();
        lblProducto = new Label();
        grpResultado = new GroupBox();
        txtResultado = new TextBox();
        lblHistorial = new Label();
        lstHistorial = new ListBox();
        grpPedido.SuspendLayout();
        grpResultado.SuspendLayout();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(430, 30);
        lblTitulo.Text = "Patrón Factory Method — Logística";

        lblExplicacion.Location = new Point(24, 54);
        lblExplicacion.Name = "lblExplicacion";
        lblExplicacion.Size = new Size(752, 40);
        lblExplicacion.Text =
            "El formulario elige un Creador (Terrestre, Marítimo o Aéreo). Cada subclase redefine CrearTransporte()\r\n" +
            "y decide el producto: Camión, Barco o Avión. El algoritmo Despachar() no conoce esas clases concretas.";

        grpPedido.Controls.Add(lblCanal);
        grpPedido.Controls.Add(cmbCanal);
        grpPedido.Controls.Add(lblDestino);
        grpPedido.Controls.Add(txtDestino);
        grpPedido.Controls.Add(btnDespachar);
        grpPedido.Location = new Point(24, 104);
        grpPedido.Name = "grpPedido";
        grpPedido.Size = new Size(752, 90);
        grpPedido.TabIndex = 0;
        grpPedido.TabStop = false;
        grpPedido.Text = "Elegir creador y destino";

        lblCanal.AutoSize = true;
        lblCanal.Location = new Point(18, 28);
        lblCanal.Name = "lblCanal";
        lblCanal.Size = new Size(90, 15);
        lblCanal.Text = "Creador (canal)";

        cmbCanal.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCanal.Location = new Point(18, 48);
        cmbCanal.Name = "cmbCanal";
        cmbCanal.Size = new Size(200, 23);
        cmbCanal.TabIndex = 0;

        lblDestino.AutoSize = true;
        lblDestino.Location = new Point(240, 28);
        lblDestino.Name = "lblDestino";
        lblDestino.Size = new Size(47, 15);
        lblDestino.Text = "Destino";

        txtDestino.Location = new Point(240, 48);
        txtDestino.Name = "txtDestino";
        txtDestino.Size = new Size(280, 23);
        txtDestino.TabIndex = 1;

        btnDespachar.Location = new Point(544, 44);
        btnDespachar.Name = "btnDespachar";
        btnDespachar.Size = new Size(184, 32);
        btnDespachar.TabIndex = 2;
        btnDespachar.Text = "Despachar (Factory Method)";
        btnDespachar.UseVisualStyleBackColor = true;
        btnDespachar.Click += btnDespachar_Click;

        lblProducto.AutoSize = true;
        lblProducto.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProducto.ForeColor = Color.FromArgb(0, 90, 158);
        lblProducto.Location = new Point(24, 208);
        lblProducto.Name = "lblProducto";
        lblProducto.Size = new Size(200, 15);
        lblProducto.Text = "Factory Method → (aún no se llamó)";

        grpResultado.Controls.Add(txtResultado);
        grpResultado.Location = new Point(24, 232);
        grpResultado.Name = "grpResultado";
        grpResultado.Size = new Size(752, 140);
        grpResultado.TabIndex = 1;
        grpResultado.TabStop = false;
        grpResultado.Text = "Resultado de Despachar()";

        txtResultado.Font = new Font("Consolas", 10F);
        txtResultado.Location = new Point(18, 24);
        txtResultado.Multiline = true;
        txtResultado.Name = "txtResultado";
        txtResultado.ReadOnly = true;
        txtResultado.ScrollBars = ScrollBars.Vertical;
        txtResultado.Size = new Size(712, 100);
        txtResultado.TabIndex = 0;

        lblHistorial.AutoSize = true;
        lblHistorial.Location = new Point(24, 384);
        lblHistorial.Name = "lblHistorial";
        lblHistorial.Size = new Size(54, 15);
        lblHistorial.Text = "Historial";

        lstHistorial.FormattingEnabled = true;
        lstHistorial.ItemHeight = 15;
        lstHistorial.Location = new Point(24, 404);
        lstHistorial.Name = "lstHistorial";
        lstHistorial.Size = new Size(752, 154);
        lstHistorial.TabIndex = 2;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 584);
        Controls.Add(lblTitulo);
        Controls.Add(lblExplicacion);
        Controls.Add(grpPedido);
        Controls.Add(lblProducto);
        Controls.Add(grpResultado);
        Controls.Add(lblHistorial);
        Controls.Add(lstHistorial);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Factory Method - Logística de envíos";
        Load += Form1_Load;
        AcceptButton = btnDespachar;
        grpPedido.ResumeLayout(false);
        grpPedido.PerformLayout();
        grpResultado.ResumeLayout(false);
        grpResultado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
