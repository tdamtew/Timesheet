$(function () {
    $("body").on("click keypress", function () {
        ResetSession();
    });
    var sessionTimeOut = 1803;
    var ticker = 0;
    function ResetSession() {
        ticker = 0;
    }
    function StartSessionTimer() {
        ticker++;
        if (ticker > sessionTimeOut) {
            window.location.href = "/Timesheet/Index";
            return;
        }
        setTimeout(function () { StartSessionTimer(); }, 1000);
    }
    StartSessionTimer();
});