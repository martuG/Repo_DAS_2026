namespace Catalogo.Modelo;

/// <summary>
/// Pasa de filas con PadreId al árbol de Composite.
/// </summary>
public static class EnsambladorCatalogo
{
    public static IReadOnlyList<IComponente> Armar(IReadOnlyList<RegistroComponente> registros)
    {
        Dictionary<int, IComponente> nodos = new();

        foreach (RegistroComponente registro in registros)
        {
            if (registro.EsCaja)
            {
                nodos[registro.Id] = new Caja
                {
                    Id = registro.Id,
                    Nombre = registro.Nombre,
                    CostoEmpaque = registro.CostoEmpaque
                };
            }
            else
            {
                nodos[registro.Id] = new Producto
                {
                    Id = registro.Id,
                    Nombre = registro.Nombre,
                    PrecioUnitario = registro.PrecioUnitario
                };
            }
        }

        List<IComponente> raices = new();
        foreach (RegistroComponente registro in registros)
        {
            IComponente nodo = nodos[registro.Id];
            if (registro.PadreId is int padreId
                && nodos.TryGetValue(padreId, out IComponente? padre)
                && padre is Caja caja)
            {
                caja.Agregar(nodo);
            }
            else
            {
                raices.Add(nodo);
            }
        }

        return raices;
    }
}
