using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class UpdateAppointment : IUpdateAppointment
    {
        public void Update(List<Appointment> al){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            foreach(Appointment i in al){
                cmd.CommandText = @"UPDATE activity SET activityName=@name,activityDescription=@desc) WHERE activityID=@activityid;";
                cmd.Parameters.AddWithValue("@name", i.appointmentActivity.activityName);
                cmd.Parameters.AddWithValue("@desc", i.appointmentActivity.description);
                cmd.Parameters.AddWithValue("activityid", i.appointmentActivity.activityId);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateAddCustomerId(Appointment a)
        {

        }

        public void UpdateDeleteCustomerId(Appointment a)
        {
            
        }
    }
}