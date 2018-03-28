$(function () {
    $("body").on("click keypress", function () {
        ResetSession();
    });
    var sessionTimeOut = 3000;
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
       setTimeout(StartSessionTimer(), 1000);
    }
    StartSessionTimer();
});