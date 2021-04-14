

//Customer Login Modal
function showCustLoginModal(){
    var modal = document.getElementById("custLoginModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];
}

function closeCustLoginModal(){
    var modal = document.getElementById("custLoginModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("custLoginModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}

//New Customer Modal
function handleNewCustomer(){
    //close customer login form
    var modal = document.getElementById("custLoginModal");
    modal.style.display = "none";

    //show modal for new customer form
    modal = document.getElementById("newCustModal");
    modal.style.display = "block";
}

function closeNewCustModal(){
    var modal = document.getElementById("newCustModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("newCustModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}

function handleCustomerSignIn()
{
    //make API call to DB to search for a Customer w/ that email and password

    //if customer with that email and password are found
        //get customer id from that Customer, pass it into the URL to send it to the dashboard
    //else
        //update modal with a message that user was not found.


    //PASS IN CUSTOMER ID TO SEND CUSTOMER DATA TO THE DASHBOARD
    window.location.href = "./customer.html";
}

function handleCreateNewCustOnClick(){
    
    //read in data and save to database
    //close modal

    //go to ./customer.html
    //PASS IN CUSTOMER ID TO SEND CUSTOMER DATA TO CUSTOMER DASHBOARD
    window.location.href = "./customer.html"

    //send customer data to customer dashboard
}



//Trainer Login Modal
function showTrainerLoginModal(){
    var modal = document.getElementById("trainerLoginModal");
    modal.style.display = "block";

    var span = document.getElementsByClassName("close")[0];
}

function closeTrainerLoginModal(){
    var modal = document.getElementById("trainerLoginModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("trainerLoginModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}

function handleNewTrainer(){ //to pop up new trainer form
    //close trainer login modal
    var modal = document.getElementById("trainerLoginModal");
    modal.style.display = "none";

    //show modal for new trainer form
    var modal = document.getElementById("newTrainerModal");
    modal.style.display = "block";

}

function handleCreateNewTrainerOnClick(){ 
    //create new trainer (get data and add to DB)
    //go to trainer.html
    window.location.href = "./trainer.html";
}

function closeNewTrainerModal(){
    var modal = document.getElementById("newTrainerModal");
    modal.style.display = "none";
}

window.onclick = function(event){
    var modal = document.getElementById("newTrainerModal");
    if(event.target == modal){
        modal.style.display = "none";
    }
}


/* NOTE: this is only enabling the price input field when the checkbox is first checked.  
It is not disabiling it once it goes from checked to unchecked */
function activitySelected(inputID){
/* only enable price input fields if its corresponding checkbox is checked */
    if(document.getElementById(inputID).checked){
        //if box gets checked, enable its price input field
        console.log(inputID + " is now checked");
        document.getElementById(inputID+"Price").disabled = false;
    }
    else{
        console.log(inputID + " is now unchecked");
        //if deselected, disable the price input field
        document.getElementById(inputID+"Price").disabled = true;
    } 

}

function handleReferredByOnClick(){
    if(document.getElementById("yesReferred").checked == true){
        document.getElementById("referrerName").disabled = false;
        document.getElementById("referrerName").required;
    }
}
