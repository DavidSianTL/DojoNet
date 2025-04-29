function mostrarAlertaError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: mensaje,
        timer: 5000,
        showConfirmButton: false
    });
}

function mostrarAlertaAdvertencia(mensaje) {
    Swal.fire({
        icon: 'warning',
        title: 'Atención',
        text: mensaje,
        timer: 5000,
        showConfirmButton: false
    });
}

function mostrarAlertaExito(mensaje) {
    Swal.fire({
        icon: 'success',
        title: 'Éxito',
        text: mensaje,
        timer: 5000,
        showConfirmButton: false
    });
}
