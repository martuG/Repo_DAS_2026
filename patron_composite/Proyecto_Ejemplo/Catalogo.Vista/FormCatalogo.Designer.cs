namespace Catalogo.Vista;

partial class FormCatalogo
{
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private TreeView tvCatalogo;
    private GroupBox grpNodo;
    private Label lblTipoTitulo;
    private Label lblTipo;
    private Label lblNombreTitulo;
    private Label lblNombreValor;
    private Label lblPrecioTitulo;
    private Label lblPrecio;
    private TextBox txtComposicion;
    private GroupBox grpAlta;
    private Label lblNuevoNombre;
    private TextBox txtNombre;
    private Label lblValor;
    private TextBox txtValor;
    private RadioButton rbProducto;
    private RadioButton rbCaja;
    private Button btnAgregarDentro;
    private Button btnAgregarRaiz;
    private Button btnQuitar;
    private Button btnActualizar;
    private Label lblTotal;

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
        components = new System.ComponentModel.Container();
        lblTitulo = new Label();
        tvCatalogo = new TreeView();
        grpNodo = new GroupBox();
        lblTipoTitulo = new Label();
        lblTipo = new Label();
        lblNombreTitulo = new Label();
        lblNombreValor = new Label();
        lblPrecioTitulo = new Label();
        lblPrecio = new Label();
        txtComposicion = new TextBox();
        grpAlta = new GroupBox();
        lblNuevoNombre = new Label();
        txtNombre = new TextBox();
        lblValor = new Label();
        txtValor = new TextBox();
        rbProducto = new RadioButton();
        rbCaja = new RadioButton();
        btnAgregarDentro = new Button();
        btnAgregarRaiz = new Button();
        btnQuitar = new Button();
        btnActualizar = new Button();
        lblTotal = new Label();
        grpNodo.SuspendLayout();
        grpAlta.SuspendLayout();
        SuspendLayout();

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(16, 12);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(520, 30);
        lblTitulo.Text = "Catálogo de la tienda · Composite";

        tvCatalogo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        tvCatalogo.Font = new Font("Segoe UI", 10F);
        tvCatalogo.HideSelection = false;
        tvCatalogo.Location = new Point(16, 56);
        tvCatalogo.Name = "tvCatalogo";
        tvCatalogo.Size = new Size(500, 540);
        tvCatalogo.TabIndex = 0;
        tvCatalogo.AfterSelect += tvCatalogo_AfterSelect;

        grpNodo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpNodo.Controls.Add(lblTipoTitulo);
        grpNodo.Controls.Add(lblTipo);
        grpNodo.Controls.Add(lblNombreTitulo);
        grpNodo.Controls.Add(lblNombreValor);
        grpNodo.Controls.Add(lblPrecioTitulo);
        grpNodo.Controls.Add(lblPrecio);
        grpNodo.Controls.Add(txtComposicion);
        grpNodo.Location = new Point(532, 56);
        grpNodo.Name = "grpNodo";
        grpNodo.Size = new Size(460, 280);
        grpNodo.TabIndex = 1;
        grpNodo.TabStop = false;
        grpNodo.Text = "Nodo seleccionado";

        lblTipoTitulo.AutoSize = true;
        lblTipoTitulo.Location = new Point(16, 32);
        lblTipoTitulo.Name = "lblTipoTitulo";
        lblTipoTitulo.Size = new Size(28, 15);
        lblTipoTitulo.Text = "Rol";

        lblTipo.AutoSize = true;
        lblTipo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTipo.Location = new Point(120, 30);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(16, 19);
        lblTipo.Text = "—";

        lblNombreTitulo.AutoSize = true;
        lblNombreTitulo.Location = new Point(16, 60);
        lblNombreTitulo.Name = "lblNombreTitulo";
        lblNombreTitulo.Size = new Size(54, 15);
        lblNombreTitulo.Text = "Nombre";

        lblNombreValor.AutoSize = true;
        lblNombreValor.Location = new Point(120, 60);
        lblNombreValor.Name = "lblNombreValor";
        lblNombreValor.Size = new Size(16, 15);
        lblNombreValor.Text = "—";

        lblPrecioTitulo.AutoSize = true;
        lblPrecioTitulo.Location = new Point(16, 92);
        lblPrecioTitulo.Name = "lblPrecioTitulo";
        lblPrecioTitulo.Size = new Size(92, 15);
        lblPrecioTitulo.Text = "Precio()";

        lblPrecio.AutoSize = true;
        lblPrecio.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblPrecio.Location = new Point(120, 84);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(24, 30);
        lblPrecio.Text = "—";

        txtComposicion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtComposicion.Font = new Font("Consolas", 9F);
        txtComposicion.Location = new Point(16, 128);
        txtComposicion.Multiline = true;
        txtComposicion.Name = "txtComposicion";
        txtComposicion.ReadOnly = true;
        txtComposicion.ScrollBars = ScrollBars.Vertical;
        txtComposicion.Size = new Size(428, 136);
        txtComposicion.TabIndex = 2;

        grpAlta.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpAlta.Controls.Add(lblNuevoNombre);
        grpAlta.Controls.Add(txtNombre);
        grpAlta.Controls.Add(lblValor);
        grpAlta.Controls.Add(txtValor);
        grpAlta.Controls.Add(rbProducto);
        grpAlta.Controls.Add(rbCaja);
        grpAlta.Controls.Add(btnAgregarDentro);
        grpAlta.Controls.Add(btnAgregarRaiz);
        grpAlta.Controls.Add(btnQuitar);
        grpAlta.Controls.Add(btnActualizar);
        grpAlta.Location = new Point(532, 348);
        grpAlta.Name = "grpAlta";
        grpAlta.Size = new Size(460, 248);
        grpAlta.TabIndex = 3;
        grpAlta.TabStop = false;
        grpAlta.Text = "Agregar al catálogo";

        lblNuevoNombre.AutoSize = true;
        lblNuevoNombre.Location = new Point(16, 36);
        lblNuevoNombre.Name = "lblNuevoNombre";
        lblNuevoNombre.Size = new Size(54, 15);
        lblNuevoNombre.Text = "Nombre";

        txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNombre.Location = new Point(150, 32);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(294, 23);
        txtNombre.TabIndex = 0;

        lblValor.AutoSize = true;
        lblValor.Location = new Point(16, 72);
        lblValor.Name = "lblValor";
        lblValor.Size = new Size(88, 15);
        lblValor.Text = "Precio unitario";

        txtValor.Location = new Point(150, 68);
        txtValor.Name = "txtValor";
        txtValor.Size = new Size(120, 23);
        txtValor.TabIndex = 1;

        rbProducto.AutoSize = true;
        rbProducto.Checked = true;
        rbProducto.Location = new Point(16, 108);
        rbProducto.Name = "rbProducto";
        rbProducto.Size = new Size(75, 19);
        rbProducto.TabIndex = 2;
        rbProducto.TabStop = true;
        rbProducto.Text = "Producto";
        rbProducto.CheckedChanged += rbTipo_CheckedChanged;

        rbCaja.AutoSize = true;
        rbCaja.Location = new Point(150, 108);
        rbCaja.Name = "rbCaja";
        rbCaja.Size = new Size(130, 19);
        rbCaja.TabIndex = 3;
        rbCaja.Text = "Caja o paquete";
        rbCaja.CheckedChanged += rbTipo_CheckedChanged;

        btnAgregarDentro.Enabled = false;
        btnAgregarDentro.Location = new Point(16, 148);
        btnAgregarDentro.Name = "btnAgregarDentro";
        btnAgregarDentro.Size = new Size(200, 30);
        btnAgregarDentro.TabIndex = 4;
        btnAgregarDentro.Text = "Agregar dentro de la caja";
        btnAgregarDentro.Click += btnAgregarDentro_Click;

        btnAgregarRaiz.Location = new Point(228, 148);
        btnAgregarRaiz.Name = "btnAgregarRaiz";
        btnAgregarRaiz.Size = new Size(160, 30);
        btnAgregarRaiz.TabIndex = 5;
        btnAgregarRaiz.Text = "Agregar en la raíz";
        btnAgregarRaiz.Click += btnAgregarRaiz_Click;

        btnQuitar.Enabled = false;
        btnQuitar.Location = new Point(16, 192);
        btnQuitar.Name = "btnQuitar";
        btnQuitar.Size = new Size(120, 30);
        btnQuitar.TabIndex = 6;
        btnQuitar.Text = "Quitar";
        btnQuitar.Click += btnQuitar_Click;

        btnActualizar.Location = new Point(148, 192);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Size = new Size(120, 30);
        btnActualizar.TabIndex = 7;
        btnActualizar.Text = "Actualizar";
        btnActualizar.Click += btnActualizar_Click;

        lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTotal.Location = new Point(16, 612);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(160, 20);
        lblTotal.Text = "Total del catálogo: —";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1008, 650);
        Controls.Add(lblTitulo);
        Controls.Add(tvCatalogo);
        Controls.Add(grpNodo);
        Controls.Add(grpAlta);
        Controls.Add(lblTotal);
        MinimumSize = new Size(900, 640);
        Name = "FormCatalogo";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Catálogo · patrón Composite";
        Load += FormCatalogo_Load;
        grpNodo.ResumeLayout(false);
        grpNodo.PerformLayout();
        grpAlta.ResumeLayout(false);
        grpAlta.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
