using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IUpdateActivity
    {
        void Update(List<Activity> act);
    }
}