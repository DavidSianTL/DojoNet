document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm");

    form.addEventListener("submit", function (e) {
        const usuarioInput = document.getElementById("usuario");
        const passwordInput = document.getElementById("password");

        const usuario = usuarioInput.value.trim();
        const password = passwordInput.value.trim();

    
        const sqlInjectionPattern = /(\b(SELECT|INSERT|DELETE|UPDATE|DROP|UNION|--|''|;)\b)/i;

        let isValid = true;

       
        usuarioInput.classList.remove("is-invalid");
        passwordInput.classList.remove("is-invalid");

    
        if (!usuario || usuario.length < 3 || sqlInjectionPattern.test(usuario)) {
            usuarioInput.classList.add("is-invalid");
            isValid = false;
        }

        if (!password || password.length < 6 || sqlInjectionPattern.test(password)) {
            passwordInput.classList.add("is-invalid");
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            e.stopPropagation();
        }
    });
});

document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm")
    const usuarioInput = document.getElementById("usuario")
    const passwordInput = document.getElementById("password")
  
    // Función para mostrar errores de validación
    function showError(input, message) {
      input.classList.add("is-invalid")
      const feedback = input.nextElementSibling
      if (feedback && feedback.classList.contains("invalid-feedback")) {
        feedback.textContent = message
      }
    }
  
    // Función para limpiar errores
    function clearError(input) {
      input.classList.remove("is-invalid")
    }
  
    // Validación en tiempo real
    usuarioInput.addEventListener("input", function () {
      if (this.value.trim() !== "") {
        clearError(this)
      }
    })
  
    passwordInput.addEventListener("input", function () {
      if (this.value.length >= 6) {
        clearError(this)
      }
    })
  
    // Validación al enviar el formulario
    form.addEventListener("submit", (e) => {
      let isValid = true
  
      // Validar usuario
      if (usuarioInput.value.trim() === "") {
        showError(usuarioInput, "Ingresa tu nombre de usuario.")
        isValid = false
      } else {
        clearError(usuarioInput)
      }
  
      // Validar contraseña
      if (passwordInput.value.length < 6) {
        showError(passwordInput, "La contraseña debe tener al menos 6 caracteres.")
        isValid = false
      } else {
        clearError(passwordInput)
      }
  
      // Si no es válido, prevenir el envío
      if (!isValid) {
        e.preventDefault()
        e.stopPropagation()
      }
  
      // Agregar clase de validación de Bootstrap
      form.classList.add("was-validated")
    })
  })

  document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm")
    const usuarioInput = document.getElementById("usuario")
    const passwordInput = document.getElementById("password")
    const loginButton = document.getElementById("loginButton")
    const loginText = document.querySelector(".login-text")
    const loginSpinner = document.querySelector(".login-spinner")
  
    // Función para mostrar errores de validación
    function showError(input, message) {
      input.classList.add("is-invalid")
      const feedback = input.nextElementSibling
      if (feedback && feedback.classList.contains("invalid-feedback")) {
        feedback.textContent = message
      }
    }
  
    // Función para limpiar errores
    function clearError(input) {
      input.classList.remove("is-invalid")
    }
  
    // Función para mostrar estado de carga
    function showLoading() {
      loginButton.disabled = true
      loginText.classList.add("d-none")
      loginSpinner.classList.remove("d-none")
    }
  
    // Función para ocultar estado de carga
    function hideLoading() {
      loginButton.disabled = false
      loginText.classList.remove("d-none")
      loginSpinner.classList.add("d-none")
    }
  
    // Validación en tiempo real
    usuarioInput.addEventListener("input", function () {
      if (this.value.trim() !== "") {
        clearError(this)
      }
    })
  
    passwordInput.addEventListener("input", function () {
      if (this.value.length >= 6) {
        clearError(this)
      }
    })
  
    // Validación al enviar el formulario
    form.addEventListener("submit", (e) => {
      let isValid = true
  
      // Validar usuario
      if (usuarioInput.value.trim() === "") {
        showError(usuarioInput, "Ingresa tu nombre de usuario.")
        isValid = false
      } else {
        clearError(usuarioInput)
      }
  
      // Validar contraseña
      if (passwordInput.value.length < 6) {
        showError(passwordInput, "La contraseña debe tener al menos 6 caracteres.")
        isValid = false
      } else {
        clearError(passwordInput)
      }
  
      // Si no es válido, prevenir el envío
      if (!isValid) {
        e.preventDefault()
        e.stopPropagation()
        return false
      }
  
      // Mostrar estado de carga
      showLoading()
  
      // Agregar clase de validación de Bootstrap
      form.classList.add("was-validated")
    })
  
    // Auto-ocultar alertas después de 5 segundos
    const alerts = document.querySelectorAll(".alert")
    alerts.forEach((alert) => {
      setTimeout(() => {
        if (alert && alert.parentNode) {
          alert.classList.remove("show")
          setTimeout(() => {
            if (alert.parentNode) {
              alert.remove()
            }
          }, 150)
        }
      }, 5000)
    })
  
    // Enfocar el primer campo vacío
    if (usuarioInput.value.trim() === "") {
      usuarioInput.focus()
    } else if (passwordInput.value === "") {
      passwordInput.focus()
    }
  })
  
  // Función para alternar visibilidad de contraseña
  function togglePassword() {
    const passwordInput = document.getElementById("password")
    const toggleIcon = document.getElementById("toggleIcon")
  
    if (passwordInput.type === "password") {
      passwordInput.type = "text"
      toggleIcon.classList.remove("bi-eye")
      toggleIcon.classList.add("bi-eye-slash")
    } else {
      passwordInput.type = "password"
      toggleIcon.classList.remove("bi-eye-slash")
      toggleIcon.classList.add("bi-eye")
    }
  }
  
  // Funciones para enlaces (placeholder)
  function forgotPassword() {
    alert("Funcionalidad de recuperación de contraseña - Próximamente disponible")
  }
  
  function register() {
    alert("Funcionalidad de registro - Próximamente disponible")
  }
  
  function privacyPolicy() {
    alert("Política de privacidad - Próximamente disponible")
  }
  
  // Función para manejar errores de red
  window.addEventListener("beforeunload", () => {
    const loginButton = document.getElementById("loginButton")
    const loginText = document.querySelector(".login-text")
    const loginSpinner = document.querySelector(".login-spinner")
  
    if (loginButton && loginButton.disabled) {
      loginButton.disabled = false
      if (loginText) loginText.classList.remove("d-none")
      if (loginSpinner) loginSpinner.classList.add("d-none")
    }
  })
  