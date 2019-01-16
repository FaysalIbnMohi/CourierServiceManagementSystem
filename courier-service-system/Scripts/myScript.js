$(function () { 
    $("#Bdatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '1980:2000'
    });

    $("#Jdatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '-10:+1'
    });
    $("#Tdatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        //dateRange: '0',
        yearRange: '0:+1',
        minDate: -20,
        maxDate: "+1M +10D"
    });

    $('#btn').click(function () {
        $.ajax({
            url: '/Home/Login',
            data: {
                username: $('#username').val(),
                password: $('#password').val()
            },
            success: function (result) {
                $('#msg').html(result);
            }
        });
    });
});

