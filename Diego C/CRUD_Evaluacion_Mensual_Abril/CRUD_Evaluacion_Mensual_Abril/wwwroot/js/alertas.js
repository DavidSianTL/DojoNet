function mostrarAlertaExito(mensaje) {
    Swal.fire({
        icon: 'success',
        title: '¡Éxito!',
        text: mensaje,
        confirmButtonColor: '#6f42c1'
    });
}

function mostrarAlertaError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: mensaje,
        confirmButtonColor: '#dc3545'
    });
}
