using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class WriteActivity : IWriteActivity
    {
        public void Write(Activity i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"INSERT into activity (activityName,activityDescription) VALUES (@name,@desc);";
            cmd.Parameters.AddWithValue("@name", i.activityName);
            cmd.Parameters.AddWithValue("@desc", i.description);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}