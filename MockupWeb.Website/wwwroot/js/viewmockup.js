
$(document).ready(function () {
    var currentMousePos = { x: -1, y: -1 };
    $(document).mousemove(function (event) {
        currentMousePos.x = event.pageX;
        currentMousePos.y = event.pageY;
    });
    $(window).resize(function () {
        CorrectImageStyleForWidth();
    });
    CorrectImageStyleForWidth();
    $('map').imageMapResize();

    $("#mockupImage").click(function () {
        console.log('mouse:[x:' + currentMousePos.x + ',y:' + currentMousePos.y + ']');
    });
    // disabling for now may revisit later
    var hijackClicks = false;
    if (hijackClicks) {
        $("#controlmap area")[0].onclick = null;
        $("#controlmap area").click(OnAreaClick);
    }
});
var _linkedControls = Array();
function SetLinkedControls(linkedControls) {
    _linkedControls = linkedControls
}
function CorrectImageStyleForWidth() {
    // if the image is larger than it's rendered make set width to not use 100%
    var mimg = document.getElementById('mockupImage');
    if (mimg.clientWidth >= mimg.naturalWidth) {
        if (!($("#mockupImage").hasClass('mockupImageFullWidth'))) {
            $("#mockupImage").removeClass('mockupImageFullWidth');
        }
    }
    else {
        if (!($("#mockupImage").hasClass('mockupImageFullWidth'))) {
            $("#mockupImage").addClass('mockupImageFullWidth');
        }
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
    var targetUrl = this.href;
    var newImgUrl = GetImageUrlFromViewMockupUrl(targetUrl);
    console.log('handle onareaclick');
    var lcApiUrl = window.location.origin + '/api/LinkedControls?mockupUrl=' + encodeURIComponent(targetUrl);

    $.get(lcApiUrl, function (data, status) {
        var mapInfo = JSON.parse(data);

        // remove map element and re-add a new one
        $("#controlmap").remove();
        $("body").append('<map name="controlmap" id="controlmap"></map>');
        var ctrlMap = $('#controlmap');

        for (var i = 0; i < mapInfo.length; i++) {
            var current = mapInfo[i];
            var elementHtml = '<area class="linkedControl" shape="rect" coords="' + current.LocationX + ',' + current.LocationY + ',' + current.MaxLocationX + ',' + current.MaxLocationY +
                '" href="' + current.MockupUrl + '" alt="' + current.MockupUrl + '" />';
            console.log('adding element: ' + elementHtml);
            ctrlMap.append(elementHtml);
        }

        // rebind events
        $("#mockupImage").removeClass('mockupImageFullWidth');
        $("#mockupImage").addClass('mockupImageFullWidth');
        CorrectImageStyleForWidth();
        $('#controlmap').imageMapResize();

        $("#controlmap area")[0].onclick = null;
        $("#controlmap area").click(OnAreaClick);

        history.pushState(null, null, targetUrl);
    });

    $('#mockupImage')[0].src = newImgUrl;
}
function GetImageUrlFromViewMockupUrl(vmUrl) {
    var mockupNameRegEx = /&mockupName=(.*)/g;
    var reResult = mockupNameRegEx.exec(vmUrl.replace(window.location.origin, ''));

    var mockupFolderPattern = /MockupPath=(.*)%5C/g;
    var folderResult = mockupFolderPattern.exec(vmUrl);

    var imgUrl = '';
    if (reResult != null &&
        reResult.length >= 2 &&
        folderResult != null &&
        folderResult.length >= 2) {

        imgUrl = window.location.origin + '/mockups/' + folderResult[1] + '/' + reResult[1] + '.png';
    }
    
    return imgUrl;
}
function GetMockupFolderpathFromUrl(srcurl) {
    var mockupFolderPattern = /MockupPath=(.*)%5C/g;
    var folderResult = mockupFolderPattern.exec(srcurl);
    if (folderResult != null && folderResult.length >= 2) {
        return folderResult[1];
    }
    return null;
}
var currentMockupRoot = GetMockupFolderpathFromUrl(window.location.href);