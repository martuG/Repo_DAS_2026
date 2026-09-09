# Lenguajes de Última Generación

## Unidad 2 — Acceso a datos: modo desconectado (ADO.NET)

### Tecnologías de referencia

- **.NET 8** — plataforma de desarrollo y ejecución
- **C# 12** — lenguaje asociado a .NET 8
- **ADO.NET** — *ActiveX Data Objects .NET*: API de acceso a datos de .NET
- **Microsoft.Data.SqlClient** — proveedor de datos para SQL Server (paquete NuGet)
- **Windows Forms** — interfaz de escritorio usada en los prácticos del curso
- **SQL Server / LocalDB** — motor de base de datos

> Este apunte actualiza el material original de **Mauricio Prinzo** (*Lenguajes de Última Generación — Unidad 2 / Acceso a datos*) a **.NET 8**. Se conservan los conceptos de la clase y se corrigen APIs, ejemplos y afirmaciones que ya no aplican.

---

## Índice

1. [Presentación](#1-presentación)
2. [Entorno desconectado](#2-entorno-desconectado)
3. [Modo desconectado](#3-modo-desconectado)
4. [Arquitectura ADO.NET](#4-arquitectura-adonet)
5. [Objeto DataSet](#5-objeto-dataset)
6. [DataTable, DataColumn y DataRow](#6-datatable-datacolumn-y-datarow)
7. [Crear un DataSet en memoria](#7-crear-un-dataset-en-memoria)
8. [Listas de objetos frente a DataSet](#9-listas-de-objetos-frente-a-dataset)
9. [DataRelation](#10-datarelation)
10. [Objeto SqlDataAdapter](#11-objeto-sqldataadapter)
11. [Fill y Update](#12-fill-y-update)
12. [SqlCommandBuilder](#13-sqlcommandbuilder)
13. [RowState y versiones de fila](#14-rowstate-y-versiones-de-fila)
14. [El problema de la sincronización](#15-el-problema-de-la-sincronización)
15. [DataView](#16-dataview)
16. [Qué cambió respecto del material original](#17-qué-cambió-respecto-del-material-original)
17. [Ejemplo integrador](#18-ejemplo-integrador)
18. [Conceptos clave](#19-conceptos-clave)
19. [Actividad práctica](#20-actividad-práctica)
20. [Glosario de siglas](#21-glosario-de-siglas)
21. [Referencias](#22-referencias)

---

# 1. Presentación

En modo **conectado**, la aplicación mantiene un canal abierto con el motor de base de datos mientras lee o escribe (`SqlConnection` + `SqlCommand` + `SqlDataReader`).

En modo **desconectado**, la aplicación:

1. Abre la conexión el tiempo mínimo necesario.
2. Copia los datos a memoria.
3. Cierra la conexión.
4. Trabaja sobre esa copia (consultas, altas, bajas, modificaciones).
5. Si hay cambios, vuelve a conectar para persistirlos.

El objeto central de esta copia en memoria es el **DataSet**. El puente entre la base de datos y esa copia es el **DataAdapter** (`SqlDataAdapter` si el proveedor es SQL Server).

---

# 2. Entorno desconectado

Un entorno desconectado es aquel en el que **los datos pueden modificarse de forma independiente** y **los cambios se escriben después** en la base de datos.

### Ventajas

- Las conexiones se usan durante el **menor tiempo posible**.
- Menos conexiones pueden atender a **más usuarios**.
- Mejora la **escalabilidad** y, en muchos escenarios de lectura, el **rendimiento** percibido (la UI trabaja contra memoria, no contra la red).

### Inconvenientes

- Los datos **no siempre están actualizados** respecto de la base.
- Pueden producirse **conflictos de cambios** que hay que detectar (y, si el diseño lo pide, resolver).

```text
Usuario A                    Usuario B
    │                            │
    │  Fill (copia en memoria)   │  Fill (otra copia)
    │                            │
    │  modifica en memoria       │  modifica en memoria
    │                            │
    │  Update ──────────────────►│  Update ──► ¿conflicto?
    └──────────► Base de datos ◄─┘
```

---

# 3. Modo desconectado

Estamos en un entorno desconectado cuando el modelo de datos **se conecta para obtener los datos** y luego **puede desconectarse sin perderlos**.

El esquema típico es:

```text
SQL Server
    │
    │  Fill()          ← abre, lee, cierra
    ▼
SqlDataAdapter
    │
    ▼
DataSet / DataTable     ← trabajo en memoria (sin conexión)
    │
    │  Update()        ← abre, escribe, cierra
    ▼
SQL Server
```

Uno de los principales problemas es la **sincronización**. Si dos usuarios se llevan una copia, modifican por separado y después actualizan la base, no es obvio cuál es el valor definitivo.

Ejemplo clásico de la clase:

- El precio en la base es **$10**.
- El usuario A lo cambia a **$12**.
- El usuario B lo cambia a **$14**.
- Ambos hacen `Update`.

¿Cuál queda? Depende de **quién escribe último** y de **si el** `UPDATE` **comprueba el valor original**. En esta asignatura **no se resuelve un sistema completo de conflictos**, pero hay que saber que el problema existe. 

Más adelante se muestra cómo ADO.NET lo detecta (`DBConcurrencyException`).

---

# 4. Arquitectura [ADO.NET](http://ADO.NET)

ADO.NET se organiza en dos grupos de objetos:

### Objetos conectados (proveedor)

Pertenecen al proveedor concreto. En este curso, **Microsoft.Data.SqlClient**:


| Objeto              | Rol                                             |
| ------------------- | ----------------------------------------------- |
| `SqlConnection`     | Canal con SQL Server                            |
| `SqlCommand`        | Instrucción SQL o procedimiento                 |
| `SqlDataReader`     | Lectura secuencial, adelante, solo lectura      |
| `SqlTransaction`    | Unidad de trabajo en el servidor                |
| `SqlDataAdapter`    | Puente Fill / Update hacia un DataSet           |
| `SqlCommandBuilder` | Genera INSERT/UPDATE/DELETE a partir del SELECT |


### Objetos desconectados (independientes del proveedor)

Viven en `System.Data` (biblioteca base de .NET). **No** dependen de SQL Server:


| Objeto         | Rol                                                |
| -------------- | -------------------------------------------------- |
| `DataSet`      | Contenedor en memoria (varias tablas + relaciones) |
| `DataTable`    | Tabla en memoria                                   |
| `DataColumn`   | Definición de una columna                          |
| `DataRow`      | Una fila                                           |
| `DataRelation` | Relación padre–hijo entre tablas                   |
| `DataView`     | Vista ordenada / filtrada sobre un `DataTable`     |
| `Constraint`   | PK, única, FK en memoria                           |


> **Corrección al material original.** El apunte decía que *“el DataSet no forma parte del modelo ADO.NET”*. En la arquitectura oficial, el **DataSet sí es parte de ADO.NET**: es el núcleo del modelo desconectado. Lo que es cierto es que **no forma parte del proveedor** (`Microsoft.Data.SqlClient`). 
>
> El DataSet es **agnóstico del motor**: el mismo objeto sirve para SQL Server, otra base o incluso para datos que nunca salieron de un archivo XML.

Para usar SQL Server en .NET 8:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Data.SqlClient" Version="7.0.2" />
</ItemGroup>
```

```csharp
using System.Data;
using Microsoft.Data.SqlClient;
```

En todos los ejemplos se usa **una sola cadena de conexión**:

```csharp
private const string CadenaConexion =
    @"Server=(localdb)\mssqllocaldb;Database=TallerDAS;Integrated Security=true;TrustServerCertificate=true;";
```

---

# 5. Objeto DataSet

El `DataSet` funciona como una **base de datos temporal en memoria**.

```text
DataSet
│
├── DataTable "Clientes"
│     ├── DataColumn Id, Nombre, Telefono
│     └── DataRow, DataRow, DataRow...
│
├── DataTable "Ordenes"
│     ├── DataColumn Id, ClienteId, Fecha, Estado
│     └── DataRow...
│
└── DataRelation "FK_Ordenes_Clientes"
      Clientes.Id  →  Ordenes.ClienteId
```

Contiene:

- `DataTable`: tablas vinculadas al modelo (o creadas solo en memoria).
- `DataRelation`: relaciones entre esas tablas.

A su vez, cada `DataTable` está formado por:

- `DataRow`: filas.
- `DataColumn`: columnas (nombre, tipo, longitud, etc.).

La ventaja práctica es que está formado por **colecciones** que se recorren con `foreach`:

```csharp
foreach (DataRow fila in ds.Tables[0].Rows)
{
    int id = (int)fila["Id"];
    string nombre = fila["Nombre"].ToString() ?? string.Empty;
}
```

También se puede acceder por índice de columna: `fila[0]`. Por nombre es más claro y menos frágil.

Un DataSet **puede crearse desvinculado de la base de datos**: no hace falta SQL Server para tener tablas, filas y relaciones en memoria. Eso sirve para prototipos, caches locales o datos que luego se serializan a XML (`WriteXml` / `ReadXml`).

---

# 6. DataTable, DataColumn y DataRow

### DataColumn

Describe una columna: nombre, longitud máxima, si admite nulos, si es autoincremental.

```csharp
DataColumn colNombre = new DataColumn("Nombre", typeof(string));
colNombre.MaxLength = 50;
colNombre.AllowDBNull = false;
```

### DataRow

Una fila se **crea** con `NewRow()` (así respeta el esquema de columnas). No se hace `new DataRow()`.

```csharp
DataRow fila = tabla.NewRow();
fila["Id"] = 1;
fila["Nombre"] = "Juan";
tabla.Rows.Add(fila);
```

Valores nulos de base de datos se representan con `DBNull.Value`, no con `null` de C# en todas las columnas.

### DataTable

Además de columnas y filas, admite:

- `PrimaryKey`
- restricciones (`UniqueConstraint`, `ForeignKeyConstraint`)
- `Select("Estado = 'Abierta'")` para filtrar en memoria

---

# 7. Crear un DataSet en memoria

El material original muestra cómo armar un DataSet **sin** base de datos. Los pasos siguen vigentes en .NET 8:

```csharp
DataSet ds = new DataSet("Personas");

DataTable dt = new DataTable("Persona");

DataColumn colId = new DataColumn("Id", typeof(int));
DataColumn colNombre = new DataColumn("Nombre", typeof(string));
colNombre.MaxLength = 50;

dt.Columns.Add(colId);
dt.Columns.Add(colNombre);

dt.PrimaryKey = new DataColumn[] { colId };

DataRow dr = dt.NewRow();
dr["Id"] = 1;
dr["Nombre"] = "Juan";
dt.Rows.Add(dr);

dr = dt.NewRow();
dr["Id"] = 2;
dr["Nombre"] = "Pedro";
dt.Rows.Add(dr);

ds.Tables.Add(dt);
```

### Pasos (resumen de la clase)

1. Crear un objeto `DataSet`.
2. Crear un `DataTable`.
3. Crear los `DataColumn` necesarios (nombre del campo y tipo).
4. Agregar las columnas al `DataTable`.
5. Completar la clave primaria (`PrimaryKey`) con las columnas que la componen.
6. Agregar el `DataTable` al `DataSet`.
7. Si existen, crear las **relaciones** (`DataRelation`).

Hasta el paso 6 ya se puede enlazar un combo o una grilla. El paso 7 hace falta cuando hay más de una tabla relacionada.

---

# 8. Listas de objetos frente a DataSet

El material original lo adelanta: cuando se trabaja orientado a objetos, a la capa de presentación se le pasan **listas de entidades**, no un DataSet.

```text
Modo desconectado “clásico”
  SqlDataAdapter.Fill → DataSet → ComboBox / DataGridView

Modo conectado del curso (DAO + entidades)
  SqlDataReader → List<Cliente> → ComboBox / DataGridView
```

Las dos formas son válidas. En los prácticos de **modo conectado** se mapea a objetos (`Cliente`, `Orden`, etc.).

En **modo desconectado** el `DataSet` *es* el modelo en memoria.


|                  | DataSet                              | Lista de objetos             |
| ---------------- | ------------------------------------ | ---------------------------- |
| Esquema          | Columnas dinámicas                   | Propiedades de una clase     |
| Relaciones       | `DataRelation`                       | Referencias entre objetos    |
| Cambio de estado | `RowState`                           | Lo define el programador     |
| Enlace WinForms  | Directo                              | Directo (`List<T>`)          |
| Tipado           | Por nombre de columna (`fila["Id"]`) | Por propiedad (`cliente.Id`) |


Ninguna reemplaza a la otra en todos los casos. El DataSet es cómodo para grillas editables y `Update` masivo. Las entidades son más claras en reglas de negocio y en capas.

---

# 9. DataRelation

Un DataSet con dos tablas no “conoce” la FK del servidor hasta que se la definimos. En memoria se declara así:

```csharp
DataRelation relacion = new DataRelation(
    "FK_Ordenes_Clientes",
    ds.Tables["Clientes"].Columns["Id"],
    ds.Tables["Ordenes"].Columns["ClienteId"]);

ds.Relations.Add(relacion);
```

Después se navega sin volver a SQL:

```csharp
DataRow cliente = ds.Tables["Clientes"].Rows.Find(1);

foreach (DataRow orden in cliente.GetChildRows("FK_Ordenes_Clientes"))
{
    DateTime fecha = (DateTime)orden["Fecha"];
}
```

---

# 10. Objeto SqlDataAdapter

El `SqlDataAdapter` es el **nexo** entre el proveedor ADO.NET (`SqlConnection` / `SqlCommand`) y el `DataSet`.

Hay un DataAdapter **por proveedor**:


| Proveedor (NuGet)          | Adapter             | Uso típico     |
| -------------------------- | ------------------- | -------------- |
| `Microsoft.Data.SqlClient` | `SqlDataAdapter`    | SQL Server     |
| `System.Data.OleDb`        | `OleDbDataAdapter`  | Access, OLE DB |
| `System.Data.Odbc`         | `OdbcDataAdapter`   | ODBC           |
| `Npgsql`                   | `NpgsqlDataAdapter` | PostgreSQL     |


El curso trabaja con **SQL Server**, por lo tanto `SqlDataAdapter`.

El constructor está **sobrecargado**. Puede no recibir argumentos, o recibir un `SqlCommand`, o un texto SQL + conexión:

```csharp
SqlDataAdapter da1 = new SqlDataAdapter();
da1.SelectCommand = comando;

SqlDataAdapter da2 = new SqlDataAdapter(comando);

SqlDataAdapter da3 = new SqlDataAdapter(
    "SELECT Id, Nombre FROM Clientes",
    conexion);
```

Las cuatro propiedades de comando son:


| Propiedad       | Cuándo se usa                |
| --------------- | ---------------------------- |
| `SelectCommand` | `Fill`                       |
| `InsertCommand` | `Update` de filas `Added`    |
| `UpdateCommand` | `Update` de filas `Modified` |
| `DeleteCommand` | `Update` de filas `Deleted`  |


### Fill con SelectCommand (versión del apunte, modernizada)

El material original asignaba transacción, conexión y texto al comando, luego `SelectCommand` y `Fill`. En .NET 8 se usa `using` en lugar de `Dispose()` a mano:

```csharp
public DataSet ObtenerClientes(SqlConnection conexion, SqlTransaction? transaccion, string consulta)
{
    DataSet ds = new DataSet();

    using SqlCommand comando = new SqlCommand(consulta, conexion);
    comando.CommandType = CommandType.Text;
    if (transaccion is not null)
    {
        comando.Transaction = transaccion;
    }

    using SqlDataAdapter adaptador = new SqlDataAdapter();
    adaptador.SelectCommand = comando;
    adaptador.Fill(ds);

    return ds;
}
```

### Misma idea con el constructor que recibe el comando

```csharp
public DataSet ObtenerClientes(SqlConnection conexion, SqlTransaction? transaccion, string consulta)
{
    using SqlCommand comando = new SqlCommand(consulta, conexion)
    {
        CommandType = CommandType.Text
    };

    if (transaccion is not null)
    {
        comando.Transaction = transaccion;
    }

    DataSet ds = new DataSet();
    using SqlDataAdapter adaptador = new SqlDataAdapter(comando);
    adaptador.Fill(ds);
    return ds;
}
```

Las líneas nuevas e importantes, como decía la clase, son:

```csharp
SqlDataAdapter adaptador = new SqlDataAdapter();
adaptador.SelectCommand = comando;
```

o, con el constructor sobrecargado:

```csharp
SqlDataAdapter adaptador = new SqlDataAdapter(comando);
```

`Fill` se encarga de abrir la conexión **si estaba cerrada** y de cerrarla al terminar si fue él quien la abrió. Por eso el modo se llama desconectado: después de `Fill` ya no hace falta mantener `Open()`.

---

# 11. Fill y Update

### Fill — de la base a memoria

```csharp
using SqlConnection conexion = new SqlConnection(CadenaConexion);
using SqlDataAdapter adaptador = new SqlDataAdapter(
    "SELECT Id, Nombre, Telefono FROM Clientes",
    conexion);

DataTable tabla = new DataTable("Clientes");
adaptador.Fill(tabla);
```

`Fill` también acepta un `DataSet`. Si la tabla no existe, la crea a partir del resultado del `SELECT`. **No copia la PK del servidor** salvo que se pida:

```csharp
adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;
adaptador.Fill(ds, "Clientes");
```

`AddWithKey` es importante si más tarde se usa `Rows.Find`, `SqlCommandBuilder` o un segundo `Fill` que debe **fusionar** por clave en lugar de duplicar filas.

### Update — de memoria a la base

`Update` recorre las filas cambiadas y ejecuta, según `RowState`, el `InsertCommand`, `UpdateCommand` o `DeleteCommand`.

Si esos comandos no están definidos, lanza excepción. Se pueden escribir a mano o generarlos con `SqlCommandBuilder`.

```csharp
int filasAfectadas = adaptador.Update(tabla);
```

Ciclo completo:

```text
Fill  →  el usuario edita la grilla  →  Update
```

Entre `Fill` y `Update` **no hay conexión abierta**.

---

# 12. SqlCommandBuilder

Si el `SelectCommand` apunta a **una sola tabla** y esa tabla tiene **clave primaria**, el constructor puede generar los comandos de escritura:

```csharp
using SqlConnection conexion = new SqlConnection(CadenaConexion);
using SqlDataAdapter adaptador = new SqlDataAdapter(
    "SELECT Id, Nombre, Telefono FROM Clientes",
    conexion);

adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;

DataTable clientes = new DataTable("Clientes");
adaptador.Fill(clientes);

using SqlCommandBuilder constructor = new SqlCommandBuilder(adaptador);

DataRow nueva = clientes.NewRow();
nueva["Nombre"] = "Dana Ruiz";
nueva["Telefono"] = "11-5555-4444";
clientes.Rows.Add(nueva);

adaptador.Update(clientes);
```

Hay que **mantener vivo** el `SqlCommandBuilder` hasta después de `Update` (por eso el `using` envuelve el bloque). Si se destruye antes, los comandos generados se pierden.

Limitaciones:

- Un solo `SELECT` sobre **una** tabla (no `JOIN`).
- La consulta debe incluir la PK.
- No reemplaza reglas de negocio complejas ni procedimientos almacenados.

Para un `UPDATE`/`INSERT` con `JOIN` o varias tablas, hay que escribir `InsertCommand` / `UpdateCommand` / `DeleteCommand` a mano, con parámetros.

---

# 13. RowState y versiones de fila

Cada `DataRow` recuerda qué le pasó desde el último `AcceptChanges` (que `Fill` aplica al terminar):


| `DataRowState` | Significado                                      | Comando en `Update` |
| -------------- | ------------------------------------------------ | ------------------- |
| `Unchanged`    | Igual que al cargar                              | ninguno             |
| `Added`        | `Rows.Add`                                       | `InsertCommand`     |
| `Modified`     | Se cambió algún campo                            | `UpdateCommand`     |
| `Deleted`      | `fila.Delete()`                                  | `DeleteCommand`     |
| `Detached`     | Creada con `NewRow` y aún no agregada, o quitada | ninguno             |


```csharp
DataRow fila = tabla.Rows[0];
fila["Telefono"] = "11-0000-0000";   // RowState = Modified

tabla.Rows[1].Delete();              // RowState = Deleted
```

`Remove` saca la fila de la colección **sin** marcarla para borrar en la base. Para persistir un borrado hay que usar `Delete()`.

Cada columna puede tener versión **Original** (la que trajo `Fill`) y **Current** (la editada). El CommandBuilder arma el `WHERE` con los valores **Originales**: así detecta si otro usuario cambió la fila.

```csharp
string actual = fila["Nombre", DataRowVersion.Current].ToString() ?? "";
string original = fila["Nombre", DataRowVersion.Original].ToString() ?? "";
```

`GetChanges()` devuelve solo las filas sucias; `HasChanges()` indica si conviene llamar a `Update`.

---

# 14. El problema de la sincronización

Volvamos al ejemplo del precio $10 → $12 y $10 → $14.

Por defecto, el `UPDATE` generado por `SqlCommandBuilder` **no** es:

```sql
UPDATE Productos SET Precio = @Precio WHERE Id = @Id
```

sino algo equivalente a:

```sql
UPDATE Productos
SET Precio = @Precio
WHERE Id = @Id
  AND Precio = @PrecioOriginal
  -- ... y el resto de columnas originales
```

Si entre el `Fill` de B y su `Update` el usuario A ya grabó $12, el `WHERE` de B **no encuentra filas**. ADO.NET interpreta “0 filas afectadas” como conflicto y lanza `DBConcurrencyException`.

```csharp
try
{
    adaptador.Update(tabla);
}
catch (DBConcurrencyException ex)
{
    // En esta materia alcanza con detectar el conflicto.
    // No se pide un algoritmo de fusión (ganar A, ganar B, mezclar, etc.).
    MessageBox.Show("Otro usuario modificó el mismo registro. Vuelva a cargar los datos.");
}
```

Eso se llama **concurrencia optimista**: se asume que no habrá choque y se comprueba al grabar. La alternativa pesimista (bloquear la fila desde el `SELECT`) es propia del **modo conectado** + transacción / hints, no del DataSet.

Opciones de diseño (solo para saber que existen):

- Recargar y pedir al usuario que reintente.
- Sobrescribir igual (comandos `WHERE` solo por PK: *last write wins*).
- `ContinueUpdateOnError = true` y revisar `RowError` fila por fila.

El curso **no** exige implementar la resolución; sí exige **entender por qué aparece**.

---

# 15. DataView

`DataView` es una vista sobre un `DataTable`: orden y filtro **en memoria**, sin otro `SELECT` al servidor.

```csharp
DataView vista = new DataView(ds.Tables["Clientes"]);
vista.Sort = "Nombre ASC";
vista.RowFilter = "Nombre LIKE 'A%'";

cmbPersonas.DataSource = vista;
cmbPersonas.DisplayMember = "Nombre";
cmbPersonas.ValueMember = "Id";
```

Útil para buscadores en grilla mientras se trabaja desconectado.

---

# 17. Ejemplo integrador

Catálogo de clientes: cargar, agregar una fila en memoria y persistir.

```csharp
using System.Data;
using Microsoft.Data.SqlClient;

internal class ClienteDesconectadoDao
{
    private const string CadenaConexion =
        @"Server=(localdb)\mssqllocaldb;Database=TallerDAS;Integrated Security=true;TrustServerCertificate=true;";

    public DataTable Cargar()
    {
        using SqlConnection conexion = new SqlConnection(CadenaConexion);
        using SqlDataAdapter adaptador = new SqlDataAdapter(
            "SELECT Id, Nombre, Telefono FROM Clientes",
            conexion);

        adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;

        DataTable tabla = new DataTable("Clientes");
        adaptador.Fill(tabla);
        return tabla;
    }

    public int Guardar(DataTable clientes)
    {
        using SqlConnection conexion = new SqlConnection(CadenaConexion);
        using SqlDataAdapter adaptador = new SqlDataAdapter(
            "SELECT Id, Nombre, Telefono FROM Clientes",
            conexion);

        adaptador.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        using SqlCommandBuilder constructor = new SqlCommandBuilder(adaptador);

        return adaptador.Update(clientes);
    }
}
```

En el formulario:

```csharp
private readonly ClienteDesconectadoDao _dao = new();
private DataTable _clientes = new();

private void Form1_Load(object sender, EventArgs e)
{
    _clientes = _dao.Cargar();
    dgvClientes.DataSource = _clientes;
}

private void btnAgregar_Click(object sender, EventArgs e)
{
    DataRow fila = _clientes.NewRow();
    fila["Nombre"] = txtNombre.Text.Trim();
    fila["Telefono"] = txtTelefono.Text.Trim();
    _clientes.Rows.Add(fila);
}

private void btnGuardar_Click(object sender, EventArgs e)
{
    try
    {
        int n = _dao.Guardar(_clientes);
        MessageBox.Show($"Se persistieron {n} cambios.");
    }
    catch (DBConcurrencyException)
    {
        MessageBox.Show("Conflicto: los datos cambiaron en el servidor. Recargue.");
    }
}
```

Mientras el usuario edita la grilla, **no hay conexión abierta**. Recién `Guardar` vuelve a SQL Server.

Comparar con el modo conectado del taller (`OrdenDao` + `SqlDataReader`): allá cada operación abre, ejecuta y cierra. Acá se acumulan cambios en el `DataTable` y se envían juntos.

---

# 19. Conceptos clave


| Concepto                     | Idea                                                            |
| ---------------------------- | --------------------------------------------------------------- |
| **Modo conectado**           | Conexión abierta mientras se lee/escribe (`SqlDataReader`)      |
| **Modo desconectado**        | Copia en memoria; conexión solo en `Fill` / `Update`            |
| **DataSet**                  | Base en memoria: tablas + relaciones                            |
| **DataTable**                | Una tabla en memoria                                            |
| **DataAdapter**              | Puente proveedor ↔ DataSet                                      |
| **Fill**                     | Base → memoria (abre y cierra si hace falta)                    |
| **Update**                   | Memoria → base, según `RowState`                                |
| **CommandBuilder**           | Genera INSERT/UPDATE/DELETE de una tabla con PK                 |
| **RowState**                 | `Added` / `Modified` / `Deleted` / `Unchanged`                  |
| **Concurrencia optimista**   | El `WHERE` usa valores originales; si nadie coincide, conflicto |
| **Microsoft.Data.SqlClient** | Proveedor de SQL Server en .NET 8                               |


---

# 20. Actividad práctica

## Objetivo

Relacionar:

```text
SQL Server → SqlDataAdapter.Fill → DataSet → edición en memoria → Update → SQL Server
```

## Actividad

1. Crear un WinForms **.NET 8** con el paquete `Microsoft.Data.SqlClient`.
2. Tener una tabla `Clientes (Id INT IDENTITY PK, Nombre NVARCHAR(80), Telefono NVARCHAR(30))`.
3. Cargar un `DataTable` con `SqlDataAdapter.Fill` y mostrarlo en un `DataGridView`.
4. Agregar una fila con `NewRow` + `Rows.Add` (sin SQL).
5. Persistir con `SqlCommandBuilder` + `Update`.
6. (Opcional) Armar un `DataSet` **solo en memoria**, como en la clase, y enlazarlo a un `ComboBox` (`DisplayMember` / `ValueMember`).

### Preguntas

**a.** ¿Qué ventaja tiene usar la conexión el menor tiempo posible?

**b.** ¿Por qué el DataSet puede existir sin una base de datos?

**c.** ¿Qué diferencia hay entre `SqlDataAdapter` y `OleDbDataAdapter`?

**d.** ¿Qué hace `Fill` con la conexión si estaba cerrada?

**e.** ¿Para qué sirven `DisplayMember` y `ValueMember` en un ComboBox?

**f.** Si dos usuarios cambian el mismo precio, ¿ADO.NET elige $12 o $14? ¿Qué excepción puede aparecer?

**g.** ¿Qué `RowState` tiene una fila luego de `Delete()`? ¿Y si se usa `Rows.Remove`?

**h.** ¿Por qué en .NET 8 se usa `Microsoft.Data.SqlClient` y no `System.Data.SqlClient`?

**i.** ¿Cuándo conviene una `List<T>` mapeada con `SqlDataReader` en lugar de un DataSet?

---

# 21. Glosario de siglas


| Sigla       | Inglés                                | Significado / uso                                                          |
| ----------- | ------------------------------------- | -------------------------------------------------------------------------- |
| **ADO**     | ActiveX Data Objects                  | API COM anterior a ADO.NET (Recordset)                                     |
| **ADO.NET** | ActiveX Data Objects .NET             | API de acceso a datos de .NET                                              |
| **API**     | Application Programming Interface     | Interfaz de programación                                                   |
| **CLR**     | Common Language Runtime               | Motor de ejecución (el tipo de `DataColumn` es un tipo CLR: `typeof(int)`) |
| **DAO**     | Data Access Object                    | Objeto de acceso a datos (capa de persistencia)                            |
| **FK**      | Foreign Key                           | Clave foránea                                                              |
| **OLE DB**  | Object Linking and Embedding Database | Proveedor genérico de datos                                                |
| **ORM**     | Object-Relational Mapper              | Mapeo objeto–relacional (p. ej. EF Core; no es esta unidad)                |
| **PK**      | Primary Key                           | Clave primaria                                                             |
| **SQL**     | Structured Query Language             | Lenguaje de consulta                                                       |
| **XML**     | Extensible Markup Language            | Formato al que un DataSet puede serializarse (`WriteXml`)                  |


---

# 22. Referencias

- [SqlDataAdapter (Microsoft.Data.SqlClient)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqldataadapter)
- [DataSet Class](https://learn.microsoft.com/en-us/dotnet/api/system.data.dataset)
- [Fill / Update de DbDataAdapter](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdataadapter.fill)
- [Generating commands with CommandBuilders](https://learn.microsoft.com/en-us/sql/connect/ado-net/generate-commands-with-commandbuilders)
- [Optimistic Concurrency](https://learn.microsoft.com/en-us/sql/connect/ado-net/optimistic-concurrency)

Material de origen: Prinzo, M. *Lenguajes de Última Generación — Unidad 2. Acceso a datos / Modo desconectado* (`LUG_MII_U2_02_ADO_02`).