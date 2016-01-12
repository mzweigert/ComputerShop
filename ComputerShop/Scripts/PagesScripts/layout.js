$(document).ready(function () {
    
    $("#search-btn").click(function (e) {

        var $searchText = $("#search-text"),
            $alertPhrase = $(".alert-phrase");

        $alertPhrase.addClass("fade");

        
        if ($searchText.val().length < 3 )
        {
            e.preventDefault();
            $alertPhrase.removeClass("fade");
        } 

    })

});