document.addEventListener('DOMContentLoaded', () => {
    const rows = Array.from(document.querySelectorAll('#proyectosTbody tr'));
    const pageSize = 10;
    let currentPage = 1;
    const totalPages = Math.ceil(rows.length / pageSize);

    const showingStart = document.getElementById('showing-start');
    const showingEnd = document.getElementById('showing-end');
    const totalItems = document.getElementById('total-items');
    const prevBtn = document.getElementById('prev-page');
    const nextBtn = document.getElementById('next-page');
    const pagesContainer = document.getElementById('pagination-pages');

    totalItems.textContent = rows.length;

    // Funcion para renderizar la paginación
    function renderPage(page) {
        currentPage = page;
        const start = (page - 1) * pageSize;
        const end = start + pageSize;

        rows.forEach((row, idx) => {
            row.style.display = (idx >= start && idx < end) ? '' : 'none';
        });

        showingStart.textContent = rows.length === 0 ? 0 : start + 1;
        showingEnd.textContent = Math.min(end, rows.length);

        prevBtn.disabled = page === 1;
        nextBtn.disabled = page === totalPages;

        // generar botones de página
        pagesContainer.innerHTML = '';
        for (let i = 1; i <= totalPages; i++) {
            const btn = document.createElement('button');
            btn.classList.add('page-btn');
            if (i === page) btn.classList.add('active');
            btn.textContent = i;
            btn.addEventListener('click', () => renderPage(i));
            pagesContainer.appendChild(btn);
        }
    }

    prevBtn.addEventListener('click', () => renderPage(currentPage - 1));
    nextBtn.addEventListener('click', () => renderPage(currentPage + 1));

    renderPage(1);
});
