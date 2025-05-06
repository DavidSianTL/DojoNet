/*Alertas*/

function alertaSesionExitosa() {
    Swal.fire({
        icon: 'success',
        title: "¡¡Bienvenido!!",
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

function alertaExitosa(){
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: "success",
        title: "Producto agregado correctamente"
    });
}