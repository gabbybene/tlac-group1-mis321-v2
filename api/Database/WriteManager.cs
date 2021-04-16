using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class WriteManager : IWriteManager
    {
        public void Write(Manager i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();;

            cmd.CommandText = @"INSERT into account (Email,Password,AcctType) VALUES (@email,@password,'manager');
                            INSERT into manager (fname, lname, acctID) VALUES (@fname,@lname, (SELECT acctID FROM Account WHERE acctID=@email))";
            cmd.Parameters.AddWithValue("@email", i.email);
            cmd.Parameters.AddWithValue("@fname", i.fName);
            cmd.Parameters.AddWithValue("@lname", i.lName);
            cmd.Parameters.AddWithValue("@password", i.password);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}