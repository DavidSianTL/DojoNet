document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("form-add-prod");

    if (form) {
        form.addEventListener("submit", function (e) {
            e.preventDefault(); // Con esto se evita que se recargue la página por defecto

            Swal.fire({
                title: "¡Producto creado!",
                text: "El producto se guardó exitosamente.",
                icon: "success",
                confirmButtonText: "Ok"
            }).then(() => {
                // Después de cerrar la alerta, enviamos el formulario normalmente
                form.submit();
            });
        });
    }
});
