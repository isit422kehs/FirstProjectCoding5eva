var uri = 'api/notes';
$(document).ready(function () {
    // Send an AJAX request
    showNotes();

});

function showNotes() {
    $('#notes').html('');
    $.getJSON(uri)
      .done(function (data) {
          // On success, 'data' contains a list of products.
          $.each(data, function (key, item) {
              // Add a list item for the product.
              $('<li>', { text: formatItem(item) }).appendTo($('#notes'));
          });
      });
}

function formatItem(item) {
    return item.Priority + '> ' + item.Subject + ': ' + item.Details;
}

function find() {
    var id = $('#noteId').val();
    $.getJSON(uri + '/' + id)
        .done(function (data) {
            $('#note').text(formatItem(data));
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#note').text('Error: ' + err);
        });
}

function add() {
    var subject = $('#subject').val();
    var details = $('#details').val();
    var priority = $('#priority').val();

    $.ajax({
        type: "POST",
        url: "api/save",
        data: {
            "Subject": subject,
            "Details": details,
            "Priority": priority
        },
        success: function () {
            $('#addNote').text("success");
            showNotes();
        },
        error: function (status) {
            $('#addNote').text(status);
        }
    });

}

function del() {

    var id = $('#delId').val();

    $.ajax({
        type: 'DELETE',
        url: 'api/delete/' + id,
        success: function () {
            $('#delNote').text('successfully deleted note ' + id);
            showNotes();
        },
        error: function (status) {
            $('#delNote').text(status);
        }
    });
}