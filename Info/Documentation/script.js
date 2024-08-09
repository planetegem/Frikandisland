const frikandisland = document.getElementById("frikandisland-header");
let toggle = true;

function toggleHeader(){
    if (toggle){
        frikandisland.innerText = "FRIKANDIELAND";
        setTimeout(toggleHeader, 200);
    } else {
        frikandisland.innerText = "FRIKANDISLAND";
        setTimeout(toggleHeader, 4000);
    }
    toggle = !toggle;
}
window.setTimeout(toggleHeader, 4000);

const selector = document.getElementById("selector");
selector.addEventListener("change", () => {
    if (window.location.hostname == "localhost"){
        window.location.href = "/Info/Documentation/" + selector.value;
    } else {
        window.location.href = "https://www.planetegem.be/eb/frikandisland/documentation/" + selector.value;
    }
})