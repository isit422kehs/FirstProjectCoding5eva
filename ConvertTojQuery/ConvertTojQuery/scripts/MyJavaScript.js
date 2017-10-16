var uri = 'api/notes';
var data = [{
    "Id": 1,
    "Priority": 1,
    "Subject": "Pay electric bill",
    "Details": "Electric bill $250"
}, {
    "Id": 2,
    "Priority": 3,
    "Subject": "Pay Macy's",
    "Details": "Macy's bill $200"
}, {
    "Id": 3,
    "Priority": 5,
    "Subject": "Pay comcast bill",
    "Details": "Comcast bill $100"
}, {
    "Id": 4,
    "Priority": 2,
    "Subject": "Buy groceries",
    "Details": "Eggs, apples, milk",
}, {
    "Id": 5,
    "Priority": 4,
    "Subject": "Do homework",
    "Details": "Finish homework on time to avoid the wrath of Kurt"
}, {
    "Id": 6,
    "Priority": 3,
    "Subject": "Wash jeep",
    "Details": "Scrub everywhere to make the car shiny"
}, {
    "Id": 7,
    "Priority": 5,
    "Subject": "Add auto refresh",
    "Details": "Refresh"
}]

data.sort(function (a, b) {
    return a.Priority > b.Priority;
});

data.sort();

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

    $('#delSubject').focus(function(){
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