using FacturacionSimple.Modelo.Datos;
using FacturacionSimple.Modelo.Entidades;

namespace FacturacionSimple.Controladora;

public class ProductoControladora
{
    private readonly ProductoDatos _datos = new();

    public List<Producto> Listar(string? filtro = null) => _datos.Listar(filtro);

    public List<Producto> ListarActivos() => _datos.ListarActivos();

    public Producto? Obtener(int id) => _datos.ObtenerPorId(id);

    public void GuardarNuevo(string codigo, string nombre, decimal precio)
    {
        Validar(codigo, nombre, precio);

        if (_datos.ExisteCodigo(codigo.Trim()))
            throw new InvalidOperationException("Ya existe un producto con ese código.");

        _datos.Agregar(new Producto
        {
            Codigo = codigo.Trim(),
            Nombre = nombre.Trim(),
            Precio = precio,
            Activo = true
        });
    }

    public void Modificar(int id, string codigo, string nombre, decimal precio, bool activo)
    {
        Validar(codigo, nombre, precio);

        if (_datos.ExisteCodigo(codigo.Trim(), id))
            throw new InvalidOperationException("Ya existe otro producto con ese código.");

        _datos.Modificar(new Producto
        {
            Id = id,
            Codigo = codigo.Trim(),
            Nombre = nombre.Trim(),
            Precio = precio,
            Activo = activo
        });
    }

    public void DarDeBaja(int id) => _datos.BajaLogica(id);

    public void Eliminar(int id)
    {
        if (_datos.EstaReferenciado(id))
            throw new InvalidOperationException(
                "El producto está referenciado en facturas. Use baja lógica (Activo = false).");

        _datos.EliminarFisico(id);
    }

    private static void Validar(string codigo, string nombre, decimal precio)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new InvalidOperationException("El código es obligatorio.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new InvalidOperationException("El nombre es obligatorio.");

        if (precio < 0)
            throw new InvalidOperationException("El precio no puede ser negativo.");
    }
}
