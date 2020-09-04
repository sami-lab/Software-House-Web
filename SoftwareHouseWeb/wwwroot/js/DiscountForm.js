
$("form.form").submit(function () {

    $.ajax({
        type: "POST",
        url: "/Promo/GetPromoCode",
        data: $("form.form").serialize(),
        success: function (data) {

            $("form.form button[type='submit']").css("disabled", true)
            $("#myModal_").modal('show');
        },
        failure: function (response) {
            $("#myModalFail_").modal('show');
        },
        error: function (response) {
            console.log(response.responseText);
            $("#myModalFail_").modal('show');
        }
    })
});