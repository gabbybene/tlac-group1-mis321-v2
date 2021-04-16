using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;

namespace api.models
{
    public class Trainer 
    {
        public int trainerId {get; set;}
        public string password {get; set;}
        public DateTime birthDate {get; set;}
        public string gender {get; set;}
        public string phoneNo {get; set;}
        public List<Activity> trainerActivities {get; set;}
        public IWriteTrainer writeBehavior {get; set;}
        public IDeleteTrainer deleteBehavior {get; set;}
        public IReadTrainer readBehavior {get; set;}

        public string fName {get; set;}
        public string lName {get; set;}
        public string email {get; set;}



        
    }
}