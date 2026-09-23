(function () {
  const arbolEl = document.getElementById("arbol");
  const padreEl = document.getElementById("padre");
  const relatoEl = document.getElementById("relato");
  const bitacoraEl = document.getElementById("bitacora");
  const totalEl = document.getElementById("total");
  const estadoTotalEl = document.getElementById("estado-total");
  const btnAnterior = document.getElementById("anterior");
  const btnSiguiente = document.getElementById("siguiente");
  const btnReproducir = document.getElementById("reproducir");

  let seq = 7;
  let arbol = ejemplo();
  let pasos = [];
  let totalArbol = 0;
  let indice = -1;
  let timer = null;

  function ejemplo() {
    return {
      id: 1,
      tipo: "caja",
      nombre: "Pedido",
      empaque: 10,
      hijos: [
        { id: 2, tipo: "producto", nombre: "Libro", precio: 20 },
        {
          id: 3,
          tipo: "caja",
          nombre: "Útiles",
          empaque: 5,
          hijos: [
            { id: 4, tipo: "producto", nombre: "Lápiz", precio: 2 },
            { id: 5, tipo: "producto", nombre: "Cuaderno", precio: 8 }
          ]
        },
        { id: 6, tipo: "producto", nombre: "Mochila", precio: 30 }
      ]
    };
  }

  function dinero(n) {
    const valor = Number(n);
    const texto = Number.isInteger(valor) ? String(valor) : valor.toFixed(2);
    return "$" + texto;
  }

  function esc(texto) {
    return String(texto)
      .replaceAll("&", "&amp;")
      .replaceAll("<", "&lt;")
      .replaceAll(">", "&gt;")
      .replaceAll('"', "&quot;");
  }

  function buscar(nodo, id) {
    if (nodo.id === id) return nodo;
    if (!nodo.hijos) return null;
    for (const hijo of nodo.hijos) {
      const hallado = buscar(hijo, id);
      if (hallado) return hallado;
    }
    return null;
  }

  function quitarDe(nodo, id) {
    if (!nodo.hijos) return false;
    const pos = nodo.hijos.findIndex((hijo) => hijo.id === id);
    if (pos >= 0) {
      nodo.hijos.splice(pos, 1);
      return true;
    }
    return nodo.hijos.some((hijo) => quitarDe(hijo, id));
  }

  function listarCajas(nodo, ruta) {
    const camino = ruta.concat(nodo.nombre);
    const lista = [{ id: nodo.id, etiqueta: camino.join(" / ") }];
    for (const hijo of nodo.hijos) {
      if (hijo.tipo === "caja") lista.push(...listarCajas(hijo, camino));
    }
    return lista;
  }

  function construir(nodo, nivel, esRaiz) {
    const pasosLocales = [];
    if (esRaiz) {
      pasosLocales.push({
        id: nodo.id,
        nivel: 0,
        fase: "cliente",
        texto: "El cliente llama a Precio() sobre «" + nodo.nombre + "». No pregunta si es una caja o un producto."
      });
    }

    if (nodo.tipo === "producto") {
      pasosLocales.push({
        id: nodo.id,
        nivel: nivel,
        fase: "hoja",
        parcial: nodo.precio,
        texto: "«" + nodo.nombre + "» es una hoja. Devuelve " + dinero(nodo.precio) + " y no delega."
      });
      return { pasos: pasosLocales, total: nodo.precio };
    }

    pasosLocales.push({
      id: nodo.id,
      nivel: nivel,
      fase: "entra",
      texto: "«" + nodo.nombre + "» es un contenedor. Pide Precio() a cada hijo."
    });

    let sumaHijos = 0;
    const partes = [];
    for (const hijo of nodo.hijos) {
      const sub = construir(hijo, nivel + 1, false);
      pasosLocales.push(...sub.pasos);
      sumaHijos += sub.total;
      partes.push(hijo.nombre + " " + dinero(sub.total));
    }

    const total = sumaHijos + nodo.empaque;
    const detalle = partes.length ? partes.join(" + ") : "sin hijos";
    pasosLocales.push({
      id: nodo.id,
      nivel: nivel,
      fase: "vuelve",
      parcial: total,
      texto: "«" + nodo.nombre + "» suma (" + detalle + ") y agrega el empaque " + dinero(nodo.empaque) + ". Devuelve " + dinero(total) + "."
    });
    return { pasos: pasosLocales, total: total };
  }

  function parcialesHasta(hasta) {
    const mapa = new Map();
    for (let i = 0; i <= hasta; i++) {
      const paso = pasos[i];
      if (paso.fase === "entra") mapa.delete(paso.id);
      if (paso.parcial != null) mapa.set(paso.id, paso.parcial);
    }
    return mapa;
  }

  function htmlNodo(nodo, parciales, activoId) {
    const clases = ["nodo", nodo.tipo];
    if (nodo.id === activoId) clases.push("activo");
    if (parciales.has(nodo.id)) clases.push("resuelto");

    const extra = nodo.tipo === "caja"
      ? "Empaque " + dinero(nodo.empaque)
      : dinero(nodo.precio);
    const resultado = parciales.has(nodo.id)
      ? '<span class="parcial">devuelve ' + dinero(parciales.get(nodo.id)) + "</span>"
      : "";
    const quitar = nodo.id === arbol.id
      ? ""
      : '<button type="button" class="quitar" data-quitar="' + nodo.id + '">Quitar</button>';
    const hijos = nodo.tipo === "caja"
      ? '<div class="hijos">' + nodo.hijos.map((hijo) => htmlNodo(hijo, parciales, activoId)).join("") + "</div>"
      : "";

    return (
      '<article class="' + clases.join(" ") + '" data-id="' + nodo.id + '">' +
        '<div class="nodo-cabeza">' +
          "<div><span class=\"tipo\">" + (nodo.tipo === "caja" ? "Caja" : "Producto") + "</span> " +
          "<strong>" + esc(nodo.nombre) + "</strong> <span class=\"ayuda\">" + extra + "</span></div>" +
          "<div>" + resultado + quitar + "</div>" +
        "</div>" +
        hijos +
      "</article>"
    );
  }

  function pintar() {
    const parciales = indice >= 0 ? parcialesHasta(indice) : new Map();
    const activoId = indice >= 0 ? pasos[indice].id : null;
    arbolEl.innerHTML = htmlNodo(arbol, parciales, activoId);

    const activo = arbolEl.querySelector(".activo");
    if (activo) activo.scrollIntoView({ block: "nearest" });

    if (indice < 0) {
      relatoEl.textContent = "El árbol ya está armado. El cliente todavía no hizo la llamada.";
      bitacoraEl.innerHTML = "";
      totalEl.textContent = "—";
      estadoTotalEl.textContent = "Todavía no se pidió el precio.";
    } else {
      relatoEl.textContent = pasos[indice].texto;
      bitacoraEl.innerHTML = pasos.slice(0, indice + 1).map((paso, i) => {
        const clase = i === indice ? "activa" : "";
        return '<li class="' + clase + '" style="--nivel:' + paso.nivel + '">' + esc(paso.texto) + "</li>";
      }).join("");
      const ultima = bitacoraEl.lastElementChild;
      if (ultima) ultima.scrollIntoView({ block: "nearest" });

      const termino = indice === pasos.length - 1;
      totalEl.textContent = termino ? dinero(totalArbol) : "…";
      estadoTotalEl.textContent = termino
        ? "El cliente recibe el total. No abrió ninguna caja."
        : "Paso " + (indice + 1) + " de " + pasos.length + ".";
    }

    btnAnterior.disabled = indice < 0;
    btnSiguiente.disabled = indice >= pasos.length - 1;
    btnSiguiente.textContent = indice < 0 ? "Empezar" : "Paso siguiente";
  }

  function reconstruir() {
    const calculo = construir(arbol, 1, true);
    pasos = calculo.pasos;
    totalArbol = calculo.total;
    pintar();
    const seleccionado = padreEl.value;
    padreEl.innerHTML = listarCajas(arbol, []).map((caja) => {
      return '<option value="' + caja.id + '">' + esc(caja.etiqueta) + "</option>";
    }).join("");
    if (seleccionado && padreEl.querySelector('option[value="' + seleccionado + '"]')) {
      padreEl.value = seleccionado;
    }
  }

  function cambiarEstructura() {
    detener();
    indice = -1;
    reconstruir();
  }

  function ir(nuevo) {
    indice = Math.max(-1, Math.min(pasos.length - 1, nuevo));
    pintar();
    if (indice >= pasos.length - 1) detener();
  }

  function detener() {
    if (timer) {
      clearInterval(timer);
      timer = null;
    }
    btnReproducir.textContent = "Reproducir";
  }

  function reproducir() {
    if (timer) {
      detener();
      return;
    }
    if (indice >= pasos.length - 1) indice = -1;
    const espera = window.matchMedia("(prefers-reduced-motion: reduce)").matches ? 2200 : 1500;
    timer = setInterval(() => ir(indice + 1), espera);
    btnReproducir.textContent = "Pausa";
    ir(indice + 1);
  }

  document.getElementById("form-alta").addEventListener("submit", (evento) => {
    evento.preventDefault();
    const tipo = evento.submitter ? evento.submitter.value : "producto";
    const nombre = document.getElementById("nombre").value.trim();
    const valor = Number(document.getElementById("valor").value);
    if (!nombre || Number.isNaN(valor) || valor < 0) return;

    const padre = buscar(arbol, Number(padreEl.value));
    if (!padre || padre.tipo !== "caja") return;

    const nodo = { id: seq++, tipo: tipo, nombre: nombre };
    if (tipo === "caja") {
      nodo.empaque = valor;
      nodo.hijos = [];
    } else {
      nodo.precio = valor;
    }
    padre.hijos.push(nodo);
    document.getElementById("nombre").value = "";
    cambiarEstructura();
  });

  arbolEl.addEventListener("click", (evento) => {
    const boton = evento.target.closest("[data-quitar]");
    if (!boton) return;
    quitarDe(arbol, Number(boton.dataset.quitar));
    cambiarEstructura();
  });

  document.getElementById("restaurar").addEventListener("click", () => {
    seq = 7;
    arbol = ejemplo();
    cambiarEstructura();
  });

  btnAnterior.addEventListener("click", () => {
    detener();
    ir(indice - 1);
  });
  btnSiguiente.addEventListener("click", () => {
    detener();
    ir(indice + 1);
  });
  btnReproducir.addEventListener("click", reproducir);

  document.addEventListener("keydown", (evento) => {
    if (evento.target.matches("input, select, textarea")) return;
    if (evento.key === "ArrowRight") {
      detener();
      ir(indice + 1);
    }
    if (evento.key === "ArrowLeft") {
      detener();
      ir(indice - 1);
    }
  });

  const enlaces = Array.from(document.querySelectorAll(".nav a"));
  const observador = new IntersectionObserver((entradas) => {
    entradas.forEach((entrada) => {
      if (!entrada.isIntersecting) return;
      enlaces.forEach((enlace) => {
        enlace.classList.toggle("activa", enlace.getAttribute("href") === "#" + entrada.target.id);
      });
    });
  }, { rootMargin: "-45% 0px -45% 0px" });
  document.querySelectorAll("main section[id]").forEach((seccion) => observador.observe(seccion));

  reconstruir();
})();
