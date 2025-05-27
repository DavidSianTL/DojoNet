document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("loginForm");

    form.addEventListener("submit", function (e) {
        const usuarioInput = document.getElementById("usuario");
        const passwordInput = document.getElementById("password");

        const usuario = usuarioInput.value.trim();
        const password = passwordInput.value.trim();
        const sqlInjectionPattern = /(\b(SELECT|INSERT|DELETE|UPDATE|DROP|UNION|--||''|;)\b)/i;

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
        }
    });
});
