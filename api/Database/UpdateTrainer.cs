using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class UpdateTrainer : IUpdateTrainer
    {
        public void Update(Trainer i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            //this is how the update can be done in one query?
            // cmd.CommandText = @"UPDATE account a, trainer t SET a.AcctID=@AcctID, t.AcctID=@AcctID, a.Password=@password,  t.fName=@fname, t.lName=@lname, t.DOB=@dob, t.Gender=@gender WHERE a.AcctID=@AcctID";
            cmd.CommandText = @"UPDATE account SET AcctID=@AcctID, Password=@password WHERE AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID", i.email);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"UPDATE trainer SET AcctID=@email, fName=@fname, lName=@lname, DOB=@dob, Gender=@gender WHERE AcctID=@email";
            cmd.Parameters.AddWithValue("@email", i.email);
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
            cmd.Parameters.AddWithValue("@trainerId", null);
            IDeleteActivity deleteObj = new DeleteActivity();
            deleteObj.DeleteTrainerActivitities(i.trainerId);
           foreach(Activity act in i.trainerActivities){
                
                cmd.CommandText = @"INSERT into cando (TrainerID,ActivityID,Price) VALUES (@trainerId,@act,@price)";
                cmd.Parameters["@act"].Value = act.activityId;
                cmd.Parameters["@price"].Value = act.trainerPriceForActivity;
                cmd.Parameters["@trainerId"].Value = i.trainerId;
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            
        }
    }
}