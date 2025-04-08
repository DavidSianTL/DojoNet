/*Alertas*/

function alertaSesionExitosa(mensaje) {
    Swal.fire({
        icon: 'success',
        title: "Ingreso exitoso",
        showClass: {
            popup: `
              animate__animated
              animate__fadeInUp
              animate__faster
    `
        },
        hideClass: {
            popup: `
              animate__animated
              animate__fadeOutDown
              animate__faster
    `
        }
    });
}

function alertaErrorSesion(mensaje) {
    Swal.fire({
        icon: "error",
        title: "Oops...",
    });

}
function alertaError(mensaje) {
    Swal.fire({
        icon: "error",
        title: "Error",
    });

}