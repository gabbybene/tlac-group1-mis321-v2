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
                cmd.CommandText = @"UPDATE appointment SET appointmentID=@apptID,TrainerID=@trnID,ActivityID=@actID,CustomerID=@custID,startTime=@start,endTime=@end,CashAmount=@cash,CardAmount=@card) WHERE activityID=@activityid;";
                cmd.Parameters.AddWithValue("@apptID", i.appointmentId);
                cmd.Parameters.AddWithValue("@trnID", i.appointmentTrainer.trainerId);
                cmd.Parameters.AddWithValue("@actID", i.appointmentActivity.activityId);
                cmd.Parameters.AddWithValue("@custID", i.appointmentCustomer.customerId);
                cmd.Parameters.AddWithValue("@start", i.startTime);
                cmd.Parameters.AddWithValue("@end", i.endTime);
                cmd.Parameters.AddWithValue("@cash", i.amountPaidByCash);
                cmd.Parameters.AddWithValue("@card", i.amountPaidByCard);
                cmd.Connection=con;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddCustToAppt(Appointment appt,int custID){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET customerID=@custID) WHERE AppointmentID=@apptID;";
            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Parameters.AddWithValue("@apptID", appt.appointmentId);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        public void RemoveCustFromAppt(Appointment appt){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET customerID=null) WHERE AppointmentID=@apptID;";
            cmd.Parameters.AddWithValue("@apptID", appt.appointmentId);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}