document.addEventListener("DOMContentLoaded", function () {
    const progressBars = document.querySelectorAll(".progress");
    const modoBtn = document.getElementById("modo-btn");
    const body = document.body;

    // Aplicar modo oscuro si está guardado en localStorage
    if (localStorage.getItem("modo") === "oscuro") {
        body.classList.add("dark-mode");
        modoBtn.textContent = "Modo Claro";
    }

    // Botón para cambiar modo oscuro
    modoBtn.addEventListener("click", function () {
        body.classList.toggle("dark-mode");
        if (body.classList.contains("dark-mode")) {
            modoBtn.textContent = "Modo Claro";
            localStorage.setItem("modo", "oscuro");
        } else {
            modoBtn.textContent = "Modo Oscuro";
            localStorage.setItem("modo", "claro");
        }
    });

    // Animación de barras de progreso
    progressBars.forEach((bar) => {
        let percent = bar.style.width; 
        bar.style.width = "0%"; 

        setTimeout(() => {
            bar.style.transition = "width 1.5s ease-in-out"; 
            bar.style.width = percent; 
        }, 200); 
    });

    // Animación de aparición de secciones
    setTimeout(() => {
        document.querySelectorAll("section").forEach(sec => sec.style.opacity = "1");
    }, 500);
});

// Función para enviar correo
function enviarCorreo() {
    let email = "dcatalana4@miumg.edu.gt";
    let asunto = "Consulta sobre tu servicio";
    let cuerpo = "Hola, me gustaría obtener más información sobre:  ";

    let mailtoLink = `mailto:${email}?subject=${encodeURIComponent(asunto)}&body=${encodeURIComponent(cuerpo)}`;
    window.location.href = mailtoLink;
}
