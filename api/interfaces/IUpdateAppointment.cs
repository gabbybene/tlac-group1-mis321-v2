using api.models;
using System.Collections.Generic;
namespace api.interfaces
{
    public interface IUpdateAppointment
    {
        // void Update(Appointment appt);
        public void Update(List<Appointment> al);
    }
}