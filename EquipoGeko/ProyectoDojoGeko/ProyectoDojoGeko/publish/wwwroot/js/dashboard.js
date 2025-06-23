// Dashboard JavaScript
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar funcionalidades del dashboard
    initializeDashboard()
    initializeSidebar()
    initializeSearch()
    initializeNotifications()
    initializeSubmenus()
})

function initializeDashboard() {
    // Animaciones de entrada para las tarjetas
    const statCards = document.querySelectorAll(".stat-card")
    statCards.forEach((card, index) => {
        card.style.opacity = "0"
        card.style.transform = "translateY(20px)"

        setTimeout(() => {
            card.style.transition = "all 0.6s ease-out"
            card.style.opacity = "1"
            card.style.transform = "translateY(0)"
        }, index * 100)
    })

    // Efectos hover para las acciones rápidas
    const quickActions = document.querySelectorAll(".quick-action-card")
    quickActions.forEach((action) => {
        action.addEventListener("mouseenter", function () {
            this.style.transform = "translateX(8px) scale(1.02)"
        })

        action.addEventListener("mouseleave", function () {
            this.style.transform = "translateX(0) scale(1)"
        })
    })

    // Actualizar tiempo en actividades recientes
    updateActivityTimes()
    setInterval(updateActivityTimes, 60000) // Actualizar cada minuto
}

function initializeSidebar() {
    const sidebar = document.querySelector(".sidebar")
    const mainContent = document.querySelector(".main-content")

    // Toggle sidebar en móviles
    if (window.innerWidth <= 768) {
        const toggleBtn = createSidebarToggle()
        document.body.appendChild(toggleBtn)

        toggleBtn.addEventListener("click", () => {
            sidebar.classList.toggle("open")
        })

        // Cerrar sidebar al hacer click fuera
        document.addEventListener("click", (e) => {
            if (!sidebar.contains(e.target) && !toggleBtn.contains(e.target)) {
                sidebar.classList.remove("open")
            }
        })
    }

    // Highlight del enlace activo
    const currentPath = window.location.pathname
    const navLinks = document.querySelectorAll(".nav-link")

    navLinks.forEach((link) => {
        if (link.getAttribute("href") === currentPath) {
            link.closest(".nav-item").classList.add("active")
        }
    })
}

function createSidebarToggle() {
    const toggle = document.createElement("button")
    toggle.className = "sidebar-toggle"
    toggle.innerHTML = '<i class="fas fa-bars"></i>'
    toggle.style.cssText = `
        position: fixed;
        top: 20px;
        left: 20px;
        z-index: 1001;
        background: var(--geko-primary);
        color: white;
        border: none;
        width: 44px;
        height: 44px;
        border-radius: 8px;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: var(--shadow-md);
        transition: var(--transition);
    `

    return toggle
}

function initializeSearch() {
    const searchInput = document.querySelector(".search-input")

    if (searchInput) {
        searchInput.addEventListener("input", (e) => {
            const query = e.target.value.toLowerCase()

            // Aquí puedes implementar la lógica de búsqueda
            if (query.length > 2) {
                // Simular búsqueda
                console.log("Buscando:", query)
                // Implementar búsqueda real aquí
            }
        })

        searchInput.addEventListener("keypress", function (e) {
            if (e.key === "Enter") {
                e.preventDefault()
                performSearch(this.value)
            }
        })
    }
}

function performSearch(query) {
    // Implementar lógica de búsqueda
    console.log("Realizar búsqueda para:", query)
    // Redirigir a página de resultados o mostrar resultados
}

function initializeNotifications() {
    const notificationBell = document.querySelector(".notification-bell")

    if (notificationBell) {
        notificationBell.addEventListener("click", () => {
            // Mostrar dropdown de notificaciones
            showNotifications()
        })
    }
}

function showNotifications() {
    // Crear y mostrar dropdown de notificaciones
    const dropdown = document.createElement("div")
    dropdown.className = "notifications-dropdown"
    dropdown.innerHTML = `
        <div class="notifications-header">
            <h3>Notificaciones</h3>
            <span class="notifications-count">3 nuevas</span>
        </div>
        <div class="notifications-list">
            <div class="notification-item unread">
                <div class="notification-icon warning">
                    <i class="fas fa-exclamation-triangle"></i>
                </div>
                <div class="notification-content">
                    <div class="notification-title">Usuario pendiente de autorización</div>
                    <div class="notification-time">Hace 5 minutos</div>
                </div>
            </div>
            <div class="notification-item unread">
                <div class="notification-icon info">
                    <i class="fas fa-user-plus"></i>
                </div>
                <div class="notification-content">
                    <div class="notification-title">Nuevo empleado registrado</div>
                    <div class="notification-time">Hace 1 hora</div>
                </div>
            </div>
            <div class="notification-item">
                <div class="notification-icon success">
                    <i class="fas fa-check"></i>
                </div>
                <div class="notification-content">
                    <div class="notification-title">Sistema actualizado correctamente</div>
                    <div class="notification-time">Hace 2 horas</div>
                </div>
            </div>
        </div>
        <div class="notifications-footer">
            <a href="#" class="view-all-notifications">Ver todas las notificaciones</a>
        </div>
    `

    // Posicionar y mostrar dropdown
    const bell = document.querySelector(".notification-bell")
    const rect = bell.getBoundingClientRect()

    dropdown.style.cssText = `
        position: fixed;
        top: ${rect.bottom + 10}px;
        right: ${window.innerWidth - rect.right}px;
        width: 320px;
        background: white;
        border-radius: 12px;
        box-shadow: var(--shadow-xl);
        border: 1px solid var(--gray-200);
        z-index: 1000;
        animation: fadeInDown 0.3s ease-out;
    `

    document.body.appendChild(dropdown)

    // Cerrar al hacer click fuera
    setTimeout(() => {
        document.addEventListener("click", function closeDropdown(e) {
            if (!dropdown.contains(e.target) && !bell.contains(e.target)) {
                dropdown.remove()
                document.removeEventListener("click", closeDropdown)
            }
        })
    }, 100)
}

function updateActivityTimes() {
    const activityTimes = document.querySelectorAll(".activity-time")

    activityTimes.forEach((timeElement) => {
        // Aquí puedes implementar lógica para actualizar los tiempos relativos
        // Por ejemplo, convertir "Hace 5 min" a "Hace 6 min" después de un minuto
    })
}

// Funciones de utilidad
function formatNumber(num) {
    return new Intl.NumberFormat("es-ES").format(num)
}

function formatTime(date) {
    return new Intl.RelativeTimeFormat("es-ES", { numeric: "auto" }).format(date)
}

// Efectos visuales adicionales
function addRippleEffect(element, event) {
    const ripple = document.createElement("span")
    const rect = element.getBoundingClientRect()
    const size = Math.max(rect.width, rect.height)
    const x = event.clientX - rect.left - size / 2
    const y = event.clientY - rect.top - size / 2

    ripple.style.cssText = `
        position: absolute;
        width: ${size}px;
        height: ${size}px;
        left: ${x}px;
        top: ${y}px;
        background: rgba(255, 255, 255, 0.3);
        border-radius: 50%;
        transform: scale(0);
        animation: ripple 0.6s linear;
        pointer-events: none;
    `

    element.style.position = "relative"
    element.style.overflow = "hidden"
    element.appendChild(ripple)

    setTimeout(() => ripple.remove(), 600)
}

// Agregar efecto ripple a botones
document.addEventListener("click", (e) => {
    if (e.target.closest(".quick-action-card, .nav-link, .sidebar-action")) {
        addRippleEffect(e.target.closest(".quick-action-card, .nav-link, .sidebar-action"), e)
    }
})

// CSS para animaciones adicionales
const additionalStyles = `
    @keyframes fadeInDown {
        from {
            opacity: 0;
            transform: translateY(-10px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
    
    @keyframes ripple {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    .notifications-dropdown {
        font-family: inherit;
    }
    
    .notifications-header {
        padding: 16px 20px;
        border-bottom: 1px solid var(--gray-200);
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    
    .notifications-header h3 {
        margin: 0;
        font-size: 16px;
        font-weight: 600;
        color: var(--gray-800);
    }
    
    .notifications-count {
        font-size: 12px;
        color: var(--geko-primary);
        font-weight: 600;
    }
    
    .notifications-list {
        max-height: 300px;
        overflow-y: auto;
    }
    
    .notification-item {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px 20px;
        border-bottom: 1px solid var(--gray-100);
        transition: background-color 0.2s;
    }
    
    .notification-item:hover {
        background: var(--gray-50);
    }
    
    .notification-item.unread {
        background: rgba(0, 57, 166, 0.02);
        border-left: 3px solid var(--geko-primary);
    }
    
    .notification-icon {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 14px;
        color: white;
    }
    
    .notification-icon.warning { background: var(--warning); }
    .notification-icon.info { background: var(--info); }
    .notification-icon.success { background: var(--success); }
    
    .notification-content {
        flex: 1;
    }
    
    .notification-title {
        font-size: 13px;
        font-weight: 500;
        color: var(--gray-800);
        margin-bottom: 2px;
    }
    
    .notification-time {
        font-size: 11px;
        color: var(--gray-500);
    }
    
    .notifications-footer {
        padding: 12px 20px;
        border-top: 1px solid var(--gray-200);
        text-align: center;
    }
    
    .view-all-notifications {
        color: var(--geko-primary);
        text-decoration: none;
        font-size: 13px;
        font-weight: 600;
    }
    
    .view-all-notifications:hover {
        text-decoration: underline;
    }
`

// Agregar estilos adicionales
const styleSheet = document.createElement("style")
styleSheet.textContent = additionalStyles
document.head.appendChild(styleSheet)
function toggleSubMenu(id) {
    const submenu = document.getElementById(id);

    if (submenu.style.display === "none" || submenu.style.display === "") {
        submenu.style.display = "block";
    } else {
        submenu.style.display = "none";
    }
}