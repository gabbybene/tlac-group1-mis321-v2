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
            cmd.CommandText = @"SELECT AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE AppointmentID=@AppointmentID";
            cmd.Parameters.AddWithValue("@AppointmentID",id);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                return new Appointment(){appointmentId=rdr.GetInt32(1),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(2)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(3)),appointmentActivity=readAct.Read(rdr.GetInt32(4)), startTime=rdr.GetDateTime(5), endTime=rdr.GetDateTime(6),appointmentCost=(rdr.GetDouble(7)),amountPaidByCash=rdr.GetDouble(8), amountPaidByCard=rdr.GetDouble(9)};
            }
            return null;        
        }
        public List<Appointment> ReadAll(){
            List<Appointment> returnList = new List<Appointment>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID";
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                returnList.Add(new Appointment(){appointmentId=rdr.GetInt32(1),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(2)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(3)),appointmentActivity=readAct.Read(rdr.GetInt32(4)), startTime=rdr.GetDateTime(5), endTime=rdr.GetDateTime(6),appointmentCost=(rdr.GetDouble(7)),amountPaidByCash=rdr.GetDouble(8), amountPaidByCard=rdr.GetDouble(9)});
            }
            return returnList;
            }
        public List<Appointment> GetCustAvailableAppointmentsByDate(DateTime searchDate){
            List<Appointment> returnList = new List<Appointment>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price,CashAmount,CardAmount FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE CustomerID=null AND starttime LIKE @date";
            cmd.Parameters.AddWithValue("@date",searchDate);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read()){
                IReadCustomer readCust = new ReadCustomer();
                IReadTrainer readTrn = new ReadTrainer();
                IReadActivity readAct = new ReadActivity();
                returnList.Add(new Appointment(){appointmentId=rdr.GetInt32(1),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(2)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(3)),appointmentActivity=readAct.Read(rdr.GetInt32(4)), startTime=rdr.GetDateTime(5), endTime=rdr.GetDateTime(6),appointmentCost=(rdr.GetDouble(7)),amountPaidByCash=rdr.GetDouble(8), amountPaidByCard=rdr.GetDouble(9)});
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
            cmd.CommandText = @"SELECT AppointmentDate,AppointmentID,CustomerID,a.TrainerID,a.ActivityID,starttime,endtime,Price FROM Appointment a JOIN Cando c on a.TrainerID=c.TrainerID AND a.activityID=c.activityID WHERE TrainerId=@TrainerId and date=@date";;
            cmd.Parameters.AddWithValue("@TrainerId",trainerId);
            cmd.Parameters.AddWithValue("@date",date);
            cmd.Prepare();
            cmd.ExecuteReader();
            
            using MySqlDataReader rdr = cmd.ExecuteReader();

            //created new ReadCustomer, ReadTrainer, and ReacActivity to get Customer, Trainer, and Activity data
            IReadCustomer readCust = new ReadCustomer();
            IReadTrainer readTrn = new ReadTrainer();
            IReadActivity readAct = new ReadActivity();
            while (rdr.Read())
            {
                Appointment temp = new Appointment(){appointmentId=rdr.GetInt32(1),appointmentCustomer=readCust.GetCustomerByID(rdr.GetInt32(2)),appointmentTrainer=readTrn.GetTrainerByID(rdr.GetInt32(3)),appointmentActivity=readAct.Read(rdr.GetInt32(4)), startTime=rdr.GetDateTime(5), endTime=rdr.GetDateTime(6),appointmentCost=(rdr.GetDouble(7)),amountPaidByCash=rdr.GetDouble(8), amountPaidByCard=rdr.GetDouble(9)};
                myAppointments.Add(temp);

            }
            return myAppointments;
        }
    }
}