# Práctica formato parcial — Pedidos de vivero (Entity Framework)

**Desarrollo y arquitectura de software**  
**Tecnologías:** .NET 8 · Windows Forms · Entity Framework Core 9.0.0 · SQL Server / LocalDB  
**Arquitectura:** solución en **3 capas** (Modelo · Controladora · Vista), según la guía de esta carpeta.

La solución ya está creada en `ViveroPedidos/`. Ábranla y trabajen ahí. No armen otro proyecto desde cero.

---

## 1. Objetivo

Completar una aplicación de escritorio para un **vivero municipal**: administrar el catálogo de plantas y **registrar / consultar pedidos**, persistiendo con **Entity Framework Core (Code First + migraciones)**.

En este práctico la **Vista ya está codificada**. Hay que resolver la capa **Modelo** y la capa **Controladora**.

El trabajo evalúa:

- entidades ya dadas, más `DbContext`, relaciones y acceso a datos en el **Modelo**;
- lógica de negocio en la **Controladora** (los métodos públicos ya están declarados);
- migración inicial aplicada a LocalDB (o SQL Server);
- flujo maestro–detalle con grabación atómica (transacción).

**No se permite** acceso a datos con ADO.NET “a mano” (`SqlConnection` / `SqlCommand` como mecanismo principal), ni poner el `DbContext` o consultas EF en la Vista o en la Controladora.

Pueden mirar el ejemplo `FacturacionSimple` como referencia de arquitectura. El dominio y el código de datos deben ser **propios** de este enunciado (vivero / pedidos).

---

## 2. Qué está hecho y qué deben resolver

| Capa | Proyecto | Estado |
| --- | --- | --- |
| Vista | `ViveroPedidos.Vista` | **Hecha.** Formularios, eventos y llamadas a la controladora. No la modifiquen para cumplir el enunciado. |
| Controladora | `ViveroPedidos.Controladora` | **A resolver.** Las clases y las firmas públicas ya existen. Cada método lanza `NotImplementedException`. Reemplacen ese cuerpo por validaciones y orquestación. No cambien las firmas: la Vista las usa. |
| Modelo | `ViveroPedidos.Modelo` | **A resolver**, salvo las tres entidades. `Planta`, `Pedido` y `PedidoDetalle` ya están declaradas con las propiedades que consume la Vista. No renombren propiedades ni namespaces. Faltan el `DbContext`, el acceso a datos, la cadena de conexión y la migración. |

La Controladora **no** habla con SQL Server ni configura EF: delega en clases de acceso a datos del Modelo (por ejemplo `PlantaDatos` y `PedidoDatos`, en la carpeta `Datos`).

Al ejecutar, los formularios se abren. Hasta que implementen cada método, la pantalla muestra el mensaje de `NotImplementedException`. Eso es el punto de partida, no un error de la Vista.

---

## 3. Dominio

El vivero toma pedidos de plantas para vecinos del municipio. El solicitante **no** es una entidad aparte: sus datos se copian en la cabecera del pedido al registrarlo.

Hay **tres entidades / tablas**:

| Tabla / entidad | Rol |
| --- | --- |
| `Plantas` | Catálogo (ABM) |
| `Pedidos` | Cabecera (número, fecha, datos del solicitante, fecha de retiro, cantidad de ítems) |
| `PedidoDetalles` | Líneas (planta, nombre histórico, cantidad pedida en esa línea) |

Regla de negocio central: al grabar el pedido se guarda el **nombre común de la planta copiado en el detalle** (`NombreHistorico`), además del `PlantaId`. Si más tarde se renombra la planta en el catálogo, los pedidos ya registrados **conservan** el nombre histórico.

No hay stock numérico. `Disponible` solo indica si la planta se puede incluir en pedidos nuevos. Registrar un pedido **no** modifica el catálogo ni descuenta existencia.

---

## 4. Modelo de datos (mínimo obligatorio)

Usar Code First. Las propiedades públicas ya están en las clases; configuren claves, longitudes, índices y relaciones en EF (anotaciones o Fluent API).

### `Planta`

- `Id` (int, identity, PK)
- `Codigo` (único, no vacío, longitud 20) — código interno de inventario
- `NombreComun` (no vacío, longitud 120)
- `Especie` (no vacío, longitud 120)
- `Disponible` (bool) — una planta no disponible no puede incluirse en pedidos nuevos

### `Pedido`

- `Id` (int, identity, PK)
- `Numero` (único; correlativo controlado por la aplicación, no es identity)
- `Fecha` (sin hora)
- `SolicitanteNombre` (longitud 120)
- `SolicitanteDocumento` (texto, longitud 30; DNI o equivalente)
- `FechaRetiro` (sin hora)
- `CantidadItems` (int) — debe coincidir con la suma de cantidades del detalle

### `PedidoDetalle`

- `Id` (int, identity, PK)
- `PedidoId` (FK → `Pedidos`, borrado en cascada)
- `PlantaId` (FK → `Plantas`; no borrar la planta si tiene detalles: borrado restringido)
- `NombreHistorico` (texto, longitud 120) — copia de `NombreComun` al pedir
- `Cantidad` (int, > 0)

No hace falta tabla `Solicitantes`.

Cadena sugerida (LocalDB):

```text
Server=(localdb)\MSSQLLocalDB;Database=ViveroPedidosDb;Trusted_Connection=True;TrustServerCertificate=True
```

---

## 5. Alcance funcional

### 5.1 Plantas

Pantalla `FormPlantas` (ya hecha). Deben hacer funcionar:

- Alta y modificación. En el **alta**, `Disponible` queda en `true` (el checkbox de la pantalla no aplica al alta). En la **modificación**, se persiste el valor del checkbox.
- Botón **No disponible**: pone `Disponible = false` sin guardar los otros cuadros de texto. Si ya estaba en false, informar con un mensaje claro.
- Eliminación física **solo si** la planta no está en ningún detalle. Si está referenciada, informar y no eliminar.
- Listado con búsqueda por código, nombre común o especie. Texto vacío: traer todas, incluidas las no disponibles.
- `ListarDisponibles`: solo las que tienen `Disponible = true`, ordenadas por nombre común. El combo del pedido usa este método.
- Validar código único (sin distinguir mayúsculas, comparando el valor ya recortado), y nombre común y especie no vacíos.

### 5.2 Registro de pedido (caso principal)

Pantalla `FormRegistrarPedido` (ya hecha):

1. Cabecera: fecha (por defecto hoy), nombre y documento del solicitante, fecha de retiro.
2. Líneas: elegir planta **disponible**, cantidad; la grilla muestra código, nombre común, especie y cantidad.
3. No se permiten dos líneas con la misma planta. La Vista avisa y no agrega la repetida. La Controladora **también** debe rechazar una lista que traiga la misma planta dos veces.
4. La pantalla recalcula la cantidad de ítems al agregar o quitar líneas. Al grabar, `CantidadItems` de la cabecera debe ser la suma de las cantidades del detalle.
5. **Grabar cabecera y detalle en una sola transacción.** Si falla algo, rollback completo. El número correlativo se asigna dentro de esa misma operación.
6. Al grabar, persistir `NombreHistorico` (copia de `NombreComun`) y `Cantidad` en cada línea.
7. No registrar pedido sin al menos una línea, ni con solicitante o documento vacíos, ni con fecha de retiro anterior a la fecha del pedido.
8. Cada línea debe tener cantidad > 0 y una planta existente y disponible. Pedir no cambia `Disponible`.

Tras grabar, la Vista muestra el número de pedido asignado. Los pedidos registrados **no se editan**.

La Vista envía las fechas con `.Date` (sin hora). Persistan `Fecha` y `FechaRetiro` del mismo modo.

### 5.3 Consulta de pedidos

Pantalla `FormConsultarPedidos` (ya hecha):

- Listado (número, fecha, solicitante, documento, fecha de retiro, cantidad de ítems), con filtro por rango de fechas **inclusivo** (ambos extremos, comparando por fecha sin hora) y/o texto contenido en el nombre del solicitante. Texto vacío: no filtrar por nombre.
- Al seleccionar un pedido, ver cabecera y detalle en **solo lectura**.
- El detalle muestra `NombreHistorico` y `Cantidad`. No muestra el nombre actual del catálogo. Si esa columna sale vacía, no se está copiando el nombre al grabar.

---

## 6. Contrato que ya usa la Vista

No cambien estas firmas ni los nombres de las propiedades de las entidades.

### `PlantaControladora`

- `List<Planta> Listar(string? filtro = null)`
- `List<Planta> ListarDisponibles()`
- `void GuardarNuevo(string codigo, string nombreComun, string especie)`
- `void Modificar(int id, string codigo, string nombreComun, string especie, bool disponible)`
- `void MarcarNoDisponible(int id)`
- `void Eliminar(int id)`

### `PedidoControladora`

- `List<Pedido> Listar(DateTime? desde = null, DateTime? hasta = null, string? solicitante = null)`
- `Pedido? ObtenerConDetalle(int id)` — debe traer las líneas del pedido
- `Pedido Registrar(DateTime fecha, string solicitanteNombre, string solicitanteDocumento, DateTime fechaRetiro, List<(int PlantaId, int Cantidad)> lineas)`

Los rechazos de negocio se informan lanzando `InvalidOperationException` con un mensaje en español. La Vista muestra `ex.Message`. No dejen que un caso previsto reviente con `NullReferenceException` u otra excepción técnica.

`ObtenerConDetalle` devuelve `null` si el id no existe. El resto de los métodos, si el id no existe, lanzan una excepción con mensaje claro.

---

## 7. Requisitos técnicos (obligatorios)

La solución, las referencias entre proyectos y los paquetes NuGet **ya están**. Verifiquen que no los saquen.

1. Tres proyectos, con este sentido de referencias: Vista → Controladora → Modelo.
   - `ViveroPedidos.Modelo` — entidades (dadas), `DbContext`, acceso a datos, migraciones.
   - `ViveroPedidos.Controladora` — validaciones y orquestación.
   - `ViveroPedidos.Vista` — Windows Forms. Proyecto de inicio. **No** instancia el `DbContext` ni escribe LINQ de persistencia.
2. Paquetes EF Core **9.0.0**: en el Modelo, `Microsoft.EntityFrameworkCore`, `SqlServer` y `Tools`. En la Vista, `Design`.
3. Cadena de conexión a LocalDB (recomendado) o SQL Server.
4. Al menos una migración y `Update-Database` aplicado.
5. `using` / dispose correcto del contexto en la capa de datos.

Comandos orientativos (Package Manager Console), con proyecto de inicio `ViveroPedidos.Vista`:

```powershell
Add-Migration Inicial -Project ViveroPedidos.Modelo -StartupProject ViveroPedidos.Vista
Update-Database -Project ViveroPedidos.Modelo -StartupProject ViveroPedidos.Vista
```

---

## 8. Entrega

- Solución que compile, con la Vista dada sin cambios de comportamiento.
- Base creada por migraciones.
- Demostración breve:
  1. Alta de 2 o 3 plantas.
  2. Marcar una como no disponible y comprobar que no aparece al armar un pedido.
  3. Registrar un pedido con varias líneas y ver el número asignado.
  4. Consultar por fechas y abrir el detalle.
  5. Renombrar una planta en el catálogo y volver a abrir el pedido: el detalle sigue mostrando el nombre histórico.

---

## 9. Criterios de evaluación (orientativos)

| Criterio | Peso relativo |
| --- | --- |
| EF y acceso a datos solo en el Modelo; la Controladora orquesta | Alto |
| `DbContext`, relaciones (cascada / restricción) y migración aplicada | Alto |
| Pedido en una transacción, número correlativo y nombre histórico | Alto |
| ABM de plantas y validaciones | Medio |
| Consulta con filtro y detalle en solo lectura | Medio |
| El contrato de la Vista sigue compilando | Medio |

La claridad visual de los formularios no se evalúa: ya están entregados. Sí se evalúa que un error de negocio llegue a pantalla con un mensaje comprensible.

---

## 10. Ayuda

- Guía: `../guia-solucion-tres-capas-ef.md`
- Ejemplo de referencia (otro dominio: facturación): `../FacturacionSimple/`
- Solución de esta práctica: `ViveroPedidos/ViveroPedidos.sln` (proyecto de inicio: `ViveroPedidos.Vista`)

---

*Misma complejidad estructural que el práctico de préstamos de biblioteca: catálogo, cabecera y detalle, con la Vista ya resuelta.*
