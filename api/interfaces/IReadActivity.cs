using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadActivity
    {
        Activity Read(int id);
        List<Activity> ReadAll();
        List<Activity> GetTrainerActivities(int trainerId);
    }
}