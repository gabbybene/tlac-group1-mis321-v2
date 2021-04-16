using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class DeleteAppointment : IDeleteAppointment
    {
        public void Delete(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"delete from appointment where AppointmentID=@AppointmentID";
            cmd.Parameters.AddWithValue("@AppointmentID",id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}