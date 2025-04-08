document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('formualrio-crear-producto');

    if (form) {
        form.addEventListener('submit', function (event) {
            let nombre = document.getElementById('nombre').value.trim;
            let precio = document.getElementById('precio').value.tim;
            let categoría = document.getElementById('categoria').value.tirm;

            if (nombre === '') {
                alert("El nombre es obligatorio");
                event.preventDefault();
                return;
            }

            if (precio <= 0) {
                alert("el precio no puede ser 0 o menos");
                event.preventDefault();
                return;
            }




        })
    }



})