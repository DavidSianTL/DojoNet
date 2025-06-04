// Funcionalidad para el formulario de empleados
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar componentes
    initializeForm()
    initializeValidation()
    initializeModal()
    initializeToggle()
    initializeFormatting()
})

// Inicializar el formulario
function initializeForm() {
    const form = document.getElementById("empleadoForm")
    const submitBtn = document.getElementById("submitBtn")
    const resetBtn = document.getElementById("resetBtn")

    if (!form) return

    // Manejar env�o del formulario
    form.addEventListener("submit", (e) => {
        e.preventDefault()

        if (validateForm()) {
            showConfirmModal()
        }
    })

    // Manejar bot�n de reset
    if (resetBtn) {
        resetBtn.addEventListener("click", (e) => {
            e.preventDefault()
            resetForm()
        })
    }

    // Auto-generar correo institucional basado en nombre y apellido
    const nombreInput = document.querySelector('input[name="NombreEmpleado"]')
    const apellidoInput = document.querySelector('input[name="ApellidoEmpleado"]')
    const correoInstitucionalInput = document.querySelector('input[name="CorreoInstitucional"]')

    if (nombreInput && apellidoInput && correoInstitucionalInput) {
        function generateInstitutionalEmail() {
            const nombre = nombreInput.value.trim().toLowerCase()
            const apellido = apellidoInput.value.trim().toLowerCase()

            if (nombre && apellido) {
                // Tomar solo el primer nombre y primer apellido
                const primerNombre = nombre.split(" ")[0]
                const primerApellido = apellido.split(" ")[0]

                if (primerNombre && primerApellido) {
                    const email = `${primerNombre}.${primerApellido}@geko.com`
                    correoInstitucionalInput.value = email
                    validateField(correoInstitucionalInput)
                }
            }
        }

        nombreInput.addEventListener("blur", generateInstitutionalEmail)
        apellidoInput.addEventListener("blur", generateInstitutionalEmail)
    }
}

// Inicializar validaci�n en tiempo real
function initializeValidation() {
    const inputs = document.querySelectorAll(".form-control")

    inputs.forEach((input) => {
        input.addEventListener("blur", function () {
            validateField(this)
        })

        input.addEventListener("input", function () {
            // Limpiar estado de error mientras el usuario escribe
            if (this.classList.contains("is-invalid")) {
                this.classList.remove("is-invalid")
                const errorMessage = this.parentElement.querySelector(".validation-message")
                if (errorMessage) {
                    errorMessage.textContent = ""
                }
            }
        })
    })
}

// Validar un campo espec�fico
function validateField(field) {
    const value = field.value.trim()
    const fieldName = field.getAttribute("name")
    let isValid = true
    let errorMessage = ""

    // Validaciones espec�ficas por campo
    switch (fieldName) {
        case "DPI":
            if (!value) {
                isValid = false
                errorMessage = "El DPI es obligatorio"
            } else if (value.length < 13) {
                isValid = false
                errorMessage = "El DPI debe tener al menos 13 d�gitos"
            } else if (!/^\d+$/.test(value)) {
                isValid = false
                errorMessage = "El DPI solo debe contener n�meros"
            }
            break

        case "NIT":
            if (!value) {
                isValid = false
                errorMessage = "El NIT es obligatorio"
            } else if (!/^[\d\-K]+$/.test(value)) {
                isValid = false
                errorMessage = "Formato de NIT no v�lido"
            }
            break

        case "NombreEmpleado":
        case "ApellidoEmpleado":
            if (!value) {
                isValid = false
                errorMessage = `El ${fieldName === "NombreEmpleado" ? "nombre" : "apellido"} es obligatorio`
            } else if (value.length < 2) {
                isValid = false
                errorMessage = `El ${fieldName === "NombreEmpleado" ? "nombre" : "apellido"} debe tener al menos 2 caracteres`
            } else if (!/^[a-zA-Z������������\s]+$/.test(value)) {
                isValid = false
                errorMessage = "Solo se permiten letras y espacios"
            }
            break

        case "CorreoPersonal":
        case "CorreoInstitucional":
            if (!value) {
                isValid = false
                errorMessage = "El correo es obligatorio"
            } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                isValid = false
                errorMessage = "Formato de correo no v�lido"
            }
            break

        case "Telefono":
            if (!value) {
                isValid = false
                errorMessage = "El tel�fono es obligatorio"
            } else if (!/^\d{8}$/.test(value)) {
                isValid = false
                errorMessage = "El tel�fono debe tener 8 d�gitos"
            }
            break

        case "FechaNacimiento":
            if (!value) {
                isValid = false
                errorMessage = "La fecha de nacimiento es obligatoria"
            } else {
                const birthDate = new Date(value)
                const today = new Date()
                const age = today.getFullYear() - birthDate.getFullYear()

                if (age < 18) {
                    isValid = false
                    errorMessage = "El empleado debe ser mayor de edad"
                } else if (age > 65) {
                    isValid = false
                    errorMessage = "Edad no v�lida para empleado"
                }
            }
            break

        case "Genero":
            if (!value) {
                isValid = false
                errorMessage = "El g�nero es obligatorio"
            }
            break

        case "Salario":
            if (!value) {
                isValid = false
                errorMessage = "El salario es obligatorio"
            } else if (Number.parseFloat(value) <= 0) {
                isValid = false
                errorMessage = "El salario debe ser mayor a 0"
            } else if (Number.parseFloat(value) < 2992.38) {
                // Salario m�nimo Guatemala 2024
                isValid = false
                errorMessage = "El salario no puede ser menor al salario m�nimo"
            }
            break
    }

    // Aplicar estilos de validaci�n
    if (isValid) {
        field.classList.remove("is-invalid")
        field.classList.add("is-valid")
    } else {
        field.classList.remove("is-valid")
        field.classList.add("is-invalid")
    }

    // Mostrar mensaje de error
    const errorElement = field.parentElement.querySelector(".validation-message")
    if (errorElement) {
        errorElement.textContent = errorMessage
    }

    return isValid
}

// Validar todo el formulario
function validateForm() {
    const inputs = document.querySelectorAll(".form-control")
    let isFormValid = true

    inputs.forEach((input) => {
        if (!validateField(input)) {
            isFormValid = false
        }
    })

    return isFormValid
}

// Inicializar modal de confirmaci�n
function initializeModal() {
    const modal = document.getElementById("confirmModal")
    const closeModal = document.getElementById("closeModal")
    const cancelAction = document.getElementById("cancelAction")
    const confirmAction = document.getElementById("confirmAction")

    if (!modal) return

    // Cerrar modal
    function closeModalHandler() {
        modal.classList.remove("active")
    }

    closeModal?.addEventListener("click", closeModalHandler)
    cancelAction?.addEventListener("click", closeModalHandler)

    // Confirmar acci�n
    confirmAction?.addEventListener("click", () => {
        const form = document.getElementById("empleadoForm")
        const submitBtn = document.getElementById("submitBtn")

        // Mostrar estado de carga
        submitBtn.classList.add("loading")
        submitBtn.disabled = true

        // Enviar formulario
        setTimeout(() => {
            form.submit()
        }, 500)

        closeModalHandler()
    })

    // Cerrar modal al hacer clic fuera
    modal.addEventListener("click", (e) => {
        if (e.target === modal) {
            closeModalHandler()
        }
    })
}

// Mostrar modal de confirmaci�n
function showConfirmModal() {
    const modal = document.getElementById("confirmModal")
    const modalMessage = document.getElementById("modalMessage")
    const isEdit = document.querySelector('input[name="IdEmpleado"]')?.value > 0

    if (modalMessage) {
        modalMessage.textContent = isEdit
            ? "�Est� seguro que desea actualizar la informaci�n del empleado?"
            : "�Est� seguro que desea crear este nuevo empleado?"
    }

    modal.classList.add("active")
}

// Inicializar toggle de estado
function initializeToggle() {
    const toggleInput = document.getElementById("estadoToggle")
    const toggleText = document.querySelector(".toggle-text")

    if (!toggleInput || !toggleText) return

    function updateToggleText() {
        toggleText.textContent = toggleInput.checked ? "Activo" : "Inactivo"
    }

    toggleInput.addEventListener("change", updateToggleText)
    updateToggleText() // Inicializar texto
}

// Inicializar formateo de campos
function initializeFormatting() {
    // Formatear DPI
    const dpiInput = document.querySelector('input[name="DPI"]')
    if (dpiInput) {
        dpiInput.addEventListener("input", function () {
            // Solo permitir n�meros
            this.value = this.value.replace(/\D/g, "")
        })
    }

    // Formatear NIT
    const nitInput = document.querySelector('input[name="NIT"]')
    if (nitInput) {
        nitInput.addEventListener("input", function () {
            // Permitir n�meros, guiones y K
            this.value = this.value.replace(/[^0-9\-K]/g, "")
        })
    }

    // Formatear tel�fono
    const telefonoInput = document.querySelector('input[name="Telefono"]')
    if (telefonoInput) {
        telefonoInput.addEventListener("input", function () {
            // Solo permitir n�meros y limitar a 8 d�gitos
            this.value = this.value.replace(/\D/g, "").substring(0, 8)
        })
    }

    // Formatear nombres (capitalizar primera letra)
    const nameInputs = document.querySelectorAll('input[name="NombreEmpleado"], input[name="ApellidoEmpleado"]')
    nameInputs.forEach((input) => {
        input.addEventListener("blur", function () {
            this.value = this.value
                .toLowerCase()
                .split(" ")
                .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
                .join(" ")
        })
    })

    // Formatear salario
    const salarioInput = document.querySelector('input[name="Salario"]')
    if (salarioInput) {
        salarioInput.addEventListener("blur", function () {
            if (this.value) {
                const value = Number.parseFloat(this.value)
                this.value = value.toFixed(2)
            }
        })
    }
}

// Resetear formulario
function resetForm() {
    const form = document.getElementById("empleadoForm")
    const inputs = form.querySelectorAll(".form-control")

    // Limpiar valores
    form.reset()

    // Limpiar clases de validaci�n
    inputs.forEach((input) => {
        input.classList.remove("is-valid", "is-invalid")
    })

    // Limpiar mensajes de error
    const errorMessages = form.querySelectorAll(".validation-message")
    errorMessages.forEach((message) => {
        message.textContent = ""
    })

    // Resetear toggle
    const toggleText = document.querySelector(".toggle-text")
    if (toggleText) {
        toggleText.textContent = "Activo"
    }

    // Enfocar primer campo
    const firstInput = form.querySelector(".form-control")
    if (firstInput) {
        firstInput.focus()
    }
}

// Funciones de utilidad
function showNotification(message, type = "success") {
    // Crear notificaci�n temporal
    const notification = document.createElement("div")
    notification.className = `notification ${type}`
    notification.textContent = message

    notification.style.cssText = `
    position: fixed;
    top: 20px;
    right: 20px;
    padding: 16px 24px;
    background: ${type === "success" ? "var(--success)" : "var(--error)"};
    color: white;
    border-radius: 8px;
    box-shadow: var(--shadow-lg);
    z-index: 1001;
    animation: slideInRight 0.3s ease-out;
  `

    document.body.appendChild(notification)

    setTimeout(() => {
        notification.remove()
    }, 3000)
}

// Agregar estilos para animaciones
const additionalStyles = `
  @keyframes slideInRight {
    from {
      transform: translateX(100%);
      opacity: 0;
    }
    to {
      transform: translateX(0);
      opacity: 1;
    }
  }
`

const styleSheet = document.createElement("style")
styleSheet.textContent = additionalStyles
document.head.appendChild(styleSheet)



document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('empleadoForm');
    const submitBtn = document.getElementById('submitBtn');
    const resetBtn = document.getElementById('resetBtn');
    const confirmModal = document.getElementById('confirmModal');
    const closeModal = document.getElementById('closeModal');
    const cancelAction = document.getElementById('cancelAction');
    const confirmAction = document.getElementById('confirmAction');

    // Validacion del formulario
    if (form) {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            showConfirmModal();
        });
    }

    // Reset del formulario
    if (resetBtn) {
        resetBtn.addEventListener('click', function() {
            if (confirm('Esta seguro que desea limpiar todos los campos?')) {
                form.reset();
            }
        });
    }

    // Modal de confirmacion
    function showConfirmModal() {
        if (confirmModal) {
            confirmModal.classList.add('show');
        }
    }

    function hideConfirmModal() {
        if (confirmModal) {
            confirmModal.classList.remove('show');
        }
    }

    // Event listeners para el modal
    if (closeModal) {
        closeModal.addEventListener('click', hideConfirmModal);
    }

    if (cancelAction) {
        cancelAction.addEventListener('click', hideConfirmModal);
    }

    if (confirmAction) {
        confirmAction.addEventListener('click', function() {
            hideConfirmModal();
            form.submit();
        });
    }

    // Cerrar modal al hacer clic fuera
    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('modal-overlay')) {
            hideConfirmModal();
        }
    });

    // Validacion en tiempo real
    const inputs = form.querySelectorAll('input, select');
    inputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });
    });

    function validateField(field) {
        const value = field.value.trim();
        const fieldName = field.name;
        let isValid = true;
        let message = '';

        // Validaciones basicas
        if (field.hasAttribute('required') && !value) {
            isValid = false;
            message = 'Este campo es requerido';
        }

        // Validaciones especificas
        if (value && (fieldName === 'CorreoPersonal' || fieldName === 'CorreoInstitucional')) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                isValid = false;
                message = 'Ingrese un correo electronico valido';
            }
        }

        if (value && fieldName === 'DPI') {
            if (value.length < 13) {
                isValid = false;
                message = 'El DPI debe tener al menos 13 caracteres';
            }
        }

        if (value && fieldName === 'Telefono') {
            if (value.length < 8) {
                isValid = false;
                message = 'El telefono debe tener al menos 8 digitos';
            }
        }

        // Mostrar/ocultar mensaje de validacion
        const validationSpan = field.parentNode.querySelector('.validation-message');
        if (validationSpan) {
            if (isValid) {
                validationSpan.textContent = '';
                field.classList.remove('error');
            } else {
                validationSpan.textContent = message;
                field.classList.add('error');
            }
        }

        return isValid;
    }
});