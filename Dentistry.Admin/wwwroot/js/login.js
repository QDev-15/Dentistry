document.addEventListener("DOMContentLoaded", function () {
    const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    $("#login-timezone").val(timeZone);
});