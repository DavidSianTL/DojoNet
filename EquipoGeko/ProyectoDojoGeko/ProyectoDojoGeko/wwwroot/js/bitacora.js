// JavaScript actualizado para bitácora con filtros mejorados
document.addEventListener("DOMContentLoaded", () => {
    console.log("Bitácora moderna cargada")

    // Referencias a elementos
    const btnAgregarRegistro = document.getElementById("btnAgregarRegistro")
    const addRecordForm = document.getElementById("addRecordForm")
    const btnCerrarForm = document.getElementById("btnCerrarForm")
    const filtroAccion = document.getElementById("filtroAccion")
    const filtroUsuario = document.getElementById("filtroUsuario")
    const filtroFechaDesde = document.getElementById("filtroFechaDesde")
    const filtroFechaHasta = document.getElementById("filtroFechaHasta")
    const btnFiltros = document.getElementById("btnFiltros")
    const btnCerrarFiltros = document.getElementById("btnCerrarFiltros")
    const filtrosPanel = document.getElementById("filtrosPanel")
    const btnAplicarFiltros = document.getElementById("btnAplicarFiltros")
    const btnLimpiarFiltros = document.getElementById("btnLimpiarFiltros")
    const btnRefrescar = document.getElementById("btnRefrescar")
    const btnExportarExcel = document.getElementById("btnExportarExcel")
    const btnExportarPDF = document.getElementById("btnExportarPDF")
    const totalRegistros = document.getElementById("totalRegistros")
    const bitacoraContent = document.querySelector(".bitacora-content")

    // Variables globales para filtros activos
    let filtrosActivos = {
        idUsuario: null,
        accion: null,
        fechaDesde: null,
        fechaHasta: null,
    }
    let modalDetails = null

    // Inicializar
    init()

    function init() {
        createDetailsModal()
        setupEventListeners()
        animateCards()
        cerrarAlertasAutomaticamente()
    }

    function setupEventListeners() {
        // Formulario agregar
        if (btnAgregarRegistro) btnAgregarRegistro.addEventListener("click", mostrarFormularioAgregar)
        if (btnCerrarForm) btnCerrarForm.addEventListener("click", ocultarFormularioAgregar)

        // Filtros
        if (btnFiltros)
            btnFiltros.addEventListener("click", (e) => {
                e.preventDefault()
                toggleFiltros()
            })
        if (btnCerrarFiltros)
            btnCerrarFiltros.addEventListener("click", (e) => {
                e.preventDefault()
                ocultarFiltros()
            })

        // Acciones de filtros
        if (btnAplicarFiltros) btnAplicarFiltros.addEventListener("click", aplicarFiltrosAjax)
        if (btnLimpiarFiltros) btnLimpiarFiltros.addEventListener("click", limpiarFiltros)
        if (btnRefrescar) btnRefrescar.addEventListener("click", () => location.reload())

        // Exportar - ahora respetan los filtros activos
        if (btnExportarExcel) btnExportarExcel.addEventListener("click", exportarExcel)
        if (btnExportarPDF) btnExportarPDF.addEventListener("click", exportarPDF)

        // Detalles de registros
        setupDetailsButtons()
    }

    function setupDetailsButtons() {
        const btnDetails = document.querySelectorAll(".btn-details")
        btnDetails.forEach((btn) => {
            btn.addEventListener("click", function (e) {
                e.preventDefault()
                const idRegistro = this.getAttribute("data-id")
                const timelineItem = this.closest(".timeline-item")
                verDetallesRegistro(idRegistro, timelineItem)
            })
        })
    }

    // Función para aplicar filtros usando AJAX
    async function aplicarFiltrosAjax() {
        try {
            mostrarCargando(true)

            // Guardar filtros activos
            filtrosActivos = {
                idUsuario: filtroUsuario && filtroUsuario.value ? Number.parseInt(filtroUsuario.value) : null,
                accion: filtroAccion && filtroAccion.value ? filtroAccion.value : null,
                fechaDesde: filtroFechaDesde && filtroFechaDesde.value ? filtroFechaDesde.value : null,
                fechaHasta: filtroFechaHasta && filtroFechaHasta.value ? filtroFechaHasta.value : null,
            }

            const params = new URLSearchParams()

            if (filtrosActivos.idUsuario) params.append("idUsuario", filtrosActivos.idUsuario.toString())
            if (filtrosActivos.accion) params.append("accion", filtrosActivos.accion)
            if (filtrosActivos.fechaDesde) params.append("fechaDesde", filtrosActivos.fechaDesde)
            if (filtrosActivos.fechaHasta) params.append("fechaHasta", filtrosActivos.fechaHasta)

            const response = await fetch(`/Bitacora/ObtenerBitacoras?${params.toString()}`)
            const result = await response.json()

            if (result.success) {
                actualizarVistaBitacora(result.data)
                mostrarNotificacion(`Se encontraron ${result.data.length} registros`, "success")

                // Actualizar indicador de filtros activos
                actualizarIndicadorFiltros()
            } else {
                mostrarNotificacion(result.error || "Error al aplicar filtros", "error")
            }
        } catch (error) {
            console.error("Error al aplicar filtros:", error)
            mostrarNotificacion("Error al aplicar filtros", "error")
        } finally {
            mostrarCargando(false)
            ocultarFiltros()
        }
    }

    function actualizarIndicadorFiltros() {
        // Crear o actualizar indicador de filtros activos
        let indicador = document.getElementById("filtros-activos-indicador")

        const hayFiltros =
            filtrosActivos.idUsuario || filtrosActivos.accion || filtrosActivos.fechaDesde || filtrosActivos.fechaHasta

        if (hayFiltros) {
            if (!indicador) {
                indicador = document.createElement("div")
                indicador.id = "filtros-activos-indicador"
                indicador.className = "alert alert-info"
                indicador.style.cssText = "margin: 10px 0; padding: 10px; border-radius: 5px;"

                const toolbar = document.querySelector(".action-toolbar")
                toolbar.parentNode.insertBefore(indicador, toolbar.nextSibling)
            }

            let textoFiltros = "<strong>Filtros activos:</strong> "
            const filtrosTexto = []

            if (filtrosActivos.idUsuario) filtrosTexto.push(`Usuario ID: ${filtrosActivos.idUsuario}`)
            if (filtrosActivos.accion) filtrosTexto.push(`Acción: ${filtrosActivos.accion}`)
            if (filtrosActivos.fechaDesde) filtrosTexto.push(`Desde: ${filtrosActivos.fechaDesde}`)
            if (filtrosActivos.fechaHasta) filtrosTexto.push(`Hasta: ${filtrosActivos.fechaHasta}`)

            textoFiltros += filtrosTexto.join(", ")
            indicador.innerHTML =
                textoFiltros +
                ' <button onclick="limpiarFiltros()" class="btn btn-sm btn-outline-secondary ms-2">Limpiar</button>'
        } else if (indicador) {
            indicador.remove()
        }
    }

    function actualizarVistaBitacora(registros) {
        if (!bitacoraContent) return

        if (!registros || registros.length === 0) {
            bitacoraContent.innerHTML = `
        <div class="no-records">
          <div class="no-records-icon">
            <i class="fas fa-info-circle"></i>
          </div>
          <h3>No hay registros disponibles</h3>
          <p>No se encontraron registros con los filtros aplicados.</p>
        </div>
      `
            if (totalRegistros) {
                totalRegistros.textContent = "0 registros encontrados"
            }
            return
        }

        // Agrupar registros por fecha
        const registrosPorFecha = {}
        registros.forEach((registro) => {
            const fecha = new Date(registro.FechaEntrada)
            const fechaKey = fecha.toISOString().split("T")[0]

            if (!registrosPorFecha[fechaKey]) {
                registrosPorFecha[fechaKey] = []
            }
            registrosPorFecha[fechaKey].push(registro)
        })

        // Generar HTML
        let html = ""
        Object.keys(registrosPorFecha)
            .sort((a, b) => new Date(b) - new Date(a))
            .forEach((fechaKey) => {
                const fecha = new Date(fechaKey)
                const diaSemana = fecha.toLocaleDateString("es-ES", { weekday: "long" })
                const fechaFormateada = fecha.toLocaleDateString("es-ES")
                const registrosDia = registrosPorFecha[fechaKey]
                const claseDia = obtenerClaseDia(fecha.getDay())

                html += `
          <div class="day-group ${claseDia}">
            <div class="day-header">
              <div class="day-info">
                <div class="day-name">${diaSemana.charAt(0).toUpperCase() + diaSemana.slice(1)}</div>
                <div class="day-date">${fechaFormateada}</div>
              </div>
              <div class="day-count">
                <span>${registrosDia.length} registros</span>
              </div>
            </div>
            <div class="day-content">
              <div class="timeline">
        `

                registrosDia
                    .sort((a, b) => new Date(b.FechaEntrada) - new Date(a.FechaEntrada))
                    .forEach((registro) => {
                        const fechaRegistro = new Date(registro.FechaEntrada)
                        const hora = fechaRegistro.toLocaleTimeString("es-ES")

                        html += `
              <div class="timeline-item">
                <div class="timeline-point"></div>
                <div class="timeline-content">
                  <div class="timeline-header">
                    <span class="timeline-time">${hora}</span>
                    <span class="timeline-action">${registro.Accion}</span>
                  </div>
                  <div class="timeline-body">
                    <p>${registro.Descripcion}</p>
                  </div>
                  <div class="timeline-footer">
                    <span class="timeline-user" data-id="${registro.FK_IdUsuario}">
                      <i class="fas fa-user"></i> Usuario: ${registro.FK_IdUsuario}
                    </span>
                    <span class="timeline-system" data-id="${registro.FK_IdSistema}">
                      <i class="fas fa-server"></i> Sistema: ${registro.FK_IdSistema}
                    </span>
                    <button class="btn-details" data-id="${registro.IdBitacora}" title="Ver detalles">
                      <i class="fas fa-eye"></i>
                    </button>
                  </div>
                </div>
              </div>
            `
                    })

                html += `
              </div>
            </div>
          </div>
        `
            })

        bitacoraContent.innerHTML = html

        // Actualizar contador
        if (totalRegistros) {
            const totalDias = Object.keys(registrosPorFecha).length
            totalRegistros.textContent = `${registros.length} registros encontrados en ${totalDias} días`
        }

        // Reconfigurar event listeners para los nuevos botones
        setupDetailsButtons()
        animateCards()
    }

    function obtenerClaseDia(dia) {
        const clases = [
            "day-sunday",
            "day-monday",
            "day-tuesday",
            "day-wednesday",
            "day-thursday",
            "day-friday",
            "day-saturday",
        ]
        return clases[dia] || ""
    }

    function limpiarFiltros() {
        if (filtroAccion) filtroAccion.selectedIndex = 0
        if (filtroUsuario) filtroUsuario.value = ""
        if (filtroFechaDesde) filtroFechaDesde.value = ""
        if (filtroFechaHasta) filtroFechaHasta.value = ""

        // Limpiar filtros activos
        filtrosActivos = {
            idUsuario: null,
            accion: null,
            fechaDesde: null,
            fechaHasta: null,
        }

        // Remover indicador
        const indicador = document.getElementById("filtros-activos-indicador")
        if (indicador) indicador.remove()

        location.reload() // Recargar para mostrar vista por defecto
    }

    // FUNCIÓN CORREGIDA PARA EXPORTAR EXCEL COMO .XLSX
    function exportarExcel() {
        const params = new URLSearchParams()

        // Usar filtros activos para la exportación
        if (filtrosActivos.idUsuario) params.append("idUsuario", filtrosActivos.idUsuario.toString())
        if (filtrosActivos.accion) params.append("accion", filtrosActivos.accion)
        if (filtrosActivos.fechaDesde) params.append("fechaDesde", filtrosActivos.fechaDesde)
        if (filtrosActivos.fechaHasta) params.append("fechaHasta", filtrosActivos.fechaHasta)

        // Forzar formato xlsx
        params.append("formato", "xlsx")

        const url = `/Bitacora/ExportarExcel?${params.toString()}`

        // Crear enlace temporal para descarga
        const link = document.createElement("a")
        link.href = url
        link.style.display = "none"

        // Ejecutar descarga
        document.body.appendChild(link)
        link.click()
        document.body.removeChild(link)

        mostrarNotificacion("Iniciando exportación a Excel (.xlsx)...", "info")
    }

    function exportarPDF() {
        const params = new URLSearchParams()

        // Usar filtros activos para la exportación
        if (filtrosActivos.idUsuario) params.append("idUsuario", filtrosActivos.idUsuario.toString())
        if (filtrosActivos.accion) params.append("accion", filtrosActivos.accion)
        if (filtrosActivos.fechaDesde) params.append("fechaDesde", filtrosActivos.fechaDesde)
        if (filtrosActivos.fechaHasta) params.append("fechaHasta", filtrosActivos.fechaHasta)

        window.location.href = `/Bitacora/ExportarPDF?${params.toString()}`
        mostrarNotificacion("Iniciando exportación a PDF...", "info")
    }

    function mostrarCargando(mostrar) {
        if (mostrar) {
            if (bitacoraContent) {
                bitacoraContent.innerHTML = `
          <div class="loading-container" style="text-align: center; padding: 50px;">
            <div class="loading-spinner" style="font-size: 2em; color: #007bff;">
              <i class="fas fa-spinner fa-spin"></i>
            </div>
            <p style="margin-top: 20px;">Cargando registros...</p>
          </div>
        `
            }
        }
    }

    function createDetailsModal() {
        modalDetails = document.createElement("div")
        modalDetails.className = "modal-details"
        modalDetails.style.cssText = `
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0,0,0,0.5);
      z-index: 9999;
      display: none;
      align-items: center;
      justify-content: center;
    `

        modalDetails.innerHTML = `
      <div class="modal-details-content" style="background: white; padding: 20px; border-radius: 8px; max-width: 500px; width: 90%;">
        <div class="modal-details-header" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
          <h3><i class="fas fa-info-circle"></i> Detalles del Registro</h3>
          <button class="btn-close-modal" style="background: none; border: none; font-size: 1.5em; cursor: pointer;">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-details-body">
          <!-- El contenido se llenará dinámicamente -->
        </div>
      </div>
    `
        document.body.appendChild(modalDetails)

        const btnCloseModal = modalDetails.querySelector(".btn-close-modal")
        btnCloseModal.addEventListener("click", cerrarModalDetalles)
        modalDetails.addEventListener("click", (e) => {
            if (e.target === modalDetails) {
                cerrarModalDetalles()
            }
        })
    }

    function mostrarFormularioAgregar() {
        if (addRecordForm) {
            if (filtrosPanel && filtrosPanel.classList.contains("active")) {
                ocultarFiltros()
            }
            addRecordForm.classList.add("active")
            setTimeout(() => {
                addRecordForm.scrollIntoView({ behavior: "smooth", block: "start" })
            }, 100)
        }
    }

    function ocultarFormularioAgregar() {
        if (addRecordForm) {
            addRecordForm.classList.remove("active")
        }
    }

    function toggleFiltros() {
        if (filtrosPanel) {
            if (addRecordForm && addRecordForm.classList.contains("active")) {
                ocultarFormularioAgregar()
            }
            if (filtrosPanel.classList.contains("active")) {
                ocultarFiltros()
            } else {
                mostrarFiltros()
            }
        }
    }

    function mostrarFiltros() {
        if (filtrosPanel) {
            filtrosPanel.classList.add("active")
            setTimeout(() => {
                filtrosPanel.scrollIntoView({ behavior: "smooth", block: "start" })
            }, 100)
        }
    }

    function ocultarFiltros() {
        if (filtrosPanel) {
            filtrosPanel.classList.remove("active")
        }
    }

    function verDetallesRegistro(id, timelineItem) {
        if (!timelineItem) {
            mostrarNotificacion("No se pudo obtener la información del registro", "error")
            return
        }

        const hora = timelineItem.querySelector(".timeline-time")?.textContent || "N/A"
        const accion = timelineItem.querySelector(".timeline-action")?.textContent || "N/A"
        const descripcion = timelineItem.querySelector(".timeline-body p")?.textContent || "N/A"
        const usuario = timelineItem.querySelector(".timeline-user")?.textContent.replace("Usuario: ", "") || "N/A"
        const sistema = timelineItem.querySelector(".timeline-system")?.textContent.replace("Sistema: ", "") || "N/A"

        const dayGroup = timelineItem.closest(".day-group")
        const fecha = dayGroup?.querySelector(".day-date")?.textContent || "N/A"
        const diaSemana = dayGroup?.querySelector(".day-name")?.textContent || "N/A"

        const modalBody = modalDetails.querySelector(".modal-details-body")
        modalBody.innerHTML = `
      <div style="margin-bottom: 15px;">
        <strong>ID Registro:</strong> ${id || "N/A"}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Fecha:</strong> ${diaSemana} - ${fecha}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Hora:</strong> ${hora}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Acción:</strong> ${accion}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Descripción:</strong> ${descripcion}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Usuario:</strong> ${usuario}
      </div>
      <div style="margin-bottom: 15px;">
        <strong>Sistema:</strong> ${sistema}
      </div>
    `

        modalDetails.style.display = "flex"
    }

    function cerrarModalDetalles() {
        if (modalDetails) {
            modalDetails.style.display = "none"
        }
    }

    function mostrarNotificacion(mensaje, tipo = "info") {
        const alertas = {
            success: "alert-success",
            error: "alert-danger",
            warning: "alert-warning",
            info: "alert-info",
        }
        const alerta = document.createElement("div")
        alerta.className = `alert ${alertas[tipo]} alert-dismissible fade show position-fixed`
        alerta.style.cssText = "top: 20px; right: 20px; z-index: 9999; min-width: 300px;"
        alerta.innerHTML = `
      <i class="fas fa-info-circle me-2"></i>
      ${mensaje}
      <button type="button" class="btn-close" onclick="this.parentElement.remove()"></button>
    `
        document.body.appendChild(alerta)

        setTimeout(() => {
            if (alerta.parentNode) {
                alerta.parentNode.removeChild(alerta)
            }
        }, 5000)
    }

    function cerrarAlertasAutomaticamente() {
        const alertas = document.querySelectorAll(".alert")
        if (alertas.length > 0) {
            setTimeout(() => {
                alertas.forEach((alerta) => {
                    alerta.style.opacity = "0"
                    setTimeout(() => {
                        if (alerta.parentNode) {
                            alerta.parentNode.removeChild(alerta)
                        }
                    }, 300)
                })
            }, 5000)
        }
    }

    function animateCards() {
        const cards = document.querySelectorAll(".timeline-item, .day-group")
        cards.forEach((card, index) => {
            card.style.animationDelay = `${index * 0.1}s`
        })
    }

    // Hacer funciones globales
    window.limpiarFiltros = limpiarFiltros
    window.exportarExcel = exportarExcel
    window.exportarPDF = exportarPDF
    window.mostrarFormularioAgregar = mostrarFormularioAgregar
    window.ocultarFormularioAgregar = ocultarFormularioAgregar
    window.cerrarModalDetalles = cerrarModalDetalles
})
