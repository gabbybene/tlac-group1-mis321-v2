using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class ReadActivity : IReadActivity
    {
        public models.Activity Read(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT (ActivityID,ActivityName,ActivityDescription) FROM Activity WHERE ActivityID=@ActivityID";
            cmd.Parameters.AddWithValue("@ActivityID",id);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                return new Activity(){activityId=rdr.GetInt32(1),activityName=rdr.GetString(2),description=rdr.GetString(3)};
            }
            return null;
        }
        public Activity GetTrainerActivity(int trnID,int actID){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT (ActivityID,ActivityName,ActivityDescription) FROM CanDo WHERE ActivityID=@ActivityID AND TrainerID=@trainerID";
            cmd.Parameters.AddWithValue("@ActivityID",actID);
            cmd.Parameters.AddWithValue("@TrainerID",trnID);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                return new Activity(){activityId=rdr.GetInt32(1),activityName=rdr.GetString(2),description=rdr.GetString(3)};
            }
            return null;
        }

        public List<models.Activity> ReadAll()
        {
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            throw new System.NotImplementedException();
        }
    }
}