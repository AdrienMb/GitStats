
function stats(name, owner) {
    event.preventDefault();
    loader("#statsLoader");
    $.ajax({
        url: "/api/Stats/" + name + "&" + owner,
        type: "GET",
        dataType: "json",
        tryCount: 0,
        retryLimit: 3,
        success: function (data, i) {
            contributors(data,name,owner);
            commitsGraph(data, name, owner);
            $("#statsLoader").css("background", "white");
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
function contributors(data, name,owner) {
    var row = "<div class='button'>";
    row += "<a href='#' id='save' onclick=\"saveBookmark('" + name + "','" + owner + "')\">Sauvegarder</a></div><br />";
    $("#contributors").html("");
    row += "<h4 class='contributor'>Nom</h3>";
    row += "<h4 class='nbCommits'>Commits</h3>";
    data.forEach(function (element, i) {
        row += "<div class='contributor'><img class='projectOwnerImg' src='" + element["avatar_url"] + "'/>";
        row += "</ br><span class='projectOwner'>" + element["login"] + "</span>";
        row += "<span class='nbCommits'>" + element["commits"].length + "</span></div>";
        $("#contributors").append(row);
        row = "";
    });
}

function clearGraph() {
    Plotly.purge('graphPlotly');
}

function commitsGraph(data, name, owner) {
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

    GRAPH = document.getElementById('graphPlotly');

    var data = [{
        type: 'bar',
        x: days,
        y: values,
        name: name+' par '+owner,
        mode: 'markers',
        transforms: [{
            type: 'aggregate',
            aggregations: [
                { target: 'y', func: 'sum', enabled: true },
            ]
        }]
    }];
    var layout = {
        title: 'Commits pour chaque semaine (sur les 100 derniers)',
        font: {
            family: 'Raleway, sans-serif'
        },
        showlegend: false,
        xaxis: {
            tickangle: -45
        },
        yaxis: {
            title: 'Nombre de commits',
            zeroline: false,
            gridwidth: 2
        },
        bargap: 0.05
    };
    Plotly.plot(GRAPH, data, layout);
}

