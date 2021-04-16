using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class ReadManager : IReadManager
    {
        
        public Manager GetManagerByID(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT ManagerID,fname,lname,m.acctid,password FROM Manager m JOIN Account a ON m.AcctID=a.AcctID WHERE managerID=@managerID";
            cmd.Parameters.AddWithValue("@managerID",id);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            Manager manager = new Manager();
            while (rdr.Read()){
                Manager temp = new Manager(){managerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2), email=rdr.GetString(3), password=rdr.GetString(4)};
                manager = temp;
            }
            return manager;
        }
        public List<models.Manager> ReadAll(){
            List<Manager> returnList = new List<Manager>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT ManagerID,fname,lname,m.acctid,password FROM Manager m JOIN Account a ON m.AcctID=a.AcctID";
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            Manager manager = new Manager();
            while (rdr.Read()){
                returnList.Add(new Manager(){managerId=rdr.GetInt32(0),fName=rdr.GetString(1),lName=rdr.GetString(2), email=rdr.GetString(3), password=rdr.GetString(4)});
            }
            return returnList;
        }
    }
}