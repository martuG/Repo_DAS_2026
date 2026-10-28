namespace ViveroPedidos.Vista;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer components = null!;
    private Label lblTitulo = null!;
    private Button btnPlantas = null!;
    private Button btnRegistrar = null!;
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
        btnPlantas = new Button();
        btnRegistrar = new Button();
        btnConsultar = new Button();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(40, 30);
        lblTitulo.Text = "Vivero municipal (EF Core)";

        btnPlantas.Location = new Point(60, 100);
        btnPlantas.Size = new Size(280, 45);
        btnPlantas.Text = "Plantas (ABM)";
        btnPlantas.Click += btnPlantas_Click;

        btnRegistrar.Location = new Point(60, 160);
        btnRegistrar.Size = new Size(280, 45);
        btnRegistrar.Text = "Registrar pedido";
        btnRegistrar.Click += btnRegistrar_Click;

        btnConsultar.Location = new Point(60, 220);
        btnConsultar.Size = new Size(280, 45);
        btnConsultar.Text = "Consultar pedidos";
        btnConsultar.Click += btnConsultar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(420, 320);
        Controls.Add(lblTitulo);
        Controls.Add(btnPlantas);
        Controls.Add(btnRegistrar);
        Controls.Add(btnConsultar);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Vivero municipal";
        ResumeLayout(false);
        PerformLayout();
    }
}
