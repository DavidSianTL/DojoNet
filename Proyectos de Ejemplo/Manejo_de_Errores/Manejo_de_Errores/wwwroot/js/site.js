const username = document.getElementById("username").value;
const password = document.getElementById("password").value;

if (username === "" || password === "") {
    event.preventDefault();
    alert("Por favor, complete todos los campos.");
}