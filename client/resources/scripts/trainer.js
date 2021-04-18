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
    let todayDate;
    if(currentMonth != today.getMonth()){
        todayDate = 0;
    }
    else {
        todayDate = today.getDate();
    }
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
            let selectedDate = d.getFullYear() + "-" + ('0' + (d.getMonth()+1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2);
            calendarTable += "<td><button onclick=\"showEditAvailabilityModal(value)\" value="+ selectedDate +">" + d.getDate() + "</button></td>";
            //(d.getMonth()+1) + "/" + d.getDate() + "/" + d.getFullYear()
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

function showEditAvailabilityModal(selectedDate){
    // when trainer clicks on a calendar date, pop up that date with any existing availabile appointments they have.

    //get any AVAILABLE appointments associated w/ TrainerID on that date
    let trainerId = getTrainerId();
    const apptApiUrl = "https://localhost:5001/api/Appointment/GetAvailableAppointmentsByDateForTrainer/"+trainerId+"/"+selectedDate;
    fetch(apptApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        // console.log("json[0].startTime ");
        // console.log(json[0].startTime.hours + ":" + json[0].startTime.minutes);
        let apptArray = [];
        for(var i in json){
            //for every json object, create simplified object with only the needed items (ID, date, startTime, endTime, ActivityId, Price)
            //stringify, then re-parse to an object **couldn't get the start and end times to read without doing this**
            var tempStr = JSON.stringify(json[i]);
            var object = JSON.parse(tempStr);
            console.log(object.startTime.hours + ":" + object.startTime.minutes);
            
            //add to apptArray
            apptArray[i] = {
                apptID: object.appointmentId,
                apptDate: getFormattedDate(object.appointmentDate),
                fullStartTime: getFullTime(object.startTime.hours, object.startTime.minutes), //e.g. 09:00, 14:00, etc
                fullEndTime: getFullTime(object.endTime.hours, object.endTime.minutes),
                // startTime: getFormattedTime(object.startTime.hours, object.startTime.minutes), //e.g. 9:00AM, 2:00PM, etc.
                // endTime: getFormattedTime(object.endTime.hours, object.endTime.minutes),
                activityId:  object.appointmentTrainer.trainerActivities[0].activityId,
                price: object.appointmentCost //MIGHT MAKE THIS BASED ON THE DIFF BETWEEN STARTTIME AND ENDTIME?
            }
        }
        //set up html
        let html = "<div class=\"modal-dialog modal-lg\"><div class=\"modal-content editTrainerAvailModal\">";
        html += "<span class=\"close\" onclick=\"closeEditAvailabilityModal()\">&times;</span>";
        html += "<h1 class=\"modal-header text-center\">Edit Availability for "+selectedDate+"</h1><br>";
        //set up table & headers
        html += "<table class=\"table table-hover\" id=\"editAvailTable\"><thead><tr><th>Start Time</th><th>End Time</th><th>Activity</th><th>Price</th><th></th><th></th>";
        html += "</tr></thead><tbody>"; //end head, begin body
        let count = 5; //use count of 5-13 so we can avoid apptIDs, which end in 4

        if(apptArray.length > 0){
            for(var i in apptArray){
                html += "<tr><td><input disabled type=\"time\" id=startTime-"+apptArray[i].apptID+" name=\"startTime"+apptArray[i].apptID+"\" value="+apptArray[i].fullStartTime+" min=\"06:00\" max=\"18:00\"></td>"; //start time, START DISABLED
                html += "<td><input disabled type=\"time\" id=endTime-"+apptArray[i].apptID+" name=\"endTime"+apptArray[i].apptID+"\" value="+apptArray[i].fullEndTime+" min=\"06:00\" max=\"18:00\" ></td>"; //end time, START DISABLED
                html += "<td><select disabled id=activity-"+apptArray[i].apptID+" name=\"activities\" value="+apptArray[i].activityId+"><option disabled value=\"4\" id=\"carOpt"+apptArray[i].apptID+"\">Cardio</option><option disabled id=\"stOpt"+apptArray[i].apptID+"\"value=\"14\">Strength Training</option><option disabled value=\"24\" id=\"kbOpt"+apptArray[i].apptID+"\">Kickboxing</option><option disabled value=\"34\" id=\"yoOpt"+apptArray[i].apptID+"\">Yoga</option></select></td>"; //activity, START DISABLED
                html += "<td><input disabled type=\"text\" name=\"price-"+apptArray[i].apptID+"\" value="+apptArray[i].price+" style=\"max-width:80px;\"></td>"; //Price: auto-calculated. TEXT DISPLAY, DISABLED.
                html += "<td id=\"editAvailApptBtn\"><button class=\"btn btn-secondary\" type=\"button\" onclick=\"enableApptEdit("+apptArray[i].apptID+")\">Edit</button></td>"; //Button for EDIT
                html += "<td><button class=\"btn btn-danger\" type=\"button\" onclick=\"deleteAppointment("+apptArray[i].apptID+")\">Delete</button></td>"; //Button for DELETE
                html += "</tr>"; //end row
                count++;

                //add disabled attributes to activities trainers don't say they can do
                //get trainer to get their activities
                const trainerApiUrl = 
                for(var j in trainer.trainerActivities){
                    if(trainer.trainerActivities[j].activityId == 4)
                    {
                        document.getElementById("carOpt"+apptArray[i].apptID).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 14){
                        document.getElementById("stOpt"+apptArray[i].apptID).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 24){
                        document.getElementById("kbOpt"+apptArray[i].apptID).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 34){
                        document.getElementById("yoOpt"+apptArray[i].apptID).disabled = false;
                    }
                }
            }
        }
        else {
            //add 3 rows to table as above, but ids of "startTime-"+count
            for(let i=count; i<8; i++){
                //add 3 ENABLED rows to table as above, but ids of property-count
                html += "<tr><td><input type=\"time\" id=startTime-"+i+" name=\"startTime"+i+"\" value=\"\" min=\"06:00\" max=\"18:00\"></td>"; //start time, START DISABLED
                html += "<td><input type=\"time\" id=endTime-"+i+" name=\"endTime"+i+"\" value=\"\" min=\"06:00\" max=\"18:00\" ></td>"; //end time, START DISABLED
                html += "<td><select id=activity-"+i+" name=\"activities\" ><option disabled id=\"carOpt"+count+"\" value=\"4\">Cardio</option><option disabled id=\"stOpt"+count+"\" value=\"14\">Strength Training</option><option disabled id=\"kbOpt"+count+"\" value=\"24\">Kickboxing</option><option disabled id=\"yoOpt"+count+"\" value=\"34\">Yoga</option></select>"; //activity, START DISABLED
                html += "<td><input disabled type=\"text\" name=\"price-"+i+"\" value=\"\"></td>"; //Price: auto-calculated. TEXT DISPLAY, DISABLED.
                html += "<td id=\"editAvailApptBtn\"><button class=\"btn btn-success\" type=\"button\" onclick=\"validateNewAppt("+count+")\">Save</button></td>"; //SAVE button, send in count to be able to access each input and validate/submit
                html += "<td></td>"; //BLANK td
                count++; //increment count as with i so it stays updated

                for(var j in trainer.trainerActivities){
                    if(trainer.trainerActivities[j].activityId == 4)
                    {
                        document.getElementById("carOpt"+count).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 14){
                        document.getElementById("stOpt"+count).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 24){
                        document.getElementById("kbOpt"+count).disabled = false;
                    }
                    if(trainer.trainerActivities[j].activityId == 34){
                        document.getElementById("yoOpt"+count).disabled = false;
                    }
                }                
            }
            //count++
        }
        //set up table close
        html += "</tbody></table>";

        //add a div with centered button for "add more rows", onclick call AddMoreRows(count)
        html += "<div class=\"row text-center\"><button class=\"btn btn-lg btn-primary\" onclick=\"addRow("+count+")\">Add More Rows</button></div>";
        //close modal-content and modal-dialog divs
        html += "</div></div>";
        //addMoreRows sends in count to populate new table row
        

        //set inner HTML of modal
        document.getElementById("editTrainerApptModal").innerHTML = html;

        //Show Modal
        var modal = document.getElementById("editTrainerApptModal");
        modal.style.display = "block";
        var span = document.getElementsByClassName("close")[0];
    }).catch(function(error){
        console.log(error);
    })

}
function addRow(count){
    //the following should go in an AddRow() method
    if(count % 10 == 4){ //if 4 is the last digit of the number, it's the same as an existing apptID
        count++;
        addRow(count);
    }
    else {
        let newRow = document.createElement('tr');
        newRow.id = "row-"+count;
        let rowHtml = "<td><input type=\"time\" id=startTime-"+count+" name=\"startTime"+count+"\" value=\"\" min=\"06:00\" max=\"18:00\"></td>";
        rowHtml += "<td><input type=\"time\" id=endTime-"+count+" name=\"endTime"+count+"\" value=\"\" min=\"06:00\" max=\"18:00\" ></td>";
        rowHtml += "<td><select id=activity-"+count+" name=\"activities\"><option value=\"4\">Cardio</option><option value=\"14\">Strength Training</option><option value=\"24\">Kickboxing</option><option value=\"34\">Yoga</option></select></td>"; 
        rowHtml += "<td><input disabled type=\"text\" name=\"price-"+count+"\" value=\"\" style=\"max-width:80px;\"></td>";
        rowHtml +="<td id=\"editAvailApptBtn\"><button class=\"btn btn-success\" type=\"button\" onclick=\"validateNewAppt("+count+")\">Save</button></td><td></td>"; //final td is blank because it's new
        count++; //increment count
        newRow.innerHTML = rowHtml;
        editAvailTable.appendChild(newRow);
    }
}

function enableApptEdit(apptID){
    //enable startTime-apptID, endTime-apptID, and activity-apptID
    document.getElementById("startTime-"+apptID).disabled = false;
    document.getElementById("endTime-"+apptID).dsabled = !disabled;
    document.getElementById("activity-"+apptID).disabled = false;
    //change edit button to a save button
    let html = "<button class=\"btn btn-success\" type=\"button\" onclick=\"editAvailableAppt("+apptID+")\">Save Changes</button>";
    document.getElementById("editAvailApptBtn").innerHTML = html;
}

function editAvailableAppt(apptID){
    console.log("appt id is " + apptID);
   
    //get values from startTime-id, endTime-id, activity-id, and price-id, create new appt object with that and send in a PUT request
    let startTime = document.getElementById("startTime-"+apptID).value;
    let newStartTime = "0001-01-01 " + startTime + ":00";

    let endTime = document.getElementById("endTime-"+apptID).value;
    let newEndTime = "0001-01-01 " + endTime + ":00";

    let newActivityID = document.getElementById("activity-"+apptID).value;
    // make new body object to send in put request
    let bodyObj = {
        appointmentId: apptID,
        startTime: newStartTime,
        endTime: newEndTime,
        activityId: newActivityID
    }
    const putApptApiUrl = "https://localhost:5001/api/Appointment/PutAvailableAppointment/"+apptID+"/"+newStartTime+"/"+newEndTime+"/"+newActivityID;
    fetch(putApptApiUrl, {
        method: "PUT",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json'
        },
        body: JSON.stringify(bodyObj)
    })
    .then(function(response){
        console.log(response);
    })

    //then disable the fields again and change the Save button back to an edit button
}


function closeEditAvailabilityModal(){
    var modal = document.getElementById("trainerEditAvailabilityModal");
    modal.style.display = "none";
}

function getFullTime(hours, minutes){
    //add leading zeroes as needed to hours or minutes, return HH:mm
    let newHours = "";
    if(hours < 10){
        newHours = "0" + hours.toString();
    }
    else {
        newHours = hours.toString();
    }
    let newMinutes = "";
    if(minutes < 10){
        newMinutes = "0" + minutes.toString();
    }
    else {
        newMinutes = minutes.toString();
    }
    return newHours + ":" + newMinutes;
}

/* EDIT TRAINER PROFILE SECTION */
function getTrainerProfileForm(trainer){
    //do we have to pass in an id?


    //fill in everything but the password. Trainer should be required to enter their current password to make changes.
    document.getElementById("currEmail").value = trainer.email;
    document.getElementById("inputFName").value = trainer.fName;
    document.getElementById("inputLName").value = trainer.lName;
    document.getElementById("birthDate").value = trainer.birthDate;
    document.getElementById("gender").value = trainer.gender;
 
    for(var i in trainer.trainerAtivities){ //update checked status of activities
        if(trainer.trainerActivities[i].activityId == 4){
            document.getElementById("cardio").checked = true;
            document.getElementById("cardioPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("cardioPrice").disabled = false;        
        }
        else if(trainer.trainerActivities[i].activityId == 14){
            document.getElementById("strengthTraining").checked = true;
            document.getElementById("strengthTrainingPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("strengthTrainingPrice").disabled = false;
        }
        else if(trainer.trainerActivities[i].activityId == 24){
            document.getElementById("kickboxing").checked = true;
            document.getElementById("kickboxingPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("kickboxingPrice").disabled = false;
        }
        else if(trainer.trainerActivities[i].activityId == 34){
            document.getElementById("yoga").checked = true;
            document.getElementById("yogaPrice").value = trainer.activities[i].activityPrice;
            document.getElementById("yogaPrice").disabled = false;
        }
    }
}