# Práctica previa — Taller  
**.NET 8 · WinForms · ADO.NET modo conectado**

## Objetivo

Armar una app de escritorio mínima para un taller: clientes, órdenes de trabajo e ítems (repuestos o tareas). La idea es practicar **conexión, comandos parametrizados, `DataReader` y una transacción**, antes del TP de facturación.

Sin `DataSet`/`DataAdapter`, sin EF, sin login.

---

## Tablas

**`Clientes`:** `Id`, `Nombre`, `Telefono`  
**`Ordenes`:** `Id`, `ClienteId` (FK), `Fecha`, `Estado` (`Abierta` / `Cerrada`)  
**`OrdenItems`:** `Id`, `OrdenId` (FK), `Descripcion`, `Cantidad`, `PrecioUnitario`, `Subtotal`

`Subtotal = Cantidad * PrecioUnitario`. El total de la orden se calcula al leer (no hace falta columna `Total`).

---

## Qué hay que hacer

1. **Clientes:** alta y listado. Baja solo si no tiene órdenes.
2. **Nueva orden:** elegir cliente (combo), fecha = hoy, estado `Abierta`. Cargar ítems en una grilla (descripción, cantidad, precio). **Guardar orden + ítems en una transacción.** Al menos un ítem. Mostrar el `Id` generado.
3. **Consulta:** listar órdenes (cliente, fecha, estado, total). Al seleccionar, ver los ítems en solo lectura.
4. **Cerrar orden:** cambiar estado a `Cerrada` (un `UPDATE` parametrizado). No editar ítems de una orden cerrada.

---

## Técnico (obligatorio)

- `SqlConnection`, `SqlCommand` con `@parametros`, `SqlDataReader`.
- `SqlTransaction` en el alta de la orden.
- Cadena de conexión fuera de los formularios.
- SQL en una clase de datos, no en el `Form`.
- `using` (o equivalente) para cerrar la conexión.