using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class WriteAppointment : IWriteAppointment
    {
        public void Write(Appointment i, Customer c, Trainer t, List<Activity> al){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"INSERT into appointment (trainerID,Date,StartTime,EndTime,AppointmentCost) VALUES (@trainer,@date,@starttime,@endtime,@cost);";
            cmd.Parameters.AddWithValue("@trainer", t.trainerId);
            cmd.Parameters.AddWithValue("@date", i.appointmentDate);
            cmd.Parameters.AddWithValue("@starttime", i.startTime);
            cmd.Parameters.AddWithValue("@endtime", i.endTime);
            cmd.Parameters.AddWithValue("@cost", i.appointmentCost);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();            
        }
    }
}