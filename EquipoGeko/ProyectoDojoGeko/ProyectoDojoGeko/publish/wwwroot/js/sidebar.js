// JavaScript para manejar el menú desplegable en la barra lateral
document.addEventListener("DOMContentLoaded", () => {
  // Seleccionar todos los elementos con la clase submenu-toggle
  const submenuToggles = document.querySelectorAll(".submenu-toggle")

  // Añadir evento de clic a cada toggle
  submenuToggles.forEach((toggle) => {
    toggle.addEventListener("click", function (e) {
      e.preventDefault()

      // Obtener el elemento padre (nav-item)
      const parentItem = this.closest(".nav-item")

      // Alternar la clase roles-active
      parentItem.classList.toggle("roles-active")
    })
  })

  // Verificar la URL actual para activar el submenu correspondiente
  function checkCurrentUrl() {
    const currentPath = window.location.pathname.toLowerCase()

    // Verificar si estamos en alguna página relacionada con roles o permisos
    if (
      currentPath.includes("/roles") ||
      currentPath.includes("/permisos") ||
      currentPath.includes("/rolpermisos") ||
      currentPath.includes("/asignarpermisos")
    ) {
      // Encontrar el elemento del menú de roles y permisos
      const rolesMenuItem = document.querySelector(".submenu-toggle").closest(".nav-item")

      // Activar el menú desplegable
      if (rolesMenuItem) {
        rolesMenuItem.classList.add("roles-active")

        // Activar el submenú específico
        const submenuItems = rolesMenuItem.querySelectorAll(".submenu-item")
        submenuItems.forEach((item) => {
          const link = item.querySelector(".submenu-link")
          if (link && currentPath.includes(link.getAttribute("href").toLowerCase())) {
            item.classList.add("active")
          }
        })
      }
    }
  }

  // Ejecutar la verificación al cargar la página
  checkCurrentUrl()
})
