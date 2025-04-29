document.querySelector("form").addEventListener("submit", function(event) {
    event.preventDefault();

    let nombre = document.querySelector("input[type='text']").value;
    let email = document.querySelector("input[type='email']").value;
    let fecha = document.querySelector("input[type='date']").value;
    let hora = document.querySelector("input[type='time']").value;

    if (!nombre || !email || !fecha || !hora) {
        alert("Por favor, completa todos los campos.");
        return;
    }

    alert(`¡Cita reservada para ${nombre} el ${fecha} a las ${hora}!`);
});
