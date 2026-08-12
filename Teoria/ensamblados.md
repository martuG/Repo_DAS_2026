# Lenguajes de Última Generación

## Unidad 1 — Ensamblados y código administrado

### Tecnologías de referencia

- **.NET 8** — plataforma de desarrollo y ejecución
- **C# 12** — lenguaje asociado a .NET 8
- **Visual Studio 2022** — entorno de desarrollo integrado (**IDE**, *Integrated Development Environment*)
- **.NET CLI** — *Command Line Interface* (interfaz de línea de comandos) de .NET
- **NuGet** — gestor de paquetes de .NET
- **CIL / IL** — *Common Intermediate Language* / *Intermediate Language* (lenguaje intermedio)
- **Reflection** — reflexión (inspección de tipos en tiempo de ejecución)
- **AssemblyLoadContext** — contexto de carga de assemblies

> **Nota sobre siglas:** al final del documento hay un **glosario de siglas**. En el texto, la primera vez que aparece una sigla importante se indica su significado.

---

# 1. Introducción

Una aplicación desarrollada con .NET no se ejecuta directamente a partir del código fuente escrito en C#.

Existe un proceso de compilación y ejecución en el que intervienen diferentes componentes:

```text
Código fuente C#
       ↓
Compilador Roslyn
       ↓
CIL / IL + Metadatos
       ↓
Assembly (.dll / .exe)
       ↓
.NET Runtime (CoreCLR)
       ↓
JIT / AOT
       ↓
Código nativo
       ↓
CPU
```

Esta arquitectura se apoya en la **CLI** (*Common Language Infrastructure*, infraestructura de lenguaje común): un modelo estándar que define cómo distintos lenguajes comparten tipos, metadatos y un entorno de ejecución común.

Por eso C#, F# y Visual Basic pueden compilarse para producir código intermedio compatible con el mismo runtime de .NET. Eso es posible porque todos respetan el **CTS** (*Common Type System*, sistema de tipos común) y, cuando corresponde, el **CLS** (*Common Language Specification*, especificación de lenguaje común).

> **Importante:** el código **IL** (*Intermediate Language*) no es código nativo de la **CPU** (*Central Processing Unit*, unidad central de procesamiento). Es una representación intermedia que posteriormente puede ser transformada en código nativo durante la ejecución.

---

# 2. ¿Qué es el código administrado?

En .NET se denomina **código administrado (managed code)** al código que se ejecuta bajo la administración del **.NET Runtime**.

En .NET moderno, ese runtime se implementa principalmente como **CoreCLR** (*Common Language Runtime* de .NET Core / .NET moderno). Históricamente, en .NET Framework se hablaba del **CLR** (*Common Language Runtime*).

El runtime proporciona servicios como:

- Administración automática de memoria.
- **GC** (*Garbage Collector*, recolector de basura).
- Manejo de excepciones.
- Type safety (seguridad de tipos).
- Carga de assemblies.
- Servicios de reflexión (**Reflection**).
- Interoperabilidad con código nativo (**P/Invoke**, *Platform Invoke*).
- Ejecución del código compilado.

Esto permite que el desarrollador no tenga que administrar manualmente la memoria de la misma forma que en lenguajes como C o C++.

Por ejemplo:

```csharp
var producto = new Producto();
```

Cuando el objeto deja de ser alcanzable, el **GC** puede recuperar la memoria utilizada.

---

# 3. Del código fuente al programa ejecutable

Supongamos una aplicación sencilla:

```csharp
Console.WriteLine("Hola .NET 8");
```

Cuando compilamos el proyecto, el compilador de C# transforma el código fuente en una representación intermedia.

El resultado contiene principalmente:

1. **IL / CIL** (*Intermediate Language* / *Common Intermediate Language*)
2. **Metadatos**
3. **Información del assembly**

El archivo generado normalmente será un `.dll` (**DLL**, *Dynamic Link Library*, biblioteca de enlace dinámico).

Por ejemplo:

```text
MiAplicacion/
│
├── MiAplicacion.csproj
├── Program.cs
│
└── bin/
    └── Debug/
        └── net8.0/
            ├── MiAplicacion.dll
            ├── MiAplicacion.exe          (en Windows: apphost)
            ├── MiAplicacion.deps.json
            └── MiAplicacion.runtimeconfig.json
```

En aplicaciones ejecutables también puede aparecer un `.exe` (**EXE**, *executable*), especialmente en Windows.

En el modelo moderno de .NET, ese `.exe` suele ser el **apphost**: un ejecutable nativo pequeño cuya función es arrancar el runtime y cargar el `.dll` que contiene el código administrado (IL + metadatos). **El código managed sigue estando en la DLL**, no “dentro” del EXE como si fuera todo el programa compilado a nativo.

Además aparecen archivos de configuración asociados:

- `***.deps.json**`: describe las dependencias del assembly (qué paquetes y assemblies se necesitan en tiempo de ejecución).
- `***.runtimeconfig.json**`: indica, entre otras cosas, el framework de destino y opciones del runtime.

---

# 4. El compilador de C#

En .NET moderno, el compilador de C# utilizado por Visual Studio es **Roslyn**.

Roslyn transforma el código fuente C# en:

```text
C# → IL/CIL + Metadata
```

Por ejemplo:

```csharp
int a = 10;
int b = 20;

Console.WriteLine(a + b);
```

no se convierte directamente en instrucciones específicas para un procesador Intel o AMD.

Primero se genera IL.

Posteriormente, durante la ejecución, el runtime puede utilizar el compilador **JIT** (*Just-In-Time*, “justo a tiempo”) para transformar ese IL en código nativo.

```text
C#
 ↓
Roslyn
 ↓
IL + Metadata
 ↓
.NET Runtime
 ↓
JIT
 ↓
Código nativo
 ↓
CPU
```

---

# 5. CIL / IL

El término histórico **MSIL** (*Microsoft Intermediate Language*) todavía puede encontrarse en documentación antigua.

Actualmente es más apropiado utilizar:

- **IL** — *Intermediate Language* (lenguaje intermedio)
- **CIL** — *Common Intermediate Language* (lenguaje intermedio común)

El IL es una representación intermedia independiente de una CPU específica.

Esto permite que un mismo assembly pueda ejecutarse en diferentes plataformas siempre que exista un runtime de .NET compatible.

Por ejemplo:

```text
C# source
   ↓
IL
   ↓
┌───────────────┐
│ .NET Runtime  │
└───────────────┘
   ↓
Código nativo
   ↓
CPU / Sistema operativo
```

### Importante

No debemos decir que:

> "El IL es comprendido directamente por la CPU."

Esto es incorrecto.

La CPU ejecuta código nativo. El runtime de .NET interviene entre el IL y la ejecución del procesador.

---

# 6. JIT — Just-In-Time Compilation

El **JIT** (*Just-In-Time Compilation*, compilación justo a tiempo) es uno de los mecanismos utilizados por .NET para convertir IL en código nativo.

Cuando una aplicación se ejecuta, el runtime puede compilar determinados métodos a código nativo cuando son necesarios.

De forma simplificada:

```text
Assembly
   ↓
IL
   ↓
JIT
   ↓
Código máquina
```

Esto permite que el mismo IL pueda utilizarse en diferentes arquitecturas.

Por ejemplo:

```text
                    IL
                     │
          ┌──────────┴──────────┐
          ↓                     ↓
       Windows                Linux
          ↓                     ↓
       JIT x64              JIT ARM64
          ↓                     ↓
      Código nativo        Código nativo
```

---

# 7. AOT y ReadyToRun

Además del JIT, las versiones modernas de .NET también permiten escenarios de **AOT** (*Ahead-of-Time compilation*, compilación anticipada).

En AOT parte o todo el código puede compilarse a código nativo **antes** de ejecutar la aplicación.

Esto puede resultar útil en determinados escenarios donde se busca:

- Menor tiempo de inicio.
- Menor consumo de memoria.
- Distribución como aplicación nativa.
- Determinados escenarios de despliegue.

### Distinción importante

No todo lo que “precompila” es lo mismo:


| Mecanismo              | Significado                           | Idea clave                                                                                                  |
| ---------------------- | ------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| **JIT**                | *Just-In-Time*                        | Compila IL → nativo **durante** la ejecución                                                                |
| **Native AOT**         | *Ahead-of-Time* nativo (`PublishAot`) | Genera un ejecutable nativo en la publicación; **no hay JIT** en runtime                                    |
| **R2R** (*ReadyToRun*) | Precompilación parcial                | Incluye código nativo preparado de antemano, pero **sigue existiendo el runtime** y puede intervenir el JIT |


Por lo tanto, el modelo actual no debe resumirse únicamente como:

```text
IL → JIT → código nativo
```

sino como:

```text
IL
│
├── JIT  → código nativo durante la ejecución
│
├── R2R  → nativo preparado + runtime (posible JIT restante)
│
└── Native AOT → código nativo durante la publicación (sin JIT)
```

---

# 8. Metadatos

Los assemblies contienen **metadatos** que describen los tipos definidos dentro del assembly.

Los metadatos pueden contener información relacionada con:

- Clases.
- Structs.
- Interfaces.
- Métodos.
- Propiedades.
- Campos.
- Parámetros.
- Atributos.
- Referencias a otros assemblies.

Por ejemplo:

```csharp
public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = "";

    public decimal Precio { get; set; }
}
```

El assembly contiene información que permite al runtime conocer que existe:

```text
Producto
 ├── Id : int
 ├── Nombre : string
 └── Precio : decimal
```

Estos metadatos son fundamentales para mecanismos como **Reflection**.

---

# 9. Reflection

**Reflection** permite inspeccionar información sobre assemblies, tipos y miembros durante la ejecución.

Por ejemplo:

```csharp
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

Console.WriteLine(assembly.FullName);
```

También podemos obtener los tipos definidos:

```csharp
Type[] tipos = assembly.GetTypes();

foreach (Type tipo in tipos)
{
    Console.WriteLine(tipo.FullName);
}
```

Reflection es utilizada, entre otros escenarios, por:

- Frameworks.
- Serializadores.
- Sistemas de inyección de dependencias (**DI**, *Dependency Injection*).
- **ORM** (*Object-Relational Mapper*, mapeador objeto-relacional).
- Frameworks de testing.
- Sistemas de plugins.
- Herramientas de diagnóstico.
- **API** (*Application Programming Interface*) frameworks que descubren controladores o endpoints.

---

# 10. ¿Qué es un Assembly?

Un **assembly** es una unidad fundamental de distribución, versionado y carga en .NET.

Puede contener:

- Código IL.
- Metadatos.
- Manifest.
- Recursos.
- Referencias a otros assemblies.

En un proyecto moderno de .NET, lo habitual es que cada proyecto genere un assembly.

Por ejemplo:

```text
MiAplicacion.csproj
        ↓
MiAplicacion.dll
```

Una biblioteca:

```text
BibliotecaProductos.csproj
        ↓
BibliotecaProductos.dll
```

Una aplicación puede utilizar la biblioteca:

```text
Aplicacion
   │
   ├── Aplicacion.dll
   │
   └── BibliotecaProductos.dll
```

---

# 11. DLL y EXE

En .NET podemos encontrar principalmente:

### DLL — *Dynamic Link Library*

Una `.dll` normalmente representa una biblioteca (o el assembly managed principal de la app) que puede ser utilizada por otros componentes o cargada por el runtime.

Ejemplo:

```text
Productos.dll
MiAplicacion.dll
```

### EXE — *Executable*

Un `.exe` representa un ejecutable, particularmente en Windows.

Sin embargo, en el modelo moderno de .NET no conviene pensar simplemente:

> EXE = aplicación / DLL = biblioteca

La forma de ejecución depende también del tipo de proyecto y del modelo de publicación.

En Windows, el `.exe` generado al compilar una app de consola suele ser el **apphost** (arranque nativo), mientras que el código administrado vive en el `.dll` homónimo.

Por ejemplo, una aplicación .NET puede ejecutarse mediante:

```bash
dotnet MiAplicacion.dll
```

o publicarse como una aplicación ejecutable para una plataforma determinada.

---

# 12. Estructura de un Assembly

Un assembly administrado contiene información estructurada que incluye, entre otros elementos:

```text
Assembly
│
├── PE information
├── Metadata
├── Manifest
├── IL / CIL
├── Resources
└── References
```

El formato físico de los assemblies administrados está basado en **PE** (*Portable Executable*) / **COFF** (*Common Object File Format*).

> **Matiz:** aunque PE se asocia históricamente a Windows, los assemblies .NET **utilizan el formato PE/COFF también en Linux y macOS**. Es el contenedor del archivo; no implica que el sistema operativo sea Windows.

El **manifest** describe información importante del assembly, incluyendo:

- Nombre.
- Versión.
- Cultura.
- Referencias.
- Identidad del assembly.
- Archivos asociados, cuando corresponde.

### Identidad del assembly

La identidad de un assembly suele caracterizarse por:

```text
Nombre
Versión
Cultura
PublicKeyToken   (si está firmado con Strong Name)
```

Esa identidad permite al runtime distinguir assemblies y resolver dependencias de forma coherente.

---

# 13. Manifest

El **manifest** forma parte de los metadatos del assembly y contiene información que permite identificarlo y describir sus dependencias.

Entre otros datos puede encontrarse:

```text
Assembly Name
Version
Culture
Referenced Assemblies
Modules
```

Una aplicación puede utilizar diferentes assemblies:

```text
MiAplicacion.dll
      │
      ├── System.Runtime
      ├── MiBiblioteca.dll
      └── OtraBiblioteca.dll
```

El runtime utiliza esta información para resolver las dependencias necesarias.

---

# 14. Proyectos modernos de .NET

Una diferencia importante respecto de versiones antiguas es el formato de proyecto.

Un proyecto actual de .NET 8 utiliza normalmente un archivo `.csproj` basado en el formato **SDK-style**.

**SDK** significa *Software Development Kit* (kit de desarrollo de software): el SDK de .NET incluye compiladores, plantillas, herramientas CLI y runtime de desarrollo.

Ejemplo:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

### Elementos importantes

```xml
<TargetFramework>net8.0</TargetFramework>
```

indica que el proyecto está destinado a .NET 8.

```xml
<ImplicitUsings>enable</ImplicitUsings>
```

permite disponer automáticamente de determinados `using`.

```xml
<Nullable>enable</Nullable>
```

activa el análisis de referencias anulables de C#.

---

# 15. Visual Studio 2022

Para trabajar con .NET 8 podemos utilizar **Visual Studio 2022** con el SDK correspondiente instalado.

Un flujo habitual es:

```text
Visual Studio 2022
       ↓
Crear proyecto
       ↓
Seleccionar .NET 8
       ↓
Escribir código C#
       ↓
Build
       ↓
Assembly
```

También podemos trabajar desde la terminal mediante la **CLI** de .NET (*Command Line Interface*).

Por ejemplo:

```bash
dotnet new console -n MiAplicacion
```

Ingresamos al proyecto:

```bash
cd MiAplicacion
```

Compilamos:

```bash
dotnet build
```

Ejecutamos:

```bash
dotnet run
```

---

# 16. Carpetas bin y obj

Al trabajar con Visual Studio o `dotnet build` aparecen generalmente dos carpetas:

```text
bin/
obj/
```

### bin

Contiene los resultados de compilación.

Por ejemplo:

```text
bin/
└── Debug/
    └── net8.0/
        ├── MiAplicacion.dll
        ├── MiAplicacion.deps.json
        └── MiAplicacion.runtimeconfig.json
```

### obj

Contiene archivos intermedios utilizados durante el proceso de compilación.

Normalmente estas carpetas no deben versionarse en Git.

Por eso es habitual incluir:

```text
bin/
obj/
```

en `.gitignore`.

---

# 17. Referencias entre proyectos

Una aplicación puede depender de otras bibliotecas.

Por ejemplo:

```text
Tienda.Api
     │
     ├── Tienda.Core
     ├── Tienda.Infrastructure
     └── Tienda.Application
```

Cada proyecto puede generar su propio assembly:

```text
Tienda.Api.dll
Tienda.Core.dll
Tienda.Infrastructure.dll
Tienda.Application.dll
```

Esto permite dividir una aplicación en componentes con responsabilidades diferentes.

---

# 18. NuGet

En .NET moderno, una de las principales formas de incorporar bibliotecas externas es mediante **NuGet** (el gestor de paquetes oficial del ecosistema .NET).

Por ejemplo:

```bash
dotnet add package Newtonsoft.Json
```

o desde Visual Studio:

```text
Proyecto
   ↓
Manage NuGet Packages
```

Las dependencias quedan registradas en el `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="..." />
</ItemGroup>
```

Esto reemplaza la necesidad de administrar manualmente muchas bibliotecas DLL.

Al restaurar y publicar, esas dependencias se reflejan también en archivos como `*.deps.json`.

---

# 19. Carga de assemblies

En la mayoría de las aplicaciones no necesitamos cargar manualmente los assemblies.

El runtime resuelve y carga las dependencias declaradas por la aplicación.

Sin embargo, existen escenarios donde necesitamos cargar assemblies dinámicamente.

Por ejemplo:

```text
Aplicación
     │
     ├── PluginVentas.dll
     ├── PluginReportes.dll
     └── PluginImportacion.dll
```

Esto puede utilizarse para implementar arquitecturas basadas en plugins.

---

# 20. AssemblyLoadContext

En el .NET moderno, el mecanismo relevante para trabajar con carga y descarga dinámica de assemblies es:

```csharp
System.Runtime.Loader.AssemblyLoadContext
```

Esto reemplaza en gran medida las estrategias que antiguamente se realizaban mediante múltiples AppDomains.

Por ejemplo, conceptualmente:

```text
Proceso
│
└── AppDomain único
      │
      ├── AssemblyLoadContext
      │       ├── PluginA.dll
      │       └── dependencias
      │
      └── aplicación principal
```

Un `AssemblyLoadContext` puede ser **collectible**, permitiendo descargar los assemblies asociados cuando ya no son necesarios.

Este mecanismo es especialmente interesante para:

- Plugins.
- Extensiones.
- Carga dinámica.
- Sistemas que necesitan aislar dependencias.
- Aplicaciones extensibles.

.NET moderno utiliza `AssemblyLoadContext` para estos escenarios. La creación de nuevos AppDomains mediante `AppDomain.CreateDomain` **no está disponible desde .NET Core** (y por lo tanto tampoco en .NET 5+, .NET 6, .NET 8, etc.).

---

# 21. AppDomain: concepto histórico

Este es uno de los puntos que debemos modificar significativamente respecto del material original.

En **.NET Framework**, `AppDomain` permitía crear diferentes dominios de aplicación dentro de un mismo proceso.

Por ejemplo:

```text
Proceso
│
├── AppDomain 1
│
├── AppDomain 2
│
└── AppDomain 3
```

Esto permitía determinados escenarios de aislamiento y descarga de assemblies.

Sin embargo, este modelo **no se aplica a .NET 8**.

.NET moderno soporta un único AppDomain por proceso y no permite crear nuevos dominios mediante `AppDomain.CreateDomain`.

Por lo tanto, el siguiente código del material original:

```csharp
AppDomain.CreateDomain(...)
```

**no debe utilizarse como ejemplo de .NET 8**.

Para cargar y descargar assemblies dinámicamente se debe estudiar:

```csharp
AssemblyLoadContext
```

Si se necesita un aislamiento fuerte entre componentes, actualmente puede ser más apropiado utilizar:

```text
Proceso independiente
        o
Contenedor
```

---

# 22. ¿Qué hacer con AppDomain.CurrentDomain?

Aunque no podemos crear nuevos AppDomains en .NET 8, la clase `AppDomain` todavía existe y algunas de sus **API** (*Application Programming Interface*, interfaces de programación) siguen estando disponibles.

Por ejemplo:

```csharp
Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
```

Esto permite obtener información del dominio de aplicación actual.

Sin embargo, para un curso moderno no deberíamos presentar `AppDomain` como mecanismo de aislamiento o como una tecnología que debemos utilizar habitualmente.

Debe estudiarse principalmente como:

> **Concepto histórico de .NET Framework y API parcialmente compatible mantenida en .NET moderno.**

---

# 23. Strong Name y firma de assemblies

El material original presenta el uso de `sn.exe` y las claves `.snk`.

En .NET Framework, los **Strong Names** (nombres fuertes / firmas fuertes) tuvieron un papel importante en la **identidad** de assemblies y en escenarios de despliegue compartido.

En .NET moderno, el Strong Name **sigue existiendo** como parte de la identidad del assembly (por ejemplo, el `PublicKeyToken`), pero **no** debe confundirse con:

```text
Strong Name
≠
Certificado digital
≠
Mecanismo de seguridad general
≠
Protección del código fuente
```

Una firma fuerte no debe presentarse como una solución general para proteger una aplicación ni como el mecanismo cotidiano de confianza en el despliegue moderno.

Para escenarios actuales de seguridad y autenticidad del software pueden intervenir otros mecanismos, como:

- Firma de código.
- Certificados.
- Firmas de paquetes NuGet.
- Seguridad de la cadena de suministro.
- Repositorios confiables.

Por eso, `sn.exe` no debería ser presentado como una herramienta de uso cotidiano en proyectos .NET 8.

---

# 24. GAC

El **GAC** (*Global Assembly Cache*, caché global de assemblies) es otro concepto asociado principalmente a .NET Framework.

El material original presenta:

```bash
gacutil /i modulo.dll
```

como una forma de instalar assemblies globalmente.

Esto **no representa el flujo habitual de .NET 8**.

En aplicaciones modernas de .NET se utiliza principalmente:

- NuGet.
- Dependencias del proyecto.
- `.deps.json`.
- Directorios de publicación.
- Runtime compartido.
- Aplicaciones self-contained.

Por lo tanto, el GAC puede estudiarse como antecedente histórico de .NET Framework, pero no debería enseñarse como mecanismo habitual para desplegar bibliotecas en .NET 8.

---

# 25. Modelos de publicación: FDD y self-contained

Al publicar una aplicación con `dotnet publish`, conviene distinguir dos modelos frecuentes:

### Framework-dependent (FDD)

**FDD** (*Framework-Dependent Deployment*): la aplicación se publica **asumiendo que el runtime de .NET ya está instalado** en la máquina destino.

- Paquete más pequeño.
- Depende de que exista el runtime compatible (por ejemplo, .NET 8).

### Self-contained (SCD)

**SCD** (*Self-Contained Deployment*): la publicación **incluye el runtime** junto con la aplicación.

- Paquete más grande.
- Facilita ejecutar la app en máquinas sin .NET preinstalado.

También existen opciones relacionadas, como publicación en un solo archivo (**single-file**) o **Native AOT**, que se eligen según el escenario de despliegue.

Estos modelos modernos reemplazan, en la práctica diaria, enfoques históricos como instalar bibliotecas en el GAC.

---

# 26. Herramientas actuales

El material original presenta herramientas como:

- `SN` — Strong Name tool (firma / identidad fuerte de assemblies)
- `FUSLOGVW` — *Fusion Log Viewer* (visor histórico de binding de assemblies en .NET Framework)
- `CSC` — *C# Compiler* (compilador de C#; hoy se usa principalmente a través de Roslyn / `dotnet build`)
- `GACUTIL` — utilidad del GAC
- `ILDASM` — *IL Disassembler* (desensamblador de IL)

Algunas siguen siendo relevantes conceptualmente, pero varias pertenecen al ecosistema de **.NET Framework**.

En .NET 8 es más importante conocer:

### .NET CLI

```bash
dotnet build
dotnet run
dotnet test
dotnet publish
dotnet restore
dotnet add package
```

### Visual Studio 2022

Permite:

- Compilar.
- Depurar.
- Administrar NuGet.
- Analizar proyectos.
- Administrar referencias.
- Inspeccionar código.
- Ejecutar tests.
- Utilizar herramientas de diagnóstico.

### ILSpy

Una herramienta muy utilizada actualmente para inspeccionar y descompilar assemblies .NET.

### ILDASM — *IL Disassembler*

También puede utilizarse para inspeccionar IL y manifest de assemblies. Microsoft documenta su utilización para visualizar información CIL de un assembly.

---

# 27. Inspección de un Assembly

Una vez compilado un proyecto:

```bash
dotnet build
```

podemos encontrar el assembly en:

```text
bin/Debug/net8.0/
```

Por ejemplo:

```text
MiAplicacion.dll
```

Ese archivo puede analizarse mediante herramientas de inspección.

Podemos visualizar:

```text
Assembly
│
├── Manifest
├── References
├── Types
│   ├── Classes
│   ├── Interfaces
│   └── Structs
│
└── Methods
    └── IL
```

Esto permite comprender que el assembly no contiene simplemente "el código fuente compilado", sino información estructurada que el runtime utiliza para ejecutar y trabajar con el programa.

---

# 28. ¿El código IL puede recuperarse?

El IL puede ser inspeccionado y, en muchos casos, descompilado para obtener una representación cercana al código fuente original.

Por ejemplo:

```text
Código fuente
     ↓
   C#
     ↓
   IL
     ↓
 Assembly
     ↓
ILSpy / ILDASM
     ↓
Inspección / descompilación
```

Esto significa que publicar una aplicación .NET **no implica que el código sea imposible de analizar**.

Por este motivo, no se debe considerar que compilar a IL constituye por sí mismo un mecanismo de protección del código fuente.

También es importante evitar la afirmación del material original de que el IL es "código abierto" por ser accesible: **open source** significa que el código fuente se publica bajo una licencia determinada; que un assembly pueda inspeccionarse o descompilarse no convierte al software en open source.

---

# 29. C# 12 y .NET 8

.NET 8 utiliza por defecto C# 12 en los proyectos correspondientes.

Entre las características relevantes de C# 12 se encuentran:

### Primary Constructors

```csharp
public class Producto(string nombre, decimal precio)
{
    public string Nombre { get; } = nombre;
    public decimal Precio { get; } = precio;
}
```

### Collection Expressions

```csharp
int[] numeros = [1, 2, 3, 4, 5];
```

Estas características forman parte de la evolución del lenguaje y pueden utilizarse en proyectos .NET 8 con Visual Studio 2022.

Sin embargo, estas funcionalidades pertenecen al **lenguaje C#** y no deben confundirse con el funcionamiento interno de los assemblies.

---

# 30. Ejemplo integrador

Consideremos una solución:

```text
SistemaVentas
│
├── SistemaVentas.Api
├── SistemaVentas.Application
├── SistemaVentas.Domain
└── SistemaVentas.Infrastructure
```

Cada proyecto puede generar un assembly:

```text
SistemaVentas.Api.dll
SistemaVentas.Application.dll
SistemaVentas.Domain.dll
SistemaVentas.Infrastructure.dll
```

El flujo general sería:

```text
Código C#
       ↓
Compilador Roslyn
       ↓
IL + Metadatos
       ↓
Assemblies
       ↓
.NET Runtime
       ↓
JIT / AOT
       ↓
Código nativo
       ↓
Sistema operativo
       ↓
CPU
```

Y las dependencias:

```text
Api
 │
 ├── Application
 │      │
 │      └── Domain
 │
 └── Infrastructure
        │
        └── Domain
```

---

# 31. Conceptos clave para recordar


| Concepto                | Definición / sigla                                                             |
| ----------------------- | ------------------------------------------------------------------------------ |
| **.NET 8**              | Plataforma moderna de desarrollo y ejecución de aplicaciones                   |
| **C# 12**               | Versión del lenguaje C# asociada a .NET 8                                      |
| **CLI**                 | *Common Language Infrastructure* — infraestructura común de lenguajes          |
| **CTS**                 | *Common Type System* — sistema de tipos común                                  |
| **CLS**                 | *Common Language Specification* — especificación común entre lenguajes         |
| **CLR / CoreCLR**       | *Common Language Runtime* — motor de ejecución (.NET Framework / .NET moderno) |
| **Roslyn**              | Compilador moderno de C#                                                       |
| **IL / CIL**            | *Intermediate Language* / *Common Intermediate Language*                       |
| **Assembly**            | Unidad de **distribución, versionado y carga**                                 |
| **Metadata**            | Información que describe los tipos y miembros                                  |
| **Manifest**            | Información que identifica y describe el assembly                              |
| **PE / COFF**           | *Portable Executable* / *Common Object File Format* — formato del archivo      |
| **Runtime**             | Entorno que ejecuta aplicaciones .NET                                          |
| **JIT**                 | *Just-In-Time* — compila IL a nativo durante la ejecución                      |
| **AOT**                 | *Ahead-of-Time* — compila a nativo antes de la ejecución                       |
| **R2R**                 | *ReadyToRun* — precompilación parcial con runtime                              |
| **GC**                  | *Garbage Collector* — recolector automático de memoria                         |
| **Reflection**          | Permite inspeccionar tipos y assemblies durante la ejecución                   |
| **NuGet**               | Sistema de paquetes y dependencias para .NET                                   |
| **SDK**                 | *Software Development Kit*                                                     |
| **FDD / SCD**           | *Framework-Dependent* / *Self-Contained Deployment*                            |
| **AssemblyLoadContext** | Mecanismo moderno para carga y descarga de assemblies                          |
| **AppDomain**           | Concepto histórico de aislamiento de .NET Framework                            |
| **GAC**                 | *Global Assembly Cache* — mecanismo principalmente de .NET Framework           |


---

# 32. Actividad práctica

## Objetivo

Comprender la relación entre:

```text
Código fuente
→ Compilación
→ Assembly
→ IL
→ Runtime
→ Ejecución
```

## Actividad

1. Crear una aplicación de consola utilizando Visual Studio 2022.
2. Seleccionar **.NET 8**.
3. Crear una clase:

```csharp
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
}
```

1. Compilar la aplicación.
2. Localizar:

```text
bin/Debug/net8.0/
```

1. Identificar el assembly generado.
2. Abrir el assembly utilizando una herramienta de inspección como ILSpy o ILDASM.
3. Identificar:
  - Nombre del assembly.
  - Versiones.
  - Referencias.
  - Clase `Producto`.
  - Propiedades.
  - Métodos.
  - IL.
4. Utilizar Reflection para obtener información sobre el assembly:

```csharp
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();

Console.WriteLine($"Assembly: {assembly.GetName().Name}");

foreach (Type type in assembly.GetTypes())
{
    Console.WriteLine(type.FullName);
}
```

1. Responder:

### Preguntas

**a.** ¿Qué diferencia existe entre el código fuente y el IL?

**b.** ¿Qué información contienen los metadatos?

**c.** ¿Qué función cumple el runtime?

**d.** ¿Qué diferencia existe entre JIT y AOT?

**e.** ¿Qué es un assembly?

**f.** ¿Qué diferencia existe entre una DLL y un EXE en el contexto de .NET?

**g.** ¿Para qué sirve Reflection?

**h.** ¿Por qué AppDomain no debe utilizarse como mecanismo moderno para crear dominios aislados en .NET 8?

**i.** ¿Qué tecnología utilizaríamos actualmente para cargar y descargar assemblies dinámicamente?

**j.** ¿Qué función cumple NuGet?

**k.** ¿Qué diferencia hay entre una publicación *framework-dependent* (FDD) y *self-contained* (SCD)?

**l.** ¿Qué significan las siglas IL, JIT, AOT y GC?

---

# 33. Síntesis final

El concepto fundamental de esta unidad puede resumirse de la siguiente manera:

```text
                 DESARROLLO
                     │
                     ↓
                 Código C#
                     │
                     ↓
                 Compilador
                  Roslyn
                     │
                     ↓
               IL + Metadata
                     │
                     ↓
                  Assembly
                     │
                     ↓
               .NET Runtime
                     │
              ┌──────┴──────┐
              ↓             ↓
             JIT            AOT
              ↓             ↓
       Código nativo   Código nativo
              │             │
              └──────┬──────┘
                     ↓
                    CPU
```

El **assembly** es una pieza central del modelo de ejecución y distribución de .NET. Contiene IL, metadatos y otra información necesaria para que el runtime pueda cargar y ejecutar el código.

Mientras que conceptos como:

```text
GAC
FUSLOGVW
CreateDomain
AppDomain isolation
```

deben considerarse principalmente desde una perspectiva histórica de **.NET Framework**, no como herramientas centrales del desarrollo actual.

---

# 34. Glosario de siglas


| Sigla                       | Inglés                             | Significado en español / uso en .NET                                      |
| --------------------------- | ---------------------------------- | ------------------------------------------------------------------------- |
| **AOT**                     | Ahead-of-Time                      | Compilación anticipada a código nativo                                    |
| **API**                     | Application Programming Interface  | Interfaz de programación de aplicaciones                                  |
| **BCL**                     | Base Class Library                 | Biblioteca de clases base de .NET                                         |
| **CIL**                     | Common Intermediate Language       | Lenguaje intermedio común                                                 |
| **CLI** *(infraestructura)* | Common Language Infrastructure     | Infraestructura común que permite varios lenguajes sobre el mismo runtime |
| **CLI** *(herramientas)*    | Command Line Interface             | Interfaz de línea de comandos (`dotnet`)                                  |
| **CLR**                     | Common Language Runtime            | Motor de ejecución histórico (.NET Framework)                             |
| **CLS**                     | Common Language Specification      | Reglas comunes para interoperar entre lenguajes .NET                      |
| **COFF**                    | Common Object File Format          | Formato de objeto asociado al contenedor PE                               |
| **CoreCLR**                 | Common Language Runtime (moderno)  | Implementación del runtime en .NET Core / .NET 5+                         |
| **CPU**                     | Central Processing Unit            | Unidad central de procesamiento                                           |
| **CSC**                     | C# Compiler                        | Compilador de C#                                                          |
| **CTS**                     | Common Type System                 | Sistema de tipos común                                                    |
| **DI**                      | Dependency Injection               | Inyección de dependencias                                                 |
| **DLL**                     | Dynamic Link Library               | Biblioteca de enlace dinámico                                             |
| **EXE**                     | Executable                         | Archivo ejecutable                                                        |
| **FDD**                     | Framework-Dependent Deployment     | Publicación dependiente del runtime instalado                             |
| **GAC**                     | Global Assembly Cache              | Caché global de assemblies (.NET Framework)                               |
| **GC**                      | Garbage Collector                  | Recolector de basura / memoria automática                                 |
| **IDE**                     | Integrated Development Environment | Entorno de desarrollo integrado (p. ej. Visual Studio)                    |
| **IL**                      | Intermediate Language              | Lenguaje intermedio                                                       |
| **ILDASM**                  | IL Disassembler                    | Herramienta para ver IL / metadatos                                       |
| **JIT**                     | Just-In-Time                       | Compilación a nativo durante la ejecución                                 |
| **MSIL**                    | Microsoft Intermediate Language    | Nombre histórico del IL                                                   |
| **ORM**                     | Object-Relational Mapper           | Mapeador objeto-relacional                                                |
| **PE**                      | Portable Executable                | Formato de archivo de assemblies .NET                                     |
| **P/Invoke**                | Platform Invoke                    | Invocación de código nativo desde managed code                            |
| **R2R**                     | ReadyToRun                         | Precompilación parcial con runtime                                        |
| **SCD**                     | Self-Contained Deployment          | Publicación que incluye el runtime                                        |
| **SDK**                     | Software Development Kit           | Kit de desarrollo de software                                             |
| **SN**                      | Strong Name (tool)                 | Herramienta / concepto de firma fuerte de assemblies                      |


> **Cuidado con la ambigüedad de CLI:** en este material, **“.NET CLI”** = *Command Line Interface* (`dotnet`). **“CLI”** como infraestructura del estándar = *Common Language Infrastructure*. El contexto indica cuál es.

