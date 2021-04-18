using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace api.Database{
    public class ReadAppointment : IReadAppointment
    {
        public Appointment Read(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
             cmd.CommandText = @"SELECT AppointmentID,COALESCE(CustomerID,0),a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount,date FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE AppointmentID=@AppointmentID";
            // cmd.CommandText = @"SELECT AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount,date FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE AppointmentID=@AppointmentID";
            cmd.Parameters.AddWithValue("@AppointmentID",id);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();

            Appointment appt = new Appointment();
            while (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                Appointment temp = new Appointment(){appointmentId=rdr.GetInt32(0),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(1)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(2)),appointmentActivity=readAct.Read(rdr.GetInt32(3)), startTime=rdr.GetDateTime(4).TimeOfDay, endTime=rdr.GetDateTime(5).TimeOfDay,appointmentCost=(rdr.GetDouble(6)),amountPaidByCash=rdr.GetDouble(7), amountPaidByCard=rdr.GetDouble(8),appointmentDate=rdr.GetDateTime(9)};
                appt = temp;
            }
            return appt;       
        }
        public List<Appointment> ReadAll(){
            List<Appointment> returnList = new List<Appointment>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT AppointmentID,COALESCE(CustomerID,0),a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount,date FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID";
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                Console.WriteLine(rdr.GetInt32(0));
                returnList.Add(new Appointment(){appointmentId=rdr.GetInt32(0),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(1)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(2)),appointmentActivity=readAct.Read(rdr.GetInt32(3)), startTime=rdr.GetDateTime(4).TimeOfDay, endTime=rdr.GetDateTime(5).TimeOfDay,appointmentCost=(rdr.GetDouble(6)),amountPaidByCash=rdr.GetDouble(7), amountPaidByCard=rdr.GetDouble(8),appointmentDate=rdr.GetDateTime(9)});
            }
            return returnList;
        }
        public List<Appointment> ReadAvailableAppointmentsByDate(DateTime searchDate){
            string searchDateString = searchDate.ToString("yyyy-MM-dd");
            List<Appointment> returnList = new List<Appointment>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT AppointmentID,IFNULL(CustomerID,0) AS CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount,date FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE CustomerID is null AND date LIKE @date";
            cmd.Parameters.AddWithValue("@date",searchDateString);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                returnList.Add(new Appointment(){appointmentId=rdr.GetInt32(0),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(1)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(2)),appointmentActivity=readAct.Read(rdr.GetInt32(3)), startTime=rdr.GetDateTime(4).TimeOfDay, endTime=rdr.GetDateTime(5).TimeOfDay,appointmentCost=(rdr.GetDouble(6)),amountPaidByCash=rdr.GetDouble(7), amountPaidByCard=rdr.GetDouble(8),appointmentDate=rdr.GetDateTime(9)});
            }
            return returnList;
            }
        public List<DateTime> ReadDistinctAvailableAppointments()
        {
            List<DateTime> returnList = new List<DateTime>();
            ConnectionString conStr = new ConnectionString();
            string cs = conStr.cs;
            //Open the connection
            using var con = new MySqlConnection(cs);
            con.Open();
            
            //Statement
            string stm = @"SELECT DISTINCT date FROM Appointment WHERE CustomerID IS NULL ORDER BY date asc";
            using var cmd = new MySqlCommand(stm, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                returnList.Add(rdr.GetDateTime(0));

            }
            return returnList;
        }
             
        public List<Appointment> ReadAvailableAppointmentsByDateForTrainer(int trainerId, DateTime date)
        {
            List<Appointment> myAppointments = new List<Appointment>();
            
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT date,AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE TrainerId=@TrainerId and date=@date ORDER BY starttime ASC";
            cmd.Parameters.AddWithValue("@TrainerId",trainerId);
            cmd.Parameters.AddWithValue("@date",date);
            cmd.Prepare();
          
            using MySqlDataReader rdr = cmd.ExecuteReader();

            //created new ReadCustomer, ReadTrainer, and ReacActivity to get Customer, Trainer, and Activity data
            IReadCustomer readCust = new ReadCustomer();
            IReadTrainer readTrn = new ReadTrainer();
            IReadActivity readAct = new ReadActivity();
            while (rdr.Read())
            {
                Appointment temp = new Appointment(){appointmentId=rdr.GetInt32(1),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(2)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(3)),appointmentActivity=readAct.Read(rdr.GetInt32(4)), startTime=rdr.GetDateTime(5).TimeOfDay, endTime=rdr.GetDateTime(6).TimeOfDay,appointmentCost=(rdr.GetDouble(7)),amountPaidByCash=rdr.GetDouble(8), amountPaidByCard=rdr.GetDouble(9)};
                myAppointments.Add(temp);

            }
            return myAppointments;
        }

        public List<Appointment> ReadConfirmedAppointmentsForCustomer(int customerId){
            List<Appointment> appointments = new List<Appointment>();

            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            // cmd.CommandText = @"SELECT * FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE CustomerID=@CustomerId";
            cmd.CommandText = @"SELECT date,startTime, endTime, AppointmentID, a.TrainerID, CustomerID, a.ActivityID, CashAmount, CardAmount, Price FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE CustomerID=@CustomerId ORDER BY a.date ASC";
            
            cmd.Parameters.AddWithValue("@CustomerId",customerId);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            IReadCustomer readCust = new ReadCustomer();
            IReadTrainer readTrn = new ReadTrainer();
            IReadActivity readAct = new ReadActivity();
            while(rdr.Read()){
                Appointment temp = new Appointment(){appointmentDate=rdr.GetDateTime(0), startTime=rdr.GetDateTime(1).TimeOfDay, endTime=rdr.GetDateTime(2).TimeOfDay, appointmentId=rdr.GetInt32(3), appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(4)), appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(5)), appointmentActivity=readAct.Read(rdr.GetInt32(6)), amountPaidByCash=rdr.GetDouble(7), amountPaidByCard=rdr.GetDouble(8), appointmentCost=rdr.GetDouble(9)};
                appointments.Add(temp);
            }

            return appointments;
        }

         public List<Appointment> ReadConfirmedAppointmentsForTrainer(int trainerId){
            List<Appointment> appointments = new List<Appointment>();

            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            // cmd.CommandText = @"SELECT * FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE CustomerID=@CustomerId";
            cmd.CommandText = @"SELECT date,startTime, endTime, AppointmentID, a.TrainerID, CustomerID, a.ActivityID, CashAmount, CardAmount, Price FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.CustomerID IS NOT NULL AND a.activityID=c.activityID WHERE a.TrainerID =@TrainerId ORDER BY a.date ASC";
            
            cmd.Parameters.AddWithValue("@TrainerId",trainerId);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            IReadCustomer readCust = new ReadCustomer();
            IReadTrainer readTrn = new ReadTrainer();
            IReadActivity readAct = new ReadActivity();
            while(rdr.Read()){
                Appointment temp = new Appointment(){appointmentDate=rdr.GetDateTime(0), startTime=rdr.GetDateTime(1).TimeOfDay, endTime=rdr.GetDateTime(2).TimeOfDay, appointmentId=rdr.GetInt32(3), appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(4)), appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(5)), appointmentActivity=readAct.Read(rdr.GetInt32(6)), amountPaidByCash=rdr.GetDouble(7), amountPaidByCard=rdr.GetDouble(8), appointmentCost=rdr.GetDouble(9)};
                appointments.Add(temp);
            }

            return appointments;
        }
    }
}