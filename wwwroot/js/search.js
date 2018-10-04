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
                var row = "";
                data.forEach(function (element, i) {
                    row += "<a href='#"+element["id"]+"'><div class='searchResult'> ";
                    row += "<h3 class='projectName'>"+element["name"]+"</h3>";
                    row += "<span class='projectStars'>" + element["stargazers_count"] + "</span><img class='projectStarsImg' src='/images/star.png' />";
                    row += "<img class='projectOwnerImg' src='" + element["owner"]["avatar_url"]+"'/>";
                    row += "</ br><span class='projectOwner'>" + element["owner"]["login"] + "</span>";
                    row += "</div>";
                    $("#searchResult").append(row);
                    row = "";
                });


            },
            failure: function (response) {
                alert(response);
            }
        });
    });
});
