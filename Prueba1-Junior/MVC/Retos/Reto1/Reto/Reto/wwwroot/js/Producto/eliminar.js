document.addEventListener("DOMContentLoaded", function () {
    const botonesEliminar = document.querySelectorAll(".btn-delete");

    botonesEliminar.forEach(boton => {
        boton.addEventListener("click", function (e) {
            e.preventDefault(); 

            const form = this.closest("form"); 

            Swal.fire({
                title: "¿Estás seguro?",
                text: "El producto se eliminará para siempre",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Eliminar"
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit(); // Envía el formulario manualmente
                }
            });
        });
    });
});
