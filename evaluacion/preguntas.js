const PREGUNTAS = [
  {
    id: 1,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Después de ejecutar este fragmento, ¿qué ocurre con la conexión?",
    codigo: `using SqlConnection conexion = new SqlConnection(CadenaConexion);
using SqlDataAdapter adaptador = new SqlDataAdapter(
    "SELECT Id, Nombre, Telefono FROM Clientes",
    conexion);

DataTable tabla = new DataTable("Clientes");
adaptador.Fill(tabla);`,
    opciones: [
      { id: "a", texto: "Fill deja la conexión abierta hasta que el DataTable se destruye." },
      { id: "b", texto: "Fill abre la conexión si estaba cerrada y la cierra al terminar, porque fue él quien la abrió." },
      { id: "c", texto: "Fill exige un Open() previo; si no, lanza excepción." },
      { id: "d", texto: "Fill no usa SqlConnection: el DataSet se conecta solo a SQL Server." }
    ],
    correcta: "b",
    explicacion: "Fill abre la conexión si estaba cerrada y la cierra al terminar si fue él quien la abrió. Por eso el modo se llama desconectado: entre Fill y Update no hace falta mantener Open()."
  },
  {
    id: 2,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "¿En cuál de estos casos SqlCommandBuilder NO puede generar INSERT/UPDATE/DELETE de forma fiable?",
    opciones: [
      { id: "a", texto: "SELECT Id, Nombre, Telefono FROM Clientes, con PK en Id." },
      { id: "b", texto: "Un SELECT sobre una sola tabla que incluye la clave primaria." },
      { id: "c", texto: "Un SELECT con JOIN entre Clientes y Ordenes." },
      { id: "d", texto: "Cuando se mantiene vivo el CommandBuilder hasta después de Update." }
    ],
    correcta: "c",
    explicacion: "SqlCommandBuilder solo sirve si el SelectCommand apunta a una sola tabla y esa tabla tiene clave primaria. Un JOIN no permite que genere los comandos; hay que escribir InsertCommand, UpdateCommand y DeleteCommand a mano."
  },
  {
    id: 3,
    tipo: "completar",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Completá la frase con una sola palabra (el nombre de la excepción).",
    fraseAntes: "Si el UPDATE generado por el CommandBuilder no encuentra filas porque otro usuario ya cambió los valores originales, ADO.NET lanza",
    fraseDespues: ".",
    aceptadas: ["dbconcurrencyexception"],
    respuestaVisible: "DBConcurrencyException",
    explicacion: "Eso es concurrencia optimista: el WHERE usa los valores Originales. Cero filas afectadas se interpreta como conflicto y se lanza DBConcurrencyException. En la materia alcanza con detectar el conflicto y recargar."
  },
  {
    id: 4,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "¿Cuál es el RowState de fila justo al terminar este fragmento?",
    codigo: `DataRow fila = tabla.NewRow();
fila["Nombre"] = "Dana Ruiz";
fila["Telefono"] = "11-5555-4444";
// todavía no se llamó a tabla.Rows.Add(fila)`,
    opciones: [
      { id: "a", texto: "Added" },
      { id: "b", texto: "Modified" },
      { id: "c", texto: "Detached" },
      { id: "d", texto: "Unchanged" }
    ],
    correcta: "c",
    explicacion: "NewRow() crea una fila que respeta el esquema, pero todavía no pertenece a la colección. Hasta Rows.Add (o si se quitó de la tabla) el estado es Detached. Recién con Add pasa a Added."
  },
  {
    id: 5,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Para que Update persista un borrado en SQL Server, ¿qué hay que hacer con la fila del DataTable?",
    opciones: [
      { id: "a", texto: "tabla.Rows.Remove(fila), porque saca la fila y el adapter la borra en la base." },
      { id: "b", texto: "fila.Delete(), que marca RowState = Deleted y dispara DeleteCommand." },
      { id: "c", texto: "Asignar null a todos los campos y llamar a Fill de nuevo." },
      { id: "d", texto: "Nada: Update borra cualquier fila Unchanged que no esté en la grilla." }
    ],
    correcta: "b",
    explicacion: "Remove saca la fila de la colección sin marcarla para borrar en la base. Para persistir el borrado hay que usar Delete(), que deja RowState = Deleted y hace que Update ejecute DeleteCommand."
  },
  {
    id: 6,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "El precio en la base es $10. A lo cambia a $12 y B a $14, ambos sobre copias propias. El UPDATE del CommandBuilder, por defecto, ¿cómo intenta detectar el conflicto?",
    opciones: [
      { id: "a", texto: "UPDATE ... SET Precio = @Precio WHERE Id = @Id (gana quien escribe último)." },
      { id: "b", texto: "UPDATE ... SET Precio = @Precio WHERE Id = @Id AND Precio = @PrecioOriginal (y el resto de columnas originales)." },
      { id: "c", texto: "Bloquea la fila con un SELECT FOR UPDATE desde el Fill." },
      { id: "d", texto: "ADO.NET elige siempre el mayor de los dos precios." }
    ],
    correcta: "b",
    explicacion: "No es last-write-wins salvo que armes el WHERE solo por PK. El CommandBuilder compara contra los valores Originales. Si A ya grabó $12, el WHERE de B no encuentra filas y aparece el conflicto. Eso es concurrencia optimista, no bloqueo pesimista."
  },
  {
    id: 7,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "En Transferir de BancoDao, el débito ya se ejecutó y el crédito lanza excepción. ¿Qué queda en la base si el catch hace Rollback?",
    codigo: `using SqlTransaction transaccion = conexion.BeginTransaction();
try
{
    decimal saldoOrigen = ObtenerSaldo(conexion, transaccion, desde);
    if (saldoOrigen < monto) throw new InvalidOperationException("Saldo insuficiente.");

    // UPDATE origen: Balance = Balance - @monto
    // UPDATE destino: Balance = Balance + @monto   ← falla aquí

    transaccion.Commit();
}
catch
{
    transaccion.Rollback();
    throw;
}`,
    opciones: [
      { id: "a", texto: "El débito queda confirmado y el crédito no: hay plata 'perdida'." },
      { id: "b", texto: "Ningún UPDATE queda persistido: débito y crédito se deshacen juntos." },
      { id: "c", texto: "SQL Server confirma solo las filas que no fallaron, sin Rollback." },
      { id: "d", texto: "Commit se ejecuta igual porque el débito ya corrió." }
    ],
    correcta: "b",
    explicacion: "La transacción es atómica: o se confirman todas las operaciones o ninguna. Rollback deshace el débito que todavía no se había hecho Commit. Por eso cabecera + detalle (o débito + crédito) van en la misma transacción."
  },
  {
    id: 8,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "¿Qué anomalía permite IsolationLevel.ReadUncommitted y no permite ReadCommitted?",
    opciones: [
      { id: "a", texto: "Dirty reads (lecturas sucias)." },
      { id: "b", texto: "Solo phantom reads; las sucias siguen prohibidas." },
      { id: "c", texto: "Ninguna: ambos niveles son equivalentes en SQL Server." },
      { id: "d", texto: "Lost updates, pero únicamente en modo desconectado." }
    ],
    correcta: "a",
    explicacion: "ReadUncommitted permite lecturas sucias, no repetibles y fantasma. ReadCommitted (el predeterminado) evita dirty reads, pero todavía permite lecturas no repetibles y fantasma."
  },
  {
    id: 9,
    tipo: "completar",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "Completá la frase con el nombre del mecanismo (una palabra / un identificador).",
    fraseAntes: "En .NET 8, para cargar y descargar assemblies dinámicamente (plugins) se usa",
    fraseDespues: ", no AppDomain.CreateDomain.",
    aceptadas: ["assemblyloadcontext"],
    respuestaVisible: "AssemblyLoadContext",
    explicacion: "AppDomain.CreateDomain no está disponible desde .NET Core / .NET 8. El mecanismo moderno es System.Runtime.Loader.AssemblyLoadContext, que además puede ser collectible para descargar el assembly."
  },
  {
    id: 10,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "Respecto de AppDomain en .NET 8, ¿cuál es la afirmación correcta?",
    opciones: [
      { id: "a", texto: "Se pueden crear varios AppDomain por proceso, igual que en .NET Framework." },
      { id: "b", texto: "AppDomain.CreateDomain sigue siendo la forma recomendada de aislar plugins." },
      { id: "c", texto: "Hay un solo AppDomain por proceso; CreateDomain no debe usarse. AppDomain.CurrentDomain aún existe para datos del dominio actual." },
      { id: "d", texto: "AppDomain desapareció por completo: ni CurrentDomain compila." }
    ],
    correcta: "c",
    explicacion: "El aislamiento por múltiples AppDomain es histórico de .NET Framework. En .NET 8 hay un dominio por proceso. CurrentDomain.BaseDirectory todavía sirve; CreateDomain no. Para aislamiento fuerte, proceso o contenedor."
  },
  {
    id: 11,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo conectado",
    enunciado: "¿Qué devuelve Insertar en ClienteDao?",
    codigo: `using SqlCommand comando = new SqlCommand(
    """
    INSERT INTO Clientes (Nombre, Telefono)
    VALUES (@Nombre, @Telefono);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
    """,
    conexion);

comando.Parameters.AddWithValue("@Nombre", cliente.Nombre);
comando.Parameters.AddWithValue("@Telefono", cliente.Telefono);
return (int)comando.ExecuteScalar();`,
    opciones: [
      { id: "a", texto: "La cantidad de filas insertadas (siempre 1)." },
      { id: "b", texto: "El Id identity recién generado de ese INSERT." },
      { id: "c", texto: "Un SqlDataReader abierto sobre Clientes." },
      { id: "d", texto: "Nada útil: ExecuteScalar no sirve con INSERT." }
    ],
    correcta: "b",
    explicacion: "ExecuteScalar devuelve la primera columna de la primera fila del resultado. Acá esa fila es SCOPE_IDENTITY(), el Id generado. ExecuteNonQuery devolvería filas afectadas, no el identity."
  },
  {
    id: 12,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "¿Para qué se usa MissingSchemaAction.AddWithKey antes de Fill?",
    opciones: [
      { id: "a", texto: "Para que Fill no copie datos, solo el esquema." },
      { id: "b", texto: "Para traer la PK del servidor. Hace falta para Rows.Find, CommandBuilder y para que un segundo Fill fusione en vez de duplicar." },
      { id: "c", texto: "Para abrir la conexión en modo desconectado." },
      { id: "d", texto: "Para serializar el DataSet a XML." }
    ],
    correcta: "b",
    explicacion: "Fill, por defecto, no copia la PK del servidor. AddWithKey agrega el esquema con clave. Sin eso, Find no funciona bien, CommandBuilder puede fallar y un segundo Fill suele agregar filas duplicadas en lugar de fusionar."
  },
  {
    id: 13,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "¿Por qué el using de SqlCommandBuilder debe envolver el bloque hasta después de Update?",
    opciones: [
      { id: "a", texto: "Porque el builder abre una segunda conexión que solo Dispose cierra." },
      { id: "b", texto: "Porque si se destruye antes, se pierden los comandos INSERT/UPDATE/DELETE que generó." },
      { id: "c", texto: "Porque Update no corre si el builder sigue vivo." },
      { id: "d", texto: "No hace falta: con crear el builder una línea y dejarlo morir alcanza." }
    ],
    correcta: "b",
    explicacion: "Hay que mantener vivo el SqlCommandBuilder hasta después de Update. Si el objeto se dispone antes, los comandos generados se pierden y Update no tiene InsertCommand/UpdateCommand/DeleteCommand."
  },
  {
    id: 14,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "La conexión ya tiene una transacción local pendiente. ¿Qué problema tiene este comando?",
    codigo: `using SqlTransaction transaccion = conexion.BeginTransaction();
using SqlCommand cmd = new SqlCommand(
    "UPDATE Cuentas SET Balance = Balance - @monto WHERE Id = @id",
    conexion);
cmd.Parameters.AddWithValue("@monto", monto);
cmd.Parameters.AddWithValue("@id", desde);
cmd.ExecuteNonQuery();
transaccion.Commit();`,
    opciones: [
      { id: "a", texto: "Ninguno: el comando hereda la transacción de la conexión automáticamente." },
      { id: "b", texto: "Falta asignar cmd.Transaction = transaccion (o pasar la transacción al constructor). Si no, ExecuteNonQuery falla." },
      { id: "c", texto: "Commit está de más: Update ya confirma cada fila." },
      { id: "d", texto: "Hay que usar ExecuteReader en lugar de ExecuteNonQuery para un UPDATE." }
    ],
    correcta: "b",
    explicacion: "Si la conexión está en una transacción local, el SqlCommand debe enlistarse (constructor de 3 argumentos o propiedad Transaction). Si no, ADO.NET lanza que el comando requiere un objeto transacción."
  },
  {
    id: 15,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "medio-alto",
    tema: "Modo conectado",
    enunciado: "SqlDataReader, en el modo conectado del curso, se caracteriza por ser:",
    opciones: [
      { id: "a", texto: "De solo lectura, hacia adelante, y exige conexión abierta mientras se recorre." },
      { id: "b", texto: "Editable y bidireccional, como un DataTable." },
      { id: "c", texto: "Desconectado: se puede cerrar la conexión y seguir leyendo." },
      { id: "d", texto: "El puente Fill/Update hacia el DataSet." }
    ],
    correcta: "a",
    explicacion: "El reader es lectura secuencial, adelante, solo lectura, con la conexión abierta. No reemplaza al DataAdapter. En los prácticos se mapea cada fila a una entidad y se cierra al salir del using."
  },
  {
    id: 16,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "TP y modelo",
    enunciado: "En el TP de facturación, el precio de venta se copia a FacturaDetalle. ¿Por qué no alcanza con leer Productos.Precio al consultar una factura vieja?",
    opciones: [
      { id: "a", texto: "Porque SqlDataReader no puede hacer JOIN." },
      { id: "b", texto: "Porque si más tarde cambia el precio del producto, las facturas ya emitidas no deben alterarse." },
      { id: "c", texto: "Porque el total se calcula solo en memoria con DataView." },
      { id: "d", texto: "Porque IDENTITY no permite guardar decimales en Productos." }
    ],
    correcta: "b",
    explicacion: "Regla de negocio: el precio vigente se congela en la línea al emitir. Consultar el catálogo actual distorsionaría facturas históricas. Por eso PrecioUnitario y Subtotal viven en el detalle."
  },
  {
    id: 17,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo conectado",
    enunciado: "Si el cliente 5 tiene dos órdenes, ¿qué hace EliminarSiNoTieneOrdenes(5)?",
    codigo: `using SqlCommand hayOrdenes = new SqlCommand(
    "SELECT COUNT(*) FROM Ordenes WHERE ClienteId = @Id",
    conexion);
hayOrdenes.Parameters.AddWithValue("@Id", id);
int cantidad = (int)hayOrdenes.ExecuteScalar();
if (cantidad > 0)
{
    throw new InvalidOperationException(
        "No se puede eliminar: el cliente tiene órdenes asociadas.");
}
using SqlCommand comando = new SqlCommand(
    "DELETE FROM Clientes WHERE Id = @Id", conexion);`,
    opciones: [
      { id: "a", texto: "Borra el cliente y, en cascada, las órdenes." },
      { id: "b", texto: "No ejecuta el DELETE: lanza InvalidOperationException por integridad referencial de negocio." },
      { id: "c", texto: "Borra solo si COUNT(*) es mayor que 0." },
      { id: "d", texto: "Hace Rollback de una transacción que no existe y borra igual." }
    ],
    correcta: "b",
    explicacion: "Antes del DELETE se consulta cuántas órdenes hay. Si hay al menos una, se aborta con excepción de negocio (el cliente está referenciado). Es el mismo criterio del TP: no dejar huérfanos ni borrar un producto/cliente usado en detalles."
  },
  {
    id: 18,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "¿Cuál es la diferencia clave entre JIT y Native AOT?",
    opciones: [
      { id: "a", texto: "JIT compila IL a nativo durante la ejecución; Native AOT genera nativo en la publicación y en runtime no hay JIT." },
      { id: "b", texto: "AOT es el nombre antiguo de JIT en .NET Framework." },
      { id: "c", texto: "JIT produce el .csproj; AOT produce el .cs." },
      { id: "d", texto: "Ambos ocurren solo en el compilador Roslyn, antes de generar IL." }
    ],
    correcta: "a",
    explicacion: "Roslyn genera IL. El JIT traduce IL → nativo cuando se ejecutan los métodos. Native AOT (PublishAot) adelanta esa traducción a la publicación. ReadyToRun es un tercio: nativo preparado, pero el runtime (y a veces el JIT) siguen existiendo."
  },
  {
    id: 19,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "Un Strong Name (PublicKeyToken / sn.exe):",
    opciones: [
      { id: "a", texto: "Equivale a un certificado digital y protege el código fuente contra descompilación." },
      { id: "b", texto: "Es parte de la identidad del assembly, pero no es certificado, ni mecanismo general de seguridad, ni protección del fuente." },
      { id: "c", texto: "Es obligatorio en todo proyecto .NET 8 para que NuGet restaure." },
      { id: "d", texto: "Reemplaza al GAC en las publicaciones self-contained." }
    ],
    correcta: "b",
    explicacion: "Strong Name ≠ certificado ≠ seguridad general ≠ ocultar el fuente. El IL se puede inspeccionar con ILSpy. sn.exe no es herramienta cotidiana de .NET 8. La identidad fuerte no convierte al software en seguro ni en open source."
  },
  {
    id: 20,
    tipo: "completar",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Completá con el nombre del DataRowState (una palabra).",
    fraseAntes: "Fill, al terminar, aplica AcceptChanges. Por eso las filas recién cargadas quedan con RowState",
    fraseDespues: ".",
    aceptadas: ["unchanged"],
    respuestaVisible: "Unchanged",
    explicacion: "Unchanged significa 'igual que al cargar'. Recién un cambio de campo pasa a Modified; Rows.Add a Added; Delete() a Deleted. Update ignora Unchanged."
  },
  {
    id: 21,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "IsolationLevel.RepeatableRead evita lecturas sucias y no repetibles. ¿Qué anomalía todavía permite?",
    opciones: [
      { id: "a", texto: "Dirty reads." },
      { id: "b", texto: "Phantom reads (lecturas fantasma)." },
      { id: "c", texto: "Ninguna: es el nivel máximo." },
      { id: "d", texto: "Solo lost updates del DataAdapter." }
    ],
    correcta: "b",
    explicacion: "La tabla del apunte: RepeatableRead impide dirty y non-repeatable, pero no phantom. Serializable previene las tres, con más bloqueo y menos rendimiento."
  },
  {
    id: 22,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "¿Cuál es la relación correcta entre DataSet y ADO.NET?",
    opciones: [
      { id: "a", texto: "El DataSet no forma parte de ADO.NET; solo existe en WinForms." },
      { id: "b", texto: "El DataSet es el núcleo del modelo desconectado de ADO.NET, vive en System.Data y no pertenece al proveedor Microsoft.Data.SqlClient." },
      { id: "c", texto: "DataSet es un tipo de Microsoft.Data.SqlClient, igual que SqlConnection." },
      { id: "d", texto: "Sin SQL Server no puede existir un DataSet." }
    ],
    correcta: "b",
    explicacion: "Corrección al material original: el DataSet sí es parte de ADO.NET, pero no del proveedor. Es agnóstico del motor: sirve para SQL Server, otra base o datos que nunca salieron de un XML."
  },
  {
    id: 23,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo conectado",
    enunciado: "¿Cuál es el problema de este comando si 'nombre' viene de un TextBox?",
    codigo: `comando.CommandText =
    "SELECT Id, Nombre FROM Clientes WHERE Nombre = '" + nombre + "'";
using SqlDataReader lector = comando.ExecuteReader();`,
    opciones: [
      { id: "a", texto: "Ninguno, si la conexión usa Integrated Security." },
      { id: "b", texto: "Hay que usar ExecuteScalar porque el SELECT no tiene JOIN." },
      { id: "c", texto: "Concatenar el valor del usuario habilita inyección SQL; debe usarse un parámetro @Nombre." },
      { id: "d", texto: "AddWithValue no existe en modo conectado." }
    ],
    correcta: "c",
    explicacion: "En el curso es obligatorio SqlCommand con parámetros (@Nombre), nunca concatenar. Los parámetros evitan inyección y manejan tipos. El taller y el TP lo exigen de forma explícita."
  },
  {
    id: 24,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "Al capturar SqlException, el número 1205 de SQL Server indica:",
    opciones: [
      { id: "a", texto: "Violación de restricción única." },
      { id: "b", texto: "Violación de clave foránea." },
      { id: "c", texto: "Deadlock; suele reintentarse con backoff." },
      { id: "d", texto: "Timeout de CommandBuilder." }
    ],
    correcta: "c",
    explicacion: "1205 = deadlock, 2627 = unique, 547 = FK. El apunte de transacciones muestra reintentos cuando el número es 1205 (o timeout -2). No hay que memorizar todos los códigos, sí distinguir deadlock de unique/FK."
  },
  {
    id: 25,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Tras definir DataRelation \"FK_Ordenes_Clientes\" (Clientes.Id → Ordenes.ClienteId), ¿cómo se listan en memoria las órdenes del cliente 1 sin otro SELECT?",
    opciones: [
      { id: "a", texto: "ds.Tables[0].SelectCommand.ExecuteReader()" },
      { id: "b", texto: "cliente.GetChildRows(\"FK_Ordenes_Clientes\") sobre la DataRow encontrada con Rows.Find(1)" },
      { id: "c", texto: "adaptador.Update(ds.Tables[\"Ordenes\"])" },
      { id: "d", texto: "No se puede: DataRelation solo existe en SQL Server, no en el DataSet." }
    ],
    correcta: "b",
    explicacion: "La FK del servidor no se 'conoce' en memoria hasta declararla. Con DataRelation se navega padre→hijos con GetChildRows. Find requiere que la tabla tenga PrimaryKey (por eso AddWithKey ayuda)."
  },
  {
    id: 26,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "¿Por qué es incorrecto decir que «el IL lo ejecuta directamente la CPU»?",
    opciones: [
      { id: "a", texto: "Porque el IL es código nativo de ARM64 únicamente." },
      { id: "b", texto: "Porque la CPU ejecuta código nativo; el runtime (JIT/AOT) traduce el IL antes." },
      { id: "c", texto: "Porque el IL vive en el .csproj, no en el assembly." },
      { id: "d", texto: "Porque Roslyn ya emite x64 en el paso a CIL." }
    ],
    correcta: "b",
    explicacion: "C# → Roslyn → IL + metadatos → assembly → runtime → JIT o AOT → código nativo → CPU. El IL es independiente de la CPU; por eso el mismo assembly puede correr en Windows x64 o Linux ARM64 con un runtime compatible."
  },
  {
    id: 27,
    tipo: "completar",
    bloque: "principal",
    dificultad: "medio-alto",
    tema: "Transacciones",
    enunciado: "Completá con la propiedad ACID (una palabra).",
    fraseAntes: "Que una transferencia sea 'todo o nada' (Commit de débito y crédito, o Rollback de ambos) se llama",
    fraseDespues: ".",
    aceptadas: ["atomicidad", "atomicidadacid", "atomicity"],
    respuestaVisible: "Atomicidad",
    explicacion: "ACID: Atomicidad (todo o nada), Consistencia (estado válido), Aislamiento (no interferir), Durabilidad (lo confirmado persiste). En el taller, orden + ítems se graban en una sola SqlTransaction por esa razón."
  },
  {
    id: 28,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Ensamblados",
    enunciado: "Al publicar con dotnet publish, ¿qué distingue FDD de SCD?",
    opciones: [
      { id: "a", texto: "FDD incluye el runtime; SCD asume que .NET ya está instalado." },
      { id: "b", texto: "FDD (framework-dependent) asume el runtime instalado y genera un paquete más chico; SCD (self-contained) incluye el runtime y el paquete es más grande." },
      { id: "c", texto: "FDD es el GAC; SCD es NuGet." },
      { id: "d", texto: "Solo FDD puede usarse con WinForms." }
    ],
    correcta: "b",
    explicacion: "Estos modelos reemplazan, en la práctica, instalar DLL en el GAC. FDD necesita .NET 8 en la máquina destino. SCD corre aunque no haya runtime preinstalado, a costa de tamaño."
  },
  {
    id: 29,
    tipo: "opcion",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "En los prácticos de modo conectado se entrega List<Cliente> a la UI. En modo desconectado el DataSet es el modelo. ¿Qué NO trae una List<T> y sí un DataTable?",
    opciones: [
      { id: "a", texto: "Enlace a ComboBox / DataGridView." },
      { id: "b", texto: "Tipado por propiedades de clase." },
      { id: "c", texto: "RowState (Added/Modified/Deleted) listo para un Update masivo del adapter." },
      { id: "d", texto: "Reglas de negocio más claras en la entidad." }
    ],
    correcta: "c",
    explicacion: "Ambos se pueden enlazar a WinForms. El DataSet tiene esquema dinámico, DataRelation y RowState para Update. En una lista, el cambio de estado lo define el programador. El curso usa entidades + reader en conectado y DataSet en desconectado."
  },
  {
    id: 30,
    tipo: "codigo",
    bloque: "principal",
    dificultad: "alto",
    tema: "Modo conectado",
    enunciado: "El método Cerrar de OrdenDao hace esto. ¿Cuál es la lectura correcta?",
    codigo: `using SqlCommand consultarEstado = new SqlCommand(
    "SELECT Estado FROM Ordenes WHERE Id = @Id", conexion);
object? resultado = consultarEstado.ExecuteScalar();
if (string.Equals((string)resultado, "Cerrada", ...))
    throw new InvalidOperationException("La orden ya está cerrada...");

using SqlCommand comando = new SqlCommand(
    "UPDATE Ordenes SET Estado = @Estado WHERE Id = @Id", conexion);
comando.Parameters.AddWithValue("@Estado", "Cerrada");
return comando.ExecuteNonQuery();`,
    opciones: [
      { id: "a", texto: "SELECT y UPDATE están en una SqlTransaction, así que el cierre es atómico frente a otro usuario." },
      { id: "b", texto: "Usa ExecuteScalar para leer un solo valor y ExecuteNonQuery para el UPDATE; no hay transacción que una ambas operaciones." },
      { id: "c", texto: "ExecuteNonQuery devuelve el nuevo Estado." },
      { id: "d", texto: "Si la orden no existe, ExecuteScalar devuelve 0 y el UPDATE igual la crea." }
    ],
    correcta: "b",
    explicacion: "ExecuteScalar sirve para un único valor (el estado). ExecuteNonQuery ejecuta el UPDATE y devuelve filas afectadas. No hay BeginTransaction: la comprobación y el cierre no son una unidad atómica. Si no hay fila, el código trata DBNull/null como 'no existe'."
  },
  {
    id: 31,
    tipo: "opcion",
    bloque: "mixto",
    dificultad: "bajo",
    tema: "Modo desconectado",
    enunciado: "El objeto que funciona como base de datos temporal en memoria (varias tablas + relaciones) es:",
    opciones: [
      { id: "a", texto: "SqlConnection" },
      { id: "b", texto: "SqlDataReader" },
      { id: "c", texto: "DataSet" },
      { id: "d", texto: "SqlTransaction" }
    ],
    correcta: "c",
    explicacion: "El DataSet contiene DataTable y DataRelation. Cada tabla tiene DataColumn y DataRow. Puede crearse incluso sin SQL Server."
  },
  {
    id: 32,
    tipo: "completar",
    bloque: "mixto",
    dificultad: "bajo",
    tema: "Modo desconectado",
    enunciado: "Completá con el nombre del método (una palabra).",
    fraseAntes: "El método del DataAdapter que copia datos desde SQL Server hacia el DataSet o DataTable se llama",
    fraseDespues: ".",
    aceptadas: ["fill"],
    respuestaVisible: "Fill",
    explicacion: "Fill: base → memoria. Update: memoria → base, según RowState. Entre ambos no hay conexión abierta."
  },
  {
    id: 33,
    tipo: "opcion",
    bloque: "mixto",
    dificultad: "medio",
    tema: "Modo conectado",
    enunciado: "En los DAO del repositorio se usa `using SqlConnection conexion = Conexion.Crear();`. ¿Qué garantiza el using?",
    opciones: [
      { id: "a", texto: "Que la consulta se cachea en el DataSet." },
      { id: "b", texto: "Que al salir del bloque se dispone la conexión (se cierra), incluso si hubo excepción." },
      { id: "c", texto: "Que se inicia una transacción Serializable." },
      { id: "d", texto: "Que NuGet restaura Microsoft.Data.SqlClient." }
    ],
    correcta: "b",
    explicacion: "using llama a Dispose (cierra la conexión). El curso pide abrir el menor tiempo posible y no dejar conexiones colgadas. En .NET 8 se prefiere using a Dispose() a mano."
  },
  {
    id: 34,
    tipo: "opcion",
    bloque: "mixto",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Si se pone ContinueUpdateOnError = true en el adapter y una fila choca por concurrencia:",
    opciones: [
      { id: "a", texto: "Se lanza siempre DBConcurrencyException y se aborta todo el Update." },
      { id: "b", texto: "Update puede seguir con las demás filas; hay que revisar RowError fila por fila." },
      { id: "c", texto: "SQL Server fusiona automáticamente $12 y $14." },
      { id: "d", texto: "Fill vuelve a ejecutarse solo sobre esa fila." }
    ],
    correcta: "b",
    explicacion: "Es una opción de diseño del apunte: no resolver conflictos en la materia, pero saber que existen last-write-wins (WHERE solo por PK), recargar, o ContinueUpdateOnError + RowError."
  },
  {
    id: 35,
    tipo: "codigo",
    bloque: "mixto",
    dificultad: "bajo",
    tema: "Modo conectado",
    enunciado: "En PeliculaDao.Eliminar, ¿qué representa el valor de retorno?",
    codigo: `using SqlCommand comando = new SqlCommand(
    "DELETE FROM Peliculas WHERE Id = @Id", conexion);
comando.Parameters.AddWithValue("@Id", id);
return comando.ExecuteNonQuery();`,
    opciones: [
      { id: "a", texto: "El Id de la película borrada." },
      { id: "b", texto: "La cantidad de filas afectadas por el DELETE (0 si no existía)." },
      { id: "c", texto: "Un DataTable con las películas restantes." },
      { id: "d", texto: "Siempre -1, porque DELETE no tiene resultado." }
    ],
    correcta: "b",
    explicacion: "ExecuteNonQuery se usa en INSERT/UPDATE/DELETE y devuelve filas afectadas. ExecuteReader recorre resultados; ExecuteScalar un único valor (COUNT, IDENTITY, un campo)."
  },
  {
    id: 36,
    tipo: "completar",
    bloque: "mixto",
    dificultad: "medio",
    tema: "Transacciones",
    enunciado: "Completá con el método (una palabra).",
    fraseAntes: "Si el alta de la orden falla a mitad de los INSERT de ítems, para deshacer lo ya ejecutado se invoca transaccion.",
    fraseDespues: "().",
    aceptadas: ["rollback"],
    respuestaVisible: "Rollback",
    explicacion: "Commit confirma; Rollback deshace. En OrdenDao.Insertar el catch hace Rollback y vuelve a lanzar. Sin eso, podrías dejar una orden sin ítems."
  },
  {
    id: 37,
    tipo: "opcion",
    bloque: "mixto",
    dificultad: "alto",
    tema: "Transacciones",
    enunciado: "transaction.Save(\"SP1\") en ADO.NET sirve para:",
    opciones: [
      { id: "a", texto: "Confirmar de forma definitiva la transacción, igual que Commit." },
      { id: "b", texto: "Crear un punto de guardado y poder hacer Rollback(\"SP1\") sin deshacer toda la transacción." },
      { id: "c", texto: "Serializar el DataSet a disco." },
      { id: "d", texto: "Cambiar el IsolationLevel a Serializable." }
    ],
    correcta: "b",
    explicacion: "Los savepoints permiten revertir parcialmente. Si Rollback(\"SP1\") también falla, el ejemplo del apunte hace Rollback completo. No reemplaza a Commit."
  },
  {
    id: 38,
    tipo: "codigo",
    bloque: "mixto",
    dificultad: "medio",
    tema: "Modo conectado",
    enunciado: "¿Qué hace este método privado del DAO?",
    codigo: `private static Cliente Mapear(SqlDataReader lector)
{
    return new Cliente
    {
        Id = (int)lector["Id"],
        Nombre = lector["Nombre"].ToString() ?? string.Empty,
        Telefono = lector["Telefono"].ToString() ?? string.Empty
    };
}`,
    opciones: [
      { id: "a", texto: "Crea un DataSet y lo llena con Fill." },
      { id: "b", texto: "Traduce la fila actual del reader a una entidad Cliente (capa de datos → objeto)." },
      { id: "c", texto: "Ejecuta el INSERT del cliente." },
      { id: "d", texto: "Cierra la conexión y confirma la transacción." }
    ],
    correcta: "b",
    explicacion: "Es el mapeo típico del modo conectado del curso: cada Read() produce un objeto. El formulario recibe List<Cliente>, no un DataTable. El SQL no vive en el Form."
  },
  {
    id: 39,
    tipo: "opcion",
    bloque: "mixto",
    dificultad: "bajo",
    tema: "Ensamblados",
    enunciado: "Para agregar Microsoft.Data.SqlClient a un proyecto .NET 8 se usa principalmente:",
    opciones: [
      { id: "a", texto: "gacutil /i Microsoft.Data.SqlClient.dll" },
      { id: "b", texto: "NuGet (dotnet add package o Manage NuGet Packages), que deja un PackageReference en el .csproj" },
      { id: "c", texto: "AppDomain.CreateDomain" },
      { id: "d", texto: "ILDASM sobre el apphost .exe" }
    ],
    correcta: "b",
    explicacion: "En .NET 8 las dependencias van por NuGet, no por el GAC. El proveedor de SQL Server ya no es el histórico System.Data.SqlClient como paquete de uso actual del curso: es Microsoft.Data.SqlClient."
  },
  {
    id: 40,
    tipo: "codigo",
    bloque: "mixto",
    dificultad: "alto",
    tema: "Modo desconectado",
    enunciado: "Se hace Fill dos veces sobre el mismo DataTable, sin AddWithKey y sin PrimaryKey. ¿Qué es lo más probable?",
    codigo: `DataTable clientes = new DataTable("Clientes");
adaptador.Fill(clientes);
adaptador.Fill(clientes);`,
    opciones: [
      { id: "a", texto: "La segunda llamada fusiona por Id y no duplica." },
      { id: "b", texto: "La segunda llamada suele agregar otra vez las mismas filas (duplicados), porque no hay clave para fusionar." },
      { id: "c", texto: "La segunda llama a Update y persiste cambios." },
      { id: "d", texto: "Fill la segunda vez lanza DBConcurrencyException siempre." }
    ],
    correcta: "b",
    explicacion: "Sin clave, Fill no tiene criterio de fusión y apila filas. AddWithKey (u definir PrimaryKey) permite fusionar por PK. No confundir con el conflicto de Update, que es otro momento del ciclo."
  }
];
