// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// JavaScript Document
$(document).ready(function () {
    $(".view").click(function () {
        $("body").css("background-color", "#fff");
        $(".color").css("color", "#555");
        $(this).addClass("fas fa-sun");
        $(this).removeClass("far fa-moon");
    });

});		