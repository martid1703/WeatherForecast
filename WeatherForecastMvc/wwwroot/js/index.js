// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function UpdateForecast(calendarName) {
    var calendar = document.getElementById(calendarName);
    var date = new Date(calendar.value);
    var jsonDate = GetDateString(date);
    var url = "/Forecast/IndexWithDateJS?jsDate=" + jsonDate;
    window.location.href = url;
}

function GetNewForecastView(calendarName) {
    var calendar = document.getElementById(calendarName);
    window.location.href = "/Forecast/Create?date=" + calendar.value;
}

function ShowNextHottestDay(calendarName, tempDtos, tableName) {
    if (tempDtos.length === 0) {
        return;
    }

    tempDtos.sort((a, b) => (a.Date > b.Date) ? 1 : ((b.Date > a.Date) ? -1 : 0))

    var hottestDay = tempDtos[0];

    var calendar = document.getElementById(calendarName);
    if (StringToDate(hottestDay.Date) <= StringToDate(calendar.value)) {
        for (let i = 1; i < tempDtos.length; i++) {
            if (tempDtos[i].AvgTemp > hottestDay.AvgTemp) {
                hottestDay = tempDtos[i];
                break;
            }
        }
    }

    var timeoutSec = 3;
    PrintHottestDay(hottestDay, timeoutSec);
    HighlightHottestDay(tableName, hottestDay.Date, timeoutSec);
}

function PrintHottestDay(hottestDay, timeoutSec) {
    var date = new Date(hottestDay.Date).toLocaleDateString();
    $('#hottestDayArea').html("Next hottest day is:" + date + ", avg. temp:" + hottestDay.AvgTemp + "&deg;C");
    setTimeout(
        function () { $('#hottestDayArea').html(""); },
        timeoutSec * 1000);
}

function HighlightHottestDay(tableBodyName, date, timeoutSec) {
    var tableBody = document.getElementById(tableBodyName);
    for (let i = 0; i < tableBody.rows.length; i++) {
        var element = tableBody.rows[i].querySelector("#dateId");
        var parts = element.innerText.split('.');
        var elementDate = new Date(parts[2], parts[1] - 1, parts[0]);
        var equal = AreEqualDates(elementDate, date);
        if (equal) {
            HighlightWithTimeout(tableBody.rows[i], '#ffd966', timeoutSec)
            break;
        }
    }
}

function HighlightWithTimeout(element, colorHexStr, timeoutSec) {
    var backgroundColor = element.style.backgroundColor;
    element.style.backgroundColor = colorHexStr;
    setTimeout(() => { element.style.backgroundColor = backgroundColor; }, timeoutSec * 1000);
}

function SetCalendarValue(calendarName, date) {
    var calendar = document.getElementById(calendarName);
    var dateStr = GetDateString(date);
    calendar.setAttribute("value", dateStr);
}


