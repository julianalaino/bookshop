$(document).ready(function() {

    $(".owl-carousel").owlCarousel({

        loop: true,

        nav: true

    });

    $('.carousel').carousel({

        interval: 2700

    });


});


function ShowHideDiv(esSuscriptor) {
    var numSuscriptor = document.getElementById("numSuscriptor");
    numSuscriptor.style.display = esSuscriptor.checked ? "block" : "none";
}
$(window).scroll(function () {

    if ($(document).scrollTop() > 200) {

        $("nav").addClass("haber-navbar-shrink");

    } else {

        $("nav").removeClass("haber-navbar-shrink");

    }

});
