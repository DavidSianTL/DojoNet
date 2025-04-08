function alerta(title, message) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: message,
    });
}