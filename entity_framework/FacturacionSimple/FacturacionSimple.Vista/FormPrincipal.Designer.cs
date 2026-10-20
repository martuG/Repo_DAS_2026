namespace FacturacionSimple.Vista;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer components = null!;
    private Label lblTitulo = null!;
    private Button btnProductos = null!;
    private Button btnEmitir = null!;
    private Button btnConsultar = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitulo = new Label();
        btnProductos = new Button();
        btnEmitir = new Button();
        btnConsultar = new Button();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(40, 30);
        lblTitulo.Text = "Facturación simple (EF Core)";

        btnProductos.Location = new Point(60, 100);
        btnProductos.Size = new Size(280, 45);
        btnProductos.Text = "Productos (ABM)";
        btnProductos.Click += btnProductos_Click;

        btnEmitir.Location = new Point(60, 160);
        btnEmitir.Size = new Size(280, 45);
        btnEmitir.Text = "Emitir factura";
        btnEmitir.Click += btnEmitir_Click;

        btnConsultar.Location = new Point(60, 220);
        btnConsultar.Size = new Size(280, 45);
        btnConsultar.Text = "Consultar facturas";
        btnConsultar.Click += btnConsultar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 320);
        Controls.Add(lblTitulo);
        Controls.Add(btnProductos);
        Controls.Add(btnEmitir);
        Controls.Add(btnConsultar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Facturación simple";
        ResumeLayout(false);
        PerformLayout();
    }
}
