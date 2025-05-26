document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("form-editar");

    if (form) {
        form.addEventListener("submit", function (e) {
            e.preventDefault(); 

            Swal.fire({
                title: "¡Producto editado!",
                text: "El producto se editó exitosamente.",
                icon: "success",
                confirmButtonText: "Ok"
            }).then(() => {
                // Después de cerrar la alerta, enviamos el formulario normalmente
                form.submit();
            });
        });
    }
});
