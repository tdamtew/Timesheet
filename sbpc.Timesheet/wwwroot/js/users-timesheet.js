$(function () {
    init();
    $(document).on("change", '#summaryDate', function (event) {
        event.preventDefault();
        val = Date.parse($(this).val());
        if (!isNaN(val)) {
            $("#wait").modal("show");
            $.get('/Timesheet/GetDate/?date=' + $(this).val() + "&employee=" + $("#userId :selected").text(), function (data) {
                $("#TimesheetWidget").html(data);
                $("#wait").modal("hide");
                init();
            });
        }
    });

    $(document).on("change", "#userId", function (event) {
        event.preventDefault();
        $("#wait").modal("show");
        $.get('/Timesheet/GetDate/?date=' + $("#summaryDate").val() + "&employee=" + $("#userId :selected").text(), function (data) {
            $("#TimesheetWidget").html(data);
            $("#wait").modal("hide");
            init();
        });
    });

    //handle crud on hours
    $(document).on("submit", "form.hour", function (event) {
        event.preventDefault();
        $("#wait").modal("show");
        $.post('/Timesheet/SaveHour?employee=' + $("#userId :selected").text(), $(this).serialize(), function (data) {
            $("#TimesheetWidget").html(data);
            $("#wait").modal("hide");
            $("#IsTravel").removeAttr("checked");
            $("#submitHour").notify("Add/Update hour successful!", "success");
            init();
        });
    });

    $(document).on("click", ".edit-hour", function (event) {
        event.preventDefault();
        $.get('/Timesheet/EditHour/?Id=' + $(this).data("hour"), function (data) {
            $("#HourWidget").html(data);
            $("#summaryDate,#hourDate,#expenseDate,#mileageDate").datepicker();
            initValidator();
        });
    });

    $(document).on("click", "#confirmButton", function (event) {
        event.preventDefault();
        var action = $(this).data("action");
        $("#wait").modal("show");
        if (action == "hour") {
            $.post('/Timesheet/DeleteHour', $(this).data("record"), function (data) {
                $("form.hour")[0].reset();
                $("#TimesheetWidget").html(data);
                $("#wait").modal("hide");
                $(".notify").notify("hour removed successfully!", "success");
                init();
            });
        }
        else if (action == "expense") {
            $.post('/Timesheet/DeleteExpense', $(this).data("record"), function (data) {
                $("#TimesheetWidget").html(data);
                $('.nav-tabs a[href="#expense"]').tab('show');
                $("#wait").modal("hide");
                $(".notify").notify("expense removed successfully!", "success");
                init();
            });
        }
        else {
            $.post('/Timesheet/DeleteMileage', $(this).data("record"), function (data) {
                $("#TimesheetWidget").html(data);
                $('.nav-tabs a[href="#mileage"]').tab('show');
                $("#wait").modal("hide");
                $(".notify").notify("mileage removed successfully!", "success");
                init();
            });
        }
        $("#confirmModal").modal("hide");
    });

    $(document).on("click", ".delete-hour", function (event) {
        event.preventDefault();
        $("#confirmButton").data("action", "hour").data("record", { Id: $(this).data("hour"), date: $(this).data("date"), employee: $("#userId :selected").text() });
        $("#confirmModal").modal("show");
    });

    //handle crud operation on expense
    $(document).on("submit", "form.expense", function (event) {
        event.preventDefault();
        $("#wait").modal("show");
        $.post('/Timesheet/SaveExpense?employee=' + $("#userId :selected").text(), $(this).serialize(), function (data) {
            $("#TimesheetWidget").html(data);
            $('.nav-tabs a[href="#expense"]').tab('show');
            $("#wait").modal("hide");
            $("#submitExpense").notify("Add/Update expense successful!", "success");
            init();
        });
    });

    $(document).on("click", ".edit-expense", function (event) {
        event.preventDefault();
        $.get('/Timesheet/EditExpense/?Id=' + $(this).data("expense"), function (data) {
            $("#ExpenseWidget").html(data);
            $("#summaryDate,#hourDate,#expenseDate,#mileageDate").datepicker();
            initValidator();
        });
    });

    $(document).on("click", ".delete-expense", function (event) {
        event.preventDefault();
        $("#confirmButton").data("action", "expense").data("record", { Id: $(this).data("expense"), date: $(this).data("date"), employee: $("#userId :selected").text() });
        $("#confirmModal").modal("show");
    });

    //handle crud operations on mileage
    $(document).on("submit", "form.mileage", function (event) {
        event.preventDefault();
        $("#wait").modal("show");
        $.post('/Timesheet/SaveMileage?employee=' + $("#userId :selected").text(), $(this).serialize(), function (data) {
            $("#TimesheetWidget").html(data);
            $('.nav-tabs a[href="#mileage"]').tab('show');
            $("#wait").modal("hide");
            $("#submitMileage").notify("Add/Update mileage successful!", "success");
            init();
        });
    });

    $(document).on("click", ".edit-mileage", function (event) {
        event.preventDefault();
        $.get('/Timesheet/EditMileage/?Id=' + $(this).data("mileage"), function (data) {
            $("#MileageWidget").html(data);
            $("#summaryDate,#hourDate,#expenseDate,#mileageDate").datepicker();
            initValidator();
        });
    });

    $(document).on("click", ".delete-mileage", function (event) {
        event.preventDefault();
        $("#confirmButton").data("action", "mileage").data("record", { Id: $(this).data("mileage"), date: $(this).data("date"), employee: $("#userId :selected").text() });
        $("#confirmModal").modal("show");
    });

    function init() {
        $("form.hour").find(":text,input[type='number']").each(function () {
            if ($(this).attr("Id").indexOf("Date") < 0) {
                $(this).val("");
            }
        });
        $("form.hour").find("input[name='Id']").val(0);
        $("form.expense").find(":text,input[type='number']").each(function () {
            if ($(this).attr("Id").indexOf("Date") < 0) {
                $(this).val("");
            }
        });
        $("form.expense").find("input[name='Id']").val(0);
        $("form.mileage").find(":text,input[type='number']").each(function () {
            if ($(this).attr("Id").indexOf("Date") < 0) {
                $(this).val("");
            }
        });
        $("form.mileage").find("input[name='Id']").val(0);
        $("#summaryDate,#hourDate,#expenseDate,#mileageDate").datepicker();
        $(".table").DataTable({
            "order": []
        });
        $('[data-toggle="tooltip"]').tooltip();
        initValidator();
    }

    function initValidator() {
        //add custom validator
        jQuery.validator.addMethod("alphanumeric",
            function (value, element) {
                return this.optional(element) || /^[a-zA-Z0-9 ,'-\\_;/()\$@]+$/.test(value);
            });
        //hour validation.
        $("form.hour").validate({
            rules: {
                Date: {
                    required: true,
                    date: true
                },
                JobName: {
                    required: true
                },
                Hours: {
                    required: true,
                    number: true,
                    min: 0,
                    max: 24
                },
                Note: {
                    alphanumeric: true,
                    maxlength: 100
                }
            },
            messages: {
                Date: {
                    required: "please enter date.",
                    date: "please enter a valid date."
                },
                JobName: "please select the job.",
                Hours: {
                    required: "please enter your hours.",
                    number: "hours can only be numbers.",
                    min: "minimum number of hours is 0.",
                    max: "maximum number of hours is 24."
                },
                Note: {
                    alphanumeric: "Note can not contain special characters, %,#,!,*,^,~.",
                    maxlength: "Note can not be more than 100 characters long."
                }
            }
        });

        //expense validation
        $("form.expense").validate({
            rules: {
                Date: {
                    required: true,
                    date: true
                },
                JobName: {
                    required: true
                },
                Category: {
                    required: true
                },
                Method: {
                    required: true
                },
                Amount: {
                    required: true,
                    number: true,
                    min: 1.0,
                    max: 9999999.99
                },
                Note: {
                    alphanumeric: true,
                    maxlength: 100
                }
            },
            messages: {
                Date: {
                    required: "please enter date.",
                    date: "please enter a valid date."
                },
                JobName: "please select the job.",
                Category: "please select the expense category.",
                Method: "please select the expense method.",
                Amount: {
                    required: "please enter your expense amount.",
                    number: "Expenses can only be numbers.",
                    min: "minimum expense amount is $1.",
                    max: "maximum expense amount is $9,999,999.99."
                },
                Note: {
                    alphanumeric: "Note can not contain special characters, %,#,!,*,^,~.",
                    maxlength: "Note can not be more than 100 characters long."
                }
            }
        });

        //mileage validation
        $("form.mileage").validate({
            rules: {
                Date: {
                    required: true,
                    date: true
                },
                JobName: {
                    required: true
                },
                mile: {
                    required: true,
                    number: true,
                    min: 1,
                },
                Note: {
                    alphanumeric: true,
                    maxlength: 100
                }
            },
            messages: {
                Date: {
                    required: "please enter date.",
                    date: "please enter a valid date."
                },
                JobName: "please select the job.",
                mile: {
                    required: "please enter your mileage.",
                    number: "mileage can only be numbers.",
                    min: "minimum number of miles is 1."
                },
                Note: {
                    alphanumeric: "Note can not contain special characters, %,#,!,*,^,~.",
                    maxlength: "Note can not be more than 100 characters long."
                }
            }
        });
    }
});