namespace LoggerAplicacion;

partial class FormSecundario
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblExplicacion;
    private Label lblGuidTitulo;
    private Label lblGuid;
    private Label lblHashTitulo;
    private Label lblHash;
    private TextBox txtMensaje;
    private Button btnRegistrar;
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
        lblGuidTitulo = new Label();
        lblGuid = new Label();
        lblHashTitulo = new Label();
        lblHash = new Label();
        txtMensaje = new TextBox();
        btnRegistrar = new Button();
        lstRegistros = new ListBox();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Location = new Point(20, 16);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(220, 25);
        lblTitulo.Text = "Segunda ventana";

        lblExplicacion.Location = new Point(20, 48);
        lblExplicacion.Name = "lblExplicacion";
        lblExplicacion.Size = new Size(540, 36);
        lblExplicacion.Text = "Esta ventana NO crea un Logger nuevo: usa la misma Instancia.\r\nEl Guid y el HashCode tienen que coincidir con los de la ventana principal.";

        lblGuidTitulo.AutoSize = true;
        lblGuidTitulo.Location = new Point(20, 96);
        lblGuidTitulo.Name = "lblGuidTitulo";
        lblGuidTitulo.Size = new Size(32, 15);
        lblGuidTitulo.Text = "Guid";

        lblGuid.AutoSize = true;
        lblGuid.Font = new Font("Consolas", 10F, FontStyle.Bold);
        lblGuid.ForeColor = Color.FromArgb(0, 90, 158);
        lblGuid.Location = new Point(20, 114);
        lblGuid.Name = "lblGuid";
        lblGuid.Size = new Size(16, 17);
        lblGuid.Text = "—";

        lblHashTitulo.AutoSize = true;
        lblHashTitulo.Location = new Point(360, 96);
        lblHashTitulo.Name = "lblHashTitulo";
        lblHashTitulo.Size = new Size(32, 15);
        lblHashTitulo.Text = "Hash";

        lblHash.AutoSize = true;
        lblHash.Font = new Font("Consolas", 10F, FontStyle.Bold);
        lblHash.ForeColor = Color.FromArgb(0, 90, 158);
        lblHash.Location = new Point(360, 114);
        lblHash.Name = "lblHash";
        lblHash.Size = new Size(16, 17);
        lblHash.Text = "—";

        txtMensaje.Location = new Point(20, 148);
        txtMensaje.Name = "txtMensaje";
        txtMensaje.PlaceholderText = "Mensaje desde la 2.ª ventana";
        txtMensaje.Size = new Size(380, 23);
        txtMensaje.TabIndex = 0;

        btnRegistrar.Location = new Point(410, 144);
        btnRegistrar.Name = "btnRegistrar";
        btnRegistrar.Size = new Size(150, 32);
        btnRegistrar.TabIndex = 1;
        btnRegistrar.Text = "Registrar";
        btnRegistrar.UseVisualStyleBackColor = true;
        btnRegistrar.Click += btnRegistrar_Click;

        lstRegistros.Font = new Font("Consolas", 9F);
        lstRegistros.FormattingEnabled = true;
        lstRegistros.ItemHeight = 14;
        lstRegistros.Location = new Point(20, 190);
        lstRegistros.Name = "lstRegistros";
        lstRegistros.Size = new Size(540, 200);
        lstRegistros.TabIndex = 2;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 416);
        Controls.Add(lblTitulo);
        Controls.Add(lblExplicacion);
        Controls.Add(lblGuidTitulo);
        Controls.Add(lblGuid);
        Controls.Add(lblHashTitulo);
        Controls.Add(lblHash);
        Controls.Add(txtMensaje);
        Controls.Add(btnRegistrar);
        Controls.Add(lstRegistros);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "FormSecundario";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Singleton - Segunda ventana";
        Load += FormSecundario_Load;
        FormClosed += FormSecundario_FormClosed;
        AcceptButton = btnRegistrar;
        ResumeLayout(false);
        PerformLayout();
    }
}
