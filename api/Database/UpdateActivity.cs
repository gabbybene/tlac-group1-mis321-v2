using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class UpdateActivity : IUpdateActivity
    {
        public void Update(List<Activity> al){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            foreach(Activity i in al){
                cmd.CommandText = @"UPDATE activity SET activityName=@name) WHERE activityID=@activityid;";
                cmd.Parameters.AddWithValue("@name", i.activityName);
                cmd.Parameters.AddWithValue("activityid", i.activityId);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}