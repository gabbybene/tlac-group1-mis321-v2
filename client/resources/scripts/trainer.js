/* Scripts relating to trainer.js, including:
    -loading trainer's confirmed appointments and related modal
    -calendar to edit trainer availability and related modal
    -edit trainer profile form
are included in this JS file.
*/


//get dates for use later
var today = new Date();
var currentMonth = today.getMonth();
var currentYear = today.getFullYear();

//Check https://github.com/niinpatel/calendarHTML-Javascript/blob/master/scripts.js for a possible JS calendar
function handleTrainerDashboardOnLoad(){ //load each part of dashboard
    let id = getTrainerId();
    let trainer = [];
    const trainerApiUrl = "https://localhost:5001/api/Trainer/GetTrainerByID/"+id;
    fetch(trainerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        trainer = json;
        getConfirmedAppointments(trainer);
        getTrainerCalendar(currentMonth, currentYear);
        getTrainerProfileForm(trainer);

    }).catch(function(error){
        console.log(error);
    }) 
}
//     getTrainerAppointments();

//     getTrainerCalendar(currentMonth, currentYear);
    
//     getTrainerProfileForm();
// }

function getTrainerId(){
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get("id");
    return id;
}

function getConfirmedAppointments(trainer){
    //get any trainer appointments that have "customer" !=null and trainerID matches trainerID
    let html = "";
    const apptApiUrl = "https://localhost:5001/api/Appointment/GetConfirmedAppointmentsForTrainer/"+trainer.trainerId;
    fetch(apptApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        if(json[0] == undefined){
            console.log("no appointments found");
             //will return the empty []
            html += "<h2>You don't have any upcoming appointments scheduled at this time.</h2>";
        }
        else{
            //if there are appointments found w/ that customerId
            for(var i in json){
                //stringify, then re-parse to an object **couldn't get the start and end times to read without doing this**
                var tempStr = JSON.stringify(json[i]);
                var object = JSON.parse(tempStr);
                //format date and time
                let apptDate = getFormattedDate(object.appointmentDate);
                let startTime = getFormattedTime(object.startTime.hours, object.startTime.minutes);
                let endTime = getFormattedTime(object.endTime.hours, object.endTime.minutes);
                let activity =  object.appointmentTrainer.trainerActivities[0].activityName;
                let customerName = object.appointmentCustomer.fName+ " " + object.appointmentCustomer.lName;
                //print buttons with appt details
                html += "<button type=\"button btn\" class=\"list-group-item list-group-item-action\" onclick=\"showEditTrainerApptModal("+object.appointmentId+")\">";
                html += apptDate + " at " + startTime + "-" + endTime + " | Activity: " + activity + " | Customer: " + customerName +"</button>";
            }
        }
        //set the innerHTML of custApptList
        document.getElementById("trainerApptList").innerHTML = html;
    }).catch(function(error){
        console.log(error);
    }) 
}

function getFormattedDate(date){
    //take a date from json and convert to mm/dd/yyyy
    let myDate = date.slice(0,10); //get first 10 characters of json string
    let splitDate = myDate.split('-'); //split at -
    let newMonth = parseInt(splitDate[1]);
    newMonth = +newMonth; //remove leading zero if exists
    let newDay = parseInt(splitDate[2]);
    newDay = +newDay; //remove leading zero if exists
    let newDate = [newMonth, newDay, splitDate[0]]; //make new date with correct order
    newDate = newDate.join("/"); //join back together with /
    return newDate;
}

function getFormattedTime(hours, minutes){
    let formattedHours = "";
    let formattedMinutes = "";
    let suffix = "AM";
    if(hours >= 12){
        suffix = "PM";
        if(hours % 12 != 0){
            hours = hours % 12;
        }
    }
    formattedHours = hours.toString();
    if(minutes < 10){
        formattedMinutes = minutes.toString();
        if(formattedMinutes == "0"){
            formattedMinutes = "00";
        }
        else {
            formattedMinutes = "0" + minutes;
        }
    }
    else {
        formattedMinutes = minutes.toString();
    }
    return (formattedHours + ":" + formattedMinutes + suffix);
}
      

/* EDIT TRAINER APPOINTMENT MODAL SCRIPTS */
function showEditTrainerApptModal(apptID){
    //get appointment by ID, then populate and show the modal
        //Make API call to get appointment with the passed-in ID from the database
        const apptApiUrl = "https://localhost:5001/api/Appointment/GetAppointmentByID/"+apptID;
        fetch(apptApiUrl).then(function(response){
            console.log(response);
            return response.json();
        }).then(function(json){
            //stringify, then re-parse to an object **couldn't get the start and end times to read without doing this**
            var tempStr = JSON.stringify(json);
            var object = JSON.parse(tempStr);
            //simplify object format with only the needed items
            var appt = {
                apptDate: getFormattedDate(object.appointmentDate),
                startTime: getFormattedTime(object.startTime.hours, object.startTime.minutes),
                endTime: getFormattedTime(object.endTime.hours, object.endTime.minutes),
                activity:  object.appointmentTrainer.trainerActivities[0].activityName,
                customerName: object.appointmentCustomer.fName+ " " + object.appointmentCustomer.lName,
                price: object.appointmentCost,
            }
            
            //set up HTML 
            let html = "<div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditCustApptModal()\">&times;</span>";
            html += "<h1 class=\"modal-header text-center\">Appoitment Details</h1><br>";
            html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Date/Time:</h3></div><div class=\"col-md-8\"><h3>";
            html += appt.apptDate + " at " + appt.startTime + " - " +  appt.endTime  +"</h3></div></div>";
            html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Customer:</h3></div><div class=\"col-md-8\"><h3>"+ appt.customerName+"</h3></div></div>";
            html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Activity:</h3></div><div class=\"col-md-8\"><h3>"+ appt.activity+"</h3></div></div>";
            html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Price:</h3></div><div class=\"col-md-8\"></h3>$"+appt.price+"</h3></div></div>";
            html += "<br><div class=\"row text-center\"><button class=\"btn btn-action btn-warning\" id=\"cancelApptButton\" onclick=\"cancelTrainerAppt("+apptID+")\">Cancel This Appointment</button></div></div>";
    
            //set inner HTML of modal
            document.getElementById("editTrainerApptModal").innerHTML = html;
    
            //Show Modal
            var modal = document.getElementById("editTrainerApptModal");
            modal.style.display = "block";
            var span = document.getElementsByClassName("close")[0];
        }).catch(function(error){
            console.log(error);
        })


    // var modal = document.getElementById("editTrainerApptModal");
    // modal.style.display = "block";

    // var span = document.getElementsByClassName("close")[0];

        
    // let html = "<div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditTrainerApptModal()\">&times;</span>";
    // html += "<h1 class=\"modal-header text-center\">Appoitment Details</h1><br>";
    // html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Date/Time:</h3></div><div class=\"col-md-8\"><h3>";
    // html += appointment.date + " at " + appointment.startTime + "-" +  appointment.endTime  +"</h3></div></div>";
    // html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Customer:</h3></div><div class=\"col-md-8\"><h3>"+appointment.customer+"</h3></div></div>";
    // html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Activity:</h3></div><div class=\"col-md-8\"><h3>"+appointment.activity+"</h3></div></div>";
    // html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Price:</h3></div><div class=\"col-md-8\"></h3>$"+appointment.price+"</h3></div></div>";
    // html += "<br><div class=\"row text-center\"><button class=\"btn btn-action btn-warning\" id=\"cancelApptButton\" onclick=\"cancelTrainerAppt("+appointment.apptID+")\">Cancel This Appointment</button></div>";

    // document.getElementById("editTrainerApptModal").innerHTML = html;
}

function closeEditTrainerApptModal(){
    var modal = document.getElementById("editTrainerApptModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("editTrainerApptModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}

function cancelTrainerAppt(apptID) {
    //tell user the appt is canceled with a close button
    const cancelApptApiUrl = "https://localhost:5001/api/Appointment/"+apptID;
    //make an int[] to send into the put request. [0]=custID, [1]=apptID
    fetch(cancelApptApiUrl, {
        method: "DELETE",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json'
        },
        body: JSON.stringify({
            appointmentId: apptID
        })
    })
    .then(function(response){
    //when they cancel the appointment, change the modal html to show that the apointment was canceled, and change button to close.
    let html = " <div class=\"modal-content\"><span class=\"close\" onclick=\"apptCanceledCloseModal()\">&times;</span>";
    html += "<h1 class=\"modal-header text-center\">Appoitment Canceled</h1><br>";
    html += "<div class=\"row text-center\"><h2>This appointment has been canceled.</h2></div>"; 
    html += "<div class=\"row text-center\"><h2>Click the button to return to your dashboard to add more appointment availability.</h2></div>";
    html += "<br><div class=\"row text-center\"><button class=\"btn btn-action\" onclick=\"apptCanceledCloseModal()\">Close</button></div>";
    document.getElementById("editTrainerApptModal").innerHTML = html;
    })
}

function apptCanceledCloseModal(){
    //reload the confirmedAppointments and TrainerCalendar sections to reflect the update
    let id = getTrainerId();
    let trainer = [];
    const trainerApiUrl = "https://localhost:5001/api/Trainer/GetTrainerByID/"+id;
    fetch(trainerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        trainer = json;
        getConfirmedAppointments(trainer);
        getTrainerCalendar(currentMonth, currentYear);
        //then close the modal
        var modal = document.getElementById("editTrainerApptModal");
        modal.style.display = "none";

    }).catch(function(error){
        console.log(error);
    }) 
}


/* FOR CALENDAR / TRAINER AVAILABILITY SCRIPTS 
calendar adapted from https://github.com/niinpatel/calendarHTML-Javascript */
function getTrainerCalendar(currentMonth, currentYear){
    /* TO-DO:   
        -get appointments with no customer id from DB */
     
    setCalendarHeader(currentMonth, currentYear); //displays current/updated Month YYYY at top of calendar

    //get month and year for use below
    let mon = currentMonth;
    let d = new Date(currentYear, mon);
    let today = new Date();
    let todayDate = today.getDate();
    //CREATE CALENDAR TABLE
    let calendarTable = "<table class=\"table table-responsive-sm calendar-table\"><thead>";
    calendarTable += "<th>Sun</th><th>Mon</th><th>Tue</th><th>Wed</th><th>Thur</th><th>Fri</th><th>Sat</th></thead><tbody id=\"calendarBody\">";
    // spaces for the first row from Sunday until the first day of the month
    for(let i = 0; i < d.getDay(); i++) {
        calendarTable += "<td></td>";
    }
    //<td> with dates for that month
    while(d.getMonth() == mon) {
        if(d.getDate() < todayDate) {
            //if date is before today's date, disable button
            calendarTable += "<td><button disabled style=\"background-color: #808080\">" + d.getDate() + "</button></td>"; 
            if(d.getDay() == 6) { //if last day of week, new table row
                calendarTable += "</tr><tr>";
            }
        }
        else {
            //if today or later, enable button
            calendarTable += "<td><button onclick=\"showEditAvailabilityModal(value)\" value="+ (d.getMonth()+1) + "/" + d.getDate() + "/" + d.getFullYear() +">" + d.getDate() + "</button></td>";
            if(d.getDay() == 6) { //if last day of week, new table row
                calendarTable += "</tr><tr>";
            }
        }
        d.setDate(d.getDate() + 1); //increment date
    }
    //add spaces after last days of month for last row if last day of the month is not Saturday
    if(d.getDay() != 6) {
        for(let i = d.getDay(); i < 7; i++) {
            calendarTable += "<td></td>";
        }
    }
    //close table
    calendarTable += "<tr></tbody></table>";
    document.getElementById("calendar").innerHTML = calendarTable;
}

function setCalendarHeader(currentMonth, currentYear)
{
    var currDate = new Date(); //current date
    var mList = [ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ];
    var monthName = mList[currentMonth];
    var monthAndYear = monthName + " " + currentYear;
    document.getElementById("currMonth").innerHTML = monthAndYear;
}

function nextMonth() {
    if(currentMonth === 11){ //if at December
        //go to the next year & adjust the month to 0
        currentYear++;
        currentMonth = (currentMonth + 1) % 12;
    }
    else { //if not at December
        currentMonth++;
    }
    //reload calendar & header 
    setCalendarHeader(currentMonth, currentYear);
    getTrainerCalendar(currentMonth, currentYear);
}

function previousMonth() {
    if(currentMonth === 0){ //if at January
        //go to the previous year & to month 11
        currentYear--;
        currentMonth = 11;
    }
    else{ //if not at January
        currentMonth--;
    }
    //reload calendar & header
    setCalendarHeader(currentMonth, currentYear);
    getTrainerCalendar(currentMonth, currentYear);
}

function showEditAvailabilityModal(value){
    // when trainer clicks on a calendar date, pop up that date with any existing availability (unconfirmed appointments - or distinguish between confirmed and available?) they have.
    // Populate a form with maybe 4 slots for start time, activity (radio buttons)
    // include an "add" button, that user could click + to add more input fields
    // At the bottom, include an "update availability" button to add those open appointments to the database
    var modal = document.getElementById("trainerEditAvailabilityModal");
    modal.style.display = "block";
    var span = document.getElementsByClassName("close")[0];

    let selectedDate = value; //selectedDate is MM/DD/YYYY format
    let html = "";
    html+="<div class=\"modal-dialog\"><div class=\"modal-content\">";
    html += "<span class=\"close\" onclick=\"closeEditAvailabilityModal()\">&times;</span><h1 class=\"modal-header text-center\">Edit Availability for "+selectedDate+"</h1>";
    //start with 5 input rows
    let idCount = 0;
    for(let i = 0; i < 5; i++){
        html += "<div class=\"row\"><div class=\"col-md-4\"><label for=startTime"+idCount+">Choose a start time:</label><input type=\"time\" id=startTime"+idCount+" name=\"startTime\"min=\"06:00\" max=\"18:00\"></div>";
        html += "<div class=\"col-md-4\"><label for=startTime"+idCount+">Choose an end time:</label><input type=\"time\" id=startTime"+idCount+" name=\"endTime\"min=\"06:00\" max=\"18:00\" ></div>";
        html += "<div class=\"col-md-4\"><label for=activity"+idCount+">Choose an activity:</label><select id=activity"+idCount+" name=\"activities\"><option value=\"cardio\">Cardio</option><option value=\"strengthTraining\">Strength Training</option>";
        html += "<option value=\"kickboxing\">Kickboxing</option><option value=\"yoga\">Yoga</option></select></div></div>";
        idCount++;
    }
    //empty div to add additional rows of input fields
    html += "<div id=\"extraRows\"></div>"
    //ADD A BUTTON TO ADD MORE INPUT FIELDS
    html += "<hr><div class=\"row text-center\"><button class=\"btn btn-lg btn-default\" onclick=\"addMoreRows("+idCount+")\">Add More Availability +</button></div><hr>";
    html += "<div class=\"row text-center\"><button class=\"btn btn-lg btn-success\" onclick=\"addAvailability()\">Add Appointment Availability</button></div>";
    html += "</div></div>"; //closes modal-content and modal-dialog divs
    document.getElementById("trainerEditAvailabilityModal").innerHTML = html;
}

function addMoreRows(idCount){
    //adds 5 more rows of data for user to input
    let html = "";
    for(let i=0; i<5; i++){
        html += "<div class=\"row\"><div class=\"col-md-4\"><label for=startTime"+idCount+">Choose a start time:</label><input type=\"time\" id=startTime"+idCount+" name=\"startTime\"min=\"06:00\" max=\"18:00\"></div>";
        html += "<div class=\"col-md-4\"><label for=startTime"+idCount+">Choose an end time:</label><input type=\"time\" id=startTime"+idCount+" name=\"endTime\"min=\"06:00\" max=\"18:00\" ></div>";
        html += "<div class=\"col-md-4\"><label for=activity"+idCount+">Choose an activity:</label><select id=activity"+idCount+" name=\"activities\"><option value=\"cardio\">Cardio</option><option value=\"strengthTraining\">Strength Training</option>";
        html += "<option value=\"kickboxing\">Kickboxing</option><option value=\"yoga\">Yoga</option></select></div></div>";
        idCount++;
    }
    document.getElementById("extraRows").innerHTML = html;
}

function addAvailability(){
    //probably have to pass in a list of the selected appointments to add to DB
    
    //for now, close the modal
    closeEditAvailabilityModal();
}

function closeEditAvailabilityModal(){
    var modal = document.getElementById("trainerEditAvailabilityModal");
    modal.style.display = "none";
}

/* EDIT TRAINER PROFILE SECTION */
function getTrainerProfileForm(trainer){
    //do we have to pass in an id?


    //fill in everything but the password. Trainer should be required to enter their current password to make changes.
    document.getElementById("currEmail").value = trainer.emailAddress;
    document.getElementById("inputFName").value = trainer.firstName;
    document.getElementById("inputLName").value = trainer.lastName;
    document.getElementById("birthDate").value = trainer.birthDate;
    document.getElementById("gender").value = trainer.gender;
 
    for(var i in trainer.activities){ //update checked status of activities
        if(trainer.activities[i].activityName == "cardio"){
            document.getElementById("cardio").checked = true;
            document.getElementById("cardioPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("cardioPrice").disabled = false;        
        }
        else if(trainer.activities[i].activityName == "strength training"){
            document.getElementById("strengthTraining").checked = true;
            document.getElementById("strengthTrainingPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("strengthTrainingPrice").disabled = false;
        }
        else if(trainer.activities[i].activityName == "kickboxing"){
            document.getElementById("kickboxing").checked = true;
            document.getElementById("kickboxingPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("kickboxingPrice").disabled = false;
        }
        else if(trainer.activities[i].activityName == "yoga"){
            document.getElementById("yoga").checked = true;
            document.getElementById("yogaPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("yogaPrice").disabled = false;
        }
    }
}