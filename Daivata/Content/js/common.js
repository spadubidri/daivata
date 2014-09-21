function resetActive(event, percent, step) {
    $(".progress-bar").css("width", percent + "%").attr("aria-valuenow", percent);
    $(".progress-completed").text(percent + "%");

    $("div").each(function () {
        if ($(this).hasClass("activestep")) {
            $(this).removeClass("activestep");
        }
    });

    if (event.target.className == "col-md-2") {
        $(event.target).addClass("activestep");
    }
    else {
        $(event.target.parentNode).addClass("activestep");
    }

    hideSteps();
    showCurrentStepInfo(step);
}

function hideSteps() {
    $("div").each(function () {
        if ($(this).hasClass("activeStepInfo")) {
            $(this).removeClass("activeStepInfo");
            $(this).addClass("hiddenStepInfo");
        }
    });
}

function showCurrentStepInfo(step) {
    var id = "#" + step;
    $(id).addClass("activeStepInfo");
}


function submitAjax(oData) {
    // debugger;
    if ((typeof (oData) != "undefined")) {
        if ((typeof (oData.type) != "undefined") && (typeof (oData.url) != "undefined") && (typeof (oData.data) != "undefined") && (typeof (oData.success) == "function")) {
            var onErrorMethod;
            if ((oData.error == "undefined") || (typeof (oData.error) != "function")) {
                onErrorMethod = function () { showErrorValidationSummary([generalErrorMessage]); }
            }
            else {
                onErrorMethod = oData.error;
            }

            $.ajax({
                type: oData.type,
                url: oData.url,
                data: oData.data,
                beforeSend: oData.beforeSend,
                success: oData.success,
                error: onErrorMethod,
                complete: oData.complete
            });
        }
    }
    return false;
}

function getFormData(formid) {
    return $("#" + formid).serialize();
}
