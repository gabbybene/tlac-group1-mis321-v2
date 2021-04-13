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

            cmd.CommandText = @"INSERT into account (Email,Password,AcctType) VALUES (@email,@password,'trainer');
                            INSERT into trainer (fname, lname, DOB, gender, acctID, phone) VALUES (@fname,@lname,@dob,@gender, (SELECT acctID FROM Account WHERE acctID=@email), @phone)";
            cmd.Parameters.AddWithValue("@email", i.email);
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@dob", i.birthDate);
            cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@phone", i.phoneNo);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}