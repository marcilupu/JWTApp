$('#loginButton').on('click', function () {
    $.ajax({
        url: 'https://localhost:7186/User/Login',
        success: function (data) {
            window.location = 'https://localhost:7186/User/Login'
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.statusText); alert(thrownError);
        }
    });
});
