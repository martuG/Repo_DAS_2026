using System.Globalization;
using Catalogo.Controlador;
using Catalogo.Modelo;

namespace Catalogo.Vista;

public partial class FormCatalogo : Form
{
    private readonly CatalogoControlador _controlador = new();

    public FormCatalogo()
    {
        InitializeComponent();
    }

    private void FormCatalogo_Load(object sender, EventArgs e)
    {
        try
        {
            _controlador.Inicializar();
            CargarArbol(null);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo conectar a SQL Server LocalDB.\n" +
                "Verificá que esté instalado o ajustá la cadena en Catalogo.Modelo/Datos/Conexion.cs.\n\n" +
                ex.Message,
                "Error de conexión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnActualizar_Click(object sender, EventArgs e)
    {
        Ejecutar(() => CargarArbol(IdSeleccionado()));
    }

    private void btnAgregarDentro_Click(object sender, EventArgs e)
    {
        if (tvCatalogo.SelectedNode?.Tag is not Caja caja)
        {
            MessageBox.Show(
                "Seleccioná una caja. Un producto es una hoja y no puede contener otros ítems.",
                "Agregar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        Agregar(caja.Id);
    }

    private void btnAgregarRaiz_Click(object sender, EventArgs e)
    {
        Agregar(null);
    }

    private void btnQuitar_Click(object sender, EventArgs e)
    {
        if (tvCatalogo.SelectedNode?.Tag is not IComponente componente)
        {
            MessageBox.Show("Seleccioná un componente del árbol.", "Quitar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult confirmacion = MessageBox.Show(
            $"¿Quitar «{componente.Nombre}» del catálogo?",
            "Confirmar",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        Ejecutar(() =>
        {
            _controlador.Quitar(componente.Id);
            CargarArbol(null);
            LimpiarDetalle();
        });
    }

    private void tvCatalogo_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not IComponente componente)
        {
            LimpiarDetalle();
            return;
        }

        bool esCaja = componente is Caja;
        lblTipo.Text = esCaja ? "Contenedor (caja)" : "Hoja (producto)";
        lblNombreValor.Text = componente.Nombre;
        lblPrecio.Text = Formatear(componente.Precio());
        txtComposicion.Text = Describir(componente);
        btnAgregarDentro.Enabled = esCaja;
        btnQuitar.Enabled = true;
    }

    private void rbTipo_CheckedChanged(object sender, EventArgs e)
    {
        lblValor.Text = rbCaja.Checked ? "Costo de empaque" : "Precio unitario";
    }

    private void Agregar(int? cajaPadreId)
    {
        string nombre = txtNombre.Text;
        if (!TryLeerDecimal(txtValor.Text, out decimal valor))
        {
            MessageBox.Show("Ingresá un número válido para el precio o el empaque.", "Agregar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Ejecutar(() =>
        {
            int id = rbCaja.Checked
                ? _controlador.AgregarCaja(nombre, valor, cajaPadreId)
                : _controlador.AgregarProducto(nombre, valor, cajaPadreId);

            txtNombre.Clear();
            txtValor.Clear();
            CargarArbol(id);
        });
    }

    private void CargarArbol(int? idASeleccionar)
    {
        IReadOnlyList<IComponente> raices = _controlador.ListarRaices();

        tvCatalogo.BeginUpdate();
        tvCatalogo.Nodes.Clear();
        foreach (IComponente raiz in raices)
        {
            tvCatalogo.Nodes.Add(CrearNodo(raiz));
        }

        tvCatalogo.ExpandAll();
        tvCatalogo.EndUpdate();

        decimal total = 0;
        foreach (IComponente raiz in raices)
        {
            total += raiz.Precio();
        }

        lblTotal.Text = "Total del catálogo: " + Formatear(total) + "   (cada raíz responde Precio())";

        if (idASeleccionar is int id)
        {
            Seleccionar(id);
        }
        else
        {
            LimpiarDetalle();
        }
    }

    private TreeNode CrearNodo(IComponente componente)
    {
        string texto = componente is Caja
            ? $"Caja · {componente.Nombre}    total {Formatear(componente.Precio())}"
            : $"Producto · {componente.Nombre}    {Formatear(componente.Precio())}";

        TreeNode nodo = new TreeNode(texto) { Tag = componente };
        if (componente is Caja caja)
        {
            foreach (IComponente hijo in caja.Hijos)
            {
                nodo.Nodes.Add(CrearNodo(hijo));
            }
        }

        return nodo;
    }

    private static string Describir(IComponente componente)
    {
        if (componente is Producto producto)
        {
            return "Hoja: no tiene hijos." + Environment.NewLine
                + "Precio() devuelve el precio unitario " + Formatear(producto.PrecioUnitario) + ".";
        }

        if (componente is Caja caja)
        {
            List<string> lineas = new()
            {
                "Contenedor: Precio() pide el precio a cada hijo y suma el empaque.",
                "  empaque " + Formatear(caja.CostoEmpaque)
            };

            if (caja.Hijos.Count == 0)
            {
                lineas.Add("  (sin hijos)");
            }

            foreach (IComponente hijo in caja.Hijos)
            {
                lineas.Add("  + " + hijo.Nombre + "  " + Formatear(hijo.Precio()));
            }

            lineas.Add("= " + Formatear(caja.Precio()));
            return string.Join(Environment.NewLine, lineas);
        }

        return string.Empty;
    }

    private void Seleccionar(int id)
    {
        TreeNode? nodo = BuscarNodo(tvCatalogo.Nodes, id);
        if (nodo is null)
        {
            return;
        }

        tvCatalogo.SelectedNode = nodo;
        nodo.EnsureVisible();
    }

    private static TreeNode? BuscarNodo(TreeNodeCollection nodos, int id)
    {
        foreach (TreeNode nodo in nodos)
        {
            if (nodo.Tag is IComponente componente && componente.Id == id)
            {
                return nodo;
            }

            TreeNode? hijo = BuscarNodo(nodo.Nodes, id);
            if (hijo is not null)
            {
                return hijo;
            }
        }

        return null;
    }

    private int? IdSeleccionado()
    {
        return (tvCatalogo.SelectedNode?.Tag as IComponente)?.Id;
    }

    private void LimpiarDetalle()
    {
        lblTipo.Text = "—";
        lblNombreValor.Text = "—";
        lblPrecio.Text = "—";
        txtComposicion.Clear();
        btnAgregarDentro.Enabled = false;
        btnQuitar.Enabled = false;
    }

    private void Ejecutar(Action accion)
    {
        try
        {
            accion();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Catálogo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool TryLeerDecimal(string texto, out decimal valor)
    {
        if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out valor))
        {
            return true;
        }

        return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
    }

    private static string Formatear(decimal valor)
    {
        return "$ " + valor.ToString("N2");
    }
}
