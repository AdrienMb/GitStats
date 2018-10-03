$(document).ready(function () {
    console.log("test");
    $("#search").submit(function (e) {
        event.preventDefault();
        console.log("test2");
        $.ajax({
            url: "/api/Search/" + $("#searchBar").val(),
            type: "GET",
            dataType: "json",
            success: function (data, i) {
                $("#searchResult").html("");
                var row = "<div class=\"row\">";
                data.forEach(function (element, i) {
                    console.log(element["name"]);
                    row += "<div class=\"col-sm-4\"><div class=\"searchResult\">" + element["name"] + "</div></div>";
                    if ((i + 1) % 3 == 0) {
                        row += "</div>";
                        $("#searchResult").append(row);
                        row = "<div class=\"row\">";
                    }
                });

            },
            failure: function (response) {
                alert(response);
            }
        });
    });
});
