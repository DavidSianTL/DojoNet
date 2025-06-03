document.addEventListener("DOMContentLoaded", () => {
    // Animaciones suaves al hacer scroll
    const observerOptions = {
      threshold: 0.1,
      rootMargin: "0px 0px -50px 0px",
    }
  
    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.style.opacity = "1"
          entry.target.style.transform = "translateY(0)"
        }
      })
    }, observerOptions)
  
    // Aplicar animaciones a las tarjetas
    const cards = document.querySelectorAll(".feature-card, .solution-card")
    cards.forEach((card) => {
      card.style.opacity = "0"
      card.style.transform = "translateY(30px)"
      card.style.transition = "opacity 0.6s ease, transform 0.6s ease"
      observer.observe(card)
    })
  
    // Manejar clics en botones de "Más Información"
    const infoButtons = document.querySelectorAll(".solution-card .btn")
    infoButtons.forEach((button) => {
      button.addEventListener("click", (e) => {
        e.preventDefault()
        const cardTitle = e.target.closest(".solution-card").querySelector("h3").textContent
        console.log(`Solicitando más información sobre: ${cardTitle}`)
  
        // Aquí puedes agregar lógica para mostrar más información
        // Por ejemplo, abrir un modal o redirigir a una página específica
        alert(`Más información sobre ${cardTitle} - Próximamente disponible`)
      })
    })
  
    // Manejar clic en botón "Iniciar sesión"
    const loginButton = document.querySelector(".hero .btn-primary")
    if (loginButton) {
      loginButton.addEventListener("click", (e) => {
        console.log("Redirigiendo a página de login...")
        // El enlace ya tiene href="/login/index", pero puedes agregar lógica adicional aquí
      })
    }
  
    // Smooth scroll para enlaces internos
    const internalLinks = document.querySelectorAll('a[href^="#"]')
    internalLinks.forEach((link) => {
      link.addEventListener("click", (e) => {
        e.preventDefault()
        const targetId = link.getAttribute("href").substring(1)
        const targetElement = document.getElementById(targetId)
  
        if (targetElement) {
          targetElement.scrollIntoView({
            behavior: "smooth",
            block: "start",
          })
        }
      })
    })
  
    // Efecto parallax sutil en la sección hero
    window.addEventListener("scroll", () => {
      const scrolled = window.pageYOffset
      const heroImage = document.querySelector(".hero-image img")
  
      if (heroImage && scrolled < window.innerHeight) {
        heroImage.style.transform = `translateY(${scrolled * 0.1}px)`
      }
    })
  
    // Contador animado (si quieres agregar estadísticas)
    function animateCounter(element, target, duration = 2000) {
      let start = 0
      const increment = target / (duration / 16)
  
      const timer = setInterval(() => {
        start += increment
        element.textContent = Math.floor(start)
  
        if (start >= target) {
          element.textContent = target
          clearInterval(timer)
        }
      }, 16)
    }
  
    // Ejemplo de uso del contador (descomenta si necesitas)
    /*
    const counters = document.querySelectorAll('.counter')
    counters.forEach(counter => {
      const target = parseInt(counter.getAttribute('data-target'))
      observer.observe(counter)
      counter.addEventListener('animateCounter', () => {
        animateCounter(counter, target)
      })
    })
    */
  })
  