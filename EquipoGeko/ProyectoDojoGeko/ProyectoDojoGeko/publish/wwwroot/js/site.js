// Script para manejar la navegación y el botón de inicio de sesión
document.addEventListener("DOMContentLoaded", () => {
    // Manejar el botón de inicio de sesión
    const loginButton = document.querySelector(".login-button .btn")
    if (loginButton) {
      loginButton.addEventListener("click", (e) => {
        // El enlace ya tiene href="/login/index", pero podemos agregar lógica adicional si es necesario
        console.log("Redirigiendo a la página de inicio de sesión...")
      })
    }
  
    // Manejar los enlaces de navegación
    const navLinks = document.querySelectorAll("nav ul li a")
    navLinks.forEach((link) => {
      link.addEventListener("click", function (e) {
        // Remover la clase active de todos los enlaces
        navLinks.forEach((l) => l.classList.remove("active"))
        // Agregar la clase active al enlace clickeado
        this.classList.add("active")
      })
    })
  
    // Manejar los botones de CTA
    const ctaButtons = document.querySelectorAll(".cta-buttons .btn")
    ctaButtons.forEach((button) => {
      button.addEventListener("click", function (e) {
        const buttonText = this.textContent.trim()
        console.log(`Botón "${buttonText}" clickeado`)
  
        // Aquí puedes agregar lógica específica para cada botón
        if (buttonText === "Solicitar Cotización") {
          console.log("Redirigiendo al formulario de cotización...")
        } else if (buttonText === "Conocer Más") {
          console.log("Mostrando más información...")
        }
      })
    })
  })
  