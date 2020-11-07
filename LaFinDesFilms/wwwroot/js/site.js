// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//trouvé ici : https://github.com/Fcmam5/nightly.js
var nightModeConfig = {
    buttons: {
        backgroundColor: "#343a40", // #757575
    },};
var Nightly = new Nightly(nightModeConfig, true); // To disable persistence, set false instead of true
if (Nightly.isDark) {
    toggleDarkTheme();
} else {
    $("#switch_theme").prop('checked', true);
    toggleLightTheme();
}

function ToggleTheme() {
    if ($('#switch_theme').is(":checked")) {
        Nightly.lightify();
        $(".navbar").addClass("navbar-light bg-white border-bottom-light")
        $(".navbar").removeClass("navbar-dark bg-dark border-bottom-dark")
    } else {
        Nightly.darkify();
        $(".navbar").removeClass("navbar-light bg-white border-bottom-light")
        $(".navbar").addClass("navbar-dark bg-dark border-bottom-dark")
    }
}

function toggleDarkTheme() {
    Nightly.darkify();
    $(".navbar").removeClass("navbar-light bg-white border-bottom-light")
    $(".navbar").addClass("navbar-dark bg-dark border-bottom-dark")
}

function toggleLightTheme() {
    Nightly.lightify();
    $(".navbar").addClass("navbar-light bg-white border-bottom-light")
    $(".navbar").removeClass("navbar-dark bg-dark border-bottom-dark")
}