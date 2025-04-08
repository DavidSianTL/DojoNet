function mostrarAlertaExito(mensaje) {
    Swal.fire({
        icon: 'succes',
        title: 'Exito',
        text: mensaje
    });

}
function mostrarAlertaAdvertencia(mensaje) {
    Swal.fire({
        icon: 'warning',
        title: 'Advertencia',
        text: mensaje
    });

}

function mostrarAlertaError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: mensaje
    });

}