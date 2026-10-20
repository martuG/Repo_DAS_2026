using System.Data;
using Microsoft.Data.SqlClient;

namespace CatalogoClientes.Datos;

internal class ClienteDesconectadoDao
{
    private const string ConsultaClientes = "SELECT Id, Nombre, Telefono FROM Clientes";

    public DataTable Cargar()
    {
        using SqlConnection conexion = Conexion.Crear();
        using SqlDataAdapter adaptador = new SqlDataAdapter(ConsultaClientes, conexion);
        adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;

        DataTable tabla = new DataTable("Clientes");
        adaptador.Fill(tabla);
        return tabla;
    }

    public void Agregar(DataTable clientes, string nombre, string telefono)
    {
        DataRow fila = clientes.NewRow();
        fila["Nombre"] = nombre;
        fila["Telefono"] = telefono;
        clientes.Rows.Add(fila);
    }

    public int AgregarLote(DataTable clientes, params (string Nombre, string Telefono)[] lote)
    {
        return AgregarLote(clientes, (IEnumerable<(string Nombre, string Telefono)>)lote);
    }

    public int AgregarLote(DataTable clientes, IEnumerable<(string Nombre, string Telefono)> lote)
    {
        int cantidad = 0;
        foreach ((string nombre, string telefono) in lote)
        {
            string nombreLimpio = nombre.Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio))
            {
                continue;
            }

            Agregar(clientes, nombreLimpio, telefono.Trim());
            cantidad++;
        }

        return cantidad;
    }

    public int AgregarLoteDesdeLineas(DataTable clientes, IEnumerable<string> lineas)
    {
        List<(string Nombre, string Telefono)> lote = new();

        foreach (string linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea))
            {
                continue;
            }

            string[] partes = linea.Split(';', 2);
            string nombre = partes[0].Trim();
            string telefono = partes.Length > 1 ? partes[1].Trim() : string.Empty;
            lote.Add((nombre, telefono));
        }

        return AgregarLote(clientes, lote);
    }

    public bool Modificar(DataTable clientes, int id, string nombre, string telefono)
    {
        DataRow? fila = BuscarFilaActiva(clientes, id);
        if (fila is null)
        {
            return false;
        }

        fila["Nombre"] = nombre;
        fila["Telefono"] = telefono;
        return true;
    }

    public bool Eliminar(DataTable clientes, int id)
    {
        DataRow? fila = BuscarFilaActiva(clientes, id);
        if (fila is null)
        {
            return false;
        }

        fila.Delete();
        return true;
    }

    public int Guardar(DataTable clientes)
    {
        using SqlConnection conexion = Conexion.Crear();
        using SqlDataAdapter adaptador = new SqlDataAdapter(ConsultaClientes, conexion);
        adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        using SqlCommandBuilder constructor = new SqlCommandBuilder(adaptador);

        return adaptador.Update(clientes);
    }

    private static DataRow? BuscarFilaActiva(DataTable clientes, int id)
    {
        DataRow? fila = clientes.Rows.Find(id);
        if (fila is null || fila.RowState == DataRowState.Deleted || fila.RowState == DataRowState.Detached)
        {
            return null;
        }

        return fila;
    }
}
