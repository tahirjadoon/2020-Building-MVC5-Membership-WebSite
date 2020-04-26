$(function () {
    //target the li that has the attribute data-admin-menu
    //we'll toggle the class open, to open and close the menu
    $('li[data-admin-menu]').hover(function () {
        $(this).toggleClass('open');
    });
});