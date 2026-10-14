# Arquitectura en tres capas (MVC)


| Capa            | Papel                            | En este sistema                          |
| --------------- | -------------------------------- | ---------------------------------------- |
| **Vista**       | Lo que el mostrador ve y toca    | Formularios Windows Forms                |
| **Controlador** | Casos de uso y reglas de negocio | Clases que la vista llama                |
| **Modelo**      | Entidades y persistencia         | Clases de dominio, más ADO.NET o EF Core |


---

## Dirección de las dependencias

```text
Vista  →  Controlador  →  Modelo  →  SQL Server
```

- La vista referencia al controlador.
- El controlador referencia al modelo.
- El modelo no referencia al controlador ni a la vista.
- Un formulario no abre una `SqlConnection`, no instancia un `DbContext` y no escribe una regla de cupo en el click del botón.

Tres proyectos de biblioteca y uno de WinForms es el esquema objetivo:

```text
LaOlla.sln
  LaOlla.Modelo
  LaOlla.Controlador
  LaOlla.Vista          ← proyecto de inicio
```

---

## Qué hace cada capa con una reserva

1. La **vista** toma los datos que el relevamiento haya definido, llama a un método del controlador y muestra el mensaje que vuelve. También muestra el cupo que el controlador le informa.
2. El **controlador** aplica las reglas relevadas (cupo, duplicados, horarios, estados). Si una regla falla, devuelve un resultado con `Ok = false` y un mensaje para el mostrador. Si pasa, le pide al modelo que guarde.
3. El **modelo** representa `Menu` y `Reserva` (los nombres los define el equipo) y persiste. Puede rechazar un dato imposible de guardar (nulo, clave duplicada). La regla de negocio que el cliente contó en la entrevista no se esconde solo en un `CHECK` de SQL: el controlador también la conoce y devuelve un mensaje propio.

Un resultado simple alcanza:

```csharp
public class Resultado
{
    public bool Ok { get; init; }
    public string Mensaje { get; init; } = "";
}
```

```csharp
public class ReservaController
{
    public Resultado Reservar(/* datos que el relevamiento haya definido */)
    {
        // acá se aplican las reglas R1, R2, R3…
        // acá se llama al modelo para leer el cupo y para guardar
    }
}
```

Los parámetros salen del relevamiento. No los inventen en la firma «para ir adelantando» antes de la entrevista: pueden dejar el método vacío y completarlo después.

---

## ADO.NET o EF Core

Las dos opciones son válidas. El equipo elige **una** y la escribe en la plantilla.

**ADO.NET** conviene si el equipo ya lo usó en el taller o en el trabajo de facturación. Modo conectado o desconectado, a elección. Comandos con parámetros, conexión abierta el menor tiempo posible, `using` para cerrarla. Si una reserva implica leer el cupo y guardar, esas operaciones van en una transacción.

**EF Core** conviene si alguien del equipo ya lo usó y puede dejar la base creada durante la primera media hora. El `DbContext` y las entidades viven en el modelo. El controlador no devuelve consultas abiertas a la vista: devuelve objetos o listas ya materializadas, y un `Resultado` cuando la operación puede fallar por una regla.

En los dos casos la cadena de conexión está fuera de los formularios (`App.config`, `appsettings.json` o una clase del modelo leída al iniciar).

---

## Qué se considera una capa de más

- Un formulario que valida el cupo y, además, un controlador que lo vuelve a validar: está bien. La vista puede evitar el viaje para un campo vacío; la decisión final es del controlador.
- Un controlador que arma sentencias SQL o usa `DbContext` directo, con el modelo reducido a un DTO sin comportamiento de persistencia: la separación queda a medias. El acceso a datos vive en el modelo.
- Reglas solo en la base, con el controlador como pase de parámetros: el programa compila y la demo puede funcionar, y en el criterio de arquitectura no llega al puntaje alto. El controlador tiene que poder explicar la regla sin abrir SQL Server Management Studio.

