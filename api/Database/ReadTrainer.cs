using System.Collections.Generic;
using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class ReadTrainer : IReadTrainer
    {
        public models.Trainer Read(string emailAddress){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT trainerid,fname,lname,dob,gender,t.acctid,a.acctid,a.password,phone FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID WHERE a.AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID",emailAddress);
            cmd.Prepare();
            
            Trainer trainer = new Trainer();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                 if(rdr.IsDBNull(8)){
                    //if phoneNo is null, return trainer without phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    trainer = temp;
                }
                else {
                    //if phone is not null, return trainer with phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7), phoneNo=rdr.GetString(8)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    trainer = temp;
                }
               
            }
            return trainer;
        }
        public Trainer GetTrainerByID(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT trainerid,fname,lname,dob,gender,t.acctid,a.acctid,a.password,phone FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID WHERE trainerID=@trainerID";
            cmd.Parameters.AddWithValue("@trainerID",id);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            Trainer trainer = new Trainer();
            while (rdr.Read()){
                if(rdr.IsDBNull(8)){
                    //if phoneNo is null, return trainer without phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    trainer = temp;
                }
                else {
                    //if phone is not null, return trainer with phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7), phoneNo=rdr.GetString(8)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    trainer = temp;
                }
                
            }
            return trainer;
            
        }
        public List<Trainer> ReadAll(){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT trainerid,fname,lname,dob,gender,t.acctid,a.acctid,a.password,phone FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID";
            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Trainer> allTrainers = new List<Trainer>();
            while(rdr.Read()){
                
                if(rdr.IsDBNull(8)){
                    //if phoneNo is null, return trainer without phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    allTrainers.Add(temp);
                }
                else {
                    //if phone is not null, return trainer with phone
                    Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7), phoneNo=rdr.GetString(8)};
                    temp.trainerActivities = GetTrainerActivities(temp);
                    allTrainers.Add(temp);
                }

            }
            return allTrainers;
        }
        private List<Activity> GetTrainerActivities(Trainer trn){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT c.activityid,activityname,price from cando c join activity a on c.activityid=a.activityid WHERE c.trainerID=@trn";
            cmd.Parameters.AddWithValue("@trn",trn.trainerId);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            List<Activity> returnList = new List<Activity>();
            while(rdr.Read()){
                returnList.Add(new Activity(){activityId=rdr.GetInt32(0),activityName=rdr.GetString(1),trainerPriceForActivity=rdr.GetDouble(2)});
            }
            return returnList;
        }
    }
}