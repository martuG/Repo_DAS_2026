# Torneo La Olla

**Desarrollo y arquitectura de software**  
**Duración:** 2 h 30 min a 3 h  
**Formato:** tres equipos, el mismo caso, un solo cliente  

---

## La situación

En el comedor universitario **La Olla** hay una sola tanda de viandas por día. La fila dobla la esquina, la libreta de papel tiene tachones encima de tachones y, cuando el cupo se acaba, el mostrador se entera tarde: ya le prometieron el almuerzo a alguien que cruzó el campus para nada.

El centro de estudiantes consiguió una tarde y tres equipos de desarrollo. Los tres escuchan al mismo cliente. Uno se queda con el trabajo: el que entienda qué hay que construir y deje en el mostrador un programa usable.

El cliente es el docente. Sabe cómo funciona el comedor. **Responde lo que le preguntan y no completa los huecos.** Una regla que nadie preguntó no forma parte del encargo, aunque el equipo la haya imaginado.

Esta frase es todo lo que el cliente adelanta por escrito:

> Quiero dejar la libreta. En el mostrador tenemos que anotar las viandas del día y quién las reserva, y el sistema no nos puede dejar prometer de más. El resto se los explico si me preguntan bien.

---



## Cómo se lee esta carpeta

1. Este enunciado (reglas del torneo y del sistema).
2. [Guía de entrevista](guia-de-entrevista.md) (cómo hablar con el cliente).
3. [Plantilla de relevamiento](plantilla-de-relevamiento.md) (lo que el equipo entrega por escrito).
4. [Arquitectura en tres capas](arquitectura-tres-capas.md) (MVC de escritorio, ADO.NET o EF Core).
5. [Entrega y criterios](entrega-y-criterios.md) (qué se muestra y cómo se puntúa).

---



## Qué hay que lograr hoy

Dos resultados, los dos cuentan:

1. **Un relevamiento escrito**, con la plantilla de esta carpeta: preguntas, respuestas, reglas, supuestos y alcance.
2. **Una aplicación de escritorio que funcione**, en .NET 8 y Windows Forms, persistiendo en SQL Server, organizada en tres capas (vista, controlador, modelo).

El piso para competir es concreto: el documento completo, la solución compilando, y un flujo persistido en el que el mostrador carga la vianda de un día y registra una reserva sin pasarse del cupo. Lo que se construya encima de eso tiene que salir de las reglas que el cliente haya dicho.

---

## Reglas del torneo

- Los tres equipos resuelven **el mismo caso**. No hay variante secreta por grupo.
- La entrevista es **privada**. Mientras un equipo habla con el cliente, los otros no escuchan ni se pasan preguntas.
- Cada equipo llega a la entrevista con una **lista de preguntas ya escrita**. Esa lista se entrega al cliente al sentarse.
- En la mesa hablan como máximo **dos personas**. El resto anota.
- El cliente no responde «¿qué más haría falta?». Si la pregunta es concreta, contesta. Si es vaga, pide que la concreten y el reloj sigue.
- Lo que el equipo no preguntó y aun así implementó se anota como **supuesto**. Un supuesto oculto resta. Un supuesto explícito se puede defender.
- Hasta el cierre no se comparten reglas entre equipos.
- El medio de entrega (carpeta compartida, pendrive o aula virtual) lo confirma el docente al abrir la clase.

---



## Cronograma

El docente puede correr los minutos unos pocos para atrás o adelante. El orden no se cambia.


| Minuto  | Qué pasa                                                                                                                                                       |
| ------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 0–10    | Lectura del enunciado. El equipo se pone nombre y reparte roles.                                                                                               |
| 10–25   | Lista escrita de preguntas. Nadie entrevista todavía.                                                                                                          |
| 25–65   | Tres entrevistas privadas de **12 minutos**, en el orden del sorteo. Quien espera sigue con el modelo y el esqueleto del proyecto, usando solo lo que ya sabe. |
| 65–155  | Construcción. El relevamiento se va completando mientras se programa: no se deja para el final.                                                                |
| 155–170 | Demo de **5 minutos** por equipo, en el mismo orden del sorteo.                                                                                                |
| 170–180 | El cliente cuenta qué reglas nadie preguntó. Eso no cambia el puntaje.                                                                                         |


Orden de las entrevistas: sorteo al empezar. El equipo que entrevista primero no tiene ventaja de información; tiene menos tiempo de código después, y el que entrevista último llega a la demo con menos margen. El cronograma ya contempla esa diferencia: los 90 minutos de construcción son comunes y arrancan cuando **termina la tercera** entrevista. Antes de eso, quien no está en la mesa adelanta proyecto vacío, carpetas y boceto de datos, sin inventar reglas.

---



## Restricciones técnicas

- **.NET 8**, proyecto **Windows Forms**.
- **SQL Server**. Cada equipo crea su propia base.
- Persistencia a elección del equipo: **ADO.NET** (modo conectado o desconectado) o **EF Core**. La elección se justifica en un párrafo del relevamiento.
- Tres capas, con la dirección de dependencias de [arquitectura-tres-capas.md](arquitectura-tres-capas.md).
- La cadena de conexión no queda escrita dentro de un formulario.
- Si usan ADO.NET, los comandos van parametrizados. Si usan EF Core, el `DbContext` vive en el modelo.
- Mensajes claros cuando una regla rechaza una operación. Una excepción sin contexto no alcanza.

Queda afuera de la tecnología, aunque el cliente lo pida: aplicación web, móvil, login de estudiantes, impresión y reportes en PDF. Si el cliente pidiera algo de eso, se anota como necesidad y se deja fuera de esta entrega.

---



## Qué se espera en la demo

Cinco minutos, de pie, con la aplicación abierta. El guion está en [entrega-y-criterios.md](entrega-y-criterios.md). En ese tiempo el equipo muestra el flujo de reserva, señala **una regla que el cliente dijo** y muestra **en qué capa** está implementada.