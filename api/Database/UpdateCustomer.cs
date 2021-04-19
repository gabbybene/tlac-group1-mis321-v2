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

            cmd.CommandText = @"UPDATE account SET AcctID=@AcctID, Password=@password WHERE AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID", i.email);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"UPDATE customer SET fName=@fname, lName=@lname, DOB=@dob, Gender=@gender, Phone=@phone, fitnessGoal=@goal WHERE CustID=@cust";
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@dob", i.birthDate);
            cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@phone", i.phoneNo);
            cmd.Parameters.AddWithValue("@goal", i.fitnessGoals);
            cmd.Parameters.AddWithValue("@cust", i.customerId);
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
                cmd.CommandText = @"UPDATE customer SET Refer_CustID=(SELECT CustID from Customer where AccountID=@referrer) WHERE CustID=@cust)";
                cmd.Parameters.AddWithValue("@referrer", i.referredBy.email);
            }
        }

        //below is the original method before I began modifying
        // public void Update(models.Customer i){
        //     ConnectionString cs = new ConnectionString();
        //     using var con = new MySqlConnection(cs.cs);
        //     con.Open();
        //     using var cmd = new MySqlCommand();

        //     cmd.CommandText = @"UPDATE account SET AccountID=@email, Password=@password WHERE AccountID=@email;
        //                     UPDATE customer SET fname=@fname, lname=@lname DOB=@dob, gender=@gender phone=@phone WHERE AccountID=@email";
        //     cmd.Parameters.AddWithValue("@email", i.email);
        //     cmd.Parameters.AddWithValue("@fname", i.fName);
        //     cmd.Parameters.AddWithValue("@lname", i.lName);
        //     cmd.Parameters.AddWithValue("@dob", i.birthDate);
        //     cmd.Parameters.AddWithValue("@gender", i.gender);
        //     cmd.Parameters.AddWithValue("@phone", i.phoneNo);
        //     cmd.Parameters.AddWithValue("@password", i.password);
        //     cmd.Connection=con;
        //     cmd.Prepare();
        //     cmd.ExecuteNonQuery();
        //     cmd.Parameters.AddWithValue("@act",null);
        //     IDeleteActivity deleteObj = new DeleteActivity();
        //     deleteObj.DeletePreferredActivitities(i.customerId);
        //     foreach(Activity act in i.customerActivities){                
        //         cmd.CommandText = @"INSERT into prefers (CustID,ActivityID) VALUES ((SELECT CustID from Customer where AccountID=@email),@act)";
        //         cmd.Parameters["@act"].Value= act.activityId;
        //         cmd.Connection=con;
        //         cmd.Prepare();
        //         cmd.ExecuteNonQuery();
        //     }
        // }
    }
}