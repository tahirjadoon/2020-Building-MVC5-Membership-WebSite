//either call the following where needed
function jwVideo(video) {
    jwplayer("video").setup({ file: video });
}

//and then on the page 
/*
$(function () {
    jwVideo($("#hiddenUrl").text());
});
*/