// Write your Javascript code.
function PreloadImages(imagestring) {
    var imageurls = imagestring.split(';');
    if (imageurls != null) {
        for (var i = 0; i < imageurls.length; i++) {
            // preload the image now
            console.log("Loading image from url: " + imageurls[i]);
            var image = new Image();
            image.src = imageurls[i];
        }
    }
}