const incorrectDateMsg = "Please select another date.";
var dateInput = document.getElementById("createForecastDateInput");
var formDate = document.getElementById("formDate");
var notificationArea = document.getElementById("notificationArea");

function OnDateSelected() {
    var date = new Date(dateInput.value);
    var jsonDate = GetDateString(date);
    var url = "/Forecast/CheckDetails";
    $.ajax({
        url: url,
        data: { "jsDate": jsonDate },
    })
        .done(function () {
            UpdateFormInputDate();
        })
        .fail(function () {
            ShowWarning(incorrectDateMsg, notificationArea);
        });
}

function ProcessStatus(response) {
    if (response.status === 200) {
        UpdateFormInputDate();
    }
}

function UpdateFormInputDate() {
    formDate.value = dateInput.value;
    HideWarning(notificationArea);
}

function CheckAndSubmitForm(formName) {
    var date = new Date(dateInput.value);
    var jsonDate = GetDateString(date);
    var url = "/Forecast/CheckDetails";
    $.ajax({
        url: url,
        data: { jsDate: jsonDate }
    })
        .done(function () {
            SubmitForm(formName);
        })
        .fail(function () {
            ShowWarning(incorrectDateMsg, notificationArea);
        });
}

function SubmitForm(formName) {
    UpdateFormInputDate()
    return document.forms[formName].submit();
}

function LimitCalendarSpan(calendarName, startDate, span) {
    var todayStr = GetDateString(startDate);
    var maxDate = GetMaxDate(startDate, span);
    var maxDateStr = GetDateString(maxDate);

    var calendar = document.getElementById(calendarName);
    calendar.setAttribute("min", todayStr);
    calendar.setAttribute("max", maxDateStr);
    calendar.setAttribute("value", todayStr);
}

function GetMaxDate(now, span) {
    const maxDate = now;
    maxDate.setDate(now.getDate() + span)
    return maxDate;
}



