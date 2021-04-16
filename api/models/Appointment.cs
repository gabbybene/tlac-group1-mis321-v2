using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;
namespace api.models
{
    public class Appointment
    {
        public int appointmentId {get; set;}
        public DateTime appointmentDate{get; set;}
        public TimeSpan startTime {get; set;}
        public TimeSpan endTime {get; set;}
        public Trainer appointmentTrainer {get; set;}
        public Customer appointmentCustomer {get; set;}
        public double appointmentCost {get; set;}
        public double amountPaidByCash {get; set;}
        public double amountPaidByCard {get; set;}
        public Activity appointmentActivity {get; set;}
        public IWriteAppointment writeBehavior {get; set;}
        public IDeleteAppointment deleteBehavior {get; set;}
        public IReadAppointment readBehavior {get; set;}
    }
}