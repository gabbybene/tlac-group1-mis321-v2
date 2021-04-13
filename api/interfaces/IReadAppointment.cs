using System.Collections.Generic;
using api.models;

namespace api.interfaces
{
    public interface IReadAppointment
    {
        Appointment Read(int id);
        List<Appointment> ReadAll();
    }
}