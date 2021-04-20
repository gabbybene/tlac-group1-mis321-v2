

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
    document.getElementById("custSignInErrorMsg").style.display = "none"; //hide the errorMsg unless sign-in is invalid
    
    //get value of user input email and password
    let inputEmail = document.getElementById("customerEmail").value;
    let inputPassword = document.getElementById("customerPassword").value;

    var success;
    let customer = "";
    let errorMsgHtml = "";

    const customerApiUrl = "https://localhost:5001/api/Customer/"+inputEmail;
    fetch(customerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        if(json.customerId == 0){
            //if no customer found w/ that email address, display error message
            errorMsgHtml = "<p>No customer found with that email address.</p>";
            document.getElementById("custSignInErrorMsg").innerHTML = errorMsgHtml;
            document.getElementById("custSignInErrorMsg").style.display = "block";
        }
        else { //if the customer was found with that email, check for the password
            if(inputPassword == json.password){
                customer = json; //set the json object to the customer
                window.location.href = "./customer.html?id="+customer.customerId; //go to customer dashboard
            }
            else {
                //if password doesn't match, display error message
                errorMsgHtml = "<p>Invalid login.</p>";
                document.getElementById("custSignInErrorMsg").innerHTML = errorMsgHtml;
                document.getElementById("custSignInErrorMsg").style.display = "block";
            }
        }
    }).catch(function(error){
        console.log(error);
    }) 
}

function validateNewCustomerInputs(){
    try{
        let email = document.getElementById("custEmail").value;
        let password = document.getElementById("custPassword").value;
        let fName = document.getElementById("custFirstName").value;
        let lName = document.getElementById("custLastName").value;
        let dob = document.getElementById("custBirthDate").value;
        let referred = false;;
        if(document.getElementById("yesReferred").checked){
            referred = true;
        }
        let referrerName = document.getElementById("referrerName").value;

        //if all required fields are empty
        if((email == null || email == "") && (password == null || password == "") && (fName ==  null || fName == "") && (lName == null || lName == "") && (dob == null || dob == "" || dob.toString() > "2008-01-01")){
            alert("Email, Password, First Name, Last Name, and Date of Birth are required.");
        }
        else {
            if(email == null || email == ""){
                alert("Email is required.");
                document.getElementById("custEmail").focus();
                // return false;
            }
            if(password == null || password == ""){
                alert("Password is required.");
                document.getElementById("custPassword").focus();
                // return false;
            }
            if(fName ==  null || fName == ""){
                alert("First Name is required.");
                document.getElementById("custFirstName").focus();
                // return false;
            }
            if(lName == null || lName == ""){
                alert("Last Name is required.");
                document.getElementById("custLastName").focus();
                // return false;
            }
            if(dob == null || dob == "" || dob.toString() > "2008-01-01"){
                if(dob == null || dob == ""){
                    alert("Date of Birth is required..");
                }
                else {
                    alert("Date of Birth must be greater than 01/01/2008");
                }
                document.getElementById("custBirthDate").focus();
                // return false;
            }
            if(referred && (referrerName == null || referrerName == "")){
                alert("If you were referred by a friend, their name is required.");
                document.getElementById("referrerName").focus();
            }
            else {
                handleCreateNewCustOnClick();
            }
        }
        
    }
    catch(e) {
        console.log(e);
        // return false;
    }
    // return true;
}



function handleCreateNewCustOnClick(){
    const customerApiUrl = "https://localhost:5001/api/Customer";

    
    //get customer data
    let inputEmail = document.getElementById("custEmail").value;
    let inputPassword = document.getElementById("custPassword").value;
    let inputFirstName = document.getElementById("custFirstName").value;
    let inputLastName = document.getElementById("custLastName").value;
    let dob = document.getElementById("custBirthDate").value;
    let inputGender = document.getElementById("custGender").value;
    let inputFitnessGoals;
    //handle fitness goals
    if(document.getElementById("fitnessGoals").value != null){
        inputFitnessGoals = document.getElementById("fitnessGoals").value;
    }
    else{
        inputFitnessGoals = null;
    }
    let inputActivityIDs = [];
    //handle preferred activities
    if(document.getElementById("cardio".checked)){
        let cardio = document.getElementById("cardio").value;
        inputActivityIDs.push(parseInt(cardio));
    }
    if(document.getElementById("strengthTraining").checked){
        let strengthTraining = document.getElementById("strengthTraining").value;
        inputActivityIDs.push(parseInt(strengthTraining));
    }
    if(document.getElementById("kickboxing").checked){
        let kickboxing = document.getElementById("kickboxing").value;
        inputActivityIDs.push(parseInt(kickboxing));
    }
    if(document.getElementById("yoga").checked){
        let yoga = document.getElementById("yoga").value;
        inputActivityIDs.push(parseInt(yoga));
    }
    let activityArray = [];
    if(inputActivityIDs[0] != null){
        for(var i in inputActivityIDs){
            activityArray[i] = {
                activityId: inputActivityIDs[i]
            }
        }
    }
    for(var i in activityArray){
        console.log("activityArray["+i+"]:" + activityArray[i].activityId);
    }

    //If yesReferred is checked, get referrerName
    if(document.getElementById("yesReferred").checked){
        let referredByName = document.getElementById("referrerName");
    }

    console.log(inputPassword + " " + dob + " " + inputGender + " " + inputFirstName + " " + inputLastName + " " + inputEmail);
    console.log("fitness goals: " + inputFitnessGoals);

    var bodyObj = {
        password: inputPassword,
        birthDate: dob, 
        gender: inputGender,
        fitnessGoals: inputFitnessGoals,
        PhoneNo: "5554443333", //putting a fake phoneNo in here for now because the form isn't set up to take in a phone number yet.
        fName: inputFirstName,
        lName: inputLastName,
        email: inputEmail,
        customerActivities: activityArray
        // referredBy: referredByName,
    };

    //make api call to CREATE customer
    fetch(customerApiUrl, {
        method: "POST",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json'
        },
        body: JSON.stringify(bodyObj)
    }).then(function(response){
        sendCustomerToDashboard(inputEmail);
        console.log(response);
    })
}

function sendCustomerToDashboard(email){
    //get new customer's id and send them to ./customer.html?id=@id@
    const getCustomerApiUrl = "https://localhost:5001/api/Customer/"+email;
    fetch(getCustomerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        let customerId = json.customerId;
        window.location.href = "./customer.html?id="+customerId;
        //console.log("customerId is " + customerId);
    }).catch(function(error){
        console.log(error);
    }) 
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


/* TRAINER login / sign-up */
function handleTrainerSignIn(){
    document.getElementById("trainerSignInErrorMsg").style.display = "none"; //hide the errorMsg unless sign-in is invalid
    
    //get value of user input email and password
    let inputEmail = document.getElementById("trainerEmail").value;
    let inputPassword = document.getElementById("trainerPassword").value;
    console.log("inputEmail is " + inputEmail);


    let trainer = "";
    let errorMsgHtml = "";

    const trainerApiUrl = "https://localhost:5001/api/Trainer/"+inputEmail;
    fetch(trainerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        if(json.trainerId == 0){
            //if no trainer found w/ that email address, display error message
            errorMsgHtml = "<p>No trainer found with that email address.</p>";
            document.getElementById("trainerSignInErrorMsg").innerHTML = errorMsgHtml;
            document.getElementById("trainerSignInErrorMsg").style.display = "block";
        }
        else { //if the trainer was found with that email, check for the password
            if(inputPassword == json.password){
                trainer = json; //set the json object to the trainer
                window.location.href = "./trainer.html?id="+trainer.trainerId; //go to trainer dashboard
            }
            else {
                //if password doesn't match, display error message
                errorMsgHtml = "<p>Invalid login.</p>";
                document.getElementById("trainerSignInErrorMsg").innerHTML = errorMsgHtml;
                document.getElementById("trainerSignInErrorMsg").style.display = "block";
            }
        }
    }).catch(function(error){
        console.log(error);
    }) 
}

function validateNewTrainerInputs(){
    try{
        let email = document.getElementById("newTrainerEmail").value;
        let password = document.getElementById("newTrainerPassword").value;
        let fName = document.getElementById("trainerFName").value;
        let lName = document.getElementById("trainerLName").value;
        let dob = document.getElementById("trainerBirthDate").value;
        //boolean variables for if checked
        let cardioSelected = document.getElementById("trnCardio").checked;
        let stSelected = document.getElementById("trnStrengthTraining").checked;
        let kbSelected = document.getElementById("trnKickboxing").checked;
        let yoSelected = document.getElementById("trnYoga").checked;

        //if all required fields are empty
        if((email == null || email == "") && (password == null || password == "") && (fName ==  null || fName == "") && (lName == null || lName == "") && (dob == null || dob == "" || dob.toString() > "2008-01-01")){
            alert("Email, Password, First Name, Last Name, and Date of Birth are required.");
        }
        else {
            if(email == null || email == ""){
                alert("Email is required.");
                document.getElementById("newTrainerEmail").focus();
                // return false;
            }
            else if(password == null || password == ""){
                alert("Password is required.");
                document.getElementById("trainerPassword").focus();
                // return false;
            }
            else if(fName ==  null || fName == ""){
                alert("First Name is required.");
                document.getElementById("trainerFName").focus();
                // return false;
            }
            else if(lName == null || lName == ""){
                alert("Last Name is required.");
                document.getElementById("trainerLName").focus();
                // return false;
            }
            else if(dob == null || dob == "" || dob.toString() > "2008-01-01"){
                if(dob == null || dob == ""){
                    alert("Date of Birth is required..");
                }
                else {
                    alert("Date of Birth must be greater than 01/01/2008");
                }
                document.getElementById("custBirthDate").focus();
                // return false;
            }
            else if(!cardioSelected && !stSelected && ! kbSelected && !yoSelected)
            {
                alert("You must select at least one activity and enter its price.");
            }
            else {
                if(cardioSelected && document.getElementById("trnCardioPrice").value <= 0){
                    alert("You must enter a price for Cardio.");
                }
                else if(stSelected && document.getElementById("trnStrengthTrainingPrice").value <= 0){
                    alert("You must enter a price for Strength Training.");
                }
                else if(kbSelected && document.getElementById("trnKickboxingPrice").value <= 0){
                    alert("You must enter a price for Kickboxing.");
                }
                else if(yoSelected && document.getElementById("trnYogaPrice").value <= 0){
                    alert("You must enter a price for Yoga.");
                }
                else {
                    createNewTrainer();
                }      
            }
        } 
    }
    catch(e) {
        console.log(e);
        // return false;
    }
}

function createNewTrainer(){ 
    const trainerApiUrl = "https://localhost:5001/api/Trainer";

    //get trainer data
    let inputEmail = document.getElementById("newTrainerEmail").value;
    let inputPassword = document.getElementById("newTrainerPassword").value;
    let inputFirstName = document.getElementById("trainerFName").value;
    let inputLastName = document.getElementById("trainerLName").value;
    let dob = document.getElementById("trainerBirthDate").value;
    let inputGender = document.getElementById("trainerGender").value;
    
    //To-do: handle training activities and price
    let activities = [];
    if(document.getElementById("trnCardio").checked){
        if(document.getElementById("trnCardioPrice").value > 0){
            activities.push({
                activityId: document.getElementById("trnCardio").value,
                trainerPriceForActivity: document.getElementById("trnCardioPrice").value
            });
        }
    }
    if(document.getElementById("trnStrengthTraining").checked){
        if(document.getElementById("trnStrengthTrainingPrice").value > 0){
            activities.push({
                activityId: document.getElementById("trnStrengthTraining").value,
                trainerPriceForActivity: document.getElementById("trnStrengthTrainingPrice").value
            });
        }
    }
    if(document.getElementById("trnKickboxing").checked){
        if(document.getElementById("trnKickboxingPrice").value > 0){
            activities.push({
                activityId: document.getElementById("trnKickboxing").value,
                trainerPriceForActivity: document.getElementById("trnKickboxingPrice").value
            });
        }
    }
    if(document.getElementById("trnYoga").checked){
        if(document.getElementById("yogaPrice".value > 0)){
            activities.push({
                activityId: document.getElementById("trnYoga").value,
                trainerPriceForActivity: document.getElementById("trnYogaPrice").value
            });
        }
    }
    if(activities.length == 0){
        document.getElementById("chooseActivityErrorMessage").style.display = "block";
    }

    else {
        //set user inputs to a body object
        var bodyObj = {
            fName: inputFirstName,
            lName: inputLastName,
            birthDate: dob,
            gender: inputGender,
            email: inputEmail,
            password: inputPassword, 
            trainerActivities: activities
        };

        //make api call to create trainer
        fetch(trainerApiUrl, {
            method: "POST",
            headers: {
                "Accept": 'application/json',
                "Content-Type": 'application/json'
            },
            body: JSON.stringify(bodyObj)
        }).then(function(response){
            console.log(response);
            console.log("made it to the post");
            sendTrainerToDashboard(inputEmail);
        })
    }
    
}

function sendTrainerToDashboard(email){
    //get new customer's id and send them to Trainer Dashboard
    const getTrainerApiUrl = "https://localhost:5001/api/Trainer/"+email;
    fetch(getTrainerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        let trainerId = json.trainerId;
        window.location.href = "./trainer.html?id="+trainerId;
    }).catch(function(error){
        console.log(error);
    }) 

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
function toggleActivitySelected(inputID){
/* only enable price input fields if its corresponding checkbox is checked */

    if(document.getElementById(inputID).checked){
        //if box gets checked, enable its price input field
        console.log(inputID + " is now checked");
        document.getElementById(inputID+"Price").disabled = false;
    }
    else{
        console.log(inputID + " is now unchecked");
        //if deselected, disable the price input field
        document.getElementById(inputID+"Price").setAttribute("disabled", true);
    } 

}

function handleReferredByOnClick(){
    if(document.getElementById("yesReferred").checked == true){
        document.getElementById("referrerName").disabled = false;
        document.getElementById("referrerName").required;
    }
}
