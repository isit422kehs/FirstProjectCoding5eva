var uri = 'api/notes';
var data = [{
    "Id": 1,
    "Priority": 1,
    "Subject": "Pay electric bill",
    "Details": "Electric bill $250"
}, {
    "Id": 2,
    "Priority": 1,
    "Subject": "Pay Macy's",
    "Details": "Macy's bill $200"
}, {
    "Id": 3,
    "Priority": 2,
    "Subject": "Pay comcast bill",
    "Details": "Comcast bill $100"
}, {
    "Id": 4,
    "Priority": 2,
    "Subject": "Buy groceries",
    "Details": "Eggs, apples, milk",
}, {
    "Id": 5,
    "Priority": 2,
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

    $.each(data, function (index, record) {
        if (id == record.Id) {
            textString = "Priority:  " + record.Priority + " Subject:  " + record.Subject + " Details:  " + record.Details;
        }
    });

    $('#showdata').text(textString);
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

    //$('#addNote').text('Successfully added new note #' + id);

    data.push(newNote);

    $('#priority').val('');
    $('#subject').val('');
    $('#details').val('');

}