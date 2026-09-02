# Trabajo práctico — Facturación simple

**Desarrollo y arquitectura de software**  
**Tecnologías:** .NET 8 · Windows Forms · ADO.NET modo conectado · SQL Server  

---

## 1. Objetivo

Desarrollar una aplicación de escritorio que permita emitir y consultar facturas de un comercio, persistiendo los datos en SQL Server con **ADO.NET en modo conectado**.

El trabajo evalúa:

- modelado relacional con integridad referencial;
- acceso a datos con `SqlConnection`, `SqlCommand` y `SqlDataReader`;
- uso de parámetros y transacciones;
- una interfaz WinForms coherente con el flujo maestro–detalle.

**No se permite** `SqlDataAdapter`, `DataSet`, `DataTable` como mecanismo principal de persistencia, ni ORMs (Entity Framework, Dapper, etc.).

---

## 2. Dominio

El comercio factura productos a clientes. Para este TP el cliente **no** es una entidad aparte: sus datos se copian en la cabecera de la factura al emitirla.

Hay **tres tablas**:


| Tabla            | Rol                                                    |
| ---------------- | ------------------------------------------------------ |
| `Productos`      | Catálogo (ABM completo)                                |
| `Facturas`       | Cabecera (número, fecha, datos del cliente, total)     |
| `FacturaDetalle` | Líneas (producto, cantidad, precio unitario, subtotal) |


Regla de negocio central: el **precio de venta se copia al detalle** en el momento de facturar. Si más tarde cambia el precio del producto, las facturas ya emitidas **no** se alteran.

---

## 3. Modelo de datos (mínimo obligatorio)

El script de creación debe incluir PK, FK, `NOT NULL` donde corresponda y tipos adecuados. Nombres de tablas y columnas pueden ajustarse, siempre que se respete el sentido.

### `Productos`

- `Id` (int, identity, PK)
- `Codigo` (único, no vacío)
- `Nombre`
- `Precio` (decimal, ≥ 0)
- `Activo` (bit) — un producto inactivo no puede usarse en facturas nuevas

### `Facturas`

- `Id` (int, identity, PK)
- `Numero` (único; puede ser identity o correlativo controlado por la aplicación)
- `Fecha`
- `ClienteNombre`
- `ClienteDocumento` (DNI/CUIT, texto)
- `Total` (decimal) — debe coincidir con la suma de subtotales del detalle

### `FacturaDetalle`

- `Id` (int, identity, PK)
- `FacturaId` (FK → `Facturas`, `ON DELETE` coherente: no dejar huérfanos)
- `ProductoId` (FK → `Productos`)
- `Cantidad` (int, > 0)
- `PrecioUnitario` (decimal, ≥ 0) — copia del precio al facturar
- `Subtotal` (decimal) = `Cantidad * PrecioUnitario`

No hace falta tabla `Clientes`. No hace falta stock.

---

## 4. Alcance funcional

### 4.1 Productos

- Alta, modificación y baja lógica (`Activo = 0`) o física **solo si** el producto no está en ningún detalle. Si está referenciado, informar y no eliminar.
- Listado con búsqueda por código o nombre.
- Validar código único, nombre no vacío y precio ≥ 0.

### 4.2 Emisión de factura (caso principal)

Pantalla maestro–detalle:

1. Datos de cabecera: fecha (por defecto hoy), nombre y documento del cliente.
2. Líneas: elegir producto **activo** (combo o búsqueda), cantidad, mostrar precio vigente y subtotal.
3. No permitir dos líneas con el mismo producto (sumar cantidades o rechazar, a criterio documentado).
4. Recalcular el total en pantalla a medida que se agregan o quitan líneas.
5. **Grabar cabecera y detalle en una sola transacción** (`SqlTransaction`). Si falla cualquier `INSERT`, rollback completo.
6. Al grabar, persistir `PrecioUnitario` y `Subtotal` en cada línea, y `Total` en la cabecera.
7. No emitir factura sin al menos una línea, ni con cliente vacío.

Tras grabar, mostrar el número de factura asignado.

### 4.3 Consulta de facturas

- Listado (número, fecha, cliente, total), con filtro por rango de fechas y/o texto de cliente.
- Al seleccionar una factura, ver cabecera y detalle **en solo lectura** (no se editan facturas emitidas).
- Opcional (recomendado): anulación por estado (`Anulada`) en lugar de `DELETE`. Si se implementa, no se puede anular dos veces.

### 4.4 Informe simple

Una consulta conectada que muestre, por producto, cantidad facturada y monto en un período. Resultado en grilla (no hace falta gráfico).

---

## 5. Requisitos técnicos (obligatorios)

1. **.NET 8** y proyecto **Windows Forms**.
2. **ADO.NET modo conectado** en toda persistencia y consulta:
  - `SqlConnection`
  - `SqlCommand` con parámetros (`@Nombre`, nunca concatenar valores del usuario)
  - `SqlDataReader` para lecturas
  - `SqlTransaction` en la emisión de factura
3. Abrir la conexión el menor tiempo posible; `using` o `try/finally` para cerrar.
4. Separación mínima en capas o responsabilidades:
  - UI (formularios)
  - acceso a datos (repositorios o clase `FacturaDao` / `ProductoDao`)
  - entidades o DTOs  
   Los formularios **no** deben contener SQL embebido.
5. Manejo de errores: mensajes claros al usuario; no tragar excepciones vacías. Distinguir, al menos, error de conexión y violación de unique/FK.
6. UI usable en escritorio: validación antes de guardar, estados vacíos (“no hay facturas en el período”) y confirmación en operaciones destructivas.

**Fuera de alcance:** login, roles, impresión PDF, AFIP, múltiples puntos de venta, stock, EF Core.

---

## 6. Arquitectura esperada (criterio de evaluación)

Se valora que el alumno pueda explicar:

- por qué el precio vive en el detalle y no se lee solo de `Productos` al consultar una factura vieja;
- por qué cabecera + detalle van en transacción;
- dónde se valida (UI vs. SQL vs. ambos).

Un esquema aceptable:

`Formulario → Servicio o Dao → SqlCommand → SQL Server`

---

## 7. Entregables

1. Solución .NET 8 compilable.
2. README breve: cómo crear la base, cadena de conexión y cómo ejecutar.
3. Diagrama de clases.

Fecha y medio de entrega: **18-09**-  
Trabajo **[individual / en parejas]**.
