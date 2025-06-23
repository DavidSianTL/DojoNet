// Funcionalidad para la vista de detalle del empleado
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar componentes
    initializeDetailView()
    initializeModal()
    initializeActions()
})

// Inicializar la vista de detalle
function initializeDetailView() {
    // Animaciones de entrada
    const sections = document.querySelectorAll(".info-section")
    sections.forEach((section, index) => {
        section.style.opacity = "0"
        section.style.transform = "translateY(20px)"

        setTimeout(() => {
            section.style.transition = "all 0.6s ease-out"
            section.style.opacity = "1"
            section.style.transform = "translateY(0)"
        }, index * 100)
    })

    // Efectos hover para enlaces
    const emailLinks = document.querySelectorAll(".email-link")
    const phoneLinks = document.querySelectorAll(".phone-link")

    emailLinks.forEach((link) => {
        link.addEventListener("mouseenter", function () {
            this.style.transform = "translateX(4px)"
        })

        link.addEventListener("mouseleave", function () {
            this.style.transform = "translateX(0)"
        })
    })

    phoneLinks.forEach((link) => {
        link.addEventListener("mouseenter", function () {
            this.style.transform = "translateX(4px)"
        })

        link.addEventListener("mouseleave", function () {
            this.style.transform = "translateX(0)"
        })
    })
}

// Inicializar modal de confirmaci�n
function initializeModal() {
    const modal = document.getElementById("deleteModal")

    if (!modal) return

    // Cerrar modal al hacer clic fuera
    modal.addEventListener("click", (e) => {
        if (e.target === modal) {
            cerrarModal()
        }
    })

    // Cerrar modal con tecla Escape
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && modal.classList.contains("active")) {
            cerrarModal()
        }
    })
}

// Inicializar acciones
function initializeActions() {
    // Efectos hover para botones
    const buttons = document.querySelectorAll(".btn-primary, .btn-secondary, .btn-danger, .btn-outline")

    buttons.forEach((button) => {
        button.addEventListener("mouseenter", function () {
            this.style.transform = "translateY(-2px)"
            this.style.boxShadow = "var(--shadow-lg)"
        })

        button.addEventListener("mouseleave", function () {
            this.style.transform = "translateY(0)"
            this.style.boxShadow = "var(--shadow-sm)"
        })
    })
}

// Funci�n para confirmar eliminaci�n
function confirmarEliminacion(empleadoId) {
    const modal = document.getElementById("deleteModal")
    const confirmBtn = document.getElementById("confirmDeleteBtn")

    if (!modal || !confirmBtn) return

    // Configurar el bot�n de confirmaci�n
    confirmBtn.onclick = () => {
        eliminarEmpleado(empleadoId)
    }

    // Mostrar modal
    modal.classList.add("active")
}

// Funci�n para cerrar modal
function cerrarModal() {
    const modal = document.getElementById("deleteModal")
    if (modal) {
        modal.classList.remove("active")
    }
}

// Funci�n para eliminar empleado
function eliminarEmpleado(empleadoId) {
    // Mostrar estado de carga
    const confirmBtn = document.getElementById("confirmDeleteBtn")
    if (confirmBtn) {
        confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Eliminando...'
        confirmBtn.disabled = true
    }

    // Simular eliminaci�n (reemplazar con llamada real)
    setTimeout(() => {
        // Redirigir a la acci�n de eliminaci�n
        window.location.href = `/Empleados/ELIMINAR/${empleadoId}`
    }, 1000)
}

// Funci�n para asociar usuario
function asociarUsuario(empleadoId) {
    // Mostrar notificaci�n temporal
    showNotification("Funcionalidad de asociar usuario en desarrollo", "info")

    // Aqu� puedes implementar la l�gica para asociar usuario
    // Por ejemplo, redirigir a una vista de creaci�n de usuario
    // window.location.href = `/Usuarios/CreateEdit?empleadoId=${empleadoId}`
}

// Funci�n para mostrar notificaciones
function showNotification(message, type = "info") {
    const notification = document.createElement("div")
    notification.className = `notification ${type}`
    notification.innerHTML = `
    <div class="notification-content">
      <i class="fas fa-${getNotificationIcon(type)}"></i>
      <span>${message}</span>
    </div>
    <button class="notification-close" onclick="this.parentElement.remove()">
      <i class="fas fa-times"></i>
    </button>
  `

    notification.style.cssText = `
    position: fixed;
    top: 20px;
    right: 20px;
    padding: 16px 20px;
    background: ${getNotificationColor(type)};
    color: white;
    border-radius: 8px;
    box-shadow: var(--shadow-lg);
    z-index: 1001;
    animation: slideInRight 0.3s ease-out;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
    min-width: 300px;
  `

    document.body.appendChild(notification)

    // Auto-remover despu�s de 5 segundos
    setTimeout(() => {
        if (notification.parentElement) {
            notification.style.animation = "slideOutRight 0.3s ease-out"
            setTimeout(() => notification.remove(), 300)
        }
    }, 5000)
}

// Funci�n auxiliar para obtener el icono de notificaci�n
function getNotificationIcon(type) {
    switch (type) {
        case "success":
            return "check-circle"
        case "error":
            return "exclamation-circle"
        case "warning":
            return "exclamation-triangle"
        case "info":
        default:
            return "info-circle"
    }
}

// Funci�n auxiliar para obtener el color de notificaci�n
function getNotificationColor(type) {
    switch (type) {
        case "success":
            return "var(--success)"
        case "error":
            return "var(--error)"
        case "warning":
            return "var(--warning)"
        case "info":
        default:
            return "var(--info)"
    }
}

// Funci�n para copiar informaci�n al portapapeles
function copyToClipboard(text, element) {
    navigator.clipboard
        .writeText(text)
        .then(() => {
            // Mostrar feedback visual
            const originalText = element.textContent
            element.textContent = "�Copiado!"
            element.style.color = "var(--success)"

            setTimeout(() => {
                element.textContent = originalText
                element.style.color = ""
            }, 2000)

            showNotification("Informaci�n copiada al portapapeles", "success")
        })
        .catch(() => {
            showNotification("Error al copiar la informaci�n", "error")
        })
}

// Agregar funcionalidad de copia a elementos espec�ficos
document.addEventListener("DOMContentLoaded", () => {
    // Hacer que el DPI sea copiable
    const dpiElement = document.querySelector('.info-item:has(.info-label:contains("DPI")) .info-value')
    if (dpiElement) {
        dpiElement.style.cursor = "pointer"
        dpiElement.title = "Clic para copiar"
        dpiElement.addEventListener("click", function () {
            copyToClipboard(this.textContent.trim(), this)
        })
    }
})

// Agregar estilos para animaciones
const additionalStyles = `
  @keyframes slideInRight {
    from {
      transform: translateX(100%);
      opacity: 0;
    }
    to {
      transform: translateX(0);
      opacity: 1;
    }
  }
  
  @keyframes slideOutRight {
    from {
      transform: translateX(0);
      opacity: 1;
    }
    to {
      transform: translateX(100%);
      opacity: 0;
    }
  }
  
  .notification {
    font-family: inherit;
  }
  
  .notification-content {
    display: flex;
    align-items: center;
    gap: 8px;
  }
  
  .notification-close {
    background: rgba(255, 255, 255, 0.2);
    border: none;
    color: inherit;
    cursor: pointer;
    padding: 4px;
    border-radius: 4px;
    transition: background-color 0.2s;
  }
  
  .notification-close:hover {
    background: rgba(255, 255, 255, 0.3);
  }
`

const styleSheet = document.createElement("style")
styleSheet.textContent = additionalStyles
document.head.appendChild(styleSheet)
