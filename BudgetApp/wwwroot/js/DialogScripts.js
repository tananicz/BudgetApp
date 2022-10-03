function toggleEditRow(currentId) {
    var alreadyOpenedRowId = "";
    $("#" + currentId).parent().children("[id]").each(function () {
        if ($(this).css("display") != "none") {
            alreadyOpenedRowId = $(this).attr("id");
        }
    });
    
    if ((alreadyOpenedRowId != "") && (alreadyOpenedRowId != currentId))
        closeTableRows();

    clearValidationErrors();

    var timeout = ((alreadyOpenedRowId != "") && (alreadyOpenedRowId != currentId)) ? 400 : 0;

    setTimeout(function () {
        $("#" + currentId).toggle(400);
    }, timeout);
}

function closeTableRows() {
    if ($("#addCategoryBtn").css("display") == "none")
        $("#addCategoryBtn").show(400);

    $("#categoriesTable > tbody").children("[id]").each(function () {
        $(this).hide(400);
    });
}

function clearValidationErrors() {
    $("#categoriesTable > tbody").children("[id]").each(function () {
        var errorMsgSpan = $(this).children("td").first().children("form").first().children("div").first().children("span").first();
        var input = errorMsgSpan.parent().next("input.form-control");
        var spanError = errorMsgSpan.hasClass("field-validation-error");

        if (spanError) {
            if ($(this).attr("id") != "addCategoryTr") {
                var catName = $(this).prev("tr").children("td").first().children("p").first().children("strong").first().html();
                input.val(catName.toString());
            }

            errorMsgSpan.removeClass("field-validation-error");
            errorMsgSpan.addClass("field-validation-valid");
            errorMsgSpan.children("#Name-error").remove();

            input.removeClass("input-validation-error");
            input.addClass("valid");
        }
    });
}

async function confirmDeleteCategory(formId, itemsUrl) {
    closeTableRows();
    clearValidationErrors();

    var formIdSplit = formId.split("_");
    var id = formIdSplit[formIdSplit.length - 1];
    var resp = await $.get("/Categories/ItemsInCategory/" + id, null, null, "json");
    var itemsCount = resp.count;

    var msgHtml = "";
    var buttonsDivHtml = "";

    if (itemsCount > 0) {
        msgHtml = "Nie można usunąć kategorii - wpierw usuń przypisane do niej pozycje";
        buttonsDivHtml = "<a href='" + itemsUrl + "' class='btn btn-light' style='float: left;'>Zobacz pozycje (" + itemsCount + ")</a><button onclick='closeDialog();' class='btn btn-light' style='float: right;'>Zamknij</button>";

    }
    else {
        msgHtml = "Czy jesteś pewien, że chcesz usunąć tę kategorię?";
        buttonsDivHtml = "<button type='submit' form='" + formId + "' class='btn btn-light' style='float: left;'>Tak</button><button onclick='closeDialog();' class='btn btn-light' style='float: right;'>Nie</button>";
    }

    showDialog(buttonsDivHtml, msgHtml);
}

function checkFilterDates(fromDateStr, toDateStr) {
    if (fromDateStr && toDateStr) {
        var fromDate = new Date(fromDateStr);
        var toDate = new Date(toDateStr);
        if (fromDate <= toDate)
            $("#applyFilter").click();
        else {
            msgHtml = "Data w polu 'Od daty' nie może być późnejsza niż data w polu 'Do daty'";
            buttonsDivHtml = "<button onclick='closeDialog();' class='btn btn-light'>Zamknij</button>";
            showDialog(buttonsDivHtml, msgHtml);
        }
    }
    else {
        $("#applyFilter").click();
    }
}

function confirmDeleteItem(formId) {
    msgHtml = "Czy jesteś pewien, że chcesz usunąć tę pozycję?";
    buttonsDivHtml = "<button type='submit' form='" + formId + "' class='btn btn-light' style='float: left;'>Tak</button><button onclick='closeDialog();' class='btn btn-light' style='float: right;'>Nie</button>";
    showDialog(buttonsDivHtml, msgHtml);
}

function showDialog(buttonsDivHtml, msgHtml) {
    var divOverlay = document.createElement("div");
    divOverlay.id = "dialogOverlay";

    var div = document.createElement("div");
    div.id = "dialog";
    div.style = "display: none;"

    divOverlay.appendChild(div);
    document.body.appendChild(divOverlay);

    jQuery("#dialogOverlay").addClass("overlay");
    jQuery("#dialog").addClass("outerDialog");

    var dialogHeight = 200;
    var dialogYPos = Math.round((screen.availHeight - dialogHeight) / 2);
    jQuery("#dialog").css("height", dialogHeight + "px");
    jQuery("#dialog").css("top", dialogYPos + "px");

    jQuery("#dialog").html("<div class='innerDialog'><div class='mb-4'>" + msgHtml + "</div><div class='buttonsDiv'>" + buttonsDivHtml + "</div></div>");
    jQuery("#dialog").show(400);
}

function closeDialog() {
    jQuery("#dialog").hide(400)
    setTimeout(function () {
        document.body.removeChild(document.getElementById("dialogOverlay"));
    }, 400);
}