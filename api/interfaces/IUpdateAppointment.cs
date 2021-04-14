using api.models;
using System.Collections.Generic;
namespace api.interfaces
{
    public interface IUpdateAppointment
    {
        // void Update(Appointment appt);
        void Update(List<Appointment> al);
        void UpdateAddCustomerId(Appointment appt, int custID);
        void UpdateDeleteCustomerId(Appointment appt);
    }
}