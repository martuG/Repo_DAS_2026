# Entity Framework — Solución en 3 capas, instalación y Code First

### Tecnologías de referencia

- **.NET 8.0** — plataforma de desarrollo y ejecución
- **C# 12** — lenguaje asociado a .NET 8
- **Entity Framework Core 9.0.0** — ORM (*Object-Relational Mapper*)
- **Windows Forms** — capa de presentación (vista)
- **SQL Server / LocalDB** — motor de base de datos

---

## Índice

1. [Presentación](#1-presentación)
2. [Arquitectura en 3 capas](#2-arquitectura-en-3-capas)
3. [Crear la solución en Visual Studio](#3-crear-la-solución-en-visual-studio)
4. [Referencias entre proyectos](#4-referencias-entre-proyectos)
5. [Instalar Entity Framework Core 9.0.0](#5-instalar-entity-framework-core-900)
6. [Code First: qué es y cómo se usa](#6-code-first-qué-es-y-cómo-se-usa)
7. [Migraciones](#7-migraciones)
8. [Flujo de trabajo recomendado](#8-flujo-de-trabajo-recomendado)
9. [Problemas frecuentes](#9-problemas-frecuentes)
10. [Conceptos clave](#10-conceptos-clave)

---

# 1. Presentación

Esta guía explica cómo armar una solución de escritorio en **tres capas** con Visual Studio, instalar **Entity Framework Core 9.0.0** sobre **.NET 8**, y trabajar con el enfoque **Code First** (incluyendo migraciones).

La estructura del curso es:


| Capa         | Proyecto                 | Tipo                 | Responsabilidad                                    |
| ------------ | ------------------------ | -------------------- | -------------------------------------------------- |
| Modelo       | `NombreApp.Modelo`       | Biblioteca de clases | Entidades, `DbContext`, acceso a datos (EF)        |
| Controladora | `NombreApp.Controladora` | Biblioteca de clases | Lógica de negocio / orquestación (usa el Modelo)   |
| Vista        | `NombreApp.Vista`        | Windows Forms        | Formularios, controles, interacción con el usuario |


> En esta organización, **todo el acceso a datos vive en el Modelo**. La Controladora no habla con SQL Server ni configura EF: delega en el Modelo.

---

# 2. Arquitectura en 3 capas

```text
┌─────────────────────────────────────┐
│  Vista (Windows Forms)              │  Formularios, eventos, validación de UI
└─────────────────┬───────────────────┘
                  │ referencia
┌─────────────────▼───────────────────┐
│  Controladora (biblioteca)          │  Reglas de negocio, orquestación
└─────────────────┬───────────────────┘
                  │ referencia
┌─────────────────▼───────────────────┐
│  Modelo (biblioteca)                │  Entidades + DbContext + acceso a datos (EF)
└─────────────────┬───────────────────┘
                  │
                  ▼
            SQL Server / LocalDB
```

### Reglas de dependencia

1. **Vista → Controladora → Modelo** (en ese sentido).
2. El **Modelo** no referencia a Controladora ni a Vista.
3. La **Controladora** no referencia a la Vista; sí referencia al Modelo para persistir y consultar.
4. La **Vista** no debería contener SQL ni configuración de EF; llama a la controladora.
5. El `**DbContext**`, las migraciones y cualquier consulta/persistencia con EF están en el **Modelo**.

---

# 3. Crear la solución en Visual Studio

Pasos con **Visual Studio 2022** (o superior) y SDK de **.NET 8**.

## 3.1 Crear la solución vacía (opcional) o el primer proyecto

1. **Archivo → Nuevo → Proyecto**.
2. Elegir **Aplicación de Windows Forms** (C#).
3. Nombre sugerido: `MiApp.Vista`.
4. Framework: **.NET 8.0**.
5. Marcar **Colocar solución y proyecto en el mismo directorio** según preferencia del curso; si se desmarca, queda una carpeta de solución y otra del proyecto.
6. Crear.

Queda la **capa Vista** y el archivo `.sln`.

## 3.2 Agregar la biblioteca del Modelo

1. Clic derecho en la **solución** → **Agregar → Nuevo proyecto**.
2. Plantilla: **Biblioteca de clases** (C#).
3. Nombre: `MiApp.Modelo`.
4. Framework: **.NET 8.0**.
5. Crear.

Se puede eliminar la clase `Class1.cs` generada por defecto. En este proyecto irán las entidades, el `DbContext` y el acceso a datos.

## 3.3 Agregar la biblioteca de la Controladora

1. Clic derecho en la **solución** → **Agregar → Nuevo proyecto**.
2. Plantilla: **Biblioteca de clases** (C#).
3. Nombre: `MiApp.Controladora`.
4. Framework: **.NET 8.0**.
5. Crear.

También se puede borrar `Class1.cs`. Aquí irá la lógica de negocio que **consume** el Modelo.

## 3.4 Verificar en el Explorador de soluciones

Debe verse algo así:

```text
MiApp (solución)
├── MiApp.Modelo          → Biblioteca de clases (.NET 8)  ← EF, DbContext, datos
├── MiApp.Controladora    → Biblioteca de clases (.NET 8)  ← negocio
└── MiApp.Vista           → Windows Forms (.NET 8)         ← UI
```

### Alternativa por línea de comandos

Desde la carpeta donde quieras la solución:

```powershell
dotnet new sln -n MiApp
dotnet new classlib -n MiApp.Modelo -f net8.0
dotnet new classlib -n MiApp.Controladora -f net8.0
dotnet new winforms -n MiApp.Vista -f net8.0
dotnet sln MiApp.sln add MiApp.Modelo MiApp.Controladora MiApp.Vista
```

---

# 4. Referencias entre proyectos

Las referencias definen qué proyecto puede usar tipos de otro.

## 4.1 Referencias necesarias


| Proyecto que referencia | Proyecto referenciado | Motivo                                                            |
| ----------------------- | --------------------- | ----------------------------------------------------------------- |
| `MiApp.Controladora`    | `MiApp.Modelo`        | La lógica de negocio usa entidades y el acceso a datos del Modelo |
| `MiApp.Vista`           | `MiApp.Controladora`  | Los formularios llaman a la controladora                          |
| `MiApp.Vista`           | `MiApp.Modelo`        | (Opcional) Si la vista necesita tipar entidades directamente      |


La Controladora **no** instala ni configura EF: solo usa clases del Modelo (por ejemplo un servicio/repositorio o el `DbContext` expuesto por esa capa).

## 4.2 Cómo agregar una referencia en Visual Studio

1. Clic derecho en el proyecto que **usa** al otro (por ejemplo `MiApp.Controladora`).
2. **Agregar → Referencia de proyecto…** (o **Agregar → Referencia…**).
3. Marcar `MiApp.Modelo`.
4. Aceptar.

Repetir para `MiApp.Vista` → `MiApp.Controladora` (y Modelo si corresponde).

### Por CLI

```powershell
dotnet add MiApp.Controladora reference MiApp.Modelo
dotnet add MiApp.Vista reference MiApp.Controladora
dotnet add MiApp.Vista reference MiApp.Modelo
```

## 4.3 Proyecto de inicio

Clic derecho en `MiApp.Vista` → **Establecer como proyecto de inicio**. Al ejecutar (F5), debe abrirse el Windows Forms.

---

# 5. Instalar Entity Framework Core 9.0.0

Entity Framework Core se instala como **paquetes NuGet**. Con **.NET 8** y **EF Core 9.0.0**, la combinación es válida: EF Core 9 puede usarse sobre .NET 8.

## 5.1 Qué paquetes instalar y en qué proyecto


| Paquete                                   | Versión   | ¿Dónde?                        | Para qué                                                  |
| ----------------------------------------- | --------- | ------------------------------ | --------------------------------------------------------- |
| `Microsoft.EntityFrameworkCore`           | **9.0.0** | **Modelo**                     | Núcleo de EF                                              |
| `Microsoft.EntityFrameworkCore.SqlServer` | **9.0.0** | **Modelo**                     | Proveedor SQL Server                                      |
| `Microsoft.EntityFrameworkCore.Tools`     | **9.0.0** | **Modelo**                     | Migraciones desde Visual Studio / Package Manager Console |
| `Microsoft.EntityFrameworkCore.Design`    | **9.0.0** | Proyecto de inicio (**Vista**) | Diseño y migraciones en tiempo de desarrollo              |


Recomendación práctica del curso:

1. Instalar **EF Core**, **SqlServer** y **Tools** en `**MiApp.Modelo`** (ahí están el `DbContext`, las entidades y el acceso a datos).
2. Instalar **Design** en `**MiApp.Vista`** (proyecto de inicio), porque las herramientas de migración suelen arrancar desde ahí.
3. La **Controladora** no necesita paquetes de EF si solo consume APIs del Modelo (clases/métodos públicos). Si en algún momento usa tipos de EF directamente, habría que agregar la referencia al paquete; lo habitual es **evitarlo**.

## 5.2 Instalación desde Visual Studio (NuGet)

1. Clic derecho en la **solución** → **Administrar paquetes NuGet para la solución…**.
2. Pestaña **Examinar**.
3. Buscar `Microsoft.EntityFrameworkCore.SqlServer`.
4. Elegir versión **9.0.0** (no la última automática si el curso fija 9.0.0).
5. Marcar el proyecto `**MiApp.Modelo**` → Instalar.
6. Repetir para:
  - `Microsoft.EntityFrameworkCore` **9.0.0** → Modelo.
  - `Microsoft.EntityFrameworkCore.Tools` **9.0.0** → Modelo.
  - `Microsoft.EntityFrameworkCore.Design` **9.0.0** → Vista (proyecto de inicio).

Aceptar licencias cuando lo pida.

### Desde Package Manager Console

```powershell
Install-Package Microsoft.EntityFrameworkCore -Version 9.0.0 -ProjectName MiApp.Modelo
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 9.0.0 -ProjectName MiApp.Modelo
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.0 -ProjectName MiApp.Modelo
Install-Package Microsoft.EntityFrameworkCore.Design -Version 9.0.0 -ProjectName MiApp.Vista
```

## 5.3 Herramientas globales de EF (opcional pero útil)

Para usar `dotnet ef` en la terminal:

```powershell
dotnet tool install --global dotnet-ef --version 9.0.0
```

Verificar:

```powershell
dotnet ef --version
```

Debe reportar una versión **9.x** alineada con los paquetes.

## 5.4 Comprobar la instalación

En el archivo `.csproj` del **Modelo** deberían aparecer referencias similares a:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
```

Compilar la solución (**Ctrl+Shift+B**). Si no hay errores de paquetes, la instalación está lista.

---

# 6. Code First: qué es y cómo se usa

## 6.1 Enfoques de trabajo con EF


| Enfoque            | Idea                                                                                                                                                               |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Code First**     | Primero se escriben las **clases C#** (entidades) y el `DbContext`. EF genera (o actualiza) la base de datos a partir del código, normalmente con **migraciones**. |
| **Database First** | Primero existe la base. Se generan entidades y contexto a partir del esquema.                                                                                      |
| **Model First**    | Histórico de EF “clásico”; en EF Core el flujo habitual es Code First + migraciones.                                                                               |


En este curso se usa **Code First**.

## 6.2 Idea central de Code First

1. Definís el dominio en C# (`Cliente`, `Producto`, etc.) **en el Modelo**.
2. Exponés conjuntos de entidades en un `DbContext` (`DbSet<T>`) **también en el Modelo**.
3. Configurás la cadena de conexión y, si hace falta, el mapeo (atributos o Fluent API).
4. Creás una **migración** que traduce el modelo a SQL (`CREATE TABLE`, etc.).
5. Aplicás la migración a la base (`database update`).

La base de datos queda **alineada con el código**, y los cambios de esquema se versionan en el proyecto Modelo (carpeta `Migrations`).

## 6.3 Ejemplo mínimo de entidades (Modelo)

En `MiApp.Modelo`:

```csharp
namespace MiApp.Modelo;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public bool Activo { get; set; } = true;
}
```

Convenciones de EF Core (resumen):

- `Id` o `NombreClaseId` → clave primaria.
- Tipos `string` → `nvarchar` (longitud según configuración).
- `decimal` → conviene precisar precisión en Fluent API si el dominio lo exige.
- Propiedades de navegación y colecciones permiten modelar relaciones 1–N, N–N, etc.

## 6.4 DbContext (Modelo)

En `MiApp.Modelo`:

```csharp
using Microsoft.EntityFrameworkCore;

namespace MiApp.Modelo;

public class AppDbContext : DbContext
{
    public DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=MiAppDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(e =>
        {
            e.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            e.Property(p => p.Precio).HasPrecision(18, 2);
        });
    }
}
```

- `OnConfiguring`: cadena de conexión (en proyectos reales conviene externalizarla; para prácticos de clase a menudo se deja aquí).
- `OnModelCreating`: configuración Fluent API (opcional si las convenciones alcanzan).

El acceso a datos (consultas, altas, bajas, modificaciones con EF) se implementa en el **Modelo**: en el propio contexto, en clases de repositorio o en métodos estáticos/helpers de esa capa.

## 6.5 Uso desde la Controladora (idea)

La Controladora orquesta la lógica y usa el Modelo; no configura EF:

```csharp
using MiApp.Modelo;

namespace MiApp.Controladora;

public class ProductoControladora
{
    public List<Producto> ListarActivos()
    {
        using var db = new AppDbContext();
        return db.Productos.Where(p => p.Activo).ToList();
    }
}
```

La Vista llama a `ProductoControladora` (u otra clase de la capa Controladora), no abre el `DbContext` directamente.

---

# 7. Migraciones

Una **migración** es un conjunto de clases generadas por EF que describen **cómo pasar** de un esquema de base de datos a otro, a partir de los cambios en el modelo C#.

## 7.1 Antes de migrar: checklist

1. Solución compila sin errores.
2. Paquetes EF **9.0.0** instalados en el **Modelo**.
3. Existe al menos una entidad y un `DbContext` con `DbSet` en el Modelo.
4. Cadena de conexión válida (LocalDB o SQL Server accesible).
5. Proyecto de inicio = **Vista**.
6. (PMC) Proyecto predeterminado = **Modelo** (donde está el `DbContext`).

## 7.2 Crear la primera migración

### Package Manager Console (Visual Studio)

Menú **Herramientas → Administrador de paquetes NuGet → Consola del Administrador de paquetes**.

```powershell
Add-Migration Inicial -Project MiApp.Modelo -StartupProject MiApp.Vista
```

- `Inicial`: nombre descriptivo de la migración (puede ser `CrearProductos`, etc.).
- `-Project`: proyecto que contiene el `DbContext` y donde se guardarán las migraciones (**Modelo**).
- `-StartupProject`: proyecto que se ejecuta al diseñar (suele ser la Vista).

Se crea una carpeta `Migrations` en el **Modelo** con archivos similares a:

```text
MiApp.Modelo/
└── Migrations/
    ├── 20260320120000_Inicial.cs
    ├── 20260320120000_Inicial.Designer.cs
    └── AppDbContextModelSnapshot.cs
```

El *snapshot* representa el modelo actual conocido por EF.

## 7.3 Aplicar la migración a la base de datos

### Package Manager Console

```powershell
Update-Database -Project MiApp.Modelo -StartupProject MiApp.Vista
```

EF crea la base (si no existe) y ejecuta el SQL de las migraciones pendientes. También crea la tabla `__EFMigrationsHistory` para saber qué migraciones ya se aplicaron.

Con LocalDB no hace falta abrir SQL Server Management Studio: la instancia `(localdb)\MSSQLLocalDB` se inicia al conectar.

## 7.4 Cambios posteriores en el modelo

Ejemplo: agregar una propiedad a `Producto`:

```csharp
public string? Codigo { get; set; }
```

Luego:

```powershell
Add-Migration AgregarCodigoProducto -Project MiApp.Modelo -StartupProject MiApp.Vista
Update-Database -Project MiApp.Modelo -StartupProject MiApp.Vista
```

## 7.5 Comandos útiles


| Acción                                    | Package Manager Console |                                   |
| ----------------------------------------- | ----------------------- | --------------------------------- |
| Nueva migración                           | `Add-Migration Nombre`  | `dotnet ef migrations add Nombre` |
| Aplicar migraciones                       | `Update-Database`       | `dotnet ef database update`       |
| Listar migraciones                        | `Get-Migration`         | `dotnet ef migrations list`       |
| Quitar última migración (si no se aplicó) | `Remove-Migration`      | `dotnet ef migrations remove`     |
| Generar script SQL                        | `Script-Migration`      | `dotnet ef migrations script`     |


Siempre indicar `-Project MiApp.Modelo` / `--project MiApp.Modelo` y `-StartupProject MiApp.Vista` / `--startup-project MiApp.Vista`.

## 7.6 `EnsureCreated` vs migraciones

- `Database.EnsureCreated()`: crea el esquema **sin** historial de migraciones. Útil para demos rápidas; **no** es el flujo recomendado para evolución del esquema.
- **Migraciones**: camino correcto en Code First para versionar y actualizar la base a lo largo del tiempo.

En el curso, priorizar **migraciones**.

---

# 8. Flujo de trabajo recomendado

Orden habitual en un práctico:

1. Crear solución: Vista (WinForms) + Modelo + Controladora (.NET 8).
2. Agregar referencias (Controladora → Modelo; Vista → Controladora / Modelo).
3. Instalar paquetes EF Core **9.0.0** en el **Modelo** (+ Design en la Vista).
4. Escribir entidades y `DbContext` en el Modelo; implementar ahí el acceso a datos.
5. `Add-Migration` / `dotnet ef migrations add` con `-Project` = Modelo.
6. `Update-Database` / `dotnet ef database update`.
7. Implementar la lógica de negocio en la Controladora (usando el Modelo).
8. Consumir la Controladora desde los formularios de la Vista.

```text
Modelo (entidades + DbContext + acceso a datos)
        │
        ▼
  Add-Migration  ──►  Migrations/ dentro del Modelo
        │
        ▼
  Update-Database ──►  SQL Server / LocalDB actualizada
        │
        ▼
  Controladora usa el Modelo  →  Vista usa la Controladora
```

---

# 9. Problemas frecuentes


| Síntoma                            | Causa probable                               | Qué revisar                                                                            |
| ---------------------------------- | -------------------------------------------- | -------------------------------------------------------------------------------------- |
| No se encuentra `dotnet ef`        | Herramienta global no instalada o PATH       | `dotnet tool install --global dotnet-ef --version 9.0.0`                               |
| “No DbContext was found”           | `-Project` apunta a Controladora u otro      | Usar `-Project MiApp.Modelo`; compilar antes                                           |
| Error al diseñar migraciones       | Falta paquete Design en el startup           | Instalar `Microsoft.EntityFrameworkCore.Design` 9.0.0 en la Vista                      |
| Conflicto de versiones de paquetes | Mezcla 8.x / 9.x / preview                   | Unificar todo en **9.0.0**                                                             |
| No conecta a LocalDB               | Instancia mal escrita o LocalDB no instalado | `(localdb)\MSSQLLocalDB`; instalar SQL Server LocalDB                                  |
| `Remove-Migration` falla           | La migración ya está aplicada en la BD       | Revertir con `Update-Database` a una migración anterior, o corregir a mano con cuidado |


---

# 10. Conceptos clave

- **Tres capas**: Modelo (entidades + EF + acceso a datos), Controladora (negocio), Vista (WinForms).
- **Referencias**: Vista → Controladora → Modelo.
- **EF Core 9.0.0** sobre **.NET 8**: paquetes NuGet en el **Modelo**; Design en el proyecto de inicio (Vista).
- **Code First**: el código C# define el modelo; la base se genera/actualiza desde ahí.
- **Migración**: cambio de esquema versionado en el Modelo; se crea con `Add-Migration` y se aplica con `Update-Database`.
- **DbContext**: vive en el Modelo; puente entre las clases y la base; expone `DbSet<T>` por entidad.
- `**DbSet<T>**`: colección consultable/persistible de una entidad.

---

## Referencias rápidas

- [Documentación EF Core](https://learn.microsoft.com/ef/core/)
- [Migraciones en EF Core](https://learn.microsoft.com/ef/core/managing-schemas/migrations/)
- [Proveedor SQL Server](https://learn.microsoft.com/ef/core/providers/sql-server/)

---

*Material orientado a prácticos de Desarrollo y Arquitectura de Software — .NET 8 + EF Core 9.0.0.*