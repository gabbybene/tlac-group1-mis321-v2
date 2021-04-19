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
            cmd.CommandText = @"SELECT ActivityID,ActivityName FROM Activity WHERE ActivityID=@ActivityID";
            cmd.Parameters.AddWithValue("@ActivityID",id);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                return new Activity(){activityId=rdr.GetInt32(0),activityName=rdr.GetString(1)};
            }
            return null;
        }
        // public Activity GetTrainerActivity(int trnID,int actID){
        //     ConnectionString cs = new ConnectionString();
        //     using var con = new MySqlConnection(cs.cs);
        //     con.Open();
        //     using var cmd = new MySqlCommand();
        //     cmd.Connection = con;
        //     cmd.CommandText = @"SELECT (ActivityID,ActivityName) FROM CanDo WHERE ActivityID=@ActivityID AND TrainerID=@trainerID";
        //     cmd.Parameters.AddWithValue("@ActivityID",actID);
        //     cmd.Parameters.AddWithValue("@TrainerID",trnID);
        //     cmd.Prepare();
        //     MySqlDataReader rdr = cmd.ExecuteReader();
        //     if (rdr.Read()){
        //         return new Activity(){activityId=rdr.GetInt32(0),activityName=rdr.GetString(1)};
        //     }
        //     return null;
        // }

        public List<int> GetTrainerActivities(int trnID){
            //returns a list of the ids from CanDo table where TrainerID matches id passed in
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT ActivityID FROM CanDo WHERE TrainerID=@trainerID";
            cmd.Parameters.AddWithValue("@TrainerID",trnID);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<int> activityIDs = new List<int>();
            while (rdr.Read()){
                activityIDs.Add(rdr.GetInt32(0));
            }
            return activityIDs;
        }

        public List<Activity> ReadAll()
        {
            List<Activity> returnList = new List<Activity>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT ActivityID,ActivityName FROM Activity";
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                returnList.Add(new Activity(){activityId=rdr.GetInt32(0),activityName=rdr.GetString(1)});
            }
            return returnList;
        }
    }
}