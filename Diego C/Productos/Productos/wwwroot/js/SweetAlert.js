// Mostrar alerta de éxito
function mostrarAlertaExito(mensaje) {
    Swal.fire({
        icon: 'success',
        title: '¡Wohoo!',
        text: mensaje,
        confirmButtonColor: '#3085d6',
        confirmButtonText: 'OK'
    });
}

// Mostrar alerta de error
function mostrarAlertaError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: '¡Oops...!',
        text: mensaje,
        confirmButtonColor: '#d33',
        confirmButtonText: 'Cerrar'
    });
}

// Mostrar alerta de confirmación
function mostrarConfirmacion(mensaje, funcionAceptar) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: mensaje,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#aaa',
        confirmButtonText: 'Sí, confirmar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed && typeof funcionAceptar === "function") {
            funcionAceptar();
        }
    });
}
