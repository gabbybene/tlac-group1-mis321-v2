using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class DeleteTrainer : IDeleteTrainer
    {
        public void Delete(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            IReadTrainer readObj = new ReadTrainer();
            Trainer delObj = readObj.GetTrainerByID(id);
            cmd.CommandText = @"delete from account where AcctID=@email";
            cmd.Parameters.AddWithValue("@email",delObj.email);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}