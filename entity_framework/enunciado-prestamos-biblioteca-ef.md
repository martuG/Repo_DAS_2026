# Trabajo en clase — Préstamos de biblioteca (Entity Framework)

**Desarrollo y arquitectura de software**  
**Tecnologías:** .NET 8 · Windows Forms · Entity Framework Core 9.0.0 · SQL Server / LocalDB  
**Arquitectura:** solución en **3 capas** (Modelo · Controladora · Vista), según la guía de esta carpeta.

---

## 1. Objetivo

Desarrollar una aplicación de escritorio para una biblioteca escolar: administrar el catálogo de libros y **registrar / consultar préstamos**, persistiendo con **Entity Framework Core (Code First + migraciones)**.

El trabajo evalúa:

- solución en tres capas con referencias correctas;
- entidades, `DbContext` y acceso a datos en la capa **Modelo**;
- lógica de negocio en la **Controladora**;
- UI WinForms en la **Vista**;
- migración inicial aplicada a LocalDB (o SQL Server);
- flujo maestro–detalle con grabación atómica (transacción).

**No se permite** acceso a datos con ADO.NET “a mano” (`SqlConnection` / `SqlCommand` como mecanismo principal), ni poner el `DbContext` o consultas EF en la Vista.

Pueden mirar el ejemplo `FacturacionSimple` como referencia de arquitectura, pero el dominio y el código deben ser **propios** de este enunciado (biblioteca / préstamos).

---

## 2. Dominio

La biblioteca presta libros a alumnos. El alumno **no** es una entidad aparte: sus datos se copian en la cabecera del préstamo al registrarlo.

Hay **tres entidades / tablas**:

| Tabla / entidad     | Rol                                                                 |
| ------------------- | ------------------------------------------------------------------- |
| `Libros`            | Catálogo (ABM)                                                      |
| `Prestamos`         | Cabecera (número, fecha, datos del alumno, cantidad de ítems)       |
| `PrestamoDetalles`  | Líneas (libro, cantidad de ejemplares prestados en esa línea)       |

Regla de negocio central: al prestar, se guarda el **título del libro copiado en el detalle** (además del `LibroId`). Si más tarde se renombra el libro en el catálogo, los préstamos ya registrados **conservan** el título histórico.

---

## 3. Modelo de datos (mínimo obligatorio)

Usar Code First. Nombres pueden ajustarse si se respeta el sentido. Incluir PK, FK, longitudes razonables.

### `Libro`

- `Id` (int, identity, PK)
- `Codigo` (único, no vacío) — ej. inventario / ISBN interno
- `Titulo` (no vacío)
- `Autor` (no vacío)
- `Disponible` (bool) — un libro no disponible no puede incluirse en préstamos nuevos

### `Prestamo`

- `Id` (int, identity, PK)
- `Numero` (único; correlativo controlado por la aplicación)
- `Fecha`
- `AlumnoNombre`
- `AlumnoDocumento` (texto, ej. DNI o legajo)
- `FechaDevolucionPrevista` (date)
- `CantidadItems` (int) — debe coincidir con la suma de cantidades del detalle

### `PrestamoDetalle`

- `Id` (int, identity, PK)
- `PrestamoId` (FK → `Prestamos`, borrado en cascada coherente)
- `LibroId` (FK → `Libros`; no borrar libro si tiene detalles)
- `TituloHistorico` (texto) — copia del título al prestar
- `Cantidad` (int, > 0) — ejemplares de ese libro en el préstamo

No hace falta tabla `Alumnos`. No hace falta stock numérico complejo: alcanza con `Disponible` en el libro.

---

## 4. Alcance funcional

### 4.1 Libros

- Alta, modificación y marca de **no disponible** (`Disponible = false`).
- Eliminación física **solo si** el libro no está en ningún detalle; si está referenciado, informar y no eliminar.
- Listado con búsqueda por código, título o autor.
- Validar código único, título y autor no vacíos.

### 4.2 Registro de préstamo (caso principal)

Pantalla maestro–detalle:

1. Cabecera: fecha (por defecto hoy), nombre y documento del alumno, fecha de devolución prevista.
2. Líneas: elegir libro **disponible**, cantidad; mostrar título y autor.
3. No permitir dos líneas con el mismo libro (sumar cantidades o rechazar; documentar la decisión).
4. Recalcular `CantidadItems` en pantalla al agregar o quitar líneas.
5. **Grabar cabecera y detalle en una sola transacción**. Si falla algo, rollback completo.
6. Al grabar, persistir `TituloHistorico` (copia del título del libro) y `Cantidad` en cada línea; `CantidadItems` en la cabecera.
7. No registrar préstamo sin al menos una línea, ni con alumno vacío, ni sin fecha de devolución prevista.

Tras grabar, mostrar el número de préstamo asignado.

### 4.3 Consulta de préstamos

- Listado (número, fecha, alumno, fecha devolución prevista, cantidad de ítems), con filtro por rango de fechas y/o texto de alumno.
- Al seleccionar un préstamo, ver cabecera y detalle en **solo lectura** (no se editan préstamos registrados).

---

## 5. Requisitos técnicos (obligatorios)

1. Solución con tres proyectos:
   - `….Modelo` — biblioteca de clases (.NET 8): entidades, `DbContext`, acceso a datos, migraciones.
   - `….Controladora` — biblioteca de clases (.NET 8): validaciones y orquestación (referencia al Modelo).
   - `….Vista` — Windows Forms (.NET 8): formularios (referencia a Controladora; Modelo solo si hace falta tipar).
2. Paquetes EF Core **9.0.0** en el Modelo (`SqlServer`, `Tools`, etc.) y `Design` en la Vista (proyecto de inicio).
3. Cadena de conexión a LocalDB (recomendado) o SQL Server.
4. Al menos una migración y `Update-Database` aplicado.
5. La Vista **no** instancia el `DbContext` ni escribe LINQ de persistencia: llama a la Controladora.
6. Usar `using` / dispose correcto del contexto en la capa de datos.

---

## 6. Entrega sugerida (clase)

- Solución que compile.
- Base creada por migraciones.
- Demostración breve: alta de 2–3 libros → registrar un préstamo con varias líneas → consultar y ver el detalle.
- (Opcional) README corto con nombre de la base y cómo correr el proyecto.

---

## 7. Criterios de evaluación (orientativos)

| Criterio                                      | Peso relativo |
| --------------------------------------------- | ------------- |
| Arquitectura 3 capas y ubicación de EF        | Alto          |
| Modelo + migración correcta                   | Alto          |
| Préstamo con transacción y título histórico   | Alto          |
| ABM de libros y validaciones                  | Medio         |
| Consulta maestro–detalle                      | Medio         |
| Claridad de UI y manejo de errores            | Bajo / medio |

---

## 8. Ayuda

- Guía: `guia-solucion-tres-capas-ef.md`
- Ejemplo de referencia (otro dominio: facturación): carpeta `FacturacionSimple/`

---

*Misma complejidad estructural que la facturación simple, en un tema distinto (biblioteca / préstamos).*
