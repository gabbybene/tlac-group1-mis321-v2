using System.Collections.Generic;
using api.models;
using System;

namespace api.interfaces
{
    public interface IReadAppointment
    {
        Appointment Read(int id);
        List<Appointment> ReadAll();
        List<DateTime> ReadDistinctAvailableAppointments();
        List<Appointment> ReadAvailableAppointmentsByDate(DateTime date);
        List<Appointment> ReadAvailableAppointmentsByDateForTrainer(int trainerId, DateTime date);
    }
}