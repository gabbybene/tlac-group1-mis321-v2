using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
namespace api.models
{
    public class Manager
    {
        public int managerId {get; set;}
        public string password {get; set;}
        public IWriteCustomer writeCustomer {get; set;}
        public IDeleteCustomer deleteCustomer {get; set;}
        public IReadCustomer readCustomer {get; set;}
        public IWriteTrainer writeTrainer {get; set;}
        public IDeleteTrainer deleteTrainer {get; set;}
        public IReadTrainer readTrainer {get; set;}
        public IWriteActivity writeActivity {get; set;}
        public IDeleteActivity deleteActivity {get; set;}
        public IReadActivity readActivity {get; set;}
        public IWriteAppointment writeAppointment {get; set;}
        public IDeleteAppointment deleteAppointment {get; set;}
        public IReadAppointment readAppointment {get; set;}
        public string fName {get; set;}
        public string lName {get; set;}
        public string email {get; set;}
    }
}