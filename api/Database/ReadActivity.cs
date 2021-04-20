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
        public List<Activity> GetTrainerActivities(int trnID){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT ActivityID, Price FROM CanDo WHERE TrainerID=@trainerID";
            // cmd.Parameters.AddWithValue("@ActivityID",actID);
            cmd.Parameters.AddWithValue("@TrainerID",trnID);
            cmd.Prepare();

            //new List<Activities> to store result
            List<Activity> activities = new List<Activity>();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                Activity temp =  new Activity(){activityId=rdr.GetInt32(0), trainerPriceForActivity = rdr.GetDouble(1)};
                activities.Add(temp);
            }
            return activities;
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