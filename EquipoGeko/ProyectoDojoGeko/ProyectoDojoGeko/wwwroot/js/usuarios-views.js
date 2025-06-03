/* ========================================
   JAVASCRIPT UNIFICADO PARA MÓDULO DE USUARIOS
   Sistema GEKO - Gestión de Usuarios
   ======================================== */

// =====================================
// VARIABLES GLOBALES
// =====================================
let usuariosData = []
let usuariosFiltrados = []
let paginaActual = 1
let registrosPorPagina = 10
const ordenActual = { campo: "IdUsuario", direccion: "asc" }
let usuarioSeleccionado = null
const $ = window.jQuery // Declare the $ variable
const toastr = window.toastr // Declare the toastr variable

// =====================================
// INICIALIZACIÓN
// =====================================
document.addEventListener("DOMContentLoaded", () => {
    inicializarModulo()
    cargarUsuarios()
    configurarEventos()
})

/**
 * Inicializa el módulo de usuarios
 */
function inicializarModulo() {
    console.log("Inicializando módulo de usuarios...")

    // Configurar tooltips
    if (typeof $.fn.tooltip === "function") {
        $("[title]").tooltip()
    }

    // Configurar select de registros por página
    const selectRegistros = document.getElementById("registrosPorPagina")
    if (selectRegistros) {
        selectRegistros.value = registrosPorPagina
    }
}

/**
 * Configura todos los eventos del módulo
 */
function configurarEventos() {
    // Evento para el campo de búsqueda
    const campoBusqueda = document.getElementById("busqueda")
    if (campoBusqueda) {
        campoBusqueda.addEventListener("input", debounce(buscarUsuarios, 300))
    }

    // Eventos para modales
    configurarModales()
}

/**
 * Configura los eventos de los modales
 */
function configurarModales() {
    // Modal de cambio de estado
    const modalCambiarEstado = document.getElementById("modalCambiarEstado")
    if (modalCambiarEstado) {
        modalCambiarEstado.addEventListener("hidden.bs.modal", () => {
            usuarioSeleccionado = null
        })
    }

    // Modal de eliminar usuario
    const modalEliminarUsuario = document.getElementById("modalEliminarUsuario")
    if (modalEliminarUsuario) {
        modalEliminarUsuario.addEventListener("hidden.bs.modal", () => {
            usuarioSeleccionado = null
        })
    }
}

// =====================================
// FUNCIONES DE CARGA DE DATOS
// =====================================

/**
 * Carga la lista de usuarios desde el servidor
 */
async function cargarUsuarios() {
    try {
        mostrarCargando(true)

        const response = await fetch("/Usuarios/ObtenerUsuarios", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        })

        if (response.ok) {
            const data = await response.json()
            usuariosData = data.usuarios || []
            usuariosFiltrados = [...usuariosData]

            actualizarTabla()
            cargarRolesUsuarios()
            actualizarContadores()
        } else {
            mostrarError("Error al cargar los usuarios")
        }
    } catch (error) {
        console.error("Error:", error)
        mostrarError("Error de conexión al cargar usuarios")
    } finally {
        mostrarCargando(false)
    }
}

/**
 * Carga los roles de todos los usuarios
 */
async function cargarRolesUsuarios() {
    for (const usuario of usuariosFiltrados) {
        await cargarRolesUsuario(usuario.IdUsuario)
    }
}

/**
 * Carga los roles de un usuario específico
 */
async function cargarRolesUsuario(idUsuario) {
    try {
        const response = await fetch(`/Usuarios/ObtenerRolesUsuario/${idUsuario}`)

        if (response.ok) {
            const roles = await response.json()
            actualizarRolesEnTabla(idUsuario, roles)
        }
    } catch (error) {
        console.error(`Error al cargar roles del usuario ${idUsuario}:`, error)
    }
}

/**
 * Actualiza la visualización de roles en la tabla
 */
function actualizarRolesEnTabla(idUsuario, roles) {
    const fila = document.querySelector(`tr[data-usuario-id="${idUsuario}"]`)
    if (fila) {
        const celdaRoles = fila.querySelector(".roles-container")
        if (celdaRoles) {
            if (roles && roles.length > 0) {
                celdaRoles.innerHTML = roles.map((rol) => `<span class="rol-badge">${rol.NombreRol}</span>`).join("")
            } else {
                celdaRoles.innerHTML = '<span class="text-muted">Sin roles</span>'
            }
        }
    }
}

// =====================================
// FUNCIONES DE FILTRADO Y BÚSQUEDA
// =====================================

/**
 * Aplica los filtros seleccionados
 */
function aplicarFiltros() {
    const filtroEstado = document.getElementById("filtroEstado").value
    const filtroRol = document.getElementById("filtroRol").value
    const busqueda = document.getElementById("busqueda").value.toLowerCase()

    usuariosFiltrados = usuariosData.filter((usuario) => {
        // Filtro por estado
        if (filtroEstado !== "" && usuario.Estado.toString() !== filtroEstado) {
            return false
        }

        // Filtro por búsqueda (username o empleado)
        if (busqueda) {
            const coincideUsername = usuario.Username.toLowerCase().includes(busqueda)
            const coincideEmpleado =
                usuario.Empleado &&
                (usuario.Empleado.Nombre.toLowerCase().includes(busqueda) ||
                    usuario.Empleado.Apellido.toLowerCase().includes(busqueda))

            if (!coincideUsername && !coincideEmpleado) {
                return false
            }
        }

        return true
    })

    paginaActual = 1
    actualizarTabla()
    actualizarContadores()
}

/**
 * Función de búsqueda con debounce
 */
function buscarUsuarios() {
    aplicarFiltros()
}

/**
 * Limpia todos los filtros
 */
function limpiarFiltros() {
    document.getElementById("filtroEstado").value = ""
    document.getElementById("filtroRol").value = ""
    document.getElementById("busqueda").value = ""

    usuariosFiltrados = [...usuariosData]
    paginaActual = 1
    actualizarTabla()
    actualizarContadores()
}

// =====================================
// FUNCIONES DE ORDENAMIENTO
// =====================================

/**
 * Ordena la tabla por el campo especificado
 */
function ordenarTabla(campo) {
    if (ordenActual.campo === campo) {
        ordenActual.direccion = ordenActual.direccion === "asc" ? "desc" : "asc"
    } else {
        ordenActual.campo = campo
        ordenActual.direccion = "asc"
    }

    usuariosFiltrados.sort((a, b) => {
        let valorA, valorB

        switch (campo) {
            case "IdUsuario":
                valorA = a.IdUsuario
                valorB = b.IdUsuario
                break
            case "Username":
                valorA = a.Username.toLowerCase()
                valorB = b.Username.toLowerCase()
                break
            case "Empleado":
                valorA = a.Empleado ? `${a.Empleado.Nombre} ${a.Empleado.Apellido}`.toLowerCase() : ""
                valorB = b.Empleado ? `${b.Empleado.Nombre} ${b.Empleado.Apellido}`.toLowerCase() : ""
                break
            case "Estado":
                valorA = a.Estado
                valorB = b.Estado
                break
            case "FechaCreacion":
                valorA = new Date(a.FechaCreacion)
                valorB = new Date(b.FechaCreacion)
                break
            default:
                return 0
        }

        if (valorA < valorB) return ordenActual.direccion === "asc" ? -1 : 1
        if (valorA > valorB) return ordenActual.direccion === "asc" ? 1 : -1
        return 0
    })

    actualizarIconosOrden()
    actualizarTabla()
}

/**
 * Actualiza los iconos de ordenamiento en la tabla
 */
function actualizarIconosOrden() {
    // Resetear todos los iconos
    document.querySelectorAll(".sortable i").forEach((icon) => {
        icon.className = "fas fa-sort"
    })

    // Actualizar el icono de la columna actual
    const columnaActual = document.querySelector(`th[onclick="ordenarTabla('${ordenActual.campo}')"] i`)
    if (columnaActual) {
        columnaActual.className = ordenActual.direccion === "asc" ? "fas fa-sort-up" : "fas fa-sort-down"
    }
}

// =====================================
// FUNCIONES DE PAGINACIÓN
// =====================================

/**
 * Cambia el número de registros por página
 */
function cambiarRegistrosPorPagina() {
    const select = document.getElementById("registrosPorPagina")
    registrosPorPagina = Number.parseInt(select.value)
    paginaActual = 1
    actualizarTabla()
}

/**
 * Cambia a una página específica
 */
function irAPagina(pagina) {
    paginaActual = pagina
    actualizarTabla()
}

/**
 * Genera la paginación
 */
function generarPaginacion() {
    const totalPaginas = Math.ceil(usuariosFiltrados.length / registrosPorPagina)
    const contenedorPaginacion = document.getElementById("paginacion")

    if (!contenedorPaginacion || totalPaginas <= 1) {
        if (contenedorPaginacion) contenedorPaginacion.innerHTML = ""
        return
    }

    let html = ""

    // Botón anterior
    html += `<li class="page-item ${paginaActual === 1 ? "disabled" : ""}">
                <a class="page-link" href="#" onclick="irAPagina(${paginaActual - 1})" aria-label="Anterior">
                    <span aria-hidden="true">&laquo;</span>
                </a>
             </li>`

    // Páginas
    const inicio = Math.max(1, paginaActual - 2)
    const fin = Math.min(totalPaginas, paginaActual + 2)

    if (inicio > 1) {
        html += `<li class="page-item"><a class="page-link" href="#" onclick="irAPagina(1)">1</a></li>`
        if (inicio > 2) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`
        }
    }

    for (let i = inicio; i <= fin; i++) {
        html += `<li class="page-item ${i === paginaActual ? "active" : ""}">
                    <a class="page-link" href="#" onclick="irAPagina(${i})">${i}</a>
                 </li>`
    }

    if (fin < totalPaginas) {
        if (fin < totalPaginas - 1) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`
        }
        html += `<li class="page-item"><a class="page-link" href="#" onclick="irAPagina(${totalPaginas})">${totalPaginas}</a></li>`
    }

    // Botón siguiente
    html += `<li class="page-item ${paginaActual === totalPaginas ? "disabled" : ""}">
                <a class="page-link" href="#" onclick="irAPagina(${paginaActual + 1})" aria-label="Siguiente">
                    <span aria-hidden="true">&raquo;</span>
                </a>
             </li>`

    contenedorPaginacion.innerHTML = html
}

// =====================================
// FUNCIONES DE ACTUALIZACIÓN DE UI
// =====================================

/**
 * Actualiza la tabla con los datos filtrados y paginados
 */
function actualizarTabla() {
    const tbody = document.getElementById("tablaUsuariosBody")
    if (!tbody) return

    const inicio = (paginaActual - 1) * registrosPorPagina
    const fin = inicio + registrosPorPagina
    const usuariosPagina = usuariosFiltrados.slice(inicio, fin)

    if (usuariosPagina.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="7" class="text-center no-data">
                    <div class="no-data-container">
                        <i class="fas fa-search fa-3x text-muted"></i>
                        <h4>No se encontraron usuarios</h4>
                        <p>Intenta ajustar los filtros de búsqueda</p>
                    </div>
                </td>
            </tr>`
        generarPaginacion()
        return
    }

    tbody.innerHTML = usuariosPagina
        .map(
            (usuario) => `
        <tr data-usuario-id="${usuario.IdUsuario}">
            <td class="usuario-id">${usuario.IdUsuario}</td>
            <td class="usuario-username">
                <div class="username-container">
                    <i class="fas fa-user user-icon"></i>
                    <span>${usuario.Username}</span>
                </div>
            </td>
            <td class="usuario-empleado">
                ${usuario.Empleado
                    ? `
                    <div class="empleado-info">
                        <span class="empleado-nombre">${usuario.Empleado.Nombre} ${usuario.Empleado.Apellido}</span>
                        <small class="empleado-puesto">${usuario.Empleado.Puesto || ""}</small>
                    </div>
                `
                    : '<span class="text-muted">Sin empleado asignado</span>'
                }
            </td>
            <td class="usuario-estado">
                ${usuario.Estado
                    ? '<span class="badge badge-success"><i class="fas fa-check-circle"></i> Activo</span>'
                    : '<span class="badge badge-danger"><i class="fas fa-times-circle"></i> Inactivo</span>'
                }
            </td>
            <td class="usuario-fecha">
                <span class="fecha-texto">${formatearFecha(usuario.FechaCreacion)}</span>
                <small class="fecha-hora">${formatearHora(usuario.FechaCreacion)}</small>
            </td>
            <td class="usuario-roles">
                <div class="roles-container">
                    <span class="loading-roles">Cargando...</span>
                </div>
            </td>
            <td class="usuario-acciones text-center">
                <div class="btn-group" role="group">
                    <button type="button" class="btn btn-sm btn-info" onclick="verDetalle(${usuario.IdUsuario})" title="Ver detalle">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-warning" onclick="editarUsuario(${usuario.IdUsuario})" title="Editar">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-secondary" onclick="asignarRoles(${usuario.IdUsuario})" title="Asignar roles">
                        <i class="fas fa-user-tag"></i>
                    </button>
                    <div class="btn-group" role="group">
                        <button type="button" class="btn btn-sm btn-outline-secondary dropdown-toggle" data-toggle="dropdown" title="Más acciones">
                            <i class="fas fa-ellipsis-v"></i>
                        </button>
                        <div class="dropdown-menu">
                            ${usuario.Estado
                    ? `<a class="dropdown-item" href="#" onclick="cambiarEstado(${usuario.IdUsuario}, false)">
                                    <i class="fas fa-ban text-warning"></i> Desactivar
                                </a>`
                    : `<a class="dropdown-item" href="#" onclick="cambiarEstado(${usuario.IdUsuario}, true)">
                                    <i class="fas fa-check text-success"></i> Activar
                                </a>`
                }
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#" onclick="resetearPassword(${usuario.IdUsuario})">
                                <i class="fas fa-key text-info"></i> Resetear contraseña
                            </a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item text-danger" href="#" onclick="eliminarUsuario(${usuario.IdUsuario})">
                                <i class="fas fa-trash"></i> Eliminar
                            </a>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    `,
        )
        .join("")

    generarPaginacion()

    // Cargar roles para los usuarios visibles
    usuariosPagina.forEach((usuario) => {
        cargarRolesUsuario(usuario.IdUsuario)
    })
}

/**
 * Actualiza los contadores de la interfaz
 */
function actualizarContadores() {
    const totalElement = document.getElementById("totalUsuarios")
    if (totalElement) {
        totalElement.textContent = usuariosFiltrados.length
    }
}

// =====================================
// FUNCIONES DE ACCIONES DE USUARIO
// =====================================

/**
 * Navega a la página de creación de usuario
 */
function crearUsuario() {
    window.location.href = "/Empleados/AsociarUsuario"
}

/**
 * Navega a la página de detalle del usuario
 */
function verDetalle(idUsuario) {
    window.location.href = `/Usuarios/Detalle/${idUsuario}`
}

/**
 * Navega a la página de edición del usuario
 */
function editarUsuario(idUsuario) {
    window.location.href = `/Usuarios/CreateEdit/${idUsuario}`
}

/**
 * Navega a la página de asignación de roles
 */
function asignarRoles(idUsuario) {
    window.location.href = `/Usuarios/AsignarRoles/${idUsuario}`
}

/**
 * Cambia el estado de un usuario
 */
function cambiarEstado(idUsuario, nuevoEstado) {
    usuarioSeleccionado = { id: idUsuario, estado: nuevoEstado }

    const mensaje = nuevoEstado
        ? "¿Está seguro que desea activar este usuario?"
        : "¿Está seguro que desea desactivar este usuario?"

    document.getElementById("mensajeCambioEstado").textContent = mensaje

    // Configurar el botón de confirmación
    const btnConfirmar = document.getElementById("confirmarCambioEstado")
    btnConfirmar.onclick = confirmarCambioEstado

    $("#modalCambiarEstado").modal("show")
}

/**
 * Confirma el cambio de estado
 */
async function confirmarCambioEstado() {
    if (!usuarioSeleccionado) return

    try {
        const response = await fetch("/Usuarios/CambiarEstado", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                idUsuario: usuarioSeleccionado.id,
                estado: usuarioSeleccionado.estado,
            }),
        })

        if (response.ok) {
            mostrarExito("Estado del usuario actualizado correctamente")
            $("#modalCambiarEstado").modal("hide")
            cargarUsuarios()
        } else {
            mostrarError("Error al cambiar el estado del usuario")
        }
    } catch (error) {
        console.error("Error:", error)
        mostrarError("Error de conexión al cambiar estado")
    }
}

/**
 * Resetea la contraseña de un usuario
 */
async function resetearPassword(idUsuario) {
    if (!confirm("¿Está seguro que desea resetear la contraseña de este usuario?")) {
        return
    }

    try {
        const response = await fetch("/Usuarios/ResetearPassword", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ idUsuario: idUsuario }),
        })

        if (response.ok) {
            const result = await response.json()
            mostrarExito("Contraseña reseteada correctamente. Nueva contraseña: " + result.nuevaPassword)
        } else {
            mostrarError("Error al resetear la contraseña")
        }
    } catch (error) {
        console.error("Error:", error)
        mostrarError("Error de conexión al resetear contraseña")
    }
}

/**
 * Elimina un usuario
 */
function eliminarUsuario(idUsuario) {
    usuarioSeleccionado = { id: idUsuario }

    // Configurar el botón de confirmación
    const btnConfirmar = document.getElementById("confirmarEliminarUsuario")
    btnConfirmar.onclick = confirmarEliminarUsuario

    $("#modalEliminarUsuario").modal("show")
}

/**
 * Confirma la eliminación del usuario
 */
async function confirmarEliminarUsuario() {
    if (!usuarioSeleccionado) return

    try {
        const response = await fetch("/Usuarios/Eliminar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ idUsuario: usuarioSeleccionado.id }),
        })

        if (response.ok) {
            mostrarExito("Usuario eliminado correctamente")
            $("#modalEliminarUsuario").modal("hide")
            cargarUsuarios()
        } else {
            mostrarError("Error al eliminar el usuario")
        }
    } catch (error) {
        console.error("Error:", error)
        mostrarError("Error de conexión al eliminar usuario")
    }
}

// =====================================
// FUNCIONES UTILITARIAS
// =====================================

/**
 * Formatea una fecha para mostrar
 */
function formatearFecha(fecha) {
    const date = new Date(fecha)
    return date.toLocaleDateString("es-ES")
}

/**
 * Formatea una hora para mostrar
 */
function formatearHora(fecha) {
    const date = new Date(fecha)
    return date.toLocaleTimeString("es-ES", { hour: "2-digit", minute: "2-digit" })
}

/**
 * Función debounce para optimizar búsquedas
 */
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

/**
 * Muestra el indicador de carga
 */
function mostrarCargando(mostrar) {
    const tabla = document.getElementById("tablaUsuarios")
    if (tabla) {
        if (mostrar) {
            tabla.style.opacity = "0.5"
            tabla.style.pointerEvents = "none"
        } else {
            tabla.style.opacity = "1"
            tabla.style.pointerEvents = "auto"
        }
    }
}

/**
 * Muestra un mensaje de éxito
 */
function mostrarExito(mensaje) {
    // Implementar según el sistema de notificaciones del proyecto
    if (toastr) {
        toastr.success(mensaje)
    } else {
        alert(mensaje)
    }
}

/**
 * Muestra un mensaje de error
 */
function mostrarError(mensaje) {
    // Implementar según el sistema de notificaciones del proyecto
    if (toastr) {
        toastr.error(mensaje)
    } else {
        alert(mensaje)
    }
}

// =====================================
// EXPORTAR FUNCIONES GLOBALES
// =====================================
window.aplicarFiltros = aplicarFiltros
window.buscarUsuarios = buscarUsuarios
window.limpiarFiltros = limpiarFiltros
window.ordenarTabla = ordenarTabla
window.cambiarRegistrosPorPagina = cambiarRegistrosPorPagina
window.irAPagina = irAPagina
window.crearUsuario = crearUsuario
window.verDetalle = verDetalle
window.editarUsuario = editarUsuario
window.asignarRoles = asignarRoles
window.cambiarEstado = cambiarEstado
window.resetearPassword = resetearPassword
window.eliminarUsuario = eliminarUsuario
