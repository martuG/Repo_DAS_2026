namespace MenuRestaurante;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblExplicacion;
    private Label lblCocina;
    private ComboBox cmbCocina;
    private Button btnGenerar;
    private Label lblFabrica;
    private GroupBox grpEntrada;
    private Label lblEntradaNombre;
    private Label lblEntradaDesc;
    private GroupBox grpPrincipal;
    private Label lblPrincipalNombre;
    private Label lblPrincipalDesc;
    private GroupBox grpPostre;
    private Label lblPostreNombre;
    private Label lblPostreDesc;

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
        lblCocina = new Label();
        cmbCocina = new ComboBox();
        btnGenerar = new Button();
        lblFabrica = new Label();
        grpEntrada = new GroupBox();
        lblEntradaNombre = new Label();
        lblEntradaDesc = new Label();
        grpPrincipal = new GroupBox();
        lblPrincipalNombre = new Label();
        lblPrincipalDesc = new Label();
        grpPostre = new GroupBox();
        lblPostreNombre = new Label();
        lblPostreDesc = new Label();
        grpEntrada.SuspendLayout();
        grpPrincipal.SuspendLayout();
        grpPostre.SuspendLayout();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(24, 18);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(420, 30);
        lblTitulo.Text = "Patrón Abstract Factory — Menú del día";

        lblExplicacion.Location = new Point(24, 54);
        lblExplicacion.Name = "lblExplicacion";
        lblExplicacion.Size = new Size(752, 40);
        lblExplicacion.Text =
            "Cada fábrica crea una familia de productos que combinan: entrada + principal + postre de la misma cocina.\r\n" +
            "El formulario pide los tres platos a IFabricaMenu y no instancia las clases concretas (Bruschetta, Mochi, etc.).";

        lblCocina.AutoSize = true;
        lblCocina.Location = new Point(24, 108);
        lblCocina.Name = "lblCocina";
        lblCocina.Size = new Size(90, 15);
        lblCocina.Text = "Elegir fábrica";

        cmbCocina.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCocina.Location = new Point(24, 128);
        cmbCocina.Name = "cmbCocina";
        cmbCocina.Size = new Size(240, 23);
        cmbCocina.TabIndex = 0;
        cmbCocina.SelectedIndexChanged += cmbCocina_SelectedIndexChanged;

        btnGenerar.Location = new Point(280, 126);
        btnGenerar.Name = "btnGenerar";
        btnGenerar.Size = new Size(160, 28);
        btnGenerar.TabIndex = 1;
        btnGenerar.Text = "Generar menú";
        btnGenerar.UseVisualStyleBackColor = true;
        btnGenerar.Click += btnGenerar_Click;

        lblFabrica.AutoSize = true;
        lblFabrica.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFabrica.ForeColor = Color.FromArgb(0, 90, 158);
        lblFabrica.Location = new Point(24, 168);
        lblFabrica.Name = "lblFabrica";
        lblFabrica.Size = new Size(80, 15);
        lblFabrica.Text = "Fábrica: —";

        grpEntrada.Controls.Add(lblEntradaNombre);
        grpEntrada.Controls.Add(lblEntradaDesc);
        grpEntrada.Location = new Point(24, 196);
        grpEntrada.Name = "grpEntrada";
        grpEntrada.Size = new Size(240, 160);
        grpEntrada.TabIndex = 2;
        grpEntrada.TabStop = false;
        grpEntrada.Text = "Entrada";

        lblEntradaNombre.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEntradaNombre.Location = new Point(16, 32);
        lblEntradaNombre.Name = "lblEntradaNombre";
        lblEntradaNombre.Size = new Size(204, 28);
        lblEntradaNombre.Text = "—";

        lblEntradaDesc.Location = new Point(16, 68);
        lblEntradaDesc.Name = "lblEntradaDesc";
        lblEntradaDesc.Size = new Size(204, 72);
        lblEntradaDesc.Text = "";

        grpPrincipal.Controls.Add(lblPrincipalNombre);
        grpPrincipal.Controls.Add(lblPrincipalDesc);
        grpPrincipal.Location = new Point(280, 196);
        grpPrincipal.Name = "grpPrincipal";
        grpPrincipal.Size = new Size(240, 160);
        grpPrincipal.TabIndex = 3;
        grpPrincipal.TabStop = false;
        grpPrincipal.Text = "Principal";

        lblPrincipalNombre.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblPrincipalNombre.Location = new Point(16, 32);
        lblPrincipalNombre.Name = "lblPrincipalNombre";
        lblPrincipalNombre.Size = new Size(204, 28);
        lblPrincipalNombre.Text = "—";

        lblPrincipalDesc.Location = new Point(16, 68);
        lblPrincipalDesc.Name = "lblPrincipalDesc";
        lblPrincipalDesc.Size = new Size(204, 72);
        lblPrincipalDesc.Text = "";

        grpPostre.Controls.Add(lblPostreNombre);
        grpPostre.Controls.Add(lblPostreDesc);
        grpPostre.Location = new Point(536, 196);
        grpPostre.Name = "grpPostre";
        grpPostre.Size = new Size(240, 160);
        grpPostre.TabIndex = 4;
        grpPostre.TabStop = false;
        grpPostre.Text = "Postre";

        lblPostreNombre.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblPostreNombre.Location = new Point(16, 32);
        lblPostreNombre.Name = "lblPostreNombre";
        lblPostreNombre.Size = new Size(204, 28);
        lblPostreNombre.Text = "—";

        lblPostreDesc.Location = new Point(16, 68);
        lblPostreDesc.Name = "lblPostreDesc";
        lblPostreDesc.Size = new Size(204, 72);
        lblPostreDesc.Text = "";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 384);
        Controls.Add(lblTitulo);
        Controls.Add(lblExplicacion);
        Controls.Add(lblCocina);
        Controls.Add(cmbCocina);
        Controls.Add(btnGenerar);
        Controls.Add(lblFabrica);
        Controls.Add(grpEntrada);
        Controls.Add(grpPrincipal);
        Controls.Add(grpPostre);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Abstract Factory - Menú de restaurante";
        Load += Form1_Load;
        grpEntrada.ResumeLayout(false);
        grpPrincipal.ResumeLayout(false);
        grpPostre.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
