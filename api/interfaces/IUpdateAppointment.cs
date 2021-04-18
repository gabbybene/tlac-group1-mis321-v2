using api.models;
using System.Collections.Generic;
namespace api.interfaces
{
    public interface IUpdateAppointment
    {
        // void Update(Appointment appt);
        void Update(List<Appointment> al);
        void UpdateAddCustomerId(int[] intArray);
        void UpdateDeleteCustomerId(int[] intArray);
    }
}