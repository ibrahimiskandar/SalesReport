
$(function () {

    var $ctitems = $('#procedure-items');
    var $items = $('.item', $ctitems).hide();

    $('#procedure').change(function () {
        $items.hide()
        $('option:selected', this).each(function () {
            var id = $(this).attr('value');
            $('#' + id ).show()
        });
    });
});