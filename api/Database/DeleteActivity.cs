using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database
{
    public class DeleteActivity : IDeleteActivity
    {
        public void Delete(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"delete from activity where activityid=@ActivityID";
            cmd.Parameters.AddWithValue("@ActivityID",id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}