$(document).ready(function () {
    var page = 1;
    function search(page) {
        event.preventDefault();
        loader("#searchLoader");
        console.log("test2");
        $.ajax({
            url: "/api/Search/" + $("#searchBar").val() + "&"+page,
            type: "GET",
            dataType: "json",
            success: function (data, i) {
                var row = "";
                data.forEach(function (element, i) {
                    row += "<a href='#" + element["id"] + "' onclick=\"stats('" + element["name"] + "','" + element["owner"]["login"] + "')\"><div class='searchResult'> ";
                    row += "<h3 class='projectName'>" + element["name"] + "</h3>";
                    row += "<span class='projectStars'>" + element["stargazers_count"] + "</span><img class='projectStarsImg' src='/images/star.png' />";
                    row += "<img class='projectOwnerImg' src='" + element["owner"]["avatar_url"] + "'/>";
                    row += "</ br><span class='projectOwner'>" + element["owner"]["login"] + "</span>";
                    row += "</div></a>";
                    $("#searchResult").append(row);
                    row = "";
                });
                $("#searchLoader").css("background", "white");
                $("#moreResult").show();

            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == 'timeout') {
                    this.tryCount++;
                    if (this.tryCount <= this.retryLimit) {
                        $.ajax(this);
                        return;
                    }
                    alert("Une erreur est survenue.");
                    return;
                }
                alert("Une erreur est survenue.");
            }
        });
    }
    $("#search").submit(function (e) {
        page = 1;
        $("#searchResult").html("");
        search(page);
        $('#moreResult').prop('disabled', false);
    });
    $("#moreResult").click(function () {
        console.log("J'y suis");
        page++;
        console.log(page);
        search(page);
    });
});
