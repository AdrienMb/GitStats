
function stats(name, owner) {
    event.preventDefault();
    $.ajax({
        url: "/api/Stats/" + name + "&" + owner,
        type: "GET",
        dataType: "json",
        success: function (data, i) {
            contributors(data,name,owner);
            commitsGraph(data);
        },
        failure: function (response) {
            alert(response);
        }
    });
}
function contributors(data, name,owner) {
    var row = "";
    row += "<a href='#' id='save' onclick=\"saveBookmark('" + name + "','" + owner + "')\">Sauvegarder</a>";
    $("#contributors").html("");
    data.forEach(function (element, i) {
        row += "<img class='projectOwnerImg' src='" + element["avatar_url"] + "'/>";
        row += "</ br><span class='projectOwner'>" + element["login"] + "</span>";
        row += "<span class='nbCommits'>" + element["commits"].length + "</span><br />";
        $("#contributors").append(row);
        row = "";
    });
}
function commitsGraph(data) {
    var dates = [];
    data.forEach(function (element, i) {
        dates = dates.concat((element["commits"]));
    });

    // à revoir
    var days = [];
    dates.forEach(function (element, i) {
        days.push(element.substring(0, 10));
    });

    var values = new Array(days.length).fill(1);
    console.log(days);
    console.log(values);

    TESTER = document.getElementById('tester');

    var data = [{
        type: 'bar',
        x: days,
        y: values,
        mode: 'markers',
        transforms: [{
            type: 'aggregate',
            aggregations: [
                { target: 'y', func: 'sum', enabled: true },
            ]
        }]
    }];
    var layout = {
        title: '100 derniers commits pour chaque semaine',
        font: {
            family: 'Raleway, sans-serif'
        },
        showlegend: false,
        xaxis: {
            tickangle: -45
        },
        yaxis: {
            zeroline: false,
            gridwidth: 2
        },
        bargap: 0.05
    };
    Plotly.plot(TESTER, data, layout);
}

