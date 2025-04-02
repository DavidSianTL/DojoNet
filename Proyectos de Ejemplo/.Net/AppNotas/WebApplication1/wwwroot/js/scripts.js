// Función para validar el formulario antes de enviarlo
function validateForm() {
    // Validar Nombres (solo letras)
    const nombres = document.getElementById("nombres").value;
    const apellidos = document.getElementById("apellidos").value;
    const correo = document.getElementById("correo").value;
    const fechaNacimiento = document.getElementById("fechaNacimiento").value;
    const direccion = document.getElementById("direccion").value;

    // Validar que nombres y apellidos contengan solo letras
    if (!/^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$/.test(nombres)) {
        alert("Los nombres deben contener solo letras.");
        return false;
    }

    if (!/^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$/.test(apellidos)) {
        alert("Los apellidos deben contener solo letras.");
        return false;
    }

    // Validar correo electrónico
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailPattern.test(correo)) {
        alert("Por favor, ingresa un correo electrónico válido.");
        return false;
    }

    // Validar fecha de nacimiento (no puede ser mayor a la fecha de hoy)
    const today = new Date();
    const birthDate = new Date(fechaNacimiento);
    if (birthDate > today) {
        alert("La fecha de nacimiento no puede ser mayor a la fecha de hoy.");
        return false;
    }

    // Validar dirección (debe ser alfanumérica y no vacía)
    if (!/^[a-zA-Z0-9\s,.-]+$/.test(direccion) || direccion.trim() === "") {
        alert("La dirección de domicilio debe ser alfanumérica y no estar vacía.");
        return false;
    }

    return true; // Si todo es válido, enviar el formulario
}
