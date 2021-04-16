/* Scripts relating to the page customer.html,
including loading customer confirmed appts,
loading available appointments to calendar, 
and loading customer profile */


var today = new Date();
var currentMonth = today.getMonth();
var currentYear = today.getFullYear();

//Check https://github.com/niinpatel/calendarHTML-Javascript/blob/master/scripts.js for a possible JS calendar
function handleCustomerDashboardOnLoad(){ //load each part of dashboard
    getCustomerAppointments();

    getAvailableAppointmentCalendar(currentMonth, currentYear);
    
    getCustomerProfileForm();
}

function getCustomer(){
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get("id");
    console.log("made it to getCustomer. Id is " + id);

    let customer = "";

    const customerApiUrl = "https://localhost:5001/api/Customer/GetCustomerByID/"+id;
    fetch(customerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        console.log("json.id is" + json.customerId);
        customer = json;
        console.log('customer id is ' + customer.customerId);
    }).catch(function(error){
        console.log(error);
    }) 
    return customer;
}

function getCustomerAppointments(){
    /* load any appointments that have customer's id, 
    change html in the element with id "custApptList"
    to the list of buttons as seen in the static customer.html page  */
    let customer = getCustomer();
    console.log("customer id is " + customer.customerId);

    let confirmedAppts = getConfirmedAppointments(customer);
    console.log("confirmedAppts is " + confirmedAppts);
    let html = "";
    if(confirmedAppts == undefined) {
        //if no appointments are found, set up some html to say they have no confirmed appointments at this time 
        html += "<h2>You don't have any upcoming appointments scheduled at this time.</h2><h2>Check out the calendar below to find some sessions and get training!</h2>";
    }
    else {
        for(let i = 0; i < confirmedAppts.length; i++) {
            //create buttons with the appointment information, onclick = cancelApptOnClick()
            html += "<button type=\"button btn\" class=\"list-group-item list-group-item-action\" onclick=\"showEditCustApptModal("+confirmedAppts[i].appointmentID+")\">";
            html += confirmedAppts[i].date + " at " + confirmedAppts[i].startTime + "-" + confirmedAppts[i].endTime + " | Activity: " + confirmedAppts[i].activity.activityName + " | Trainer: " + confirmedAppts[i].trainer.trainerfName+ " " + confirmedAppts[i].trainer.trainerlName +"</button>";
        }
    }
    document.getElementById("custApptList").innerHTML = html;
}


function getConfirmedAppointments(customer){
    //Get appointments from DB that match the customer ID In the url & have a date of today or in the future.
    //return that array of appointment objects
    let confirmedAppts = "";
    const apptApiUrl = "https://localhost:5001/api/Appointment/GetConfirmedAppointmentsForCustomer/"+customer.customerId;
    fetch(apptApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        if(json[0] == undefined){
            console.log("no appointments found");
             //set to a default value if no appointments were found for that customer
            return undefined;
        }
        else{
            console.log("Appointments found!");
            confirmedAppts = json;
            return confirmedAppts;
        }
    }).catch(function(error){
        console.log(error);
    }) 

    // console.log("confirmedAppts is " + confirmedAppts)
    // return confirmedAppts;
}

//FOR EDIT CUSTOMER APPT MODAL in viewCustAppointments section
function showEditCustApptModal(apptID){
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

    var modal = document.getElementById("editCustApptModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];

        
    let html = "<div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditCustApptModal()\">&times;</span>";
    html += "<h1 class=\"modal-header\">Appoitment Details</h1><br>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Date/Time:</h3></div><div class=\"col-md-8\"><h3>";
    html += appointment.date + " at " + appointment.startTime + "-" +  appointment.endTime  +"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Trainer:</h3></div><div class=\"col-md-8\"><h3>"+appointment.trainer+"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Activity:</h3></div><div class=\"col-md-8\"><h3>"+appointment.activity+"</h3></div></div>";
    html += "<div class=\"row\"><div class=\"col-md-4\"><h3>Price:</h3></div><div class=\"col-md-8\"></h3>$"+appointment.price+"</h3></div></div>";
    html += "<br><button class=\"btn btn-action btn-warning\" id=\"cancelApptButton\" onclick=\"cancelCustApptOnClick("+appointment.apptID+")\">Cancel This Appointment</button></div>";

    document.getElementById("editCustApptModal").innerHTML = html;
}

function cancelCustApptOnClick(apptID){
    //get that appointment with the id from the database. For now, a static appointment below. 
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
    let html = " <div class=\"modal-content\"><span class=\"close\" onclick=\"closeEditCustApptModal()\">&times;</span>";
    html += "<h1 class=\"modal-header\">Appoitment Canceled</h1><br>";
    html += "<div class=\"row text-center\"><h2>This appointment has been canceled.</h2></div>"; 
    html += "<div class=\"row text-center\"><h2>Click the button to return to your dashboard to search for other appointments.</h2></div>";
    html += "<br><button class=\"btn btn-action\" onclick=\"closeEditCustApptModal()\">Close</button>";
    document.getElementById("editCustApptModal").innerHTML = html;
}

function closeEditCustApptModal(){
    var modal = document.getElementById("editCustApptModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("editCustApptModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}
/*END CUSTOMER EDIT APPOINTMENTS & MODAL*/




//CUSTOMER AVAILABLE APPOINTMENT CALENDAR SECTION
function getAvailableAppointmentCalendar(currentMonth, currentYear){
    /* TO-DO:   
        -get appointments with no customer id from DB
        -on a date, if no appointments, set button to disabled
        -figure out how to pass in the date value to the showMakeAppointmentModal() */
    
        //display current month at top of calendar
    /*STATIC APPOINTMENTS FOR TESTING
      -will have to get DISTINCT appts from database where customer = null, sorted by date ASCENDING*/
      let appointments = [
        {
            id: 1,
            date: "4/4/2021",
            startTime : "10:00am",
            activity: "Cardio",
            trainer: "Josh Hargrove",
            customer: null,
            price: 50.00
        },
        {
            id: 2,
            date: "4/6/2021",
            startTime: "12:00pm",
            activity: "Strength Training",
            trainer: "Callie Jones",
            customer: null,
            price: 75.00
        },
        {
            id: 3,
            date: "4/8/2021",
            startTime: "4:00pm",
            activity: "Cardio",
            trainer: "Eric Blackburn",
            customer: null,
            price: 65.00
        },
        {
            id: 4,
            date: "4/10/2021",
            startTime: "6:00pm",
            activity: "Kickboxing",
            trainer: "Princess Smith",
            customer: null,
            price: 60.00
        },
        {
            id: 5,
            date: "4/12/2021",
            startTime: "8:00am",
            activity: "Pilates",
            trainer: "Kim Berry",
            customer: null,
            price: 70.00
        }
    ];




    setCalendarHeader(currentMonth, currentYear);

    //get month and year for use below
    let mon = currentMonth;
    let d = new Date(currentYear, mon);
    //CREATE CALENDAR TABLE
    let calendarTable = "<table class=\"table table-responsive-sm calendar-table\"><thead>";
    calendarTable += "<th>Mon</th><th>Tue</th><th>Wed</th><th>Thur</th><th>Fri</th><th>Sat</th><th>Sun</th></thead><tbody id=\"calendarBody\">";
    // spaces for the first row from Sunday until the first day of the month
    for(let i = 0; i < d.getDay(); i++) {
        calendarTable += "<td></td>";
    }
    let found = false;
    //<td> with dates for that month
    while(d.getMonth() == mon) {
        //for each available appointment with distinct date (sorted in ascending order)
        let fullDate = (d.getMonth()+1) + "/" + d.getDate() + "/" + d.getFullYear();
        for(let i = 0; i < appointments.length; i++){
            if(appointments[i].date == fullDate){ //if date matches the date of an appointment
                calendarTable += "<td><button onclick=\"showMakeAppointmentModal(value)\" value="+(d.getMonth()+1) + "/" + d.getDate() + "/" + d.getFullYear()+">" + d.getDate() + "</button></td>";
                //delete the element from the array
                appointments.shift();
                found = true;
            }
        }
        if(found == false){
            //if no matching appointments with date are found, disable button
            calendarTable += "<td><button disabled style=\"background-color: #808080\">" + d.getDate() + "</button></td>"; 
        }
        
        if(d.getDay() == 6) { //if last day of week (Saturday), new table row
            calendarTable += "</tr><tr>";
        }
        d.setDate(d.getDate() + 1); //increment date
        found = false; //reset found to false for the next loop
    }
    //add spaces after last days of month for last row if Saturday isn't the last day of the month
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
    // var currDate = new Date(); //current date
    var mList = [ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ];
    var monthName = mList[currentMonth];
    var monthAndYear = monthName + " " + currentYear;
    document.getElementById("currMonth").innerHTML = monthAndYear;
}

function nextMonth() {
    currentYear = (currentMonth === 11) ? currentYear + 1 : currentYear;
    currentMonth = (currentMonth + 1) % 12;
    setCalendarHeader(currentMonth, currentYear);
    getAvailableAppointmentCalendar(currentMonth, currentYear);
}

function previousMonth() {
    currentYear = (currentMonth === 0) ? currentYear - 1 : currentYear;
    currentMonth = (currentMonth === 0) ? 11 : currentMonth - 1;
    setCalendarHeader(currentMonth, currentYear);
    getAvailableAppointmentCalendar(currentMonth, currentYear);
}



//FOR MODAL TO MAKE APPOINTMENTS
function showMakeAppointmentModal(value){
    var modal = document.getElementById("custMakeApptModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];

    let selectedDate = value;
    console.log("Selected Date is" + selectedDate);
    /* STATIC APPTS FOR TESTING
       - will have to get appts from database WHERE customer = null */
    let appointments = [
        {
            id: 1,
            date: "4/4/2021",
            startTime : "10:00am",
            activity: "Cardio",
            trainer: "Josh Hargrove",
            customer: null,
            price: 50.00
        },
        {
            id: 2,
            date: "4/6/2021",
            startTime: "12:00pm",
            activity: "Strength Training",
            trainer: "Callie Jones",
            customer: null,
            price: 75.00
        },
        {
            id: 3,
            date: "4/8/2021",
            startTime: "4:00pm",
            activity: "Cardio",
            trainer: "Eric Blackburn",
            customer: null,
            price: 65.00
        },
        {
            id: 4,
            date: "4/10/2021",
            startTime: "6:00pm",
            activity: "Kickboxing",
            trainer: "Princess Smith",
            customer: null,
            price: 60.00
        },
        {
            id: 5,
            date: "4/12/2021",
            startTime: "8:00am",
            activity: "Pilates",
            trainer: "Kim Berry",
            customer: null,
            price: 70.00
        }
    ];
    
    let html = document.getElementById("custMakeApptModal");
    /* display a button list of appointments from the selected date (get value of date selected to search for appointments with that date?), 
    in ascending order by time, with time, trainer, and activity.  */
    html+="<div class=\"modal-dialog\"><div class=\"modal-content\">";
    html += "<span class=\"close\" onclick=\"closeMakeAppointmentModal()\">&times;</span><h1 class=\"modal-header\">Available Appointments</h1>";
    html += "<div class=\"list-group avail-appt-list text-center\" id=\"availApptList\">";
    for(var i in appointments){
        console.log("Appointment date: " + appointments[i].date);
        if(selectedDate == appointments[i].date){
            // html += "<a class=\"list-group-item\" onclick=\"custConfirmAppt()\"><div>";
            // html += "<h5 class=\"list-group-item-heading\">"+appointments[i].startTime+"</h5><small>"+appointments[i].price+"</small></div>"
            // html += "<p class=\"list-group-item-text\">Activity: "+appointments[i].activity+"</p>";
            // html += "<p class=\"list-group-item-text\">Trainer: "+appointments[i].trainer+"</p></a>";
            html += "<button type=\"button btn\" class=\"list-group-item list-group-item-action\" value="+appointments[i].id+" onclick=\"showApptDetails("+appointments[i].id+")\">";
            html += "Start Time: "+appointments[i].startTime+" "+ " | Price: $"+appointments[i].price+" ";
            html += " | Activity: "+appointments[i].activity+" "+ " | Trainer: "+appointments[i].trainer+" ";
        }
        html += "</button>";
    }
    html += "</div></div></div>";
    document.getElementById("custMakeApptModal").innerHTML = html;
    /************ TO-DO ***********/



}

function showApptDetails(value){
    let apptID = value;

    /*FOR TESTING static appt list */
    let appointments = [
        {
            id: 1,
            date: "4/4/2021",
            startTime : "10:00am",
            activity: "Cardio",
            trainer: "Josh Hargrove",
            customer: null,
            price: 50.00
        },
        {
            id: 2,
            date: "4/6/2021",
            startTime: "12:00pm",
            activity: "Strength Training",
            trainer: "Callie Jones",
            customer: null,
            price: 75.00
        },
        {
            id: 3,
            date: "4/8/2021",
            startTime: "4:00pm",
            activity: "Cardio",
            trainer: "Eric Blackburn",
            customer: null,
            price: 65.00
        },
        {
            id: 4,
            date: "4/10/2021",
            startTime: "6:00pm",
            activity: "Kickboxing",
            trainer: "Princess Smith",
            customer: null,
            price: 60.00
        },
        {
            id: 5,
            date: "4/10/2021",
            startTime: "8:00am",
            activity: "Pilates",
            trainer: "Kim Berry",
            customer: null,
            price: 70.00
        }
    ];
    /*in api call will get the appt where id = value*/

    let html = document.getElementById("custMakeApptModal"); 
    html = ""; //reset html
    for(var i in appointments){
        if(appointments[i].id == apptID){
            html += "<div class=\"modal-dialog\"><div class=\"modal-content\">";
            html += "<span class=\"close\" onclick=\"closeMakeAppointmentModal()\">&times;</span><h1 class=\"modal-header\">Pay and Confirm Appointment</h1>";
            html += "<div id=\"apptDetails\"><div class=\"row text-left\"><h4>Date: "+appointments[i].date+"</h4></div>";
            html += "<div class=\"row text-left\"><h4>Time: "+appointments[i].startTime+"</h4></div>";
            html += "<div class=\"row text-left\"><h4>Activity: "+appointments[i].activity+"</h4></div>";
            html += "<div class=\"row text-left\"><h4>Trainer: "+appointments[i].trainer+"</h4></div>";
            html += "<div class=\"row text-left\"><h4>Price: $"+appointments[i].price+"</h4></div></div>";
            /*radio button for paying cash, and credit card (disabled for now)*/
            html += "<form><div class=\"form-check\"><div class=\"row\">";
            html += "<div class=\"col-md-12\"><input class=\"form-check-input\" type=\"radio\" name=\"flexRadioDefault\" id=\"payingCash\" checked>";
            html += "<label class=\"form-check-label\" for=\"payingCash\">Cash</label></div></div>";
            html += "<div class=\"col-md-12\"><label for=\"amountPaid\" style=\"padding-left: 24px;\">Enter amount:</label>";
            html += "<input type=\"number\" id=\"amountPaid\" placeholder=\"Enter amount here\"></div><small id=\"insufficientCashMessage\" class=\"text-muted\" style=\"display: none\">Amount entered must be equal to the price of the appointment.</small></div>";
            html += "<div class=\"form-check\"><div class=\"row\"><div class=\"col-md-12\"><input class=\"form-check-input\" type=\"radio\" name=\"flexRadioDefault\" id=\"payingCard\" disabled>";
            html += "<label class=\"form-check-label\" for=\"payingCard\">Credit Card</label></div></div></div>";
            html += "</form>";
        }  
    }
    html += "<div class=\"text-center\"><button class=\"btn btn-lg btn-success\" type=\"submit\" onclick=\"addCustToAppointment("+apptID+")\">Pay and Confirm</button></div></div></div>";
    document.getElementById("custMakeApptModal").innerHTML = html;


    // Might use the following for credit card info form: 
    // https://bbbootstrap.com/snippets/credit-card-payment-form-78109411
    // div for the CC form would go right after the form but before the button. Set display to hide. Set to show if radio button for credit card is selected.
}

function addCustToAppointment(apptID){
    /*WILL PROBABLY ALSO HAVE TO READ IN CUSTOMER TO ADD TO APPOINTMENT*/

    /* FOR TESTING: static appointments to search for.
    will not need the for loop and if statement once we're getting the 1 appointment from the database*/
    let appointments = [
        {
            id: 1,
            date: "4/4/2021",
            startTime : "10:00am",
            endTime: "11:00am",
            activity: "Cardio",
            trainer: "Josh Hargrove",
            customer: null,
            price: 50.00
        },
        {
            id: 2,
            date: "4/6/2021",
            startTime: "12:00pm",
            endTime: "1:00pm",
            activity: "Strength Training",
            trainer: "Callie Jones",
            customer: null,
            price: 75.00
        },
        {
            id: 3,
            date: "4/8/2021",
            startTime: "4:00pm",
            endTime: "5:00pm",
            activity: "Cardio",
            trainer: "Eric Blackburn",
            customer: null,
            price: 65.00
        },
        {
            id: 4,
            date: "4/10/2021",
            startTime: "6:00pm",
            endTime: "7:00pm",
            activity: "Kickboxing",
            trainer: "Princess Smith",
            customer: null,
            price: 60.00
        },
        {
            id: 5,
            date: "4/10/2021",
            startTime: "8:00am",
            endTime: "9:00am",
            activity: "Pilates",
            trainer: "Kim Berry",
            customer: null,
            price: 70.00
        }
    ];

     /*in api call will get the appt where id = value*/
    let appointment = "";
    for(var i in appointments){
        if(appointments[i].id == apptID){
            appointment = appointments[i];
        }
    }

    //parse amountPaid to a number
    let value = document.getElementById("amountPaid").value;
    let amountPaid = parseFloat(value);
    // let message = document.getElementById("insufficientCashMessage");
    //if not enough $$ entered or if no number is entered, display error message
    if(amountPaid < appointment.price || Number.isNaN(amountPaid)){
        document.getElementById("insufficientCashMessage").style.display = "block";
    }
    else //if amount entered is enough, add customer to appointment
    {
        let html = document.getElementById("custMakeApptModal");
        html = "";
        html += "<div class=\"modal-dialog\"><div class=\"modal-content\">";
        html += "<span class=\"close\" onclick=\"closeMakeAppointmentModal()\">&times;</span><h1 class=\"modal-header\">Appointment Confirmed!</h1>";
        html += "<h2 style=\"padding-top: 20px;\">Your appointment is confirmed!  See the details below:</h3>";
        html += "<div id=\"apptDetails\"><div class=\"row text-left\"><h4>Date: "+appointment.date+"</h4></div>";
        html += "<div class=\"row text-left\"><h4>Time: "+appointment.startTime+"-"+appointment.endTime+"</h4></div>";
        html += "<div class=\"row text-left\"><h4>Activity: "+appointment.activity+"</h4></div>";
        html += "<div class=\"row text-left\"><h4>Trainer: "+appointment.trainer+"</h4></div>";
        html += "<div class=\"row text-left\"><h4>Price: $"+appointment.price+"</h4></div>";
        html += "<div class=\"row text-left\"><h4>Amount Paid: $"+amountPaid+"</h4></div>";
        html += "<div class=\"row text-center\"><button class=\"btn btn-lg btn-secondary\" role=\"button\" onclick=\"closeMakeAppointmentModal()\">Close this window</button></div>";
            
        document.getElementById("custMakeApptModal").innerHTML = html;
    }
   
    
}

function closeMakeAppointmentModal(){
    var modal = document.getElementById("custMakeApptModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("custMakeApptModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}
/*END CALENDAR/MAKE APPOINTMENTS SECTION*/

/* GET / UPDATE CUSTOMER PROFILE SECTION */
function getCustomerProfileForm(){
    console.log("made it to customerProfileForm");
   
    //get customer from API call using id in url
    let customer = getCustomer();
    
    //yet these are undefined?
    console.log(customer.email);
    console.log(customer.fName);
   
    document.getElementById("currEmail").value = customer.email;
    document.getElementById("inputFName").value = customer.fName;
    document.getElementById("inputLName").value = customer.lName;
    document.getElementById("birthDate").value = customer.birthDate;
    document.getElementById("gender").value = customer.gender;
    document.getElementById("fitnessGoals").value = customer.fitnessGoals;

    for(var i in customer.activities){ //update checked status of activities
        if(customer.activities[i] == "cardio"){
            document.getElementById("cardio").checked = true;
            
        }
        else if(customer.activities[i] == "strength training"){
            document.getElementById("strengthTraining").checked = true;
        }
        else if(customer.activities[i] == "kickboxing"){
            document.getElementById("kickboxing").checked = true;
        }
        else if(customer.activities[i] == "yoga"){
            document.getElementById("yoga").checked = true;
        }
    }

    if(customer.referredBy != null) { //update checked/referrer name if there is a referred by
        document.getElementById("yesReferred").checked = true;
        document.getElementById("referrerName").value = customer.referredBy;
        document.getElementById("referrerName").disabled = false;
    }
}

function handleReferredByOnClick(){
    if(document.getElementById("yesReferred").checked == true){
        document.getElementById("referrerName").disabled = false;
    }
}