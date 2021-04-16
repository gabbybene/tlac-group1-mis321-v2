using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class WriteCustomer : IWriteCustomer
    {
        public void Write(Customer i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();



            //Updated Insert operations
            cmd.CommandText = @"INSERT into account (AcctID,Password,AcctType) VALUES (@email, @password,'customer')"; 
            cmd.Parameters.AddWithValue("@email", i.email);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT into customer (fname, lname, DOB, gender, AccountID, phone) VALUES (@fname,@lname,@dob,@gender, (SELECT AcctID FROM Account WHERE AcctID=@email), @phone)";
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@dob", i.birthDate);
            cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@phone", i.phoneNo);
            cmd.Parameters.AddWithValue("@act",null);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            foreach(Activity act in i.customerActivities){
                
                cmd.CommandText = @"INSERT into prefers (CustID,ActivityID) VALUES ((SELECT CustID from Customer where AccountID=@email),@act)";
                cmd.Parameters["@act"].Value= act.activityId;
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}