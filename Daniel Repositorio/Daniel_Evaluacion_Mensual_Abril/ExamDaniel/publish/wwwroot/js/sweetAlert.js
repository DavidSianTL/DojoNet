function mostrarSweetAlertExito(mensaje) {
    Swal.fire({
        icon: 'success',
        title: 'Éxito',
        text: mensaje,
        confirmButtonColor: '#198754'
    });
}

function mostrarSweetAlertError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: mensaje,
        confirmButtonColor: '#dc3545' 
    });
}

