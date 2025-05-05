function mostrarAlertaExito(mensaje) {
    Swal.fire({
        icon: 'success',
        title: "Exito",
        html: mensaje,
        width: '60em', // Ajustar el ancho del popup
        showClass: {
            popup: 'animate__animated animate__fadeInUp animate__faster'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutDown animate__faster'
        }
    });
}

function mostrarAlertaAdvertencia(data) {
    if (data && data.length > 0) {
        let message = "Idiomas disponibles:\n";
        data.forEach(item => {
            message += `- ${JSON.stringify(item, null, 2)}\n`;
        });

        Swal.fire({
            icon: 'info',
            title: 'Idiomas por Código',
            html: `<pre>${message}</pre>`, // Usar <pre> para mantener el formato
            width: '60em', // Ajustar el ancho del popup
            customClass: {
                popup: 'swal-wide'
            }
        });
    } else {
        Swal.fire({
            icon: 'warning',
            title: 'Advertencia',
            text: 'No hay idiomas disponibles.',
            width: '60em', // Ajustar el ancho del popup
        });
    }
}

function mostrarAlertaError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: mensaje,
        showClass: {
            popup: 'animate__animated animate__fadeInUp animate__faster'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutDown animate__faster'
        }
    });

}