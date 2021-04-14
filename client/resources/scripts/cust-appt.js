//custom calendar JS
//temporary var to hold some appointments
var appointments = [
    {

    },
    {
        
    },
    {

    }
];

function handleCalendarOnLoad(){ //eventually this will load the entire calendar
    var date = new Date();
    var month_name = function(dt){
        mlist = [ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" ];
          return mlist[dt.getMonth()];
        };
    console.log(month_name(new Date(new Date())) + ' ' + date.getFullYear());
   
    var currMonth = month_name(new Date(new Date())) + ' ' + date.getFullYear();
    document.getElementById("currMonth").innerHTML = currMonth;
    //for some reason, this is still not working
   
}


// relating to confirming customer appointment 
function handleDateOnClick(){
    //show modal with appointment list
    showCustConfirmApptModal();
}

function showCustConfirmApptModal(){
    var modal = document.getElementById("custConfirmApptModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];
}

function closeCustConfirmApptModal(){
    var modal = document.getElementById("custConfirmApptModal");
    modal.style.display = "none";
}

function confirmCustApptOnClick(){
    //use the customer's id to add them to the appointment 
    //when they confirm appointment, change modal content to "Appointment Confirmed" with a close button-->
    closeCustConfirmApptModal();
}

window.onclick = function(event){
    var modal = document.getElementById("custConfirmApptModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}


//relating to letting customer cancel appointment
function showEditCustApptModal(){
    var modal = document.getElementById("editCustApptModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];
}

function closeEditCustApptModal(){
    var modal = document.getElementById("editCustApptModal");
    modal.style.display = "none";
}

function cancelCustApptOnClick(){
    //use the customer's id to remove them from the appointment
    //when they cancel the appointment, change the modal html to show that the apointment was canceled, and change button to close.
    closeEditCustApptModal();
}

window.onclick = function(event){
    var modal = document.getElementById("editCustApptModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}




// // this calendar JS was downloaded, the modified, from https://codepen.io/CallMeFrank/pen/RYzpmQ
// 'use strict';

// let daysOfTheWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
// let months = [
// 	'January',
// 	'February',
// 	'March',
// 	'April',
// 	'May',
// 	'June',
// 	'July',
// 	'August',
// 	'September',
// 	'October',
// 	'November',
// 	'December'
// ];
// let yearNode = document.getElementById('year');
// let currYear = yearNode.value || new Date().getFullYear();
// let monthNode = document.getElementById('month');
// let currMonth = monthNode.value || new Date().getMonth();

// /*  Set current year and month as default values in inputs 
// *	@yearNode: input with id 'year'
// *	@monthNode: input with id 'month'
//  */
// yearNode.value = currYear;
// monthNode.value = months[currMonth];


// createCalendar('JS-calendarDays', currYear, currMonth + 1, daysOfTheWeek);

// ['JS-monthControl--next', 'JS-monthControl--prev', 'JS-yearControl--prev', 'JS-yearControl--next'].forEach(elem => {
// 	document.getElementsByClassName(elem)[0].onclick = (e) => {
// 		e.preventDefault();

// 		let target = document.getElementsByClassName(elem)[0].getAttribute('data-storage');

// 		if (target == 'year') {
// 			changeCurrDate('year', elem);
// 		}
// 		else if (target == 'month') {
// 			changeCurrDate('month', elem);
// 		}

// 		createCalendar('JS-calendarDays', +yearNode.value, months.indexOf(monthNode.value) + 1, daysOfTheWeek);
// 	}
// });

// /* Initialize a calendar creator : params = id, year, month, weekDays
// *	@id: id of a block where the function will render the calendar
// *	@year: set a year of a required date
// *	@month: set a month of a required date
// *	@weekDays: a stack of the weekday names
//  */
// function createCalendar(id, year, month, weekDays) {
// 	let mon = month - 1;
// 	let date = new Date(year, mon);
// 	let html = '<tr>' + generateDaysOfTheWeek(weekDays) + '</tr><tr>';

// 	// Generate days of the last month up to the current day of the week
// 	for (let i = 0; i < getWeekDay(date); i++) {
// 		html += '<td></td>';
// 	}

// 	html += generateCalendarDays(date, mon);

// 	// Generate days of the next month up to the current day of the week
// 	if (getWeekDay(date) != 0) {
// 		for (let i = getWeekDay(date); i < 7; i++) {
// 			html += '<td></td>';
// 		}
// 	}

// 	document.getElementById(id).innerHTML = html;
// }

// /* getWeekDay(date): calculate a day of the week 
// *	@date: 
//  */
// function getWeekDay(date) {
// 	let day = date.getDay();

// 	// If the day == 0 it means the is Sunday and it should follow by Saturday
// 	if (day == 0) day = 7;

// 	return day - 1;
// }

// /* generateDaysOfTheWeek(days): generate short view of the day of the week
// *	@days: stack of the days for the week
// */
// function generateDaysOfTheWeek(days) {
// 	days = days.map(d => '<th class="day-of-the-week">' + d.slice(0, 3) + '</th>').join('');

// 	return days;
// }

// /* The calendar cell generator : params = date, month
// *	@date: a date required for display
// *	@month: a month required for display
// */
// function generateCalendarDays(date, month) {
// 	let html = '';

// 	while (date.getMonth() == month) {
// 		let dayWeekend = '<td class="day-of-the-month day-of-the-month--weekend" title="Weekend">' + date.getDate() + '</td>';
// 		let day = '<td class="day-of-the-month">' + date.getDate() + '</td>';

// 		// If installed date is equal to current date
// 		if (new Date().getFullYear() == date.getFullYear() && new Date().getMonth() == date.getMonth() && new Date().getDate() == date.getDate()) {
// 			day = '<td class="day-of-the-month day-of-the-month--today" title="Today">' + date.getDate() + '</td>';
// 		}

// 		// Detecting weekends in the running date
// 		html += getWeekDay(date) % 7 == 6 || getWeekDay(date) % 7 == 5 ? dayWeekend : day;

// 		// Generate new cell row each 7 cells (each week)
// 		if (getWeekDay(date) % 7 == 6) html += '</tr><tr>';

// 		date.setDate(date.getDate() + 1);
// 	}

// 	return (html);
// }

// /* changeCurrdate(id, elem) : update rendered year and month at the calendar
// *	@id: id of the element which update is required
// *	@elem: class of the element that is a trigger to update #id element
//  */
// function changeCurrDate(id, elem) {
// 	let trigger = document.getElementsByClassName(elem)[0];
// 	let move = trigger.getAttribute('data-move'); // Coordinate a target of the trigger @id

// 	if (id == 'year' && move == 'increment' && +document.getElementById(id).value < 2500) {
// 		document.getElementById(id).value = incrementYear(id);
// 	}
// 	else if (id == 'year' && move == 'decrement' && +document.getElementById(id).value > 1970) {
// 		document.getElementById(id).value = decrementYear(id);
// 	}
// 	else if (id == 'month' && move == 'increment') {
// 		document.getElementById(id).value = nextMonth(id);
// 	}
// 	else if (id == 'month' && move == 'decrement') {
// 		document.getElementById(id).value = prevMonth(id);
// 	}

// 	/* prevMonth(id): generate a following month by the current month
// 	*	@id: indetificator of the element which takes an update
// 	 */
// 	function prevMonth(id) {
// 		let currNewMonth = document.getElementById(id).value;
// 		let prevNewMonth = months[months.indexOf(currNewMonth) - 1];

// 		if (months.indexOf(currNewMonth) <= 0) {
// 			document.getElementById('year').value = decrementYear('year');
// 			prevNewMonth = months[11];
// 		}

// 		return prevNewMonth;
// 	}

// 	/* nextMonth(id): generate a previous month by the current month
// 	*	@id: indetificator of the element which takes an update
// 	 */
// 	function nextMonth(id) {
// 		let currNewMonth = document.getElementById(id).value;
// 		let nextNewMonth = months[months.indexOf(currNewMonth) + 1];

// 		if (months.indexOf(currNewMonth) >= months.length - 1) {
// 			document.getElementById('year').value = incrementYear('year');
// 			nextNewMonth = months[0];
// 		};

// 		return nextNewMonth;
// 	}

// 	/* incrementYear(id): generate a following year by the current year
// 	*	@id: indetificator of the element which takes an update
// 	 */
// 	function incrementYear(id) {
// 		return +document.getElementById(id).value + 1;
// 	}

// 	/* decrementYear(id): generate a following year by the current year
// 	*	@id: indetificator of the element which takes an update
// 	 */
// 	function decrementYear(id) {
// 		return +document.getElementById(id).value - 1;
// 	}
// }

