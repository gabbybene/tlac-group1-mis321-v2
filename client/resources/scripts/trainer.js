/* Scripts relating to trainer.js, including:
    -loading trainer's confirmed appointments and related modal
    -calendar to edit trainer availability and related modal
    -edit trainer profile form
are included in this JS file.
*/

//TEMPORARY: a list of trainer's appointments, some confirmed, some not


//get dates for use later
var today = new Date();
var currentMonth = today.getMonth();
var currentYear = today.getFullYear();

//Check https://github.com/niinpatel/calendarHTML-Javascript/blob/master/scripts.js for a possible JS calendar
function handleTrainerDashboardOnLoad(){ //load each part of dashboard
    getTrainerAppointments();

    getTrainerCalendar(currentMonth, currentYear);
    
    getTrainerProfileForm();
}

function getTrainerAppointments(){
    //get any trainer appointments that have "customer" !=null and trainerID matches trainerID
    let confirmedAppts = getConfirmedAppointments();
    let html = "";
    if(confirmedAppts.length == 0) {
        //set up some html to say they have no confirmed appointments at this time 
    }
    else {
        for(let i = 0; i < confirmedAppts.length; i++) {
            //create buttons with the appointment information, onclick = cancelApptOnClick()
            html += "<button type=\"button btn\" class=\"list-group-item list-group-item-action\" onclick=\"showEditTrainerApptModal("+confirmedAppts[i].apptID+")\">";
            html += confirmedAppts[i].date + " at " + confirmedAppts[i].startTime + " | Activity: " + confirmedAppts[i].activity + " | Customer: " + confirmedAppts[i].customer + "</button>";
        }
    }
    document.getElementById("trainerApptList").innerHTML = html;
}

function getConfirmedAppointments(){
    //TEMPORARY: static appointments for testing
    let testAppointments = [
        {
            apptID: 1,
            "date": "4/6/2021",
            "startTime" : "10:00am",
            "endTime" : "11:00am",
            "activity": "Cardio",
            "customer": "none",
            "price": 50.00
        },
        {
            apptID: 2,
            "date": "4/6/2021",
            "startTime": "12:00pm",
            "activity": "Strength Training",
            "customer": "Callie Jones",
            "price": 75.00
        },
        {
            apptID: 3,
            "date": "4/8/2021",
            "startTime": "4:00pm",
            "activity": "Cardio",
            "customer": "none",
            "price": 50.00
        },
        {
            apptID: 4,
            "date": "4/10/2021",
            "startTime": "6:00pm",
            "activity": "Kickboxing",
            "customer": "Princess Smith",
            "price": 60.00
        }
    ];

    var confirmedAppts = [];
    for(var appt of testAppointments) {
        if(appt.customer != "none") {
            confirmedAppts.push(appt);
        }
    }
    return confirmedAppts;
}

/* EDIT TRAINER APPOINTMENT MODAL SCRIPTS */
function showEditTrainerApptModal(apptID){
    //get appointment with that ID from the database. For now, I'll put one static appointment here
    let appointment = {
        apptID: 1,
        date: "4/13/2021",
        startTime: "8:00AM",
        endTime: "9:00AM",
        activity:"Cardio",
        trainer: "Josh Hargrave",
        customer: "Anna Smith",
        price: 55.00
    };

    var modal = document.getElementById("editTrainerApptModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];

        
    let html = "<div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditTrainerApptModal()\">&times;</span>";
    html += "<h1 class=\"modal-header text-center\">Appoitment Details</h1><br>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Date/Time:</h3></div><div class=\"col-md-8\"><h3>";
    html += appointment.date + " at " + appointment.startTime + "-" +  appointment.endTime  +"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Customer:</h3></div><div class=\"col-md-8\"><h3>"+appointment.customer+"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Activity:</h3></div><div class=\"col-md-8\"><h3>"+appointment.activity+"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Price:</h3></div><div class=\"col-md-8\"></h3>$"+appointment.price+"</h3></div></div>";
    html += "<br><div class=\"row text-center\"><button class=\"btn btn-action btn-warning\" id=\"cancelApptButton\" onclick=\"cancelTrainerAppt("+appointment.apptID+")\">Cancel This Appointment</button></div>";

    document.getElementById("editTrainerApptModal").innerHTML = html;
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
    let appointment = {
        apptID: 1,
        date: "4/13/2021",
        startTime: "8:00AM",
        endTime: "9:00AM",
        activity:"Cardio",
        trainer: "Josh Hargrave",
        customer: "Anna Smith",
        price: 55.00
    };
    //use the customer's id to remove them from the appointment. Then save that appointment back to the database. 
    //when they cancel the appointment, change the modal html to show that the apointment was canceled, and change button to close.
    let html = " <div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditTrainerApptModal()\">&times;</span>";
    html += "<h1 class=\"modal-header text-center\">Appoitment Canceled</h1><br>";
    html += "<div class=\"row text-center\"><h2>This appointment has been canceled.</h2></div>"; 
    html += "<div class=\"row text-center\"><h2>Click the button to return to your dashboard to add more appointment availability.</h2></div>";
    html += "<br><div class=\"row text-center\"><button class=\"btn btn-action\" onclick=\"closeEditTrainerApptModal()\">Close</button></div>";
    document.getElementById("editTrainerApptModal").innerHTML = html;
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
        console.log("todayDate: " + todayDate + " | d.GetDate():" + d.getDate());
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
function getTrainerProfileForm(){
    //do we have to pass in an id?

    //FOR TESTING: static trainer
    let trainer = {
        id: 1,
        emailAddress: "gnmoody@crimson.ua.edu",
        password: "abc123",
        firstName: "Gabby",
        lastName: "Benefield",
        birthDate: "1994-11-12",
        gender: "female",
        activities: [
            {activityName:"cardio", 
            activityPrice: 65.00},
            {activityName: "strength training", 
            activityPrice: 75.00}]
    };

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