using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;
namespace api.models
{
    public class Customer
    {
       public int customerId {get; set;}
       public string password{get; set;}
       public DateTime birthDate {get; set;}
       public string gender {get; set;}
       public string phoneNo {get; set;}
       public Customer referredBy  {get; set;}
       public string fitnessGoals {get; set;}
       public List<Activity> customerActivities {get; set;} 
       public IWriteCustomer writeBehavior {get; set;}
       public IDeleteCustomer deleteBehavior {get; set;}
       public IReadCustomer readBehavior {get; set;}
        public string fName {get; set;}
        public string lName {get; set;}
        public string email {get; set;}

    }
}