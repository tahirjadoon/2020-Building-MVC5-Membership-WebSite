/*
 ProductContent
 When the panel is collapsed then we want to remove the gly-rotate-90 class. We are using toggleClass function
 We also want to toggle pressed class
 check Carret.css for details
*/
$(function () {
    $(".panel-carret").click(function (e) {
        //pressed class is rotating back to 0deg
        $(this).toggleClass("pressed");

        //change the title attribute accordingly as well
        var state = "collapse"; //default - expand state
        if ($(this).hasClass("pressed")) {
            state = "expand"; //collapsed state
        }
        var data = $(this).attr("data");
        $(this).attr("title", "Click to " + state + " " + data);

        //glyphicon-play is a child element
        $(this).children("glyphicon-play").toggleClass("gly-rotate-90");
        //stop any default behavior for this a element
        e.preventDefault();
    });
});