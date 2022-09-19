$(document).ready(function () {



    $("#txtLanguages").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/PriceList/GetEditorials',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                messages: {
                    noResults: '',
                    results: function () { }
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item,
                            value: item + ""
                        }
                    }))           
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {           
            $('#txtLanguages').val(ui.item.label);         
            return false;
        }
    }); 
   
});


$(window).scroll(function () {

    if ($(document).scrollTop() > 200) {

        $("nav").addClass("haber-navbar-shrink");

    } else {

        $("nav").removeClass("haber-navbar-shrink");

    }

});

