// Funcionalidad para asociar usuario
document.addEventListener("DOMContentLoaded", () => {
    // Inicializar componentes
    initializeForm()
    initializeValidation()
    initializeGenerators()
    initializePasswordStrength()
    initializeRoleDescriptions()
    initializeModals()
})

// Inicializar el formulario
function initializeForm() {
    const form = document.getElementById("asociarUsuarioForm")

    if (!form) return

    // Manejar env�o del formulario
    form.addEventListener("submit", (e) => {
        e.preventDefault()

        if (validateForm()) {
            mostrarVistaPrevia()
        }
    })

    // Inicializar toggle
    const toggleInput = document.getElementById("estadoToggle")
    const toggleText = document.querySelector(".toggle-text")

    if (toggleInput && toggleText) {
        function updateToggleText() {
            toggleText.textContent = toggleInput.checked ? "Activo" : "Inactivo"
        }

        toggleInput.addEventListener("change", updateToggleText)
        updateToggleText()
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
            if (this.classList.contains("is-invalid")) {
                this.classList.remove("is-invalid")
                const errorMessage = this.parentElement.querySelector(".validation-message")
                if (errorMessage) {
                    errorMessage.textContent = ""
                }
            }
        })
    })

    // Validaci�n de coincidencia de contrase�as
    const passwordInput = document.querySelector('input[name="Usuario.Contrasenia"]')
    const confirmInput = document.querySelector('input[name="ConfirmarContrasenia"]')

    if (passwordInput && confirmInput) {
        confirmInput.addEventListener("input", () => {
            checkPasswordMatch(passwordInput, confirmInput)
        })
    }
}

// Validar un campo espec�fico
function validateField(field) {
    const value = field.value.trim()
    const fieldName = field.getAttribute("name")
    let isValid = true
    let errorMessage = ""

    switch (fieldName) {
        case "Usuario.Username":
            if (!value) {
                isValid = false
                errorMessage = "El nombre de usuario es obligatorio"
            } else if (value.length < 3) {
                isValid = false
                errorMessage = "El nombre de usuario debe tener al menos 3 caracteres"
            } else if (!/^[a-zA-Z0-9._-]+$/.test(value)) {
                isValid = false
                errorMessage = "Solo se permiten letras, n�meros, puntos, guiones y guiones bajos"
            }
            break

        case "Usuario.Contrasenia":
            if (!value) {
                isValid = false
                errorMessage = "La contrase�a es obligatoria"
            } else if (value.length < 8) {
                isValid = false
                errorMessage = "La contrase�a debe tener al menos 8 caracteres"
            }
            break

        case "ConfirmarContrasenia":
            const originalPassword = document.querySelector('input[name="Usuario.Contrasenia"]').value
            if (!value) {
                isValid = false
                errorMessage = "Debe confirmar la contrase�a"
            } else if (value !== originalPassword) {
                isValid = false
                errorMessage = "Las contrase�as no coinciden"
            }
            break

        case "Usuario.FK_IdRol":
            if (!value) {
                isValid = false
                errorMessage = "Debe seleccionar un rol"
            }
            break

        case "Usuario.FK_IdSistema":
            if (!value) {
                isValid = false
                errorMessage = "Debe seleccionar un sistema"
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

// Inicializar generadores autom�ticos
function initializeGenerators() {
    // Ya definidas globalmente
}

// Generar nombre de usuario autom�ticamente
function generarUsername() {
    const nombreCompleto = "@Model.Empleado.NombreEmpleado @Model.Empleado.ApellidoEmpleado"
    const nombres = nombreCompleto.toLowerCase().split(" ")

    if (nombres.length >= 2) {
        const username = `${nombres[0]}.${nombres[nombres.length - 1]}`
        document.querySelector('input[name="Usuario.Username"]').value = username
        validateField(document.querySelector('input[name="Usuario.Username"]'))
    }
}

// Generar contrase�a autom�ticamente
function generarContrasena() {
    const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*"
    let password = ""

    // Asegurar al menos una may�scula, una min�scula, un n�mero y un s�mbolo
    password += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[Math.floor(Math.random() * 26)]
    password += "abcdefghijklmnopqrstuvwxyz"[Math.floor(Math.random() * 26)]
    password += "0123456789"[Math.floor(Math.random() * 10)]
    password += "!@#$%^&*"[Math.floor(Math.random() * 8)]

    // Completar con caracteres aleatorios
    for (let i = 4; i < 12; i++) {
        password += charset[Math.floor(Math.random() * charset.length)]
    }

    // Mezclar la contrase�a
    password = password
        .split("")
        .sort(() => Math.random() - 0.5)
        .join("")

    const passwordInput = document.querySelector('input[name="Usuario.Contrasenia"]')
    passwordInput.value = password
    passwordInput.type = "text"

    // Mostrar por 3 segundos y luego ocultar
    setTimeout(() => {
        passwordInput.type = "password"
    }, 3000)

    validateField(passwordInput)
    updatePasswordStrength(password)
}

// Inicializar indicador de fuerza de contrase�a
function initializePasswordStrength() {
    const passwordInput = document.querySelector('input[name="Usuario.Contrasenia"]')

    if (passwordInput) {
        passwordInput.addEventListener("input", function () {
            updatePasswordStrength(this.value)
        })
    }
}

// Actualizar indicador de fuerza de contrase�a
function updatePasswordStrength(password) {
    const strengthBar = document.getElementById("strengthBar")
    const strengthText = document.getElementById("strengthText")

    if (!strengthBar || !strengthText) return

    let score = 0
    let feedback = ""

    if (password.length === 0) {
        feedback = "Ingrese una contrase�a"
    } else {
        if (password.length >= 8) score++
        if (/[a-z]/.test(password)) score++
        if (/[A-Z]/.test(password)) score++
        if (/\d/.test(password)) score++
        if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) score++

        const percentage = (score / 5) * 100

        strengthBar.className = "strength-fill"
        strengthBar.style.width = percentage + "%"

        if (score <= 2) {
            strengthBar.classList.add("weak")
            feedback = "Contrase�a d�bil"
        } else if (score === 3) {
            strengthBar.classList.add("fair")
            feedback = "Contrase�a regular"
        } else if (score === 4) {
            strengthBar.classList.add("good")
            feedback = "Contrase�a buena"
        } else if (score === 5) {
            strengthBar.classList.add("strong")
            feedback = "Contrase�a fuerte"
        }
    }

    strengthText.textContent = feedback
}

// Verificar coincidencia de contrase�as
function checkPasswordMatch(passwordInput, confirmInput) {
    const matchIndicator = document.getElementById("matchIndicator")
    const matchMessage = document.getElementById("matchMessage")

    if (!matchIndicator || !matchMessage) return

    const password = passwordInput.value
    const confirm = confirmInput.value

    if (confirm.length > 0) {
        matchIndicator.classList.add("show")

        if (password === confirm) {
            matchIndicator.classList.add("match")
            matchIndicator.classList.remove("no-match")
            matchIndicator.innerHTML = '<i class="fas fa-check"></i>'
            matchMessage.textContent = "Las contrase�as coinciden"
            matchMessage.className = "match-message success"
        } else {
            matchIndicator.classList.add("no-match")
            matchIndicator.classList.remove("match")
            matchIndicator.innerHTML = '<i class="fas fa-times"></i>'
            matchMessage.textContent = "Las contrase�as no coinciden"
            matchMessage.className = "match-message error"
        }
    } else {
        matchIndicator.classList.remove("show")
        matchMessage.textContent = ""
        matchMessage.className = "match-message"
    }
}

// Inicializar descripciones de roles
function initializeRoleDescriptions() {
    const rolSelect = document.getElementById("rolSelect")
    const roleDescription = document.getElementById("roleDescription")

    if (!rolSelect || !roleDescription) return

    const descriptions = {
        1: "Acceso completo al sistema. Puede gestionar usuarios, configuraciones y todos los m�dulos.",
        2: "Acceso a la mayor�a de funciones. Puede supervisar operaciones y generar reportes.",
        3: "Acceso a funciones operativas b�sicas. Puede realizar tareas diarias del sistema.",
        4: "Acceso de solo lectura. Puede consultar informaci�n pero no realizar cambios.",
    }

    rolSelect.addEventListener("change", function () {
        const selectedRole = this.value

        if (selectedRole && descriptions[selectedRole]) {
            roleDescription.textContent = descriptions[selectedRole]
            roleDescription.classList.add("show")
        } else {
            roleDescription.classList.remove("show")
        }
    })
}

// Inicializar modales
function initializeModals() {
    // Toggle de contrase�as
    const toggleButtons = document.querySelectorAll(".toggle-password")

    toggleButtons.forEach((button) => {
        button.addEventListener("click", function () {
            const targetId = this.getAttribute("data-target")
            const targetInput = document.getElementById(targetId) || document.querySelector(`input[name="${targetId}"]`)

            if (targetInput) {
                const isPassword = targetInput.type === "password"
                targetInput.type = isPassword ? "text" : "password"

                const icon = this.querySelector("i")
                icon.className = isPassword ? "fas fa-eye-slash" : "fas fa-eye"
            }
        })
    })
}

// Mostrar vista previa
function mostrarVistaPrevia() {
    const modal = document.getElementById("previewModal")

    // Llenar datos de vista previa
    updatePreviewData()

    modal.classList.add("active")
}

// Actualizar datos de vista previa
function updatePreviewData() {
    const username = document.querySelector('input[name="Usuario.Username"]').value
    const password = document.querySelector('input[name="Usuario.Contrasenia"]').value
    const rolSelect = document.getElementById("rolSelect")
    const sistemaSelect = document.querySelector('select[name="Usuario.FK_IdSistema"]')
    const estadoToggle = document.getElementById("estadoToggle")

    document.getElementById("previewUsername").textContent = username || "-"
    document.getElementById("previewPassword").textContent = password ? "��������" : "-"
    document.getElementById("previewRole").textContent = rolSelect.options[rolSelect.selectedIndex].text
    document.getElementById("previewSystem").textContent = sistemaSelect.options[sistemaSelect.selectedIndex].text
    document.getElementById("previewStatus").textContent = estadoToggle.checked ? "Activo" : "Inactivo"

    // Configuraciones
    const configPreview = document.getElementById("configPreview")
    const configs = []

    if (document.getElementById("cambiarContrasena").checked) {
        configs.push(
            '<div class="config-preview-item"><i class="fas fa-check"></i> Cambiar contrase�a en primer acceso</div>',
        )
    }

    if (document.getElementById("enviarCredenciales").checked) {
        configs.push('<div class="config-preview-item"><i class="fas fa-check"></i> Enviar credenciales por correo</div>')
    }

    if (document.getElementById("habilitarNotificaciones").checked) {
        configs.push('<div class="config-preview-item"><i class="fas fa-check"></i> Habilitar notificaciones</div>')
    }

    configPreview.innerHTML = configs.join("")
}

// Cerrar vista previa
function cerrarVistaPrevia() {
    document.getElementById("previewModal").classList.remove("active")
}

// Confirmar creaci�n desde vista previa
function confirmarCreacion() {
    cerrarVistaPrevia()

    const confirmModal = document.getElementById("confirmModal")
    confirmModal.classList.add("active")
}

// Cerrar confirmaci�n
function cerrarConfirmacion() {
    document.getElementById("confirmModal").classList.remove("active")
}

// Procesar creaci�n final
function procesarCreacion() {
    const form = document.getElementById("asociarUsuarioForm")
    const confirmBtn = document.querySelector("#confirmModal .btn-confirm")

    // Mostrar estado de carga
    confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Creando Usuario...'
    confirmBtn.disabled = true

    // Simular procesamiento
    setTimeout(() => {
        // Enviar formulario
        form.submit()
    }, 2000)
}

// Funciones de utilidad
function showNotification(message, type = "success") {
    const notification = document.createElement("div")
    notification.className = `notification ${type}`
    notification.innerHTML = `
    <div class="notification-content">
      <i class="fas fa-${getNotificationIcon(type)}"></i>
      <span>${message}</span>
    </div>
  `

    notification.style.cssText = `
    position: fixed;
    top: 20px;
    right: 20px;
    padding: 16px 20px;
    background: ${getNotificationColor(type)};
    color: white;
    border-radius: 8px;
    box-shadow: var(--shadow-lg);
    z-index: 1001;
    animation: slideInRight 0.3s ease-out;
  `

    document.body.appendChild(notification)

    setTimeout(() => {
        notification.remove()
    }, 4000)
}

function getNotificationIcon(type) {
    switch (type) {
        case "success":
            return "check-circle"
        case "error":
            return "exclamation-circle"
        case "warning":
            return "exclamation-triangle"
        case "info":
        default:
            return "info-circle"
    }
}

function getNotificationColor(type) {
    switch (type) {
        case "success":
            return "var(--success)"
        case "error":
            return "var(--error)"
        case "warning":
            return "var(--warning)"
        case "info":
        default:
            return "var(--info)"
    }
}
