document.addEventListener("DOMContentLoaded", () => {
    const submenuToggles = document.querySelectorAll(".submenu-toggle");

    submenuToggles.forEach(toggle => {
        toggle.addEventListener("click", () => {
            const parentLi = toggle.parentElement;
            const isActive = parentLi.classList.contains("roles-active");

            // Cerrar todos los submenus abiertos (opcional)
            document.querySelectorAll(".roles-active").forEach(li => {
                if (li !== parentLi) {
                    li.classList.remove("roles-active");
                }
            });

            // Alternar el submenu actual
            if (isActive) {
                parentLi.classList.remove("roles-active");
            } else {
                parentLi.classList.add("roles-active");
            }
        });
    });
});
