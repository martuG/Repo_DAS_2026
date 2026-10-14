# Guía de entrevista

El cliente es una sola persona: quien coordina el comedor La Olla. Habla de su trabajo en el mostrador, no de tablas ni de clases. El equipo traduce.

---

## Roles sugeridos

Un equipo de alrededor de diez personas no programa de a diez sobre el mismo formulario. Repartan roles al arrancar. Si falta gente, se funden roles vecinos; el escriba y quien defiende la arquitectura en la demo siguen existiendo.

| Rol | Cuántos | Qué hace en estas 3 horas |
| --- | --- | --- |
| Coordinación | 1 | Cuida el reloj, el alcance y que nadie programe una regla que no esté escrita. |
| Entrevista | 2 | Hablan con el cliente. Llevan la lista. Uno pregunta, el otro repregunta. |
| Escriba | 1 | Anota la respuesta literal, al lado de la pregunta. No resume de memoria al final. |
| Relevamiento | 1–2 | Pasa las notas a la plantilla: reglas, supuestos, dentro y fuera de alcance. |
| Datos | 1–2 | Modelo relacional, script o migraciones, base local. |
| Controlador | 1–2 | Casos de uso y reglas de negocio, sin formularios. |
| Vista | 1–2 | Formularios. Llama al controlador y muestra lo que vuelve. |
| Prueba y demo | 1 | Arma datos de prueba, recorre el flujo y ensaya los 5 minutos. |

Quien termina su parte se suma a prueba o a completar la plantilla. No abre un segundo caso de uso «por si sobra tiempo» sin acuerdo de la coordinación.

---

## Antes de sentarse (15 minutos)

Escriban la lista en papel o en un archivo y entréguenla al cliente al empezar. Numeren las preguntas. Agrúpenlas para no gastar los 12 minutos en un solo tema:

1. Quién usa el sistema y en qué momento del día.
2. Qué se anota de la vianda del día.
3. Qué se anota de una reserva y qué la hace válida.
4. Qué pasa con el cupo cuando se reserva, cuando se cancela y cuando se retira.
5. Qué errores ve el mostrador y qué mensaje espera.
6. Qué pidieron alguna vez y **no** quieren en esta versión.

Una buena pregunta se puede contestar con un hecho: «¿un mismo estudiante puede reservar dos veces el mismo día?». Una pregunta que el cliente va a devolver es: «¿cómo se imagina la pantalla?».

---

## Durante los 12 minutos

- Entran las dos personas de entrevista y el escriba. El resto del equipo puede mirar, en silencio.
- Lean la pregunta, esperen la respuesta, anoten, repregunten solo si quedó un número, una fecha o un sí/no sin definir.
- Si el cliente dice «eso decídanlo ustedes», anótenlo como decisión del equipo y sigan. No discutan el diseño en la mesa.
- No pidan que les dibuje el modelo de datos. Pregunten por el trabajo.
- A los 12 minutos se corta, aunque queden preguntas. Las que no se hicieron se copian en la plantilla, en la sección correspondiente.

---

## Después de la entrevista

El escriba lee las notas en voz alta, cinco minutos, con todo el equipo. Recién ahí se actualizan alcance y reglas. A partir de ese momento:

- Cada regla que va a código tiene una frase en la plantilla y un dueño (controlador, base, o los dos).
- Cada comportamiento que el equipo necesita y el cliente no dijo queda marcado como **supuesto**, con el nombre de quien lo propone.
- La coordinación tacha del alcance lo que no entra en el tiempo que queda.

El cliente no concede una segunda ronda. Si falta un dato, el equipo asume, lo escribe y lo dice en la demo.
