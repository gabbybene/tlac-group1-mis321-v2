

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
    //get value of user input email and password
    let inputEmail = document.getElementById("customerEmail").value;
    let inputPassword = document.getElementById("customerPassword").value;

    let customer = "";
    //make API call to DB to get all customers, then search for a customer w/ that email and password
    const customerApiUrl = "https://localhost:5001/api/Customer";
    fetch(customerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        var success = false;
        for(var i in json){
            if(json[i].email == inputEmail && json[i].password == inputPassword){
                success = true;
                customer = json[i];
                console.log("json[i] found: " + json[i].emailAddress + " " + json[i].password);
            }
        }
    }).catch(function(error){
        console.log(error);
    }) 

    if(success){
        //go to the Customer dashboard but send in the customer id into the URL
        // window.location.href = "./customer.html?id=@"+customer.customerId+"@";
        window.location.href = "./customer.html?id="+customer.customerId;
        console.log("customerId is " + customerId);
        
    }
    else {
        //if not successfull, show the error message above the sign in button.
    }

    //PASS IN CUSTOMER ID TO SEND CUSTOMER DATA TO THE DASHBOARD
    // window.location.href = "./customer.html";
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
    console.log("birthDate is " + dob);
    if(document.getElementById("fitnessGoals").value != null){
        inputFitnessGoals = document.getElementById("fitnessGoals").value;
    }
    else{
        inputFitnessGoals = null;
    }
    //To-do: handle preferred activities

    //If yesReferred is checked, get referrerName
    if(document.getElementById("yesReferred").checked){
        let referredByName = document.getElementById("referrerName");
    }

    //set user inputs to a body object
    // var bodyObj = {
    //     "password": password,
    //     "birthDate": dob, 
    //     "gender": gender,
    //     "fitnessGoals": fitnessGoals,
    //     "PhoneNo": "000-123-4567", //putting a fake phoneNo in here for now because the form isn't set up to take in a phone number yet.
    //     "fName": firstName,
    //     "lName": lastName,
    //     "email": email
    //     // referredBy: referredByName,
    // };

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
        email: inputEmail
        // referredBy: referredByName,
    };

    //make api call to create customer
    fetch(customerApiUrl, {
        method: "POST",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json'
        },
        body: JSON.stringify(bodyObj)
    }).then(function(response){
        console.log("made it to the POST");
        console.log(response);
        
    })

    sendCustomerToDashboard(inputEmail);
}

function sendCustomerToDashboard(email){
    //get new customer's id and send them to ./customer.html?id=@id@
    const getCustomerApiUrl = "https://localhost:5001/api/Customer/"+email;
    fetch(getCustomerApiUrl).then(function(response){
        console.log(response);
        return response.json();
    }).then(function(json){
        // for(var i in json){
        //     if(json[i].email == email){
        //         customerId = json[i].customerId;
        //     }
        // }
        let customerId = json.customerId;
        // window.location.href = "./customer.html?customerId="+customerId;
        console.log("customerId is " + customerId);
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

function handleCreateNewTrainerOnClick(){ 
    const trainerApiUrl = "https://localhost:5001/api/Trainer";

    //get customer data
    let email = document.getElementById("trainerEmail").value;
    let password = document.getElementById("trainerPassword").value;
    let firstName = document.getElementById("trainerFirstName").value;
    let lastName = document.getElementById("trainerLastName").value;
    let dob = document.getElementById("trainerBirthDate").value;
    let gender = document.getElementById("trainerGender").value;
    
    //To-do: handle training activities and price


    //set user inputs to a body object
    var bodyObj = {
        "fName": firstName,
        "lName": lastName,
        "birthDate": dob,
        "gender": gender,
        "email": email,
        "password": password, 
        //preferred activities []
    };
    
    //make api call to create customer
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
    })
    //create new trainer (get data and add to DB)
    //go to trainer.html
    //window.location.href = "./trainer.html";
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
