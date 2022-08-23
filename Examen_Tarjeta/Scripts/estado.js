function isChecked() {
    if (document.getElementById("Estado").checked) {
        document.getElementById("mensaje").textContent = "Activo"
    } else {
        document.getElementById("mensaje").textContent = "Inactivo"
    }
}