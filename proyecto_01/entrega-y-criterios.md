# Entrega y criterios

## Qué se entrega

1. La [plantilla de relevamiento](plantilla-de-relevamiento.md) completa.
2. La solución .NET 8, compilando en al menos una máquina del equipo.
3. Un script SQL, una migración o los pasos escritos para crear la base y cargar la cadena de conexión.
4. La lista de preguntas con la que entraron a la entrevista (puede ir embebida en la sección 3 de la plantilla).

El docente confirma al inicio si eso viaja en pendrive, carpeta compartida o aula virtual. No hace falta un repositorio remoto propio del equipo para competir.

---

## Guion de la demo (5 minutos)


| Minuto | Qué muestran                                                                                                                                                                                   |
| ------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 0–1    | El problema en una frase y una regla numerada de la plantilla (R1, R2…).                                                                                                                       |
| 1–3    | El flujo en la aplicación, con los datos de la sección 10 de la plantilla: cargar la vianda del día y registrar una reserva. Si les queda tiempo de reloj, una operación que la regla rechaza. |
| 3–4    | En el código: el método del controlador donde vive esa regla, y la vista llamándolo.                                                                                                           |
| 4–5    | Un supuesto de la sección 5, dicho en voz alta.                                                                                                                                                |


Habla quien ensayó. El resto interviene solo si le piden el código. Cortan a los 5 minutos.

---



## Puntaje (100)

El relevamiento pesa igual que el programa. Un equipo que programa de más e ignora lo que el cliente dijo no le gana a otro que dejó el alcance corto y fiel.

### Relevamiento — 35


| Puntos | Qué se ve en la plantilla                                                                                                               |
| ------ | --------------------------------------------------------------------------------------------------------------------------------------- |
| 29–35  | Preguntas concretas, respuestas anotadas, reglas numeradas, supuestos separados de lo que dijo el cliente, alcance recortado al tiempo. |
| 18–28  | Está el problema, las reglas y el alcance. Faltan preguntas sin hacer, o algún supuesto quedó mezclado con una respuesta del cliente.   |
| 8–17   | Hay notas de la entrevista y un modelo de datos, con poca distinción entre dicho, supuesto y deseo del equipo.                          |
| 0–7    | La plantilla está vacía o se completó al final, sin relación con lo que se programó.                                                    |




### Arquitectura — 25


| Puntos | Qué se ve en el código                                                                                                                                     |
| ------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 21–25  | Vista → controlador → modelo. La regla mostrada en la demo está en el controlador y el acceso a datos está en el modelo. El formulario muestra el mensaje. |
| 13–20  | Las tres responsabilidades se distinguen. Alguna regla quedó en el formulario o el controlador habla directo con SQL o con el `DbContext`.                 |
| 6–12   | Hay clases separadas, y el formulario sigue decidiendo el negocio o conteniendo la persistencia.                                                           |
| 0–5    | Toda la solución vive en el formulario.                                                                                                                    |




### Programa en funcionamiento — 25


| Puntos | Qué ocurre en la demo                                                                                                                                                  |
| ------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 21–25  | Se crea la vianda del día, se reserva y el cupo se respeta según una regla relevada. Los datos siguen ahí al reabrir. Una operación inválida muestra un mensaje claro. |
| 13–20  | El alta de vianda y la reserva persisten. El cupo o la regla elegida fallan en un caso, o el mensaje de error no se entiende.                                          |
| 6–12   | Compila y una de las dos operaciones persiste.                                                                                                                         |
| 0–5    | No compila en la demo o no llega a guardar.                                                                                                                            |




### Demo — 15


| Puntos | Qué ocurre en los 5 minutos                                                               |
| ------ | ----------------------------------------------------------------------------------------- |
| 13–15  | Siguen el guion: regla, flujo, capa, supuesto.                                            |
| 8–12   | Muestran el programa y el código, y se quedan sin decir el supuesto o sin citar la regla. |
| 0–7    | Recorren pantallas sin relacionarlas con el relevamiento.                                 |


---



## Desempate

Si dos equipos quedan a tres puntos o menos, define la calidad de los supuestos: gana quien declaró menos comportamientos inventados y pudo decir, en la demo, cuál era el riesgo de cada uno.

---

## Cierre

Después de las tres demos el cliente enumera reglas que ningún equipo preguntó. Sirve para ver qué se perdió en la entrevista. No se reabre el puntaje ni se acepta una corrección de código.