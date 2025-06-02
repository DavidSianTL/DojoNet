// Funcion para mostrar/ocultar contrasena
function togglePassword(inputId) {
    const input = document.getElementById(inputId)
    const button = input.closest(".input-container").querySelector(".toggle-password")
    const icon = button.querySelector("i")

    if (input.type === "password") {
        input.type = "text"
        icon.classList.remove("fa-eye")
        icon.classList.add("fa-eye-slash")
        button.classList.add("active")
        button.setAttribute("title", "Ocultar contrasena")
    } else {
        input.type = "password"
        icon.classList.remove("fa-eye-slash")
        icon.classList.add("fa-eye")
        button.classList.remove("active")
        button.setAttribute("title", "Mostrar contrasena")
    }
}

// Sistema de validacion mejorado
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
            text: "Minimo 8 caracteres",
            test: (pwd) => pwd.length >= 8,
        },
        {
            id: "uppercase",
            text: "Una letra mayuscula (A-Z)",
            test: (pwd) => /[A-Z]/.test(pwd),
        },
        {
            id: "lowercase",
            text: "Una letra minuscula (a-z)",
            test: (pwd) => /[a-z]/.test(pwd),
        },
        {
            id: "number",
            text: "Un numero (0-9)",
            test: (pwd) => /\d/.test(pwd),
        },
        {
            id: "special",
            text: "Un caracter especial (!@%^&*)",
            test: (pwd) => /[!@%^&*(),.?":{}|<>[\]\\;'_\-=+]/.test(pwd),
        },
    ]

    function validatePassword(password) {
        var score = 0
        var failedRequirements = []

        for (var j = 0; j < requirements.length; j++) {
            var req = requirements[j]
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
            var html = ""
            for (var k = 0; k < failedReqs.length; k++) {
                html +=
                    '<div class="requirement-item">' +
                    '<i class="fas fa-times-circle"></i>' +
                    "<span>" +
                    failedReqs[k] +
                    "</span>" +
                    "</div>"
            }
            requirementsList.innerHTML = html

            // Animacion de entrada
            requirementsContainer.style.animation = "slideDown 0.3s ease-out"
        } else {
            requirementsContainer.style.display = "none"
        }
    }

    function updateStrengthBar(score, length) {
        var percentage = length > 0 ? (score / 5) * 100 : 0

        strengthBar.className = "strength-fill"
        strengthBar.style.width = percentage + "%"
        strengthPercentage.textContent = Math.round(percentage) + "%"

        if (length === 0) {
            strengthText.textContent = "Ingrese una contrasena"
            strengthBar.style.background = "#E5E7EB"
        } else if (score <= 2) {
            strengthBar.classList.add("weak")
            strengthText.textContent = "Contrasena debil"
        } else if (score === 3) {
            strengthBar.classList.add("fair")
            strengthText.textContent = "Contrasena regular"
        } else if (score === 4) {
            strengthBar.classList.add("good")
            strengthText.textContent = "Contrasena buena"
        } else if (score === 5) {
            strengthBar.classList.add("strong")
            strengthText.textContent = "Contrasena fuerte"
        }
    }

    function checkPasswordMatch() {
        var nuevaPassword = nuevaPasswordInput.value
        var confirmarPassword = confirmarPasswordInput.value

        if (confirmarPassword.length > 0) {
            matchIndicator.classList.add("show")
            if (nuevaPassword === confirmarPassword) {
                matchIndicator.classList.add("match")
                matchIndicator.classList.remove("no-match")
                matchIndicator.innerHTML = '<i class="fas fa-check"></i>'
                matchMessage.textContent = "Las contrasenas coinciden"
                matchMessage.className = "match-message success"
                return true
            } else {
                matchIndicator.classList.add("no-match")
                matchIndicator.classList.remove("match")
                matchIndicator.innerHTML = '<i class="fas fa-times"></i>'
                matchMessage.textContent = "Las contrasenas no coinciden"
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
        var passwordValid = validatePassword(nuevaPasswordInput.value)
        var passwordsMatch = checkPasswordMatch()

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
        strengthText.textContent = "Ingrese una contrasena"
        strengthPercentage.textContent = "0%"
        matchIndicator.classList.remove("show")
        matchMessage.textContent = ""
        requirementsContainer.style.display = "none"

        // Resetear iconos de mostrar contrasena
        var toggleButtons = document.querySelectorAll(".toggle-password")
        for (var l = 0; l < toggleButtons.length; l++) {
            var btn = toggleButtons[l]
            var icon = btn.querySelector("i")
            icon.className = "fas fa-eye"
            btn.classList.remove("active")
            btn.setAttribute("title", "Mostrar contrasena")
        }

        // Resetear tipos de input
        var textInputs = document.querySelectorAll('input[type="text"]')
        for (var m = 0; m < textInputs.length; m++) {
            var input = textInputs[m]
            if (input.name.includes("contrasena") || input.name.includes("Password")) {
                input.type = "password"
            }
        }
    }

    // Event listeners
    nuevaPasswordInput.addEventListener("input", updateSubmitButton)
    confirmarPasswordInput.addEventListener("input", updateSubmitButton)
    resetBtn.addEventListener("click", resetForm)

    // Event listeners para botones de mostrar contrasena
    var toggleButtons = document.querySelectorAll(".toggle-password")
    for (var n = 0; n < toggleButtons.length; n++) {
        var toggleButton = toggleButtons[n]
        toggleButton.addEventListener("click", function () {
            var targetId = this.getAttribute("data-target")
            togglePassword(targetId)
        })
    }

    // Efecto ripple mejorado
    var rippleButtons = document.querySelectorAll(".btn")
    for (var o = 0; o < rippleButtons.length; o++) {
        var rippleButton = rippleButtons[o]
        rippleButton.addEventListener("click", function (e) {
            var ripple = this.querySelector(".btn-ripple")
            if (ripple) {
                var rect = this.getBoundingClientRect()
                var size = Math.max(rect.width, rect.height)
                var x = e.clientX - rect.left - size / 2
                var y = e.clientY - rect.top - size / 2

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
    var formGroups = document.querySelectorAll(".form-group")
    for (var p = 0; p < formGroups.length; p++) {
        var group = formGroups[p]
        group.style.animationDelay = p * 0.1 + "s"
        group.classList.add("fade-in")
    }

    // Tooltips para botones de mostrar contrasena
    for (var q = 0; q < toggleButtons.length; q++) {
        toggleButtons[q].setAttribute("title", "Mostrar contrasena")
    }
})
