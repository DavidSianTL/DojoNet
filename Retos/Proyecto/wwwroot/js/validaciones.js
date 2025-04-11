function confirmDelete(id, nombre) {
    Swal.fire({
        title: '¿Eliminar producto?',
        text: `Vas a eliminar ${nombre}`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Crear formulario dinámico para enviar la solicitud POST
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = `/Producto/Delete/${id}`;

            // Verificar que el token anti-CSRF existe
            const token = document.querySelector('input[name="__RequestVerificationToken"]');
            if (token) {
                const tokenInput = document.createElement('input');
                tokenInput.type = 'hidden';
                tokenInput.name = "__RequestVerificationToken";
                tokenInput.value = token.value;
                form.appendChild(tokenInput);
            } else {
                console.error("Token anti-CSRF no encontrado en el DOM.");
                Swal.fire("Error", "El token de seguridad no está disponible. Intenta recargar la página.", "error");
                return;
            }

            // Agregar el formulario al DOM y enviarlo
            document.body.appendChild(form);
            form.submit();
        }
    });
}
