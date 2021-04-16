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
            cmd.CommandText = @"SELECT * FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID WHERE a.AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID",emailAddress);
            cmd.Prepare();
            
            Trainer trainer = new Trainer();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                trainer = temp;
            }
            return trainer;
        }
        public Trainer GetTrainerByID(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT * FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID WHERE trainerID=@trainerID";
            cmd.Parameters.AddWithValue("@trainerID",id);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            Trainer trainer = new Trainer();
            while (rdr.Read()){
                Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                trainer = temp;
            }
            return trainer;
            
        }
        public List<Trainer> ReadAll(){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT * FROM Trainer t JOIN Account a ON t.AcctID=a.AcctID";
            using MySqlDataReader rdr = cmd.ExecuteReader();

            List<Trainer> allTrainers = new List<Trainer>();
            while(rdr.Read()){
                Trainer temp = new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5), password=rdr.GetString(7)};
                allTrainers.Add(temp);
            }
            return allTrainers;
        }
    }
}