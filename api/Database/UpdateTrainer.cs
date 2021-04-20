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

<<<<<<< HEAD
<<<<<<< HEAD
            cmd.CommandText = @"UPDATE account SET AccountID=@email, Password=@password WHERE AcctID=(SELECT AcctID from Trainer WHERE TrainerID=@trnid);
                            UPDATE trainer SET fname=@fname, lname=@lname DOB=@dob, gender=@gender phone=@phone WHERE AccountID=@email";
            cmd.Parameters.AddWithValue("@trnid", i.trainerId);
            cmd.Parameters.AddWithValue("@email", i.email);
=======
            //this is how the update can be done in one query
            cmd.CommandText = @"UPDATE account a, trainer t SET a.AcctID=@AcctID, t.AcctID=@AcctID, a.Password=@password,  t.fName=@fname, t.lName=@lname, t.DOB=@dob, t.Gender=@gender WHERE a.AcctID=@AcctID";
            // cmd.CommandText = @"UPDATE account SET AcctID=@AcctID, Password=@password WHERE AcctID=@AcctID";
=======
            //this is how the update can be done in one query?
            // cmd.CommandText = @"UPDATE account a, trainer t SET a.AcctID=@AcctID, t.AcctID=@AcctID, a.Password=@password,  t.fName=@fname, t.lName=@lname, t.DOB=@dob, t.Gender=@gender WHERE a.AcctID=@AcctID";
            cmd.CommandText = @"UPDATE account SET AcctID=@AcctID, Password=@password WHERE AcctID=@AcctID";
>>>>>>> 00885447d603e0e6afcb0e0cc67ebff8dfe75caf
            cmd.Parameters.AddWithValue("@AcctID", i.email);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();

<<<<<<< HEAD
            // cmd.CommandText = @"UPDATE trainer SET AcctID=@email, fName=@fname, lName=@lname, DOB=@dob, Gender=@gender WHERE AcctID=@email";
            // cmd.Parameters.AddWithValue("@email", i.email);
>>>>>>> dfa569ca802d411d6e3e1b66f06f22f25241bb3a
=======
            cmd.CommandText = @"UPDATE trainer SET AcctID=@email, fName=@fname, lName=@lname, DOB=@dob, Gender=@gender WHERE AcctID=@email";
            cmd.Parameters.AddWithValue("@email", i.email);
>>>>>>> 00885447d603e0e6afcb0e0cc67ebff8dfe75caf
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