using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class UpdateManager : IUpdateManager
    {
        public void Update(models.Manager i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE account SET AccountID=@email, Password=@password WHERE AccountID=@email;
                            UPDATE manager SET name=@name, DOB=@dob, gender=@gender WHERE AccountID=@email";
            cmd.Parameters.AddWithValue("@email", i.email);
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            //the below lines were commented out because Manager does not have a birthDate or gender at this time
            // cmd.Parameters.AddWithValue("@dob", i.birthDate);
            // cmd.Parameters.AddWithValue("@gender", i.gender);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}