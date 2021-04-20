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

            cmd.CommandText = @"INSERT into appointment (trainerID,Date,StartTime,EndTime,ActivityID,CashAmount,CardAmount) VALUES (@trainer,@date,@starttime,@endtime,@actid,@cash,@card);";
            cmd.Parameters.AddWithValue("@trainer", i.appointmentTrainer.trainerId);
            cmd.Parameters.AddWithValue("@date", i.appointmentDate);
            cmd.Parameters.AddWithValue("@starttime", i.startTime);
            cmd.Parameters.AddWithValue("@endtime", i.endTime);
            cmd.Parameters.AddWithValue("@actid", i.appointmentActivity.activityId);
            cmd.Parameters.AddWithValue("@cash", i.amountPaidByCash);
            cmd.Parameters.AddWithValue("@card", i.amountPaidByCard);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void WriteAvailableAppointment(Appointment i, string startTime, string endTime){
            //for when trainers create available appointments
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"INSERT into appointment (TrainerID,date,starttime, endtime,ActivityID) VALUES (@trainer,@date,@starttime,@endtime,@actid);";
            cmd.Parameters.AddWithValue("@trainer", i.appointmentTrainer.trainerId);
            cmd.Parameters.AddWithValue("@date", i.appointmentDate);
            cmd.Parameters.AddWithValue("@starttime", startTime);
            cmd.Parameters.AddWithValue("@endtime", endTime);
            cmd.Parameters.AddWithValue("@actid", i.appointmentActivity.activityId);

            //How do we keep track of the appointment cost?
            cmd.Connection = con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}