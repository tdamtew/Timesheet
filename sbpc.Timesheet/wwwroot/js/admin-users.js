$(function () {
    init();

    $(document).on("click", "button[type=submit]", function () {
        $("button[type=submit]", $(this).parents("form")).removeAttr("clicked");
        $(this).attr("clicked", "true");
    });

    //Save employee
    $(document).on("submit", "form.employee", function (event) {
        event.preventDefault();
        var btn = $("button[type=submit][clicked=true]").attr("name");
        $("#wait").modal("show");
        if (btn === "save") {
            $.ajax({
                type: "post",
                url: '/Admin/SaveUser',
                data: $(this).serialize(),
                success: function (data, textStatus, xhr) {
                    $("#EmployeesWidget").html(data);
                    $("#wait").modal("hide");
                    $(".savenotify").notify("Employee has been added/updated successfully!", "success");
                    init();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $("#wait").modal("hide");
                    if (jqXhr.status === 403)
                        $(".savenotify").notify("There is an employee registered with the same first, middle and lastname.", "error");
                    else {
                        $(".savenotify").notify("Error occurred while adding/updating employee. Please try again later.", "error");
                    }
                }
            });
        }
        else {
            $.ajax({
                type: "post",
                url: '/Admin/ResetUserPassword',
                data: $("form.employee").serialize(),
                success: function (data, textStatus, xhr) {
                    $("#wait").modal("hide");
                    $(".savenotify").notify("Employee password has been reset successfully!", "success");
                    init();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $("#wait").modal("hide");
                    $(".savenotify").notify("Error occurred while resetting employee password. Please try again later.", "error");
                }
            });
        }

    });

    $(document).on("click", ".delete-user", function (event) {
        event.preventDefault();
        $("#userId").html($(this).data("user"));
        $("#confirmModal").modal("show");

    });

    $(document).on("click", ".edit-user", function (event) {
        event.preventDefault();
        $.get('/Admin/EditUser/?userId=' + $(this).data("user"), function (data) {
            $("#EmployeeWidget").html(data);
            InitValidator();
        });
    });

    //Delete employee
    $(document).on("click", "#confirmButton", function (event) {
        event.preventDefault();
        $("#confirmModal").modal("hide");
        $("#wait").modal("show");
        $.ajax({
            type: "post",
            url: '/Admin/DeleteUser',
            data: {
                userId: $("#userId").html(),
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data, textStatus, xhr) {
                $("#EmployeesWidget").html(data);
                $("#wait").modal("hide");
                $(".notify").notify("Employee has been removed successfully!", "success");
                init();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#wait").modal("hide");
                $(".notify").notify(jqXhr.status == 400 ? "System was unable to find the employee to remove." : "Error occurred while removing the employee. Please try again later.", "error");
            }
        });
    });

    function init() {
        $("#usersTable").DataTable();
        $("form.employee").find(":text, input[type='tel'], input[type='email']").val("");
        InitValidator();
    }

    function InitValidator() {
        $.validator.addMethod("alphabet",
            function (value, element) {
                return this.optional(element) || /^[a-zA-Z \'\-]+$/.test(value);
            });
        $.validator.addMethod("phone",
            function (value, element) {
                return this.optional(element) || /^(\+0?1\s)?\(?\d{3}\)?([\s-])?\d{3}([\s-])?\d{4}$/.test(value);
            });
        $("form.employee").validate({
            rules: {
                Email: {
                    required: true,
                    email: true
                },
                FirstName: {
                    required: true,
                    alphabet: true,
                    maxlength: 25
                },
                MiddleName: {
                    alphabet: true,
                    maxlength: 25
                },
                LastName: {
                    required: true,
                    alphabet: true,
                    maxlength: 25
                },
                PhoneNumber: {
                    phone: true
                }
            },
            messages: {
                Email: {
                    required: "please enter email address.",
                    date: "please enter a valid email address."
                },
                FirstName: {
                    required: "please enter first name.",
                    alphabet: "please enter a valid first name.",
                    maxlength: "First name can not be longer than 25 characters."
                },
                MiddleName: {
                    alphabet: "please enter a valid middle name.",
                    maxlength: "Middle name can not be longer than 25 characters."
                },
                LastName: {
                    required: "please enter last name.",
                    alphabet: "please enter a valid last name.",
                    maxlength: "Last name can not be longer than 25 characters."
                },
                PhoneNumber: {
                    phone: "Please provide a valid phone number."
                }
            }
        });
    }
});