document.getElementById('btnFiltrarFechas').addEventListener('click', function () {
    const fechaInicio = document.getElementById('fechaInicio').value;
    const fechaFin = document.getElementById('fechaFin').value;

    if (!fechaInicio || !fechaFin) {
        alert('Por favor selecciona ambas fechas.');
        return;
    }

    fetch(`/Solicitudes/FiltrarPorFechas?inicio=${fechaInicio}&fin=${fechaFin}`)
        .then(response => response.json())
        .then(data => {
            console.log(data)
        })
        .catch(error => console.error(error));
});