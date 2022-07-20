let $cell = $('.f-books-card');

//open and close f-books-card when clicked on f-books-card
$cell.find('.js-expander').click(function () {
    let $thisCell = $(this).closest('.f-books-card');

    if ($thisCell.hasClass('is-collapsed')) {
        $cell.not($thisCell).removeClass('is-expanded').addClass('is-collapsed').addClass('is-inactive');
        $thisCell.removeClass('is-collapsed').addClass('is-expanded');

        if ($cell.not($thisCell).hasClass('is-inactive')) {
            //do nothing
        } else {
            $cell.not($thisCell).addClass('is-inactive');
        }
    } else {
        $thisCell.removeClass('is-expanded').addClass('is-collapsed');
        $cell.not($thisCell).removeClass('is-inactive');
    }
});

//close f-books-card when click on cross
$cell.find('.js-collapser').click(function () {
    let $thisCell = $(this).closest('.f-books-card');

    $thisCell.removeClass('is-expanded').addClass('is-collapsed');
    $cell.not($thisCell).removeClass('is-inactive');
});