document.addEventListener("DOMContentLoaded", () => {
    initializeSearch()
    initializeFilters()
    initializePagination()
    initializeDeleteModal()
})

// Búsqueda
function initializeSearch() {
    const searchInput = document.getElementById("searchInput")
    if (!searchInput) return

    searchInput.addEventListener("input", function () {
        const searchTerm = this.value.toLowerCase().trim()
        applyFilters(searchTerm)
    })
}

// Filtros
function initializeFilters() {
    const estadoFilter = document.getElementById("estadoFilter")
    const resetFilters = document.getElementById("resetFilters")

    if (!estadoFilter || !resetFilters) return

    estadoFilter.addEventListener("change", () => {
        applyFilters()
    })

    resetFilters.addEventListener("click", () => {
        document.getElementById("estadoFilter").value = "todos"
        document.getElementById("searchInput").value = ""
        applyFilters()
    })
}

function applyFilters(searchTerm = null) {
    const estado = document.getElementById("estadoFilter").value
    if (searchTerm === null) {
        searchTerm = document.getElementById("searchInput").value.toLowerCase().trim()
    }

    filterTable(searchTerm, estado)
}

// Filtrar tabla
function filterTable(searchTerm = "", estado = "todos") {
    const table = document.getElementById("departamentoTable")
    if (!table) return

    const rows = table.querySelectorAll("tbody tr")
    let visibleCount = 0

    rows.forEach(row => {
        if (row.classList.contains("no-data")) return

        const rowText = row.textContent.toLowerCase()
        const rowEstado = row.getAttribute("data-estado") // activo o inactivo

        const matchesSearch = searchTerm === "" || rowText.includes(searchTerm)
        const matchesEstado = estado === "todos" || rowEstado === estado

        const isVisible = matchesSearch && matchesEstado
        row.style.display = isVisible ? "" : "none"

        if (isVisible) visibleCount++
    })

    updateCounters(visibleCount)
    showNoResultsMessage(visibleCount === 0)
}

// Contadores
function updateCounters(visibleCount) {
    const showingStart = document.getElementById("showing-start")
    const showingEnd = document.getElementById("showing-end")

    if (showingStart && showingEnd) {
        showingStart.textContent = visibleCount > 0 ? "1" : "0"
        showingEnd.textContent = visibleCount.toString()
    }
}

// Mensaje si no hay resultados
function showNoResultsMessage(show) {
    let noDataRow = document.querySelector(".no-data-row")

    if (show) {
        if (!noDataRow) {
            const table = document.getElementById("departamentosTable")
            const tbody = table.querySelector("tbody")

            noDataRow = document.createElement("tr")
            noDataRow.classList.add("no-data-row")
            noDataRow.innerHTML = `
                <td colspan="7" class="no-data">
                    <div class="no-data-message">
                        <i class="fas fa-building-slash"></i>
                        <p>No se encontraron departamento con los criterios actuales.</p>
                    </div>
                </td>
            `
            tbody.appendChild(noDataRow)
        }
    } else {
        if (noDataRow) {
            noDataRow.remove()
        }
    }
}

// Paginación (por ahora deshabilitada o básica)
function initializePagination() {
    const prevBtn = document.getElementById("prev-page")
    const nextBtn = document.getElementById("next-page")

    if (prevBtn) prevBtn.disabled = true
    if (nextBtn) nextBtn.disabled = true
}

// Modal de eliminación (básico, solo interfaz)
function initializeDeleteModal() {
    const deleteButtons = document.querySelectorAll(".btn-action.delete")
    const modal = document.getElementById("deleteModal")
    const closeModalBtn = document.getElementById("closeModal")
    const cancelBtn = document.getElementById("cancelDelete")
    const confirmBtn = document.getElementById("confirmDelete")

    if (!modal) return

    let idToDelete = null

    deleteButtons.forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault()
            idToDelete= this.dataset.id
            modal.style.display = "flex"
        })
    })

    closeModalBtn?.addEventListener("click", () => {
        modal.style.display = "none"
    })

    cancelBtn?.addEventListener("click", () => {
        modal.style.display = "none"
    })

    confirmBtn?.addEventListener("click", () => {
        if (idToDelete) {
            window.location.href = `/Departamentos/Eliminar/${idToDelete}`
        }
    })
}

    // Cerrar modal
    [closeEditModal, cancelEdit].forEach(btn => {
        btn.addEventListener('click', function () {
            editModal.classList.remove('active');
        });
    });

    // Guardar cambios
    saveChanges.addEventListener('click', async function () {
        const form = document.getElementById('editDepartamentoForm');
        const formData = new FormData(form);

        const empresaData = {
            IdEmpresa: parseInt(formData.get('IdDepartamento')),
            Nombre: formData.get('Nombre'),
            Descripcion: formData.get('Descripcion'),
            Codigo: formData.get('Codigo'),
            Estado: formData.get('Estado') === 'true'
        };

        const btn = this;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Guardando...';
        btn.disabled = true;

        try {
            const response = await fetch('/Departamentos/Editar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify(empresaData)
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || 'Error en la respuesta del servidor');
            }

            if (data.success) {
                showToast('success', data.message);
                setTimeout(() => location.reload(), 1500);
            } else {
                showToast('error', data.message);
                if (data.errors) {
                    console.error('Errores de validación:', data.errors);
                }
            }
        } catch (error) {
            console.error('Error:', error);
            showToast('error', error.message);
        } finally {
            btn.innerHTML = '<i class="fas fa-save"></i> Guardar Cambios';
            btn.disabled = false;
        }
    });

    // Función para mostrar notificaciones
    function showToast(type, message) {

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        toast.textContent = message;
        document.body.appendChild(toast);

        setTimeout(() => toast.remove(), 5000);
    }
});