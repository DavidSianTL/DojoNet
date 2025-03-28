// Función para cambiar entre páginas
function showPage(pageId) {
    const pages = document.querySelectorAll('.page');
    pages.forEach(page => page.style.display = 'none');
    document.getElementById(pageId).style.display = 'block';
}

// Validación de formulario
document.getElementById('orderForm').addEventListener('submit', function(e) {
    e.preventDefault();

    let isValid = true;
    const nombre = document.getElementById('nombre').value;
    const apellidos = document.getElementById('apellidos').value;
    const direccion = document.getElementById('direccion').value;
    const telefono = document.getElementById('telefono').value;
    const correo = document.getElementById('correo').value;
    const fechaEntrega = document.getElementById('fechaEntrega').value;

    // Validar Nombre y Apellidos (solo letras)
    const namePattern = /^[a-zA-Z\s]+$/;
    if (!namePattern.test(nombre) || !namePattern.test(apellidos)) {
        alert('El nombre y los apellidos solo pueden contener letras.');
        isValid = false;
    }

    // Validar Dirección (alfanumérico)
    const addressPattern = /^[a-zA-Z0-9\s,.-]+$/;
    if (!addressPattern.test(direccion)) {
        alert('La dirección solo puede contener caracteres alfanuméricos.');
        isValid = false;
    }

    // Validar Teléfono (solo números)
    const phonePattern = /^[0-9]{10}$/;
    if (!phonePattern.test(telefono)) {
        alert('El teléfono debe ser un número de 10 dígitos.');
        isValid = false;
    }

    // Validar Correo Electrónico
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailPattern.test(correo)) {
        alert('El correo electrónico no es válido.');
        isValid = false;
    }

    // Validar Fecha de Entrega
    if (!fechaEntrega) {
        alert('Por favor, selecciona una fecha de entrega.');
        isValid = false;
    }
	
	 // Validar Fecha de Entrega
    const today = new Date().toISOString().split('T')[0];  // Obtiene la fecha de hoy en formato YYYY-MM-DD
    if (fechaEntrega < today) {
        alert('La fecha de entrega no puede ser menor a la fecha actual.');
        isValid = false;
    }

    if (isValid) {
        document.getElementById('mensaje').style.display = 'block';
        setTimeout(() => {
            document.getElementById('mensaje').style.display = 'none';
        }, 3000);
    }
});
