function toggleFilterDiv() {
    var filterDiv = $("#filterDiv");
    if (filterDiv.attr("style").includes("display: none")) {
        filterDiv.show();
        $("#filterBtn").html("Ukryj filtrowanie");
        $("#filterBtn").parent().css("border-bottom-left-radius", "0");
        $("#filterBtn").parent().css("border-bottom-right-radius", "0");
    }
    else {
        filterDiv.hide();
        $("#filterBtn").html("Filtruj pozycje");
        $("#filterBtn").parent().css("border-bottom-left-radius", "0.5em");
        $("#filterBtn").parent().css("border-bottom-right-radius", "0.5em");
    }
}