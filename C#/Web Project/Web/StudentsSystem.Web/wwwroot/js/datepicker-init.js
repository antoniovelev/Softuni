$(function () {
    $("#datepicker").datepicker({
        format: "dd-mm-yyyy",
        startView: 2,
        language: "",
        theme: "dark"
    });
    $(function () {
        $("#second").datepicker({
            format: "dd-mm-yyyy",
            startView: 2,
            language: "",
            theme: "dark"
        });
    });
});

