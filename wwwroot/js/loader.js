function loader(bar) {
    var elem = $(bar);
    elem.css("background","#357CE0");
    var width = 1;
    var id = setInterval(frame, 10);
    function frame() {
        if (width >= 100) {
            clearInterval(id);
        } else {
            width++;
            elem.css("width", width + '%');
        }
    }
}
