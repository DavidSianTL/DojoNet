const GRID = {
    cellWidth: 260,
    cellHeight: 110,
    paddingX: 30,
    paddingY: 30,
};

const OrgChart = {
    _datosOriginales: [],
    _mapaNodos: new Map(),

    // Construye jerarquía anidada a partir de datos planos
    construirJerarquia: function (datos) {
        const mapa = {};
        datos.forEach(d => mapa[d.USUARIO] = { ...d, hijos: [] });
        datos.forEach(d => {
            if (d.COD_USUARIO_JEFE && mapa[d.COD_USUARIO_JEFE]) {
                mapa[d.COD_USUARIO_JEFE].hijos.push(mapa[d.USUARIO]);
            }
        });
        // Retornar nodos raíz (sin jefe válido)
        return Object.values(mapa).filter(n => !n.COD_USUARIO_JEFE || !mapa[n.COD_USUARIO_JEFE]);
    },

    // Calcula posición fija en cuadrícula
    calcularPosicion: function (col, row) {
        return {
            x: col * GRID.cellWidth + GRID.paddingX,
            y: row * GRID.cellHeight + GRID.paddingY,
        };
    },

    renderizarPorColumnas: function (datos, contenedor) {
        this._datosOriginales = datos;
        // Inicializar el estado para todos los nodos visibles por defecto
        this.inicializarEstadoNodos();

        contenedor.innerHTML = "";

        if (!this._estadoNodos) this._estadoNodos = {}; // Asegurar mapa de estados

        const raices = this.construirJerarquia(datos);
        const wrapper = document.createElement("div");
        wrapper.className = "org-tree-wrapper";
        wrapper.id = "orgZoomContainer";
        wrapper.dataset.scale = 1;
        wrapper.style.position = "relative";
        wrapper.style.width = "max-content";
        wrapper.style.height = "max-content";

        contenedor.appendChild(wrapper);

        this._mapaNodos.clear();
        let filaGlobal = 0;

        const renderNodo = (nodo, col, padreOculto = false) => {
            const estaVisible = this._estadoNodos[nodo.USUARIO] !== false;

            // Si el nodo está oculto o su padre está oculto, no se renderiza
            if (!estaVisible || padreOculto) return;

            // Aplicación del cambio solicitado con validaciones de seguridad
            const pos = this.calcularPosicion(col, filaGlobal);

            // Validamos que nodo sea un objeto antes de acceder a sus propiedades
            let claseNivel = "nivel-desconocido";
            let claseRol = "rol-desconocido";

            if (typeof nodo === "object" && nodo !== null) {
                if (nodo.COD_PUESTO) {
                    claseNivel = `nivel-${String(nodo.COD_PUESTO).toLowerCase()}`;
                } else {
                    console.warn("COD_PUESTO indefinido en nodo:", nodo);
                }

                if (nodo.PUESTO) {
                    claseRol = `rol-${nodo.PUESTO.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/\s+/g, "-")}`;
                } else {
                    console.warn("PUESTO indefinido en nodo:", nodo);
                }
            } else {
                console.error("Nodo inválido o no es un objeto:", nodo);
            }


            const div = document.createElement("div");
            div.className = `org-node ${claseNivel} ${claseRol}`;
            div.id = `node-${nodo.USUARIO ?? "sin-usuario"}`;
            div.style.position = "absolute";
            div.style.left = `${pos.x}px`;
            div.style.top = `${pos.y}px`;

            div.dataset.usuario = nodo.USUARIO?.toLowerCase() ?? "desconocido";
            div.dataset.nombre = nodo.NOMBRE_USUARIO?.toLowerCase() ?? "desconocido";
            div.dataset.puesto = nodo.PUESTO?.toLowerCase() ?? "desconocido";

            div.title = `${nodo.PUESTO ?? "Sin puesto"} - ${nodo.NOMBRE_USUARIO ?? "Sin nombre"}`;

            div.innerHTML = `
              <div class="org-usuario">${nodo.USUARIO ?? ""}</div>
              <div class="org-puesto">${nodo.PUESTO ?? ""}</div>
              <div class="org-nombre">${nodo.NOMBRE_USUARIO ?? ""}</div>`;


            // Botón de toggle si tiene hijos
            if (nodo.hijos?.length) {
                const toggle = document.createElement("div");
                toggle.className = "toggle-subarbol";

                const hijosVisibles = this.tieneHijosVisibles(nodo.USUARIO);
                toggle.innerText = hijosVisibles ? "−" : "+";

                toggle.id = `toggle-${nodo.USUARIO}`;
                toggle.title = "Expandir/Contraer";

                toggle.onclick = (e) => {
                    e.stopPropagation();
                    this.toggleSubarbol(nodo.USUARIO);
                };

                div.appendChild(toggle);
            }



            // Eventos de interacción
            div.addEventListener("click", () => this.mostrarModal(nodo));
            div.addEventListener("dblclick", () => this.toggleSubarbol(nodo.USUARIO));
            div.addEventListener("click", () => this.resaltarLinea(nodo.COD_USUARIO_JEFE, nodo.USUARIO));

            wrapper.appendChild(div);

            this._mapaNodos.set(nodo.USUARIO, { element: div, col, row: filaGlobal });
            filaGlobal++;

            // Renderizar hijos
            if (nodo.hijos?.length) {
                nodo.hijos.forEach(hijo => renderNodo(hijo, col + 1, !estaVisible));
            }
        };

        raices.forEach(r => renderNodo(r, 0));

        this._crearSVGConexiones(wrapper);
        this.dibujarConexiones(this._datosOriginales);
        this._habilitarPanZoom(contenedor, wrapper);
    },

    inicializarEstadoNodos() {
        if (!this._estadoNodos) this._estadoNodos = {};
        this._datosOriginales.forEach(nodo => {
            if (this._estadoNodos[nodo.USUARIO] === undefined) {
                this._estadoNodos[nodo.USUARIO] = true; // visible por defecto
            }
        });
    },


    _crearSVGConexiones(wrapper) {
        let svg = wrapper.querySelector("svg#conexionesSVG");
        if (!svg) {
            svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
            svg.id = "conexionesSVG";
            svg.style.position = "absolute";
            svg.style.top = 0;
            svg.style.left = 0;
            svg.style.pointerEvents = "none";
            wrapper.insertBefore(svg, wrapper.firstChild);
        }

        // Ajustar tamaño SVG al tamaño total del contenido (wrapper)
        svg.setAttribute("width", wrapper.scrollWidth);
        svg.setAttribute("height", wrapper.scrollHeight);

        // Limpiar líneas previas
        while (svg.firstChild) svg.removeChild(svg.firstChild);
    },

    // Construir jerarquía filtrando nodos ocultos por colapsados
    construirJerarquiaFiltrada: function (datos) {
        const mapa = {};
        datos.forEach(d => {
            mapa[d.USUARIO] = { ...d, hijos: [] };
        });

        datos.forEach(d => {
            const padreId = d.COD_USUARIO_JEFE;
            if (padreId && mapa[padreId] && !this._nodosColapsados.has(padreId)) {
                mapa[padreId].hijos.push(mapa[d.USUARIO]);
            }
        });

        // Raíces: sin jefe o cuyo jefe está colapsado (se consideran raíz visible)
        return Object.values(mapa).filter(n => !n.COD_USUARIO_JEFE || this._nodosColapsados.has(n.COD_USUARIO_JEFE));
    },

    dibujarConexiones: function (datos, svgId = "conexionesSVG") {
        const svg = document.getElementById(svgId);
        if (!svg) return;

        const wrapper = svg.parentElement;
        if (!wrapper) return;

        const scale = parseFloat(wrapper.dataset.scale || "1");
        while (svg.firstChild) svg.removeChild(svg.firstChild);

        const wrapperRect = wrapper.getBoundingClientRect();

        datos.forEach(emp => {
            const jefeId = emp.COD_USUARIO_JEFE;
            if (!jefeId) return;

            const nodoHijo = document.getElementById("node-" + emp.USUARIO);
            const nodoJefe = document.getElementById("node-" + jefeId);
            if (!nodoHijo || !nodoJefe) return;

            // Evitar dibujar si el nodo hijo está oculto
            if (nodoHijo.classList.contains("oculto")) return;


            const rectJefe = nodoJefe.getBoundingClientRect();
            const rectHijo = nodoHijo.getBoundingClientRect();

            const startX = (rectJefe.right - wrapperRect.left) / scale;
            const startY = (rectJefe.top + rectJefe.height / 2 - wrapperRect.top) / scale;

            const endX = (rectHijo.left - wrapperRect.left) / scale;
            const endY = (rectHijo.top + rectHijo.height / 2 - wrapperRect.top) / scale;

            const midX = (startX + endX) / 2;

            const d = `M ${startX} ${startY} C ${midX} ${startY}, ${midX} ${endY}, ${endX} ${endY}`;

            const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
            path.setAttribute("d", d);
            path.setAttribute("stroke", "#0f3420");
            path.setAttribute("stroke-width", "1"); //Grosor de lineas
            path.setAttribute("fill", "none");
            path.setAttribute("stroke-linecap", "round");
            path.setAttribute("stroke-linejoin", "round");
            path.setAttribute("id", `linea-${jefeId}-${emp.USUARIO}`);

            svg.appendChild(path);
        });
    },

    resaltarLinea: function (idPadre, idHijo) {
        const lineas = document.querySelectorAll("path.linea-resaltada");
        lineas.forEach(l => l.classList.remove("linea-resaltada"));

        const linea = document.getElementById(`linea-${idPadre}-${idHijo}`);
        if (linea) linea.classList.add("linea-resaltada");
    },

    buscarNodos: function (textoBuscar) {
        textoBuscar = textoBuscar.toLowerCase();
        const contenedor = document.getElementById("contenedorArbol");
        const resultado = document.getElementById("resultadoBusqueda");
        const navegador = document.getElementById("navegadorBusqueda");
        const contador = document.getElementById("contadorBusqueda");

        if (!contenedor || !resultado || !navegador || !contador) return;

        const nodos = contenedor.querySelectorAll(".org-node");
        nodos.forEach(n => n.classList.remove("resaltado", "opacado", "activo"));

        resultado.style.display = "none";
        navegador.style.display = "none";
        resultado.innerText = "";

        if (!textoBuscar) {
            OrgChart._coincidencias = [];
            return;
        }

        // Buscar nodos
        OrgChart._coincidencias = Array.from(nodos).filter(nodo => {
            const nombre = nodo.dataset.nombre || "";
            const usuario = nodo.dataset.usuario || "";
            const puesto = nodo.dataset.puesto || "";
            return nombre.includes(textoBuscar) || usuario.includes(textoBuscar) || puesto.includes(textoBuscar);
        });

        if (OrgChart._coincidencias.length === 0) {
            resultado.style.display = "block";
            resultado.innerText = "No se encontraron resultados para: " + textoBuscar;
            OrgChart._coincidencias = [];
            return;
        }

        // Opacar los demás
        nodos.forEach(nodo => {
            if (!OrgChart._coincidencias.includes(nodo)) nodo.classList.add("opacado");
        });

        // Mostrar navegador
        navegador.style.display = "block";
        OrgChart._coincidenciaActual = 0;
        OrgChart._actualizarNavegacion();
    },

    buscarSiguiente: function () {
        if (!OrgChart._coincidencias || OrgChart._coincidencias.length === 0) return;
        OrgChart._coincidenciaActual = (OrgChart._coincidenciaActual + 1) % OrgChart._coincidencias.length;
        OrgChart._actualizarNavegacion();
    },

    buscarAnterior: function () {
        if (!OrgChart._coincidencias || OrgChart._coincidencias.length === 0) return;
        OrgChart._coincidenciaActual = (OrgChart._coincidenciaActual - 1 + OrgChart._coincidencias.length) % OrgChart._coincidencias.length;
        OrgChart._actualizarNavegacion();
    },

    _actualizarNavegacion: function () {
        const contenedor = document.getElementById("contenedorArbol");
        const contenido = contenedor.querySelector("#orgZoomContainer");
        const contador = document.getElementById("contadorBusqueda");

        const nodos = contenedor.querySelectorAll(".org-node");
        nodos.forEach(n => n.classList.remove("activo"));

        const nodo = OrgChart._coincidencias[OrgChart._coincidenciaActual];
        if (!nodo) return;

        nodo.classList.add("activo");

        // Obtener posición relativa del nodo al contenedor
        const contRect = contenedor.getBoundingClientRect();
        const nodoRect = nodo.getBoundingClientRect();

        const scrollLeftActual = contenedor.scrollLeft;
        const scrollTopActual = contenedor.scrollTop;

        const scale = parseFloat(contenido.dataset.scale || "1");

        const nodoCenterX = (nodo.offsetLeft + nodo.offsetWidth / 2) * scale;
        const nodoCenterY = (nodo.offsetTop + nodo.offsetHeight / 2) * scale;

        const contenedorCenterX = contenedor.clientWidth / 2;
        const contenedorCenterY = contenedor.clientHeight / 2;

        const targetScrollLeft = nodoCenterX - contenedorCenterX;
        const targetScrollTop = nodoCenterY - contenedorCenterY;

        contenedor.scrollTo({
            left: targetScrollLeft,
            top: targetScrollTop,
            behavior: "smooth"
        });

        // Actualiza contador visual
        contador.innerText = `${OrgChart._coincidenciaActual + 1} de ${OrgChart._coincidencias.length}`;
    },

    toggleSubarbol(idNodo) {
        if (!this._estadoNodos) this._estadoNodos = {};

        const hijos = this._datosOriginales.filter(emp => emp.COD_USUARIO_JEFE === idNodo);

        const hijosVisibles = hijos.some(h => this._estadoNodos[h.USUARIO] !== false);

        if (hijosVisibles) {
            // Colapsar: ocultar todos los hijos y descendientes
            const ocultarDescendientes = (idPadre) => {
                const subhijos = this._datosOriginales.filter(emp => emp.COD_USUARIO_JEFE === idPadre);
                subhijos.forEach(hijo => {
                    this._estadoNodos[hijo.USUARIO] = false;
                    ocultarDescendientes(hijo.USUARIO);
                });
            };

            hijos.forEach(h => {
                this._estadoNodos[h.USUARIO] = false;
                ocultarDescendientes(h.USUARIO);
            });

        } else {
            // Expandir: mostrar hijos directos
            hijos.forEach(h => {
                this._estadoNodos[h.USUARIO] = true;
            });
        }

        // Asegurar que el nodo padre siempre está visible
        this._estadoNodos[idNodo] = true;

        // Re-renderizar
        const contenedor = document.getElementById("contenedorArbol");
        this.renderizarPorColumnas(this._datosOriginales, contenedor);
    },


    tieneHijosVisibles(idNodo) {
        const hijos = this._datosOriginales.filter(emp => emp.COD_USUARIO_JEFE === idNodo);
        return hijos.some(hijo => this._estadoNodos[hijo.USUARIO] !== false);
    },


    mostrarModal(usuario) {
        const modal = document.getElementById("orgModal");
        const body = document.getElementById("orgModalBody");
        if (!modal || !body) return;

        body.innerHTML = `
            <strong>Usuario:</strong> ${usuario.USUARIO}<br>
            <strong>Nombre:</strong> ${usuario.NOMBRE_USUARIO}<br>
            <strong>Puesto:</strong> ${usuario.PUESTO}<br>
            <strong>Código Puesto:</strong> ${usuario.COD_PUESTO}<br>
            <strong>Jefe:</strong> ${usuario.NOMBRE_JEFE || "-"}<br>
            <strong>Código Jefe:</strong> ${usuario.COD_USUARIO_JEFE || "-"}`;
        modal.style.display = "block";
    },

    inicializarModal: function () {
        const modal = document.getElementById("orgModal");
        const close = document.getElementById("orgModalClose");
        if (!modal || !close) return;

        close.onclick = () => {
            modal.style.display = "none";
        };

        window.onclick = (event) => {
            if (event.target === modal) {
                modal.style.display = "none";
            }
        };
    },


    _habilitarPanZoom(contenedor, contenido) {
        let startX, startY, scrollLeft, scrollTop, isPanning = false;

        contenedor.addEventListener("mousedown", e => {
            if (e.button !== 0 && e.button !== 1) return;
            isPanning = true;
            startX = e.clientX;
            startY = e.clientY;
            scrollLeft = contenedor.scrollLeft;
            scrollTop = contenedor.scrollTop;
            contenedor.style.cursor = "grabbing";
            e.preventDefault();
        });

        contenedor.addEventListener("mousemove", e => {
            if (!isPanning) return;
            contenedor.scrollLeft = scrollLeft - (e.clientX - startX);
            contenedor.scrollTop = scrollTop - (e.clientY - startY);
        });

        document.addEventListener("mouseup", () => {
            isPanning = false;
            contenedor.style.cursor = "default";
        });

        contenedor.addEventListener("wheel", e => {
            if (!e.ctrlKey) return;
            e.preventDefault();

            let scale = parseFloat(contenido.dataset.scale || "1");
            scale += e.deltaY < 0 ? 0.1 : -0.1;
            scale = Math.min(Math.max(scale, 0.5), 2.5);
            contenido.dataset.scale = scale.toFixed(2);

            contenido.style.transform = `scale(${scale})`;
            contenido.style.transformOrigin = "0 0";

            this.dibujarConexiones(this._datosOriginales);
        }, { passive: false });
    },

    // Zoom botones
    zoomIn() {
        const cont = document.getElementById("contenedorArbol");
        const wrapper = cont.querySelector("#orgZoomContainer");
        let scale = parseFloat(wrapper.dataset.scale || "1");
        scale = Math.min(2.5, scale + 0.1);
        wrapper.dataset.scale = scale.toFixed(2);
        wrapper.style.transform = `scale(${scale})`;
        wrapper.style.transformOrigin = "0 0";
        this.dibujarConexiones(this._datosOriginales);
    },

    zoomOut() {
        const cont = document.getElementById("contenedorArbol");
        const wrapper = cont.querySelector("#orgZoomContainer");
        let scale = parseFloat(wrapper.dataset.scale || "1");
        scale = Math.max(0.5, scale - 0.1);
        wrapper.dataset.scale = scale.toFixed(2);
        wrapper.style.transform = `scale(${scale})`;
        wrapper.style.transformOrigin = "0 0";
        this.dibujarConexiones(this._datosOriginales);
    },

    centrar() {
        const cont = document.getElementById("contenedorArbol");
        const wrapper = cont.querySelector("#orgZoomContainer");
        const scale = parseFloat(wrapper.dataset.scale || "1");
        cont.scrollTo({
            left: (wrapper.scrollWidth * scale - cont.clientWidth) / 2,
            top: (wrapper.scrollHeight * scale - cont.clientHeight) / 2,
            behavior: "smooth"
        });
    }
};
