using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IWriteAppointment
    {
        //  public void Write(Appointment myAppointment);
        public void Write(Appointment i, Customer c, Trainer t, List<Activity> al);
    }
}