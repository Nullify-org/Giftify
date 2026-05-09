window.addEventListener('click', function (e) {
    const menu = document.getElementById("dropdownMenu");
    const toggleBtn = document.querySelector('.dropdown-toggle');
    if (menu && toggleBtn && !toggleBtn.contains(e.target) && !menu.contains(e.target)) {
        menu.classList.remove("show");
    }
});
function toggleDropdown() {
    var menu = document.getElementById("dropdownMenu");
    menu.classList.toggle("show");
}
function toggleMobileMenu() {
    const menu = document.getElementById("mobileMenu");
    menu.classList.toggle("show");
}

window.addEventListener('click', function (e) {
    const menu = document.getElementById("mobileMenu");
    const btn = document.querySelector('.menu-toggle-btn');
    if (menu && btn && !btn.contains(e.target) && !menu.contains(e.target)) {
        menu.classList.remove("show");
    }
});