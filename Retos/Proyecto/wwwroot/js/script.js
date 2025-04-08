// script.js
window.onload = function () {
    if (document.getElementById("error-message")) {
        // Desaparece el mensaje de error después de 5 segundos
        setTimeout(function () {
            var errorMessage = document.getElementById('error-message');
            if (errorMessage) {
                errorMessage.style.display = 'none';
            }
        }, 5000);

        // Animación de aparición
        var errorMessage = document.getElementById('error-message');
        errorMessage.style.opacity = 0;
        setTimeout(function () {
            errorMessage.style.transition = "opacity 0.5s";
            errorMessage.style.opacity = 1;
        }, 100);
    }
};
