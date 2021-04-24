using System;
using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class UpdateCustomer : IUpdateCustomer
    {
        public void Update(Customer i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            

            if(i.email!=""){
                Console.WriteLine("Updating Email to "+i.email +"Customer ID: "+i.customerId);
                cmd.CommandText = @"UPDATE account SET AcctID=@newemail WHERE AcctID=(SELECT AccountID from Customer WHERE CustID=@cust)";
                cmd.Parameters.AddWithValue("@cust", i.customerId);
                cmd.Parameters.AddWithValue("@newemail", i.email);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            if(i.password!=""){
                cmd.CommandText = @"UPDATE account SET Password=@password WHERE AcctID=(SELECT AccountID from Customer WHERE CustID=@cust)";
                cmd.Parameters.AddWithValue("@password", i.password);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = @"UPDATE customer SET fName=@fname, lName=@lname, DOB=@dob, Gender=@gender, Phone=@phone, fitnessGoal=@goal WHERE CustID=@cust";
            cmd.Parameters.AddWithValue("@cust", i.customerId);
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@dob", i.birthDate);
            cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@phone", i.phoneNo);
            cmd.Parameters.AddWithValue("@goal", i.fitnessGoals);
            Console.WriteLine(i.fitnessGoals);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Parameters.AddWithValue("@CustID", i.customerId);
            cmd.Parameters.AddWithValue("@act",null);
            IDeleteActivity deleteObj = new DeleteActivity();
            deleteObj.DeletePreferredActivitities(i.customerId);
            foreach(Activity act in i.customerActivities){  
                System.Console.WriteLine(act.activityId);
                cmd.CommandText = @"INSERT into prefers (CustID,ActivityID) VALUES (@CustID,@act)";
                cmd.Parameters["@act"].Value= act.activityId;
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            if(i.referredBy!=null){
                Console.WriteLine("Referral Customer Not Null" + i.referredBy.customerId);
                cmd.CommandText = @"UPDATE customer SET Refer_CustID=@referrer WHERE CustID=@cust";
                cmd.Parameters.AddWithValue("@referrer", i.referredBy.customerId);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            else if(i.referredBy==null){
                cmd.CommandText = @"UPDATE customer SET Refer_CustID=null WHERE CustID=@cust";
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}