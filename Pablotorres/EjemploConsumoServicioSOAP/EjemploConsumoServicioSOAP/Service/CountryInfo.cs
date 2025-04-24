@model string
@{
    ViewData["Title"] = "Bandera del País";
}

<div class="container mt-4">
    <div class="card shadow">
        <div class="card-header bg-danger text-white">
            <i class="fas fa-flag"></i> @ViewData["Title"]
        </div>
        <div class="card-body text-center">
            @if (!string.IsNullOrEmpty(Model))
            {
                <img src="@Model" alt="Bandera del país" class="img-fluid border" style="max-height: 300px;">
                <div class="mt-3">
                    <a href="@Model" target="_blank" class="btn btn-outline-danger">
                        <i class="fas fa-external-link-alt"></i> Ver en tamaño completo
                    </a>
                </div>
            }
            else
            {
                <div class="alert alert-warning">No se encontró bandera disponible.</div>
            }
        </div>
        <div class="card-footer">
            <a asp-action="ConsultarBandera" class="btn btn-secondary">
                <i class="fas fa-redo"></i> Nueva Consulta
            </a>
            <a asp-action="Index" class="btn btn-outline-secondary">
                <i class="fas fa-home"></i> Inicio
            </a>
        </div>
    </div>
</div>@model string
@{
    ViewData["Title"] = "Bandera del País";
}

<div class="container mt-4">
    <div class="card shadow">
        <div class="card-header bg-danger text-white">
            <i class="fas fa-flag"></i> @ViewData["Title"]
        </div>
        <div class="card-body text-center">
            @if (!string.IsNullOrEmpty(Model))
            {
                <img src="@Model" alt="Bandera del país" class="img-fluid border" style="max-height: 300px;">
                <div class="mt-3">
                    <a href="@Model" target="_blank" class="btn btn-outline-danger">
                        <i class="fas fa-external-link-alt"></i> Ver en tamaño completo
                    </a>
                </div>
            }
            else
            {
                <div class="alert alert-warning">No se encontró bandera disponible.</div>
            }
        </div>
        <div class="card-footer">
            <a asp-action="ConsultarBandera" class="btn btn-secondary">
                <i class="fas fa-redo"></i> Nueva Consulta
            </a>
            <a asp-action="Index" class="btn btn-outline-secondary">
                <i class="fas fa-home"></i> Inicio
            </a>
        </div>
    </div>
</div>