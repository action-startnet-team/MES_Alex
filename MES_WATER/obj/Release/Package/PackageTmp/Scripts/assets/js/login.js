let inputDiv_index;
$(".input").focus(function () {
    inputDiv_index = $(this).parent().parent().index();
    $(".input_div").eq(inputDiv_index - 1).addClass('focus');
});

$(".input").blur(function () {
    inputDiv_index = $(this).parent().parent().index();
    $(".input_div").eq(inputDiv_index - 1).removeClass('focus');
});