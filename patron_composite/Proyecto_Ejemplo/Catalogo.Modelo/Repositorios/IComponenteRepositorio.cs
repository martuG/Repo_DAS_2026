using Catalogo.Modelo;

namespace Catalogo.Modelo.Repositorios;

public interface IComponenteRepositorio
{
    void Inicializar();
    IReadOnlyList<IComponente> ListarRaices();
    int Agregar(string nombre, bool esCaja, decimal? precioUnitario, decimal? costoEmpaque, int? padreId);
    bool? EsCaja(int id);
    int ContarHijos(int id);
    int Eliminar(int id);
}
