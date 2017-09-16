$(document).ready(function () {

    var currentMousePos = { x: -1, y: -1 };
    $(document).mousemove(function (event) {
        currentMousePos.x = event.pageX;
        currentMousePos.y = event.pageY;
    });

    CorrectImageStyleForWidth();
    $('map').imageMapResize();
    console.log('called imageMapResize');

    //$("#controlmap area")[0].onclick = null;
    //$("#controlmap area").click(OnAreaClick);

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
function OnAreaClick() {
    event.preventDefault();
    var newImgUrl = GetImageUrlFromViewMockupUrl(this.href);
    $('#mockupImage')[0].src = newImgUrl;
}
function GetImageUrlFromViewMockupUrl(vmUrl) {
    // http://localhost:63108/ViewMockupPage?MockupPath=mockupname%5Cpublish.2017.09.12-pub-spec.bmpr.json&mockupName=publish-tab-publishing
    // http://localhost:63108/mockups/mockupname/publish-tab-publishing.png
    
    var mockupNameRegEx = /&mockupName=(.*)/g;
    var reResult = mockupNameRegEx.exec(vmUrl.replace(window.location.origin, ''));

    var mockupFolderPattern = /MockupPath=(.*)%5C/g;
    var folderResult = mockupFolderPattern.exec(vmUrl);

    var imgUrl = '';
    if (reResult != null &&
        reResult.length >= 2 &&
        folderResult != null &&
        folderResult.length >= 2) {

        imgUrl = decodeURI(window.location.origin + '/mockups/' + folderResult[1] + '/' + reResult[1] + '.png');
    }
    
    return imgUrl;
}