using api.models;

namespace api.interfaces
{
    public interface IDeleteActivity
    {
        void Delete(int id);
        void DeletePreferredActivitities(int id);
        void DeleteTrainerActivitities(int id);
    }
}