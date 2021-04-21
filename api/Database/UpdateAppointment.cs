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
                cmd.CommandText = @"UPDATE appointment SET appointmentID=@apptID,date=@date,TrainerID=@trnID,ActivityID=@actID,CustomerID=@custID,startTime=@start,endTime=@end,CashAmount=@cash,CardAmount=@card) WHERE activityID=@activityid;";
                cmd.Parameters.AddWithValue("@apptID", i.appointmentId);
                cmd.Parameters.AddWithValue("@date", i.appointmentDate);
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
        public void UpdateAddCustomerId(int[] idArray){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET CustomerID=@custID WHERE AppointmentID=@apptID;";
            //in id array, idArray[0] is custID, and idArray[1] is apptID
            cmd.Parameters.AddWithValue("@custID", idArray[0]);
            cmd.Parameters.AddWithValue("@apptID", idArray[1]);
            // cmd.Parameters.AddWithValue("@custID", appt.appointmentCustomer.customerId);
            // cmd.Parameters.AddWithValue("@apptID", appt.appointmentId);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }  

        public void UpdateAddCustomer(Appointment i){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET CustomerID=@custID, CashAmount=@cashAmt, CardAmount=@cardAmt WHERE AppointmentID=@apptID;";
            cmd.Parameters.AddWithValue("@apptID", i.appointmentId);
            cmd.Parameters.AddWithValue("@custID", i.appointmentCustomer.customerId);
            cmd.Parameters.AddWithValue("@cashAmt", i.amountPaidByCash);
            cmd.Parameters.AddWithValue("@cardAmt", i.amountPaidByCard);
            cmd.Connection = con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }     
        
        public void UpdateDeleteCustomerId(/*Appointment appt*/int[] idArray){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET CustomerID=null WHERE AppointmentID=@apptID AND CustomerID=@custID;";
            //in id array, idArray[0] is custID, and idArray[1] is apptID
            cmd.Parameters.AddWithValue("@custID", idArray[0]);
            cmd.Parameters.AddWithValue("@apptID", idArray[1]);
            // cmd.Parameters.AddWithValue("@apptID", appt.appointmentId);
            cmd.Connection=con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void UpdateAvailableAppointment(int apptId, string startTime, string endTime, int activityId){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE appointment SET ActivityID=@actID, startTime=@start,endTime=@end WHERE AppointmentID=@apptID";
            cmd.Parameters.AddWithValue("@apptID", apptId);
            cmd.Parameters.AddWithValue("@actID", activityId);
            cmd.Parameters.AddWithValue("@start", startTime);
            cmd.Parameters.AddWithValue("@end", endTime);
            cmd.Connection = con;
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}