using Catalogo.Modelo;
using Catalogo.Modelo.Repositorios;

namespace Catalogo.Controlador;

public class CatalogoControlador
{
    private readonly IComponenteRepositorio _repositorio = new ComponenteRepositorio();

    public void Inicializar()
    {
        _repositorio.Inicializar();
    }

    public IReadOnlyList<IComponente> ListarRaices()
    {
        return _repositorio.ListarRaices();
    }

    public int AgregarProducto(string nombre, decimal precioUnitario, int? cajaPadreId)
    {
        ValidarNombre(nombre);
        if (precioUnitario < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        ExigirCajaSiHayPadre(cajaPadreId);
        return _repositorio.Agregar(nombre.Trim(), esCaja: false, precioUnitario, null, cajaPadreId);
    }

    public int AgregarCaja(string nombre, decimal costoEmpaque, int? cajaPadreId)
    {
        ValidarNombre(nombre);
        if (costoEmpaque < 0)
        {
            throw new ArgumentException("El costo de empaque no puede ser negativo.");
        }

        ExigirCajaSiHayPadre(cajaPadreId);
        return _repositorio.Agregar(nombre.Trim(), esCaja: true, null, costoEmpaque, cajaPadreId);
    }

    public void Quitar(int id)
    {
        if (_repositorio.ContarHijos(id) > 0)
        {
            throw new InvalidOperationException(
                "Esa caja todavía tiene productos u otras cajas adentro. Quitá primero el contenido.");
        }

        if (_repositorio.Eliminar(id) == 0)
        {
            throw new InvalidOperationException("No se encontró el componente.");
        }
    }

    private void ExigirCajaSiHayPadre(int? padreId)
    {
        if (padreId is null)
        {
            return;
        }

        bool? esCaja = _repositorio.EsCaja(padreId.Value);
        if (esCaja is null)
        {
            throw new InvalidOperationException("No existe la caja elegida.");
        }

        if (esCaja == false)
        {
            throw new InvalidOperationException("Un producto es una hoja: no puede contener otros componentes.");
        }
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.");
        }
    }
}
