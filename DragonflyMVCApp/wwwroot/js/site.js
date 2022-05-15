
function updateFooterDate() {
    const footerDate = document.getElementById("footer-date");
    const currentYear = new Date().getFullYear();
    footerDate.innerHTML = `&copy; ${currentYear} by Andre Bačić`;
}


function translucentCoverOnClick(t) { // TODO: Move this unused code to the graveyard
    t.style.display = "none"
    for (m of menus) {
        m.style.display = "none"
    }
    for (a of arrows) {
        a.style.transform = "rotateZ(-90deg)"
    }
}


updateFooterDate();
