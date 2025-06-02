// Funcionalidad para la página de empleados
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar componentes
    initializeSearch()
    initializeFilters()
    initializeSorting()
    initializePagination()
    initializeDeleteModal()
})

// Búsqueda de empleados
function initializeSearch() {
    const searchInput = document.getElementById("searchInput")
    if (!searchInput) return

    searchInput.addEventListener("input", function () {
        const searchTerm = this.value.toLowerCase().trim()
        filterTable(searchTerm)
    })
}

// Filtrar la tabla según el término de búsqueda
function filterTable(searchTerm = "", estado = "todos", genero = "todos") {
    const table = document.getElementById("empleadosTable")
    if (!table) return

    const rows = table.querySelectorAll("tbody tr")
    let visibleCount = 0

    rows.forEach((row) => {
        if (row.classList.contains("no-data-row")) return

        const rowText = row.textContent.toLowerCase()
        const rowEstado = row.getAttribute("data-estado")
        const rowGenero = row.getAttribute("data-genero")

        const matchesSearch = searchTerm === "" || rowText.includes(searchTerm)
        const matchesEstado = estado === "todos" || rowEstado === estado
        const matchesGenero = genero === "todos" || rowGenero === genero

        const isVisible = matchesSearch && matchesEstado && matchesGenero
        row.style.display = isVisible ? "" : "none"

        if (isVisible) visibleCount++
    })

    // Actualizar contadores
    updateCounters(visibleCount)

    // Mostrar mensaje de no resultados si no hay filas visibles
    showNoResultsMessage(visibleCount === 0)
}

// Inicializar filtros
function initializeFilters() {
    const estadoFilter = document.getElementById("estadoFilter")
    const generoFilter = document.getElementById("generoFilter")
    const resetFilters = document.getElementById("resetFilters")

    if (!estadoFilter || !generoFilter || !resetFilters) return

    estadoFilter.addEventListener("change", applyFilters)
    generoFilter.addEventListener("change", applyFilters)

    resetFilters.addEventListener("click", () => {
        estadoFilter.value = "todos"
        generoFilter.value = "todos"
        document.getElementById("searchInput").value = ""
        applyFilters()
    })
}

// Aplicar filtros a la tabla
function applyFilters() {
    const searchTerm = document.getElementById("searchInput").value.toLowerCase().trim()
    const estado = document.getElementById("estadoFilter").value
    const genero = document.getElementById("generoFilter").value

    filterTable(searchTerm, estado, genero)
}

// Actualizar contadores de empleados
function updateCounters(visibleCount) {
    const showingStart = document.getElementById("showing-start")
    const showingEnd = document.getElementById("showing-end")
    const totalItems = document.getElementById("total-items")

    if (showingStart && showingEnd && totalItems) {
        showingStart.textContent = visibleCount > 0 ? "1" : "0"
        showingEnd.textContent = visibleCount.toString()
        // No actualizamos totalItems porque es el total real, no filtrado
    }
}

// Mostrar mensaje de no resultados
function showNoResultsMessage(show) {
    const table = document.getElementById("empleadosTable")
    if (!table) return

    let noResultsRow = table.querySelector(".no-results-row")

    if (show) {
        if (!noResultsRow) {
            const tbody = table.querySelector("tbody")
            noResultsRow = document.createElement("tr")
            noResultsRow.className = "no-results-row"
            noResultsRow.innerHTML = `
        <td colspan="9" class="no-data">
          <div class="no-data-message">
            <i class="fas fa-search"></i>
            <p>No se encontraron empleados con los criterios de búsqueda</p>
            <button class="btn-reset-filters" onclick="document.getElementById('resetFilters').click()">
              <i class="fas fa-sync-alt"></i>
              Reiniciar Filtros
            </button>
          </div>
        </td>
      `
            tbody.appendChild(noResultsRow)
        }
        noResultsRow.style.display = ""
    } else if (noResultsRow) {
        noResultsRow.style.display = "none"
    }
}

// Inicializar ordenamiento de columnas
function initializeSorting() {
    const table = document.getElementById("empleadosTable")
    if (!table) return

    const headers = table.querySelectorAll("th.sortable")
    headers.forEach((header) => {
        header.addEventListener("click", function () {
            const column = this.getAttribute("data-sort")
            const isAsc = this.classList.contains("asc")

            // Eliminar clases de ordenamiento de todos los encabezados
            headers.forEach((h) => {
                h.classList.remove("asc", "desc")
            })

            // Establecer la dirección de ordenamiento
            if (isAsc) {
                this.classList.add("desc")
                sortTable(column, false)
            } else {
                this.classList.add("asc")
                sortTable(column, true)
            }
        })
    })
}

// Ordenar tabla por columna
function sortTable(column, asc) {
    const table = document.getElementById("empleadosTable")
    if (!table) return

    const tbody = table.querySelector("tbody")
    const rows = Array.from(tbody.querySelectorAll("tr:not(.no-results-row):not(.no-data-row)"))

    // Ordenar filas
    const sortedRows = rows.sort((a, b) => {
        let aValue, bValue

        // Obtener el índice de la columna
        const headers = table.querySelectorAll("th")
        let columnIndex = 0

        for (let i = 0; i < headers.length; i++) {
            if (headers[i].getAttribute("data-sort") === column) {
                columnIndex = i
                break
            }
        }

        aValue = a.cells[columnIndex].textContent.trim()
        bValue = b.cells[columnIndex].textContent.trim()

        // Convertir a número si es posible
        if (!isNaN(aValue) && !isNaN(bValue)) {
            return asc ? Number(aValue) - Number(bValue) : Number(bValue) - Number(aValue)
        }

        // Manejar fechas
        if (column === "ingreso") {
            const dateA = parseDate(aValue)
            const dateB = parseDate(bValue)
            return asc ? dateA - dateB : dateB - dateA
        }

        // Ordenamiento de texto
        return asc ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue)
    })

    // Reordenar el DOM
    sortedRows.forEach((row) => tbody.appendChild(row))
}

// Parsear fecha en formato dd/mm/yyyy
function parseDate(dateStr) {
    const parts = dateStr.split("/")
    return new Date(parts[2], parts[1] - 1, parts[0])
}

// Inicializar paginación
function initializePagination() {
    // Esta es una implementación básica. Para una tabla grande,
    // se recomienda implementar paginación del lado del servidor.
    const table = document.getElementById("empleadosTable")
    if (!table) return

    const rowsPerPage = 10
    const rows = table.querySelectorAll("tbody tr:not(.no-results-row):not(.no-data-row)")
    const totalPages = Math.ceil(rows.length / rowsPerPage)

    if (totalPages <= 1) {
        document.getElementById("prev-page").disabled = true
        document.getElementById("next-page").disabled = true
        return
    }

    // Generar botones de página
    const paginationPages = document.getElementById("pagination-pages")
    paginationPages.innerHTML = ""

    for (let i = 1; i <= totalPages; i++) {
        const pageBtn = document.createElement("button")
        pageBtn.className = i === 1 ? "page-btn active" : "page-btn"
        pageBtn.textContent = i.toString()
        pageBtn.addEventListener("click", () => {
            goToPage(i)
        })
        paginationPages.appendChild(pageBtn)
    }

    // Configurar botones de navegación
    document.getElementById("prev-page").addEventListener("click", () => {
        const currentPage = getCurrentPage()
        if (currentPage > 1) {
            goToPage(currentPage - 1)
        }
    })

    document.getElementById("next-page").addEventListener("click", () => {
        const currentPage = getCurrentPage()
        if (currentPage < totalPages) {
            goToPage(currentPage + 1)
        }
    })

    // Mostrar primera página
    goToPage(1)
}

// Obtener página actual
function getCurrentPage() {
    const activeBtn = document.querySelector(".page-btn.active")
    return activeBtn ? Number.parseInt(activeBtn.textContent) : 1
}

// Ir a una página específica
function goToPage(pageNumber) {
    const table = document.getElementById("empleadosTable")
    if (!table) return

    const rowsPerPage = 10
    const rows = Array.from(table.querySelectorAll("tbody tr:not(.no-results-row):not(.no-data-row)"))
    const totalPages = Math.ceil(rows.length / rowsPerPage)

    // Actualizar botones de página
    const pageButtons = document.querySelectorAll(".page-btn")
    pageButtons.forEach((btn) => {
        btn.classList.remove("active")
        if (Number.parseInt(btn.textContent) === pageNumber) {
            btn.classList.add("active")
        }
    })

    // Actualizar botones de navegación
    document.getElementById("prev-page").disabled = pageNumber === 1
    document.getElementById("next-page").disabled = pageNumber === totalPages

    // Mostrar filas de la página actual
    const startIndex = (pageNumber - 1) * rowsPerPage
    const endIndex = Math.min(startIndex + rowsPerPage, rows.length)

    rows.forEach((row, index) => {
        row.style.display = index >= startIndex && index < endIndex ? "" : "none"
    })

    // Actualizar información de paginación
    document.getElementById("showing-start").textContent = rows.length > 0 ? startIndex + 1 : 0
    document.getElementById("showing-end").textContent = endIndex
    document.getElementById("total-items").textContent = rows.length
}

// Inicializar modal de confirmación para eliminar
function initializeDeleteModal() {
    const deleteModal = document.getElementById("deleteModal")
    if (!deleteModal) return

    // Obtener todos los botones de eliminar
    const deleteButtons = document.querySelectorAll(".btn-action.delete")

    deleteButtons.forEach((button) => {
        button.addEventListener("click", function (e) {
            e.preventDefault()
            const deleteUrl = this.getAttribute("href")

            // Mostrar modal
            deleteModal.classList.add("active")

            // Configurar botón de confirmación
            const confirmButton = document.getElementById("confirmDelete")
            confirmButton.onclick = () => {
                window.location.href = deleteUrl
            }
        })
    })

    // Configurar botones para cerrar el modal
    const closeButtons = [document.getElementById("closeModal"), document.getElementById("cancelDelete")]

    closeButtons.forEach((button) => {
        if (button) {
            button.addEventListener("click", () => {
                deleteModal.classList.remove("active")
            })
        }
    })

    // Cerrar modal al hacer clic fuera
    deleteModal.addEventListener("click", (e) => {
        if (e.target === deleteModal) {
            deleteModal.classList.remove("active")
        }
    })
}
