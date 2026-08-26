namespace Banco;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private DataGridView dgvCuentas;
    private Label lblOrigen;
    private Label lblDestino;
    private Label lblMonto;
    private ComboBox cmbOrigen;
    private ComboBox cmbDestino;
    private TextBox txtMonto;
    private Button btnTransferir;
    private Button btnActualizar;

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
        dgvCuentas = new DataGridView();
        lblOrigen = new Label();
        lblDestino = new Label();
        lblMonto = new Label();
        cmbOrigen = new ComboBox();
        cmbDestino = new ComboBox();
        txtMonto = new TextBox();
        btnTransferir = new Button();
        btnActualizar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvCuentas).BeginInit();
        SuspendLayout();

        dgvCuentas.AllowUserToAddRows = false;
        dgvCuentas.AllowUserToDeleteRows = false;
        dgvCuentas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvCuentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCuentas.Location = new Point(16, 16);
        dgvCuentas.Name = "dgvCuentas";
        dgvCuentas.ReadOnly = true;
        dgvCuentas.RowHeadersVisible = false;
        dgvCuentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCuentas.Size = new Size(556, 240);

        lblOrigen.AutoSize = true;
        lblOrigen.Location = new Point(16, 272);
        lblOrigen.Text = "Origen";

        cmbOrigen.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOrigen.Location = new Point(16, 292);
        cmbOrigen.Size = new Size(220, 23);

        lblDestino.AutoSize = true;
        lblDestino.Location = new Point(252, 272);
        lblDestino.Text = "Destino";

        cmbDestino.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDestino.Location = new Point(252, 292);
        cmbDestino.Size = new Size(220, 23);

        lblMonto.AutoSize = true;
        lblMonto.Location = new Point(16, 328);
        lblMonto.Text = "Monto";

        txtMonto.Location = new Point(16, 348);
        txtMonto.Size = new Size(220, 23);

        btnTransferir.Location = new Point(252, 346);
        btnTransferir.Size = new Size(120, 27);
        btnTransferir.Text = "Transferir";
        btnTransferir.Click += btnTransferir_Click;

        btnActualizar.Location = new Point(384, 346);
        btnActualizar.Size = new Size(88, 27);
        btnActualizar.Text = "Actualizar";
        btnActualizar.Click += btnActualizar_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(588, 396);
        Controls.Add(dgvCuentas);
        Controls.Add(lblOrigen);
        Controls.Add(cmbOrigen);
        Controls.Add(lblDestino);
        Controls.Add(cmbDestino);
        Controls.Add(lblMonto);
        Controls.Add(txtMonto);
        Controls.Add(btnTransferir);
        Controls.Add(btnActualizar);
        MinimumSize = new Size(500, 360);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Banco - Transferencias";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dgvCuentas).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
