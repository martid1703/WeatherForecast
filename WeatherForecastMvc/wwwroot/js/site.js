function getRandomInt(min, max) {
    var rnd = Math.floor(min + Math.random() * (max - min));
    return rnd;
}

function GetDateString(date) {
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var dateStr = date.getFullYear() + "-" + (month) + "-" + (day);
    return dateStr;
}

function ShowWarning(msg, div, timeoutSec = 3) {
    div.style = "display: inline-block;";
    $(div).text(msg);
    // setTimeout(() =>
    //     HideWarning(div),
    //     timeoutSec * 1000);
}

function HideWarning(div) {
    div.style = "display: none;";
}

function FillRandomTemp(tableBodyName, min, max) {
    var tableBody = document.getElementById(tableBodyName);
    for (let i = 0; i < tableBody.rows.length; i++) {
        var rnd = getRandomInt(min, max);
        tableBody.rows[i].cells[1].querySelector("#tempId").value = rnd;
    }
}

function AreEqualDates(a, b) {
    var date1 = new Date(a);
    date1.setHours(0, 0, 0, 0);
    var date2 = new Date(b);
    date2.setHours(0, 0, 0, 0);
    var equal = date1.getTime() === date2.getTime();
    return equal;
}

function StringToDate(dateStr) {
    var date = new Date(dateStr);
    date.setHours(0, 0, 0, 0);
    return date;
}