var uri = 'api/notes';
$(document).ready(function () {
    // Send an AJAX request
    showNotes();

});

// from convert project ***********************************************************************************************************************************************

$(document).on('pagebeforeshow ', '#pageone', function () {   // see: https://stackoverflow.com/questions/14468659/jquery-mobile-document-ready-vs-page-events
    var info_view = "";      //string to put HTML in
    $('#notes').empty();  // since I do this everytime the page is redone, I need to remove existing before apending them all again

    $.each(data, function (index, record) {   // make up each li as an <a> to the details-page
        $('#notes').append('<li><a data-transition="pop" data-parm=' + record.Id + ' href="#details-page">' + record.Priority + ' => ' + record.Subject + '</a></li>');
    });

    $("#notes").listview('refresh');  // need this so jquery mobile will apply the styling to the newly added li's

    $("a").on("click", function (event) {    // set up an event, if user clicks any, it writes that items data-parm into the 
        //details page's html so I can get it there
        var parm = $(this).attr("data-parm");  // passing in the record.Id
        //do something here with parameter on  details page
        $("#detailParmHere").html(parm);
    });
});

$(document).on('pagebeforeshow', '#details-page', function () {
    var textString = 'fix me';
    var id = $('#detailParmHere').text();

    // to make newline space
    $.fn.multiline = function (text) {
        this.text(text);
        this.html(this.html().replace(/\n/g, '<br/>'));
        return this;
    }

    $.each(data, function (index, record) {
        if (id == record.Id) {
            textString = 'Priority: ' + record.Priority + '\n\nSubject: ' + record.Subject + '\n\nDetails: ' + record.Details;
        }
    });

    $('#showdata').multiline(textString);
});

$(document).on('pagebeforeshow', '#add-page', function () {

    // reset #addNote from previous success msg
    $('#addNote').text('');

});

function addNote() {

    var id = data.length + 1;
    var priority = $('#priority').val();
    var subject = $('#subject').val();
    var details = $('#details').val();

    newNote = {
        "Id": id,
        "Priority": Number(priority),
        "Subject": subject,
        "Details": details
    };

    data.push(newNote);

    $('#addNote').text('Successfully added new note assigned id #' + id);

    // sort the list w/ the added new note
    data.sort(function (a, b) {
        return a.Priority > b.Priority;
    });
    data.sort();

    // empty fields after submit
    $('#priority').val('');
    $('#subject').val('');
    $('#details').val('');

}

$(document).on('pagebeforeshow', '#delete-page', function () {
    $('#delNote').text('');
    showDelOptions();

    $('#delSubject').focus(function () {
        $('#delId').val('');
    });

    $('#delId').focus(function () {
        $('#delSubject').val('');
    });
});

function showDelOptions() {
    $('#delList').text('');
    $.each(data, function (index, record) {
        $('#delList').append('Id: ' + record.Id + ' / Subject: ' + record.Subject + '<br />');
    });
}

function delNote() {
    let sub = $('#delSubject').val();
    let delID = $('#delId').val();

    $.each(data, function (index, record) {

        if (sub == record.Subject || delID == record.Id) {
            $('#delNote').text('Your choice with the id of ' + record.Id + ' and subject of ' + record.Subject + ' has been deleted');
            data.splice(index, 1);
        }

        $('#delSubject').val('');
        $('#delId').val('');
        showDelOptions();

    });
}

// end code from convert project *************************************************************************************************************************************

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