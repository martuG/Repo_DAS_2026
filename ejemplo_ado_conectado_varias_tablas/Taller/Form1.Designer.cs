namespace Taller;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private TabControl tabPrincipal;
    private TabPage tabClientes;
    private TabPage tabNuevaOrden;
    private TabPage tabConsulta;

    private GroupBox grpCliente;
    private Label lblNombreCliente;
    private Label lblTelefono;
    private TextBox txtNombreCliente;
    private TextBox txtTelefono;
    private Button btnAgregarCliente;
    private Button btnEliminarCliente;
    private Button btnListarClientes;
    private Button btnLimpiarCliente;
    private DataGridView dgvClientes;

    private Label lblClienteOrden;
    private ComboBox cmbCliente;
    private Label lblFechaOrden;
    private Label lblFechaValor;
    private GroupBox grpItem;
    private Label lblDescripcion;
    private Label lblCantidad;
    private Label lblPrecio;
    private TextBox txtDescripcion;
    private TextBox txtCantidad;
    private TextBox txtPrecio;
    private Button btnAgregarItem;
    private Button btnQuitarItem;
    private DataGridView dgvItemsNuevos;
    private Label lblTotalOrden;
    private Button btnGuardarOrden;

    private DataGridView dgvOrdenes;
    private Label lblItemsConsulta;
    private DataGridView dgvItemsConsulta;
    private Button btnCerrarOrden;
    private Button btnActualizarConsulta;

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
        tabPrincipal = new TabControl();
        tabClientes = new TabPage();
        tabNuevaOrden = new TabPage();
        tabConsulta = new TabPage();

        grpCliente = new GroupBox();
        lblNombreCliente = new Label();
        txtNombreCliente = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        btnAgregarCliente = new Button();
        btnEliminarCliente = new Button();
        btnListarClientes = new Button();
        btnLimpiarCliente = new Button();
        dgvClientes = new DataGridView();

        lblClienteOrden = new Label();
        cmbCliente = new ComboBox();
        lblFechaOrden = new Label();
        lblFechaValor = new Label();
        grpItem = new GroupBox();
        lblDescripcion = new Label();
        txtDescripcion = new TextBox();
        lblCantidad = new Label();
        txtCantidad = new TextBox();
        lblPrecio = new Label();
        txtPrecio = new TextBox();
        btnAgregarItem = new Button();
        btnQuitarItem = new Button();
        dgvItemsNuevos = new DataGridView();
        lblTotalOrden = new Label();
        btnGuardarOrden = new Button();

        dgvOrdenes = new DataGridView();
        lblItemsConsulta = new Label();
        dgvItemsConsulta = new DataGridView();
        btnCerrarOrden = new Button();
        btnActualizarConsulta = new Button();

        tabPrincipal.SuspendLayout();
        tabClientes.SuspendLayout();
        tabNuevaOrden.SuspendLayout();
        tabConsulta.SuspendLayout();
        grpCliente.SuspendLayout();
        grpItem.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvItemsNuevos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvItemsConsulta).BeginInit();
        SuspendLayout();

        tabPrincipal.Controls.Add(tabClientes);
        tabPrincipal.Controls.Add(tabNuevaOrden);
        tabPrincipal.Controls.Add(tabConsulta);
        tabPrincipal.Dock = DockStyle.Fill;
        tabPrincipal.Location = new Point(0, 0);
        tabPrincipal.Name = "tabPrincipal";
        tabPrincipal.SelectedIndex = 0;
        tabPrincipal.Size = new Size(944, 621);
        tabPrincipal.TabIndex = 0;
        tabPrincipal.SelectedIndexChanged += tabPrincipal_SelectedIndexChanged;

        tabClientes.Controls.Add(grpCliente);
        tabClientes.Controls.Add(btnAgregarCliente);
        tabClientes.Controls.Add(btnEliminarCliente);
        tabClientes.Controls.Add(btnListarClientes);
        tabClientes.Controls.Add(btnLimpiarCliente);
        tabClientes.Controls.Add(dgvClientes);
        tabClientes.Location = new Point(4, 24);
        tabClientes.Name = "tabClientes";
        tabClientes.Padding = new Padding(12);
        tabClientes.Size = new Size(936, 593);
        tabClientes.TabIndex = 0;
        tabClientes.Text = "Clientes";
        tabClientes.UseVisualStyleBackColor = true;

        grpCliente.Controls.Add(lblNombreCliente);
        grpCliente.Controls.Add(txtNombreCliente);
        grpCliente.Controls.Add(lblTelefono);
        grpCliente.Controls.Add(txtTelefono);
        grpCliente.Location = new Point(16, 16);
        grpCliente.Name = "grpCliente";
        grpCliente.Size = new Size(896, 100);
        grpCliente.TabIndex = 0;
        grpCliente.TabStop = false;
        grpCliente.Text = "Datos del cliente";

        lblNombreCliente.AutoSize = true;
        lblNombreCliente.Location = new Point(18, 32);
        lblNombreCliente.Name = "lblNombreCliente";
        lblNombreCliente.Size = new Size(51, 15);
        lblNombreCliente.Text = "Nombre";

        txtNombreCliente.Location = new Point(18, 50);
        txtNombreCliente.Name = "txtNombreCliente";
        txtNombreCliente.Size = new Size(320, 23);
        txtNombreCliente.TabIndex = 0;

        lblTelefono.AutoSize = true;
        lblTelefono.Location = new Point(360, 32);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(52, 15);
        lblTelefono.Text = "Teléfono";

        txtTelefono.Location = new Point(360, 50);
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(200, 23);
        txtTelefono.TabIndex = 1;

        btnAgregarCliente.Location = new Point(16, 128);
        btnAgregarCliente.Name = "btnAgregarCliente";
        btnAgregarCliente.Size = new Size(110, 32);
        btnAgregarCliente.TabIndex = 1;
        btnAgregarCliente.Text = "Agregar";
        btnAgregarCliente.UseVisualStyleBackColor = true;
        btnAgregarCliente.Click += btnAgregarCliente_Click;

        btnEliminarCliente.Location = new Point(140, 128);
        btnEliminarCliente.Name = "btnEliminarCliente";
        btnEliminarCliente.Size = new Size(110, 32);
        btnEliminarCliente.TabIndex = 2;
        btnEliminarCliente.Text = "Eliminar";
        btnEliminarCliente.UseVisualStyleBackColor = true;
        btnEliminarCliente.Click += btnEliminarCliente_Click;

        btnListarClientes.Location = new Point(264, 128);
        btnListarClientes.Name = "btnListarClientes";
        btnListarClientes.Size = new Size(110, 32);
        btnListarClientes.TabIndex = 3;
        btnListarClientes.Text = "Listar";
        btnListarClientes.UseVisualStyleBackColor = true;
        btnListarClientes.Click += btnListarClientes_Click;

        btnLimpiarCliente.Location = new Point(388, 128);
        btnLimpiarCliente.Name = "btnLimpiarCliente";
        btnLimpiarCliente.Size = new Size(110, 32);
        btnLimpiarCliente.TabIndex = 4;
        btnLimpiarCliente.Text = "Limpiar";
        btnLimpiarCliente.UseVisualStyleBackColor = true;
        btnLimpiarCliente.Click += btnLimpiarCliente_Click;

        ConfigurarGrilla(dgvClientes);
        dgvClientes.Location = new Point(16, 176);
        dgvClientes.Name = "dgvClientes";
        dgvClientes.Size = new Size(896, 388);
        dgvClientes.TabIndex = 5;
        dgvClientes.SelectionChanged += dgvClientes_SelectionChanged;

        tabNuevaOrden.Controls.Add(lblClienteOrden);
        tabNuevaOrden.Controls.Add(cmbCliente);
        tabNuevaOrden.Controls.Add(lblFechaOrden);
        tabNuevaOrden.Controls.Add(lblFechaValor);
        tabNuevaOrden.Controls.Add(grpItem);
        tabNuevaOrden.Controls.Add(dgvItemsNuevos);
        tabNuevaOrden.Controls.Add(lblTotalOrden);
        tabNuevaOrden.Controls.Add(btnGuardarOrden);
        tabNuevaOrden.Location = new Point(4, 24);
        tabNuevaOrden.Name = "tabNuevaOrden";
        tabNuevaOrden.Padding = new Padding(12);
        tabNuevaOrden.Size = new Size(936, 593);
        tabNuevaOrden.TabIndex = 1;
        tabNuevaOrden.Text = "Nueva orden";
        tabNuevaOrden.UseVisualStyleBackColor = true;

        lblClienteOrden.AutoSize = true;
        lblClienteOrden.Location = new Point(16, 16);
        lblClienteOrden.Name = "lblClienteOrden";
        lblClienteOrden.Size = new Size(44, 15);
        lblClienteOrden.Text = "Cliente";

        cmbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCliente.Location = new Point(16, 36);
        cmbCliente.Name = "cmbCliente";
        cmbCliente.Size = new Size(320, 23);
        cmbCliente.TabIndex = 0;

        lblFechaOrden.AutoSize = true;
        lblFechaOrden.Location = new Point(360, 16);
        lblFechaOrden.Name = "lblFechaOrden";
        lblFechaOrden.Size = new Size(38, 15);
        lblFechaOrden.Text = "Fecha";

        lblFechaValor.AutoSize = true;
        lblFechaValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFechaValor.Location = new Point(360, 40);
        lblFechaValor.Name = "lblFechaValor";
        lblFechaValor.Size = new Size(80, 15);
        lblFechaValor.Text = "hoy";

        grpItem.Controls.Add(lblDescripcion);
        grpItem.Controls.Add(txtDescripcion);
        grpItem.Controls.Add(lblCantidad);
        grpItem.Controls.Add(txtCantidad);
        grpItem.Controls.Add(lblPrecio);
        grpItem.Controls.Add(txtPrecio);
        grpItem.Controls.Add(btnAgregarItem);
        grpItem.Controls.Add(btnQuitarItem);
        grpItem.Location = new Point(16, 76);
        grpItem.Name = "grpItem";
        grpItem.Size = new Size(896, 108);
        grpItem.TabIndex = 1;
        grpItem.TabStop = false;
        grpItem.Text = "Ítem (repuesto o tarea)";

        lblDescripcion.AutoSize = true;
        lblDescripcion.Location = new Point(18, 28);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(69, 15);
        lblDescripcion.Text = "Descripción";

        txtDescripcion.Location = new Point(18, 46);
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(280, 23);
        txtDescripcion.TabIndex = 0;

        lblCantidad.AutoSize = true;
        lblCantidad.Location = new Point(316, 28);
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Size = new Size(55, 15);
        lblCantidad.Text = "Cantidad";

        txtCantidad.Location = new Point(316, 46);
        txtCantidad.Name = "txtCantidad";
        txtCantidad.Size = new Size(90, 23);
        txtCantidad.TabIndex = 1;

        lblPrecio.AutoSize = true;
        lblPrecio.Location = new Point(424, 28);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(86, 15);
        lblPrecio.Text = "Precio unitario";

        txtPrecio.Location = new Point(424, 46);
        txtPrecio.Name = "txtPrecio";
        txtPrecio.Size = new Size(110, 23);
        txtPrecio.TabIndex = 2;

        btnAgregarItem.Location = new Point(556, 45);
        btnAgregarItem.Name = "btnAgregarItem";
        btnAgregarItem.Size = new Size(120, 27);
        btnAgregarItem.TabIndex = 3;
        btnAgregarItem.Text = "Agregar ítem";
        btnAgregarItem.UseVisualStyleBackColor = true;
        btnAgregarItem.Click += btnAgregarItem_Click;

        btnQuitarItem.Location = new Point(688, 45);
        btnQuitarItem.Name = "btnQuitarItem";
        btnQuitarItem.Size = new Size(120, 27);
        btnQuitarItem.TabIndex = 4;
        btnQuitarItem.Text = "Quitar ítem";
        btnQuitarItem.UseVisualStyleBackColor = true;
        btnQuitarItem.Click += btnQuitarItem_Click;

        ConfigurarGrilla(dgvItemsNuevos);
        dgvItemsNuevos.Location = new Point(16, 200);
        dgvItemsNuevos.Name = "dgvItemsNuevos";
        dgvItemsNuevos.Size = new Size(896, 300);
        dgvItemsNuevos.TabIndex = 2;

        lblTotalOrden.AutoSize = true;
        lblTotalOrden.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTotalOrden.Location = new Point(16, 516);
        lblTotalOrden.Name = "lblTotalOrden";
        lblTotalOrden.Size = new Size(90, 21);
        lblTotalOrden.Text = "Total: 0,00";

        btnGuardarOrden.Location = new Point(756, 512);
        btnGuardarOrden.Name = "btnGuardarOrden";
        btnGuardarOrden.Size = new Size(156, 32);
        btnGuardarOrden.TabIndex = 3;
        btnGuardarOrden.Text = "Guardar orden";
        btnGuardarOrden.UseVisualStyleBackColor = true;
        btnGuardarOrden.Click += btnGuardarOrden_Click;

        tabConsulta.Controls.Add(dgvOrdenes);
        tabConsulta.Controls.Add(lblItemsConsulta);
        tabConsulta.Controls.Add(dgvItemsConsulta);
        tabConsulta.Controls.Add(btnCerrarOrden);
        tabConsulta.Controls.Add(btnActualizarConsulta);
        tabConsulta.Location = new Point(4, 24);
        tabConsulta.Name = "tabConsulta";
        tabConsulta.Padding = new Padding(12);
        tabConsulta.Size = new Size(936, 593);
        tabConsulta.TabIndex = 2;
        tabConsulta.Text = "Consulta";
        tabConsulta.UseVisualStyleBackColor = true;

        ConfigurarGrilla(dgvOrdenes);
        dgvOrdenes.Location = new Point(16, 16);
        dgvOrdenes.Name = "dgvOrdenes";
        dgvOrdenes.Size = new Size(896, 240);
        dgvOrdenes.TabIndex = 0;
        dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;

        lblItemsConsulta.AutoSize = true;
        lblItemsConsulta.Location = new Point(16, 268);
        lblItemsConsulta.Name = "lblItemsConsulta";
        lblItemsConsulta.Size = new Size(148, 15);
        lblItemsConsulta.Text = "Ítems de la orden (solo lectura)";

        ConfigurarGrilla(dgvItemsConsulta);
        dgvItemsConsulta.Location = new Point(16, 288);
        dgvItemsConsulta.Name = "dgvItemsConsulta";
        dgvItemsConsulta.Size = new Size(896, 216);
        dgvItemsConsulta.TabIndex = 1;

        btnCerrarOrden.Location = new Point(16, 520);
        btnCerrarOrden.Name = "btnCerrarOrden";
        btnCerrarOrden.Size = new Size(140, 32);
        btnCerrarOrden.TabIndex = 2;
        btnCerrarOrden.Text = "Cerrar orden";
        btnCerrarOrden.UseVisualStyleBackColor = true;
        btnCerrarOrden.Click += btnCerrarOrden_Click;

        btnActualizarConsulta.Location = new Point(168, 520);
        btnActualizarConsulta.Name = "btnActualizarConsulta";
        btnActualizarConsulta.Size = new Size(110, 32);
        btnActualizarConsulta.TabIndex = 3;
        btnActualizarConsulta.Text = "Actualizar";
        btnActualizarConsulta.UseVisualStyleBackColor = true;
        btnActualizarConsulta.Click += btnActualizarConsulta_Click;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(944, 621);
        Controls.Add(tabPrincipal);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Taller - Órdenes de trabajo";
        Load += Form1_Load;

        tabPrincipal.ResumeLayout(false);
        tabClientes.ResumeLayout(false);
        tabNuevaOrden.ResumeLayout(false);
        tabNuevaOrden.PerformLayout();
        tabConsulta.ResumeLayout(false);
        tabConsulta.PerformLayout();
        grpCliente.ResumeLayout(false);
        grpCliente.PerformLayout();
        grpItem.ResumeLayout(false);
        grpItem.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvItemsNuevos).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvItemsConsulta).EndInit();
        ResumeLayout(false);
    }

    private static void ConfigurarGrilla(DataGridView grilla)
    {
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grilla.MultiSelect = false;
        grilla.ReadOnly = true;
        grilla.RowHeadersVisible = false;
        grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }
}
