using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadActivity
    {
        Activity Read(int id);
        List<Activity> ReadAll();
        List<int> GetTrainerActivities(int trainerId);
    }
}