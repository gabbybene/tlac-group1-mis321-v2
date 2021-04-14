using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class WriteAppointment : IWriteAppointment
    {
        public void Write(Appointment i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"INSERT into appointment (trainerID,Date,StartTime,EndTime,AppointmentCost,ActivityID,CashAmount,CardAmount) VALUES (@trainer,@date,@starttime,@endtime,@cost,@actid,@cash,@card);";
            cmd.Parameters.AddWithValue("@trainer", i.appointmentTrainer.trainerId);
            cmd.Parameters.AddWithValue("@date", i.appointmentDate);
            cmd.Parameters.AddWithValue("@starttime", i.startTime);
            cmd.Parameters.AddWithValue("@endtime", i.endTime);
            cmd.Parameters.AddWithValue("@cost", i.appointmentCost);
            cmd.Parameters.AddWithValue("@actid", i.appointmentActivity.activityId);
            cmd.Parameters.AddWithValue("@cash", i.amountPaidByCash);
            cmd.Parameters.AddWithValue("@card", i.amountPaidByCard);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}