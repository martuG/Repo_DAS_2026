(function () {
  const app = document.getElementById("app");

  const estado = {
    indice: 0,
    orden: [],
    respuestas: [],
    fase: "inicio"
  };

  function mezclar(lista) {
    const copia = lista.slice();
    for (let i = copia.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [copia[i], copia[j]] = [copia[j], copia[i]];
    }
    return copia;
  }

  function normalizar(texto) {
    return String(texto || "")
      .trim()
      .toLowerCase()
      .normalize("NFD")
      .replace(/\p{M}/gu, "")
      .replace(/[^a-z0-9]/g, "");
  }

  function etiquetaTipo(tipo) {
    if (tipo === "codigo") return "Interpretar código";
    if (tipo === "completar") return "Completar la frase";
    return "Opción múltiple";
  }

  function etiquetaDificultad(d) {
    if (d === "bajo") return "Baja";
    if (d === "medio") return "Media";
    if (d === "alto") return "Alta";
    return "Media-alta";
  }

  function escapeHtml(texto) {
    return String(texto)
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;");
  }

  function preguntaActual() {
    return estado.orden[estado.indice];
  }

  function iniciar() {
    estado.indice = 0;
    estado.respuestas = [];
    estado.fase = "pregunta";
    estado.orden = mezclar(PREGUNTAS).map((p) => {
      const pregunta = { ...p };
      if (pregunta.opciones) {
        pregunta.opciones = mezclar(pregunta.opciones);
      }
      return pregunta;
    });
    render();
  }

  function renderInicio() {
    app.innerHTML = `
      <header class="header">
        <div>
          <div class="marca">DAS 2026 · Autodiagnóstico</div>
          <h1>Acceso a datos y plataforma .NET</h1>
        </div>
      </header>
      <section class="card intro">
        <p>
          Evaluación de autoevaluación a partir del material del repositorio:
          ensamblados, ADO.NET modo conectado y desconectado, transacciones
          y los prácticos (taller y facturación).
        </p>
        <div class="lista-info">
          <div><strong>40</strong><span>preguntas en total</span></div>
          <div><strong>30</strong><span>nivel medio-alto</span></div>
          <div><strong>10</strong><span>dificultad mezclada</span></div>
        </div>
        <p>
          Hay tres tipos de ítem: opción múltiple, completar una frase con una palabra
          e interpretar un fragmento de código. Al responder se muestra de inmediato
          si está bien o mal, con la corrección. Al final verás el puntaje.
        </p>
        <p>El orden de las preguntas y de las opciones se mezcla en cada intento.</p>
        <div class="acciones">
          <button class="btn btn-primario" id="btn-empezar">Comenzar</button>
        </div>
      </section>
    `;
    document.getElementById("btn-empezar").onclick = iniciar;
  }

  function renderPregunta() {
    const p = preguntaActual();
    const n = estado.indice + 1;
    const total = estado.orden.length;
    const pct = Math.round(((n - 1) / total) * 100);
    const ya = estado.respuestas[estado.indice];

    let cuerpo = "";
    if (p.codigo) {
      cuerpo += `<pre class="codigo">${escapeHtml(p.codigo)}</pre>`;
    }

    if (p.tipo === "completar") {
      const valor = ya ? ya.dado : "";
      cuerpo += `
        <p class="enunciado">${escapeHtml(p.enunciado)}</p>
        <div class="completar">
          <span>${escapeHtml(p.fraseAntes)}</span>
          <input id="respuesta-fill" type="text" autocomplete="off" spellcheck="false"
            ${ya ? "disabled" : ""} value="${escapeHtml(valor)}">
          <span>${escapeHtml(p.fraseDespues)}</span>
        </div>
      `;
    } else {
      cuerpo += `<p class="enunciado">${escapeHtml(p.enunciado)}</p>`;
      cuerpo += `<div class="opciones">`;
      p.opciones.forEach((op) => {
        let extra = "";
        if (ya) {
          if (op.id === p.correcta) extra = "ok";
          else if (op.id === ya.dado) extra = "mal";
        }
        cuerpo += `
          <label class="opcion ${extra}">
            <input type="radio" name="opcion" value="${op.id}"
              ${ya ? "disabled" : ""} ${ya && ya.dado === op.id ? "checked" : ""}>
            <span>${escapeHtml(op.texto)}</span>
          </label>
        `;
      });
      cuerpo += `</div>`;
    }

    let feedback = "";
    if (ya) {
      const clase = ya.ok ? "ok" : "mal";
      const titulo = ya.ok ? "Correcto." : "Incorrecto.";
      const extra = ya.ok
        ? ""
        : ` La respuesta correcta es <strong>${escapeHtml(textoCorrecta(p))}</strong>.`;
      feedback = `
        <div class="feedback ${clase}">
          <strong>${titulo}</strong>${extra}
          <div>${escapeHtml(p.explicacion)}</div>
        </div>
      `;
    }

    app.innerHTML = `
      <header class="header">
        <div>
          <div class="marca">DAS 2026 · Autodiagnóstico</div>
          <h1>Pregunta ${n} de ${total}</h1>
        </div>
      </header>
      <div class="progreso-wrap">
        <span>${p.tema}</span>
        <span>${n} / ${total}</span>
      </div>
      <div class="barra"><span style="width:${pct}%"></span></div>
      <section class="card">
        <div class="meta">
          <span class="badge tipo">${etiquetaTipo(p.tipo)}</span>
          <span class="badge ${p.dificultad}">${etiquetaDificultad(p.dificultad)}</span>
          <span class="badge">${p.bloque === "principal" ? "Bloque medio-alto" : "Bloque mixto"}</span>
        </div>
        ${cuerpo}
        ${feedback}
        <div class="acciones">
          ${
            ya
              ? `<button class="btn btn-primario" id="btn-siguiente">${
                  n === total ? "Ver puntaje" : "Siguiente"
                }</button>`
              : `<button class="btn btn-primario" id="btn-corregir">Corregir</button>`
          }
        </div>
      </section>
    `;

    const btnC = document.getElementById("btn-corregir");
    const btnS = document.getElementById("btn-siguiente");
    if (btnC) btnC.onclick = corregir;
    if (btnS) btnS.onclick = siguiente;

    const input = document.getElementById("respuesta-fill");
    if (input && !ya) {
      input.focus();
      input.addEventListener("keydown", (e) => {
        if (e.key === "Enter") corregir();
      });
    }
  }

  function textoCorrecta(p) {
    if (p.tipo === "completar") return p.respuestaVisible;
    const op = p.opciones.find((o) => o.id === p.correcta);
    return op ? op.texto : p.correcta;
  }

  function leerRespuesta(p) {
    if (p.tipo === "completar") {
      const input = document.getElementById("respuesta-fill");
      return input ? input.value : "";
    }
    const marcada = document.querySelector("input[name=opcion]:checked");
    return marcada ? marcada.value : "";
  }

  function esCorrecta(p, dado) {
    if (p.tipo === "completar") {
      const n = normalizar(dado);
      return n.length > 0 && p.aceptadas.includes(n);
    }
    return dado === p.correcta;
  }

  function corregir() {
    const p = preguntaActual();
    const dado = leerRespuesta(p);
    if (!String(dado).trim()) {
      alert("Respondé la pregunta antes de corregir.");
      return;
    }
    estado.respuestas[estado.indice] = {
      ok: esCorrecta(p, dado),
      dado
    };
    render();
  }

  function siguiente() {
    if (estado.indice >= estado.orden.length - 1) {
      estado.fase = "fin";
    } else {
      estado.indice += 1;
    }
    render();
  }

  function mensajeNivel(pct) {
    if (pct >= 90) return "Excelente dominio del material del curso.";
    if (pct >= 75) return "Muy buen nivel. Repasá solo los temas marcados en incorrectas.";
    if (pct >= 60) return "Nivel intermedio: conviene reforzar desconectado, transacciones o ensamblados según el detalle.";
    return "Hace falta repasar el apunte y los DAO del repositorio antes de un parcial.";
  }

  function contar(filtro) {
    let ok = 0;
    let total = 0;
    estado.orden.forEach((p, i) => {
      if (!filtro(p)) return;
      total += 1;
      if (estado.respuestas[i] && estado.respuestas[i].ok) ok += 1;
    });
    return { ok, total };
  }

  function renderFin() {
    const total = estado.orden.length;
    const correctas = estado.respuestas.filter((r) => r.ok).length;
    const pct = Math.round((correctas / total) * 100);
    const principal = contar((p) => p.bloque === "principal");
    const mixto = contar((p) => p.bloque === "mixto");
    const temas = {};
    estado.orden.forEach((p, i) => {
      if (!temas[p.tema]) temas[p.tema] = { ok: 0, total: 0 };
      temas[p.tema].total += 1;
      if (estado.respuestas[i].ok) temas[p.tema].ok += 1;
    });

    const statsTema = Object.entries(temas)
      .map(
        ([tema, s]) =>
          `<div class="stat"><strong>${s.ok}/${s.total}</strong><em>${escapeHtml(tema)}</em></div>`
      )
      .join("");

    const revision = estado.orden
      .map((p, i) => {
        const r = estado.respuestas[i];
        const marca = r.ok ? '<span class="ok">Bien</span>' : '<span class="mal">Mal</span>';
        const tuya =
          p.tipo === "completar"
            ? r.dado
            : (p.opciones.find((o) => o.id === r.dado) || {}).texto || r.dado;
        return `
          <div class="item-rev">
            ${marca} · P${i + 1} · ${escapeHtml(p.tema)} · ${etiquetaTipo(p.tipo)}<br>
            ${escapeHtml(p.enunciado)}<br>
            Tu respuesta: ${escapeHtml(tuya)}
            ${r.ok ? "" : `<br>Correcta: ${escapeHtml(textoCorrecta(p))}`}
          </div>
        `;
      })
      .join("");

    app.innerHTML = `
      <header class="header">
        <div>
          <div class="marca">DAS 2026 · Autodiagnóstico</div>
          <h1>Resultado</h1>
        </div>
      </header>
      <section class="card">
        <div class="puntaje-grande">
          <div class="numero">${correctas} / ${total}</div>
          <p>${pct}% · ${mensajeNivel(pct)}</p>
        </div>
        <div class="grilla-stats">
          <div class="stat"><strong>${principal.ok}/${principal.total}</strong><em>Bloque medio-alto</em></div>
          <div class="stat"><strong>${mixto.ok}/${mixto.total}</strong><em>Bloque mixto</em></div>
          <div class="stat"><strong>${pct}%</strong><em>Puntaje global</em></div>
        </div>
        <div class="grilla-stats">${statsTema}</div>
        <div class="nota-final">
          Cada acierto suma 1 punto. Ya viste la corrección pregunta por pregunta;
          abajo queda el listado para repasar.
        </div>
        <div class="revision">
          <h3>Revisión</h3>
          ${revision}
        </div>
        <div class="acciones">
          <button class="btn btn-secundario" id="btn-inicio">Volver al inicio</button>
          <button class="btn btn-primario" id="btn-rehacer">Nuevo intento</button>
        </div>
      </section>
    `;
    document.getElementById("btn-rehacer").onclick = iniciar;
    document.getElementById("btn-inicio").onclick = () => {
      estado.fase = "inicio";
      render();
    };
  }

  function render() {
    if (estado.fase === "inicio") renderInicio();
    else if (estado.fase === "fin") renderFin();
    else renderPregunta();
  }

  render();
})();
