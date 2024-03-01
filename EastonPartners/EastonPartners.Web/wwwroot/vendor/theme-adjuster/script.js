const body = document.body;

if (body.classList.contains("theme-light")) {
    document.getElementById("unactivated").classList.add("btn-outline-dark");
}
else if (body.classList.contains("theme-dark")) {
    document.getElementById("unactivated").classList.add("btn-outline-light");
}