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
            cmd.CommandText = @"SELECT (TrainerID,fName,lName,DOB,Gender,AcctID) FROM Trainer WHERE AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID",emailAddress);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                return new Trainer(){trainerId=rdr.GetInt32(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), email=rdr.GetString(6)};
            }
            return null;
        }
        public Trainer GetTrainerByID(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT TrainerID,fName,lName,DOB,Gender,AcctID FROM Trainer WHERE TrainerID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID",id);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                return new Trainer(){trainerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2),birthDate=rdr.GetDateTime(3), gender=rdr.GetString(4), email=rdr.GetString(5)};
            }
            return null;
        }
        public List<models.Trainer> ReadAll(){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            throw new System.NotImplementedException();
        }
    }
}