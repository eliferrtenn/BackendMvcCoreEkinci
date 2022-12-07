(function () {
    var PlaceHolderElement = $('PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function () {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find(".modal").modal('show');
        });

    })
 })
