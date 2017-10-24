var uri = 'api/notes';
let parm = '';

$(document).on('click', 'a', function () {
    parm = $(this).attr("data-parm");
    $("#detailParmHere").html(parm);
    
});


$(document).on('pagebeforeshow ', '#pageone', function () {   /* see: https://stackoverflow.com/questions/14468659/jquery-mobile-document-ready-vs-page-events */
    var info_view = "";      //string to put HTML in
    
    showNotes();
    $("#notes").listview('refresh');  // need this so jquery mobile will apply the styling to the newly added li's

    $("a").on("click", function (event) {    // set up an event, if user clicks any, it writes that items data-parm into the 
        //details page's html so I can get it there
        //var parm = $(this).attr("data-parm");  // passing in the record.Id
        
        //do something here with parameter on  details page
        $("#detailParmHere").html(parm);
        
    });
});

$(document).on('pagebeforeshow pageshow', '#details-page', function () {
    var id = $('#detailParmHere').text();
    let textString = 'fix me';
    // to make newline space
    $.fn.multiline = function (text) {
        this.text(text);
        this.html(this.html().replace(/\n/g, '<br/>'));
        return this;
    }
    
    $.getJSON(uri + '/' + id)
        .done(function (data) {
            textString = 'Priority: ' + data.Priority + '\n\nSubject: ' + data.Subject + '\n\nDetails: ' + data.Details;
            $('#showdata').multiline(textString);
        })
        .fail(function (jqXHR, textStatus, err) {
            textString = 'Error: ' + err;
            $('#showdata').multiline(textString);
        });
    
    //$('#showdata').multiline(textString);
    
});

$(document).on('pagebeforeshow', '#add-page', function () {

    // reset #addNote from previous success msg
    $('#addNote').text('');

});

$(document).on('pagebeforeshow', '#delete-page', function () {
    $('#delNote').text('');
    showDelOptions();

});

function showNotes() {
    $('#notes').empty();
    $.getJSON(uri)
      .done(function (data) {

          $.each(data, function (key, record) {
              $('#notes').append('<li><a data-transition="pop" data-parm=' + record.Subject + ' href="#details-page" class="ui-btn ui-btn-icon-right ui-icon-carat-r">' + record.Priority + ' => ' + record.Subject + '</a></li>');
          });
      });
}

function addNote() {

    var subject = $('#subject').val();
    var details = $('#details').val();
    var priority = $('#priority').val();

    $.ajax({
        type: "POST",
        url: uri,
        data: {
            "Subject": subject,
            "Details": details,
            "Priority": priority
        },
        success: function () {
            $('#addNote').text('Successfully added new note about ' + subject);
            //showNotes();
        },
        error: function (status) {
            $('#addNote').text(status);
        }
    });

    // empty fields after submit
    $('#priority').val('');
    $('#subject').val('');
    $('#details').val('');

}

function showDelOptions() {
    $('#delList').text('');

    $.getJSON(uri)
      .done(function (data) {
          $.each(data, function (key, record) {
              $('#delList').append('Id: ' + record.Id + ' / Subject: ' + record.Subject + '<br />');
          });
      });
}

function delNote() {
    
    let delID = $('#delId').val().trim();

    $.ajax({
        type: 'DELETE',
        url: uri + "/" + delID,
        success: function () {
            $('#delNote').text('successfully deleted note with id of: ' + delID);
            
            showDelOptions();
        },
        error: function (status) {
            $('#delNote').text(status);
        }
    });

    $('#delId').val('');
}

/*
function formatItem(item) {
    return item.Priority + '> ' + item.Subject + ': ' + item.Details;
}
*/

