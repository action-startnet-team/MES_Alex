function lightbox(title, url, width, height) {
    $.colorbox({ width: width, height: height, iframe: true, title: title, href: url, escKey: false, overlayClose: false });
}
function lightbox_close() {
    $.colorbox.remove();
}