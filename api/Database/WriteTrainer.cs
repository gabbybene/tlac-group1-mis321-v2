using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;


namespace api.Database
{
    public class WriteTrainer : IWriteTrainer
    {
        public void Write(Trainer i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"INSERT into account (AcctID,Password,AcctType) VALUES (@email, @password,'customer')"; 
            cmd.Parameters.AddWithValue("@email", i.email);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT into trainer (fName, lName, DOB, Gender, AcctID, Phone) VALUES (@fname,@lname,@dob,@gender, (SELECT acctID FROM Account WHERE acctID=@email), @phone)";
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@dob", i.birthDate);
            cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@phone", i.phoneNo);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Parameters.AddWithValue("@act",null);
            cmd.Parameters.AddWithValue("@price", null);
            foreach(Activity act in i.trainerActivities){
                
                cmd.CommandText = @"INSERT into cando (TrainerID,ActivityID,Price) VALUES ((SELECT TrainerID from Trainer where AcctID=@email),@act,@price)";
                cmd.Parameters["@act"].Value = act.activityId;
                cmd.Parameters["@price"].Value = act.trainerPriceForActivity;
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}