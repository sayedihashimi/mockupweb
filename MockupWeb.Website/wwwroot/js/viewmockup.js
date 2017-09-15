﻿$(document).ready(function () {

    var currentMousePos = { x: -1, y: -1 };
    $(document).mousemove(function (event) {
        currentMousePos.x = event.pageX;
        currentMousePos.y = event.pageY;
    });

    CorrectImageStyleForWidth();
    $('map').imageMapResize();
    console.log('called imageMapResize');

    $("#mockupImage").click(function () {
        console.log('mouse:[x:' + currentMousePos.x + ',y:' + currentMousePos.y + ']');
        var mockupUrl = GetMockupUrlFromClick(currentMousePos.x, currentMousePos.y);
        if (mockupUrl != null) {
            window.location.href = mockupUrl;
        }
    });
});
var _linkedControls = Array();
function SetLinkedControls(linkedControls) {
    _linkedControls = linkedControls
}
function CorrectImageStyleForWidth() {
    // if the image is larger than it's rendered make set width to not use 100%
    var mimg = document.getElementById('mockupImage');
    if (mimg.clientWidth > mimg.naturalWidth) {
        $("#mockupImage").removeClass('mockupImageFullWidth');
    }
}
function GetMockupUrlFromClick(mouseX, mouseY) {
    // see if the point is contained in any linked control
    for (var i = 0; i < _linkedControls.length; i++) {
        var lc = _linkedControls[i];

        var minX = lc.LocationX;
        var maxX = minX + lc.Width;

        var minY = lc.LocationY;
        var maxY = minY + lc.Height;

        if (mouseX >= minX && mouseX <= maxX &&
            mouseY >= minY && mouseY <= maxY) {
            return lc.MockupUrl;
        }
    }
}
