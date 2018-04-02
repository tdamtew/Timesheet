$(function () {
    init();
    function init() {
        $("#jobsTable").DataTable();
        $("form.job").find(":text").val("");
        initValidator();
    }
    //Save jobs.
    $(document).on("submit", "form.job", function (event) {
        event.preventDefault();
        $("#confirmModal").modal("hide");
        $("#wait").modal("show");
        $.ajax({
            type: "post",
            url: '/Admin/SaveJob',
            data: $(this).serialize(),
            success: function (data, textStatus, xhr) {
                $("#JobsWidget").html(data);
                $("#wait").modal("hide");
                $("#submitJob").notify("Job has been added/updated successfully!", "success");
                init();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#wait").modal("hide");
                $("#submitJob").notify("Error occurred while adding/updating job. Please try again later.", "error");
            }
        });
    });

    $(document).on("click", ".delete-job", function (event) {
        event.preventDefault();
        $("#jobId").html($(this).data("name")).data("job", $(this).data("job"));
        $("#confirmModal").modal("show");

    });

    $(document).on("click", ".edit-job", function (event) {
        event.preventDefault();
        $.get('/Admin/EditJob/?jobId=' + $(this).data("job"), function (data) {
            $("#JobWidget").html(data);
            initValidator();
        });
    });

    //delete jobs.
    $(document).on("click", "#confirmButton", function (event) {
        event.preventDefault();
        $("#confirmModal").modal("hide");
        $("#wait").modal("show");
        $.ajax({
            type: "post",
            url: '/Admin/DeleteJob',
            data: {
                jobId: $("#jobId").data("job"),
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data, textStatus, xhr) {
                $("#JobsWidget").html(data);
                $("#wait").modal("hide");
                $(".notify").notify("Job has been removed successfully!", "success");
                init();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#wait").modal("hide");
                $(".notify").notify(jqXhr.status == 400 ? "System was unable to find the job to remove." : "Error occurred while removing the job. Please try again later.", "error");
            }
        });
    });

    function initValidator() {
        $.validator.addMethod("job",
            function (value, element) {
                return this.optional(element) || /^[a-zA-Z0-9(.:)(\-) ]+$/.test(value);
            });
        $("form.job").validate({
            rules: {
                Name: {
                    required: true,
                    job: true,
                    maxlength: 50
                },
                OverTimeRate: {
                    required: true,
                    number: true,
                    min: 1
                },
                CostPerMile: {
                    required: true,
                    number: true,
                    min: 0
                }
            },
            messages: {
                Name: {
                    required: "please enter job name.",
                    job: "job name can only contain letters, numbers and ':', '-'.",
                    maxlength: "job name can not be more than 50 characters."
                },
                OverTimeRate: {
                    required: "please provide overtime rate.",
                    number: "overtime rate can only be numbers.",
                    min: "overtime rate can not be less than 1."
                },
                CostPerMile: {
                    required: "please provide cost per mile.",
                    number: "cost per mile can only be numbers.",
                    min: "cost per mile can not be less than 0."
                }
            }
        });
    }
});
