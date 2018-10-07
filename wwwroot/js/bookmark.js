function saveBookmark(name, owner) {
    $.cookie("name", name);
    $.cookie("owner", owner);

}
function getBookmarks() {
    if ($.cookie("owner") != null) {
        $("#searchResult").html("");
        var row = "";
        row += "<a href='#' onclick=\"stats('" + $.cookie("name") + "','" + $.cookie("owner") + "')\"><div class='searchResult'> ";
        row += "<h3 class='projectName'>" + $.cookie("name") + "</h3>";
        row += "</ br>" + $.cookie("owner");
        row += "</div></a>";
        $("#searchResult").append(row);
        row = "";
    }
}
