// JavaScript moderno para la bitácora
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
  const dayGroups = document.querySelectorAll(".day-group")
  const btnDetails = document.querySelectorAll(".btn-details")

  // Modal para detalles
  let modalDetails = null

  // Datos originales para filtrado
  let originalData = []

  // Importar Bootstrap y pdfMake
  const bootstrap = window.bootstrap
  const pdfMake = window.pdfMake

  // Inicializar
  init()

  function init() {
    // Crear modal para detalles
    createDetailsModal()

    // Guardar datos originales
    saveOriginalData()

    // Event listeners para mostrar/ocultar formulario de agregar
    if (btnAgregarRegistro && addRecordForm) {
      btnAgregarRegistro.addEventListener("click", mostrarFormularioAgregar)
    }

    if (btnCerrarForm && addRecordForm) {
      btnCerrarForm.addEventListener("click", ocultarFormularioAgregar)
    }

    // Event listeners para filtros - CORREGIDO
    if (btnFiltros && filtrosPanel) {
      btnFiltros.addEventListener("click", (e) => {
        e.preventDefault()
        toggleFiltros()
      })
    }

    if (btnCerrarFiltros && filtrosPanel) {
      btnCerrarFiltros.addEventListener("click", (e) => {
        e.preventDefault()
        ocultarFiltros()
      })
    }

    // Event listeners para acciones
    if (btnAplicarFiltros)
      btnAplicarFiltros.addEventListener("click", () => {
        aplicarFiltros()
        ocultarFiltros()
      })
    if (btnLimpiarFiltros) btnLimpiarFiltros.addEventListener("click", limpiarFiltros)
    if (btnRefrescar) btnRefrescar.addEventListener("click", () => location.reload())
    if (btnExportarExcel) btnExportarExcel.addEventListener("click", exportarExcel)
    if (btnExportarPDF) btnExportarPDF.addEventListener("click", exportarPDF)

    // Event listener para ver detalles de registro - CORREGIDO
    if (btnDetails.length > 0) {
      btnDetails.forEach((btn) => {
        btn.addEventListener("click", function (e) {
          e.preventDefault()
          const idRegistro = this.getAttribute("data-id")
          const timelineItem = this.closest(".timeline-item")
          verDetallesRegistro(idRegistro, timelineItem)
        })
      })
    }

    // Filtros en tiempo real - ACTUALIZADO para select de acción
    if (filtroAccion) filtroAccion.addEventListener("change", aplicarFiltros)
    if (filtroUsuario) filtroUsuario.addEventListener("input", debounce(aplicarFiltros, 300))
    if (filtroFechaDesde) filtroFechaDesde.addEventListener("change", aplicarFiltros)
    if (filtroFechaHasta) filtroFechaHasta.addEventListener("change", aplicarFiltros)

    // Animaciones de entrada
    animateCards()

    // Auto-refresh cada 30 segundos
    setInterval(autoRefresh, 30000)

    // Cerrar alertas automáticamente
    cerrarAlertasAutomaticamente()
  }

  // Función para crear modal de detalles
  function createDetailsModal() {
    modalDetails = document.createElement("div")
    modalDetails.className = "modal-details"
    modalDetails.innerHTML = `
      <div class="modal-details-content">
        <div class="modal-details-header">
          <h3><i class="fas fa-info-circle"></i> Detalles del Registro</h3>
          <button class="btn-close-modal">
            <i class="fas fa-times"></i>
          </button>
        </div>
        <div class="modal-details-body">
          <!-- El contenido se llenará dinámicamente -->
        </div>
      </div>
    `
    document.body.appendChild(modalDetails)

    // Event listener para cerrar modal
    const btnCloseModal = modalDetails.querySelector(".btn-close-modal")
    btnCloseModal.addEventListener("click", cerrarModalDetalles)

    // Cerrar modal al hacer clic fuera
    modalDetails.addEventListener("click", (e) => {
      if (e.target === modalDetails) {
        cerrarModalDetalles()
      }
    })
  }

  // Función para mostrar el formulario de agregar registro
  function mostrarFormularioAgregar() {
    if (addRecordForm) {
      // Ocultar filtros si están abiertos
      if (filtrosPanel && filtrosPanel.classList.contains("active")) {
        ocultarFiltros()
      }

      addRecordForm.classList.add("active")
      // Scroll suave hacia el formulario
      setTimeout(() => {
        addRecordForm.scrollIntoView({ behavior: "smooth", block: "start" })
      }, 100)
    }
  }

  // Función para ocultar el formulario de agregar registro
  function ocultarFormularioAgregar() {
    if (addRecordForm) {
      addRecordForm.classList.remove("active")
    }
  }

  // Función para mostrar/ocultar filtros - CORREGIDO
  function toggleFiltros() {
    if (filtrosPanel) {
      // Ocultar formulario si está abierto
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

  // Función para mostrar filtros
  function mostrarFiltros() {
    if (filtrosPanel) {
      filtrosPanel.classList.add("active")
      // Scroll suave hacia los filtros
      setTimeout(() => {
        filtrosPanel.scrollIntoView({ behavior: "smooth", block: "start" })
      }, 100)
    }
  }

  // Función para ocultar filtros
  function ocultarFiltros() {
    if (filtrosPanel) {
      filtrosPanel.classList.remove("active")
    }
  }

  function saveOriginalData() {
    const timelineDays = document.querySelectorAll(".day-group")
    originalData = Array.from(timelineDays).map((day) => ({
      element: day.cloneNode(true),
      date: day.querySelector(".day-date")?.textContent.trim() || "",
      activities: Array.from(day.querySelectorAll(".timeline-item")).map((item) => ({
        element: item.cloneNode(true),
        accion: item.querySelector(".timeline-action")?.textContent.trim() || "",
        descripcion: item.querySelector(".timeline-body p")?.textContent.trim() || "",
        usuario: item.querySelector(".timeline-user")?.textContent.trim() || "",
        fecha: item.querySelector(".timeline-time")?.textContent.trim() || "",
      })),
    }))
  }

  // Función aplicarFiltros actualizada para manejar el select de acción
  function aplicarFiltros() {
    const accion = filtroAccion ? filtroAccion.value : ""
    const usuario = filtroUsuario ? filtroUsuario.value.toLowerCase() : ""
    const fechaDesde = filtroFechaDesde ? new Date(filtroFechaDesde.value) : null
    const fechaHasta = filtroFechaHasta ? new Date(filtroFechaHasta.value) : null

    // Si no hay filtros, mostrar todo
    if (!accion && !usuario && !fechaDesde && !fechaHasta) {
      mostrarTodosLosRegistros()
      actualizarContador()
      return
    }

    // Filtrar por día
    let diasVisibles = 0
    let registrosVisibles = 0

    dayGroups.forEach((dayGroup) => {
      const fechaStr = dayGroup.querySelector(".day-date").textContent
      const partes = fechaStr.split("/")
      const fechaDia = new Date(partes[2], partes[1] - 1, partes[0])

      // Verificar si el día está dentro del rango de fechas
      let mostrarDia = true
      if (fechaDesde && fechaDia < fechaDesde) mostrarDia = false
      if (fechaHasta && fechaDia > fechaHasta) mostrarDia = false

      // Si el día está fuera del rango, ocultarlo
      if (!mostrarDia) {
        dayGroup.style.display = "none"
        return
      }

      // Filtrar registros dentro del día
      const timelineItems = dayGroup.querySelectorAll(".timeline-item")
      let itemsVisibles = 0

      timelineItems.forEach((item) => {
        const itemAccion = item.querySelector(".timeline-action").textContent.trim()
        const itemUsuario = item.querySelector(".timeline-user").textContent.toLowerCase()

        // Aplicar filtros de acción y usuario
        let mostrarItem = true

        // Para el filtro de acción, hacer coincidencia exacta si se selecciona una opción específica
        if (accion && accion !== "" && itemAccion !== accion) {
          mostrarItem = false
        }

        if (usuario && !itemUsuario.includes(usuario)) {
          mostrarItem = false
        }

        if (mostrarItem) {
          item.style.display = "block"
          itemsVisibles++
          registrosVisibles++
        } else {
          item.style.display = "none"
        }
      })

      // Si no hay items visibles en el día, ocultar el día
      if (itemsVisibles === 0) {
        dayGroup.style.display = "none"
      } else {
        dayGroup.style.display = "block"
        diasVisibles++

        // Actualizar contador de registros en el día
        const dayCount = dayGroup.querySelector(".day-count span")
        if (dayCount) {
          dayCount.textContent = `${itemsVisibles} registros`
        }
      }
    })

    // Actualizar contador total
    if (totalRegistros) {
      totalRegistros.textContent = `${registrosVisibles} registros encontrados en ${diasVisibles} días`
    }
  }

  function mostrarTodosLosRegistros() {
    dayGroups.forEach((dayGroup) => {
      dayGroup.style.display = "block"

      const timelineItems = dayGroup.querySelectorAll(".timeline-item")
      let itemsVisibles = 0

      timelineItems.forEach((item) => {
        item.style.display = "block"
        itemsVisibles++
      })

      // Actualizar contador de registros en el día
      const dayCount = dayGroup.querySelector(".day-count span")
      if (dayCount) {
        dayCount.textContent = `${itemsVisibles} registros`
      }
    })
  }

  function limpiarFiltros() {
    if (filtroAccion) {
      filtroAccion.selectedIndex = 0 // Resetear al placeholder
    }
    if (filtroUsuario) filtroUsuario.value = ""
    if (filtroFechaDesde) filtroFechaDesde.value = ""
    if (filtroFechaHasta) filtroFechaHasta.value = ""

    // Mostrar todos los registros
    mostrarTodosLosRegistros()
    actualizarContador()
  }

  function actualizarContador() {
    if (totalRegistros) {
      let total = 0
      let dias = 0

      dayGroups.forEach((dayGroup) => {
        if (dayGroup.style.display !== "none") {
          dias++
          const items = dayGroup.querySelectorAll(".timeline-item")
          items.forEach((item) => {
            if (item.style.display !== "none") {
              total++
            }
          })
        }
      })

      totalRegistros.textContent = `${total} registros encontrados en ${dias} días`
    }
  }

  function exportarExcel() {
    // Crear una tabla temporal con los datos visibles
    const table = document.createElement("table")
    const thead = document.createElement("thead")
    const tbody = document.createElement("tbody")

    // Crear encabezados
    const headerRow = document.createElement("tr")
    ;["ID", "Fecha", "Hora", "Acción", "Descripción", "ID Usuario", "ID Sistema"].forEach((text) => {
      const th = document.createElement("th")
      th.textContent = text
      headerRow.appendChild(th)
    })
    thead.appendChild(headerRow)
    table.appendChild(thead)

    // Agregar datos
    dayGroups.forEach((dayGroup) => {
      if (dayGroup.style.display !== "none") {
        const fecha = dayGroup.querySelector(".day-date").textContent
        const timelineItems = dayGroup.querySelectorAll(".timeline-item")

        timelineItems.forEach((item) => {
          if (item.style.display !== "none") {
            const row = document.createElement("tr")

            // ID (extraer del botón de detalles)
            const tdId = document.createElement("td")
            const btnDetail = item.querySelector(".btn-details")
            tdId.textContent = btnDetail ? btnDetail.getAttribute("data-id") : ""
            row.appendChild(tdId)

            // Fecha
            const tdFecha = document.createElement("td")
            tdFecha.textContent = fecha
            row.appendChild(tdFecha)

            // Hora
            const tdHora = document.createElement("td")
            tdHora.textContent = item.querySelector(".timeline-time").textContent
            row.appendChild(tdHora)

            // Acción
            const tdAccion = document.createElement("td")
            tdAccion.textContent = item.querySelector(".timeline-action").textContent
            row.appendChild(tdAccion)

            // Descripción
            const tdDesc = document.createElement("td")
            tdDesc.textContent = item.querySelector(".timeline-body p").textContent
            row.appendChild(tdDesc)

            // ID Usuario - Extraer solo el número del texto "Usuario: X"
            const tdUsuario = document.createElement("td")
            const usuarioText = item.querySelector(".timeline-user").textContent
            tdUsuario.textContent = usuarioText.replace("Usuario: ", "").trim()
            row.appendChild(tdUsuario)

            // ID Sistema - Extraer solo el número del texto "Sistema: X"
            const tdSistema = document.createElement("td")
            const sistemaText = item.querySelector(".timeline-system").textContent
            tdSistema.textContent = sistemaText.replace("Sistema: ", "").trim()
            row.appendChild(tdSistema)

            tbody.appendChild(row)
          }
        })
      }
    })

    table.appendChild(tbody)

    // Convertir tabla a CSV
    const csv = []
    const rows = table.querySelectorAll("tr")

    for (let i = 0; i < rows.length; i++) {
      const row = [],
        cols = rows[i].querySelectorAll("td, th")

      for (let j = 0; j < cols.length; j++) {
        // Escapar comillas y añadir texto
        const data = cols[j].textContent.replace(/"/g, '""')
        row.push('"' + data + '"')
      }
      csv.push(row.join(","))
    }

    // Descargar CSV
    const csvFile = new Blob([csv.join("\n")], { type: "text/csv" })
    const downloadLink = document.createElement("a")
    downloadLink.download = "Bitacora_" + new Date().toISOString().slice(0, 10) + ".csv"
    downloadLink.href = window.URL.createObjectURL(csvFile)
    downloadLink.style.display = "none"
    document.body.appendChild(downloadLink)
    downloadLink.click()
    document.body.removeChild(downloadLink)

    // Mostrar mensaje de éxito
    mostrarNotificacion("Archivo Excel exportado exitosamente", "success")
  }

  function exportarPDF() {
    // Verificar si pdfmake está disponible
    if (typeof pdfMake === "undefined") {
      mostrarNotificacion("La biblioteca pdfMake no está disponible. No se puede generar el PDF.", "error")
      return
    }

    // Preparar datos para el PDF
    const content = []
    let registros = []

    // Título
    content.push({
      text: "Bitácora del Sistema",
      style: "header",
      margin: [0, 0, 0, 10],
    })

    // Fecha de generación
    content.push({
      text: "Generado el: " + new Date().toLocaleDateString() + " " + new Date().toLocaleTimeString(),
      style: "subheader",
      margin: [0, 0, 0, 20],
    })

    // Recopilar datos visibles
    dayGroups.forEach((dayGroup) => {
      if (dayGroup.style.display !== "none") {
        const fecha = dayGroup.querySelector(".day-date").textContent
        const diaSemana = dayGroup.querySelector(".day-name").textContent

        // Agregar encabezado del día
        content.push({
          text: diaSemana + " - " + fecha,
          style: "dayHeader",
          margin: [0, 15, 0, 10],
        })

        const timelineItems = dayGroup.querySelectorAll(".timeline-item")

        timelineItems.forEach((item) => {
          if (item.style.display !== "none") {
            const hora = item.querySelector(".timeline-time").textContent
            const accion = item.querySelector(".timeline-action").textContent
            const descripcion = item.querySelector(".timeline-body p").textContent
            const usuario = item.querySelector(".timeline-user").textContent.replace("Usuario: ", "")
            const sistema = item.querySelector(".timeline-system").textContent.replace("Sistema: ", "")

            // Agregar registro
            registros.push([hora, accion, descripcion, usuario, sistema])
          }
        })

        // Agregar tabla de registros del día
        if (registros.length > 0) {
          content.push({
            table: {
              headerRows: 1,
              widths: ["auto", "auto", "*", "auto", "auto"],
              body: [["Hora", "Acción", "Descripción", "Usuario", "Sistema"], ...registros],
            },
            layout: "lightHorizontalLines",
          })

          // Limpiar registros para el siguiente día
          registros = []
        }
      }
    })

    // Definir estilos
    const docDefinition = {
      content: content,
      styles: {
        header: {
          fontSize: 18,
          bold: true,
          color: "#2c3e50",
        },
        subheader: {
          fontSize: 12,
          color: "#7f8c8d",
        },
        dayHeader: {
          fontSize: 14,
          bold: true,
          color: "#4a90e2",
        },
      },
      defaultStyle: {
        fontSize: 10,
      },
    }

    // Generar PDF
    pdfMake.createPdf(docDefinition).download("Bitacora_" + new Date().toISOString().slice(0, 10) + ".pdf")

    // Mostrar mensaje de éxito
    mostrarNotificacion("Archivo PDF exportado exitosamente", "success")
  }

  // Función para ver detalles de un registro - COMPLETAMENTE NUEVA
  function verDetallesRegistro(id, timelineItem) {
    if (!timelineItem) {
      mostrarNotificacion("No se pudo obtener la información del registro", "error")
      return
    }

    // Extraer información del elemento timeline
    const hora = timelineItem.querySelector(".timeline-time")?.textContent || "N/A"
    const accion = timelineItem.querySelector(".timeline-action")?.textContent || "N/A"
    const descripcion = timelineItem.querySelector(".timeline-body p")?.textContent || "N/A"
    const usuario = timelineItem.querySelector(".timeline-user")?.textContent.replace("Usuario: ", "") || "N/A"
    const sistema = timelineItem.querySelector(".timeline-system")?.textContent.replace("Sistema: ", "") || "N/A"

    // Obtener fecha del día
    const dayGroup = timelineItem.closest(".day-group")
    const fecha = dayGroup?.querySelector(".day-date")?.textContent || "N/A"
    const diaSemana = dayGroup?.querySelector(".day-name")?.textContent || "N/A"

    // Llenar el modal con los detalles
    const modalBody = modalDetails.querySelector(".modal-details-body")
    modalBody.innerHTML = `
      <div class="detail-item">
        <div class="detail-label">ID Registro:</div>
        <div class="detail-value">${id || "N/A"}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Fecha:</div>
        <div class="detail-value">${diaSemana} - ${fecha}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Hora:</div>
        <div class="detail-value">${hora}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Acción:</div>
        <div class="detail-value">${accion}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Descripción:</div>
        <div class="detail-value">${descripcion}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Usuario:</div>
        <div class="detail-value">${usuario}</div>
      </div>
      <div class="detail-item">
        <div class="detail-label">Sistema:</div>
        <div class="detail-value">${sistema}</div>
      </div>
    `

    // Mostrar el modal
    modalDetails.classList.add("active")
  }

  // Función para cerrar modal de detalles
  function cerrarModalDetalles() {
    if (modalDetails) {
      modalDetails.classList.remove("active")
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

    // Auto-remover después de 5 segundos
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
          // Verificar si Bootstrap está disponible
          if (bootstrap && bootstrap.Alert) {
            const bsAlert = new bootstrap.Alert(alerta)
            bsAlert.close()
          } else {
            // Fallback si Bootstrap no está disponible
            alerta.style.opacity = "0"
            setTimeout(() => {
              alerta.style.display = "none"
            }, 300)
          }
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

  function autoRefresh() {
    // En producción, aquí harías una llamada AJAX para obtener nuevos datos
    console.log("Auto-refresh ejecutado")
  }

  // Utility function
  function debounce(func, wait) {
    let timeout
    return function executedFunction(...args) {
      const later = () => {
        clearTimeout(timeout)
        func(...args)
      }
      clearTimeout(timeout)
      timeout = setTimeout(later, wait)
    }
  }

  // Hacer funciones globales para uso en HTML
  window.limpiarFiltros = limpiarFiltros
  window.exportarExcel = exportarExcel
  window.exportarPDF = exportarPDF
  window.mostrarFormularioAgregar = mostrarFormularioAgregar
  window.ocultarFormularioAgregar = ocultarFormularioAgregar
  window.cerrarModalDetalles = cerrarModalDetalles
})
