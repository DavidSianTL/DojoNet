// Función para mostrar/ocultar contraseña
function togglePassword(inputId) {
    const input = document.getElementById(inputId)
    const button = input.closest(".input-container").querySelector(".toggle-password")
    const icon = button.querySelector("i")

    if (input.type === "password") {
        input.type = "text"
        icon.classList.remove("fa-eye")
        icon.classList.add("fa-eye-slash")
        button.classList.add("active")
        button.setAttribute("title", "Ocultar contraseña")
    } else {
        input.type = "password"
        icon.classList.remove("fa-eye-slash")
        icon.classList.add("fa-eye")
        button.classList.remove("active")
        button.setAttribute("title", "Mostrar contraseña")
    }
}

// Sistema de validación mejorado
document.addEventListener("DOMContentLoaded", () => {
    const nuevaPasswordInput = document.getElementById("nuevaPassword")
    const confirmarPasswordInput = document.getElementById("confirmarPassword")
    const submitBtn = document.getElementById("submitBtn")
    const strengthBar = document.getElementById("strengthBar")
    const strengthText = document.getElementById("strengthText")
    const strengthPercentage = document.getElementById("strengthPercentage")
    const matchIndicator = document.getElementById("matchIndicator")
    const matchMessage = document.getElementById("matchMessage")
    const requirementsContainer = document.getElementById("requirementsContainer")
    const requirementsList = document.getElementById("requirementsList")
    const resetBtn = document.getElementById("resetBtn")

    // Definir requisitos
    const requirements = [
        {
            id: "length",
            text: "Mínimo 8 caracteres",
            test: (pwd) => pwd.length >= 8,
        },
        {
            id: "uppercase",
            text: "Una letra mayúscula (A-Z)",
            test: (pwd) => /[A-Z]/.test(pwd),
        },
        {
            id: "lowercase",
            text: "Una letra minúscula (a-z)",
            test: (pwd) => /[a-z]/.test(pwd),
        },
        {
            id: "number",
            text: "Un número (0-9)",
            test: (pwd) => /\d/.test(pwd),
        },
        {
            id: "special",
            text: "Un carácter especial (!@%^&*)",
            test: (pwd) => /[!@%^&*(),.?":{}|<>[\]\\;'_\-=+]/.test(pwd),
        },
    ]

    function validatePassword(password) {
        let score = 0
        const failedRequirements = []

        for (let i = 0; i < requirements.length; i++) {
            const req = requirements[i]
            if (req.test(password)) {
                score++
            } else if (password.length > 0) {
                failedRequirements.push(req.text)
            }
        }

        // Mostrar solo requisitos faltantes
        showFailedRequirements(failedRequirements)

        // Actualizar barra de fuerza
        updateStrengthBar(score, password.length)

        return score === 5
    }

    function showFailedRequirements(failedReqs) {
        if (failedReqs.length > 0) {
            requirementsContainer.style.display = "block"
            let html = ""
            for (let i = 0; i < failedReqs.length; i++) {
                html +=
                    '<div class="requirement-item">' +
                    '<i class="fas fa-times-circle"></i>' +
                    "<span>" +
                    failedReqs[i] +
                    "</span>" +
                    "</div>"
            }
            requirementsList.innerHTML = html

            // Animación de entrada
            requirementsContainer.style.animation = "slideDown 0.3s ease-out"
        } else {
            requirementsContainer.style.display = "none"
        }
    }

    function updateStrengthBar(score, length) {
        const percentage = length > 0 ? (score / 5) * 100 : 0

        strengthBar.className = "strength-fill"
        strengthBar.style.width = percentage + "%"
        strengthPercentage.textContent = Math.round(percentage) + "%"

        if (length === 0) {
            strengthText.textContent = "Ingrese una contraseña"
            strengthBar.style.background = "#E5E7EB"
        } else if (score <= 2) {
            strengthBar.classList.add("weak")
            strengthText.textContent = "Contraseña débil"
        } else if (score === 3) {
            strengthBar.classList.add("fair")
            strengthText.textContent = "Contraseña regular"
        } else if (score === 4) {
            strengthBar.classList.add("good")
            strengthText.textContent = "Contraseña buena"
        } else if (score === 5) {
            strengthBar.classList.add("strong")
            strengthText.textContent = "Contraseña fuerte"
        }
    }

    function checkPasswordMatch() {
        const nuevaPassword = nuevaPasswordInput.value
        const confirmarPassword = confirmarPasswordInput.value

        if (confirmarPassword.length > 0) {
            matchIndicator.classList.add("show")
            if (nuevaPassword === confirmarPassword) {
                matchIndicator.classList.add("match")
                matchIndicator.classList.remove("no-match")
                matchIndicator.innerHTML = '<i class="fas fa-check"></i>'
                matchMessage.textContent = "Las contraseñas coinciden"
                matchMessage.className = "match-message success"
                return true
            } else {
                matchIndicator.classList.add("no-match")
                matchIndicator.classList.remove("match")
                matchIndicator.innerHTML = '<i class="fas fa-times"></i>'
                matchMessage.textContent = "Las contraseñas no coinciden"
                matchMessage.className = "match-message error"
                return false
            }
        } else {
            matchIndicator.classList.remove("show")
            matchMessage.textContent = ""
            matchMessage.className = "match-message"
            return false
        }
    }

    function updateSubmitButton() {
        const passwordValid = validatePassword(nuevaPasswordInput.value)
        const passwordsMatch = checkPasswordMatch()

        if (passwordValid && passwordsMatch && nuevaPasswordInput.value.length > 0) {
            submitBtn.disabled = false
            submitBtn.classList.add("enabled")
        } else {
            submitBtn.disabled = true
            submitBtn.classList.remove("enabled")
        }
    }

    function resetForm() {
        document.getElementById("passwordForm").reset()
        submitBtn.disabled = true
        submitBtn.classList.remove("enabled")
        strengthBar.className = "strength-fill"
        strengthBar.style.width = "0%"
        strengthBar.style.background = "#E5E7EB"
        strengthText.textContent = "Ingrese una contraseña"
        strengthPercentage.textContent = "0%"
        matchIndicator.classList.remove("show")
        matchMessage.textContent = ""
        requirementsContainer.style.display = "none"

        // Resetear iconos de mostrar contraseña
        const toggleButtons = document.querySelectorAll(".toggle-password")
        for (let i = 0; i < toggleButtons.length; i++) {
            const btn = toggleButtons[i]
            const icon = btn.querySelector("i")
            icon.className = "fas fa-eye"
            btn.classList.remove("active")
            btn.setAttribute("title", "Mostrar contraseña")
        }

        // Resetear tipos de input
        const textInputs = document.querySelectorAll('input[type="text"]')
        for (let i = 0; i < textInputs.length; i++) {
            const input = textInputs[i]
            if (input.name.includes("contraseña") || input.name.includes("Password")) {
                input.type = "password"
            }
        }
    }

    // Event listeners
    nuevaPasswordInput.addEventListener("input", updateSubmitButton)
    confirmarPasswordInput.addEventListener("input", updateSubmitButton)
    resetBtn.addEventListener("click", resetForm)

    // Event listeners para botones de mostrar contraseña
    const toggleButtons = document.querySelectorAll(".toggle-password")
    for (let i = 0; i < toggleButtons.length; i++) {
        const button = toggleButtons[i]
        button.addEventListener("click", function () {
            const targetId = this.getAttribute("data-target")
            togglePassword(targetId)
        })
    }

    // Efecto ripple mejorado
    const buttons = document.querySelectorAll(".btn")
    for (let i = 0; i < buttons.length; i++) {
        const button = buttons[i]
        button.addEventListener("click", function (e) {
            const ripple = this.querySelector(".btn-ripple")
            if (ripple) {
                const rect = this.getBoundingClientRect()
                const size = Math.max(rect.width, rect.height)
                const x = e.clientX - rect.left - size / 2
                const y = e.clientY - rect.top - size / 2

                ripple.style.width = ripple.style.height = size + "px"
                ripple.style.left = x + "px"
                ripple.style.top = y + "px"
                ripple.classList.add("active")

                setTimeout(() => {
                    ripple.classList.remove("active")
                }, 600)
            }
        })
    }

    // Animaciones de entrada
    const formGroups = document.querySelectorAll(".form-group")
    for (let i = 0; i < formGroups.length; i++) {
        const group = formGroups[i]
        group.style.animationDelay = i * 0.1 + "s"
        group.classList.add("fade-in")
    }

    // Tooltips para botones de mostrar contraseña
    for (let i = 0; i < toggleButtons.length; i++) {
        toggleButtons[i].setAttribute("title", "Mostrar contraseña")
    }
})
