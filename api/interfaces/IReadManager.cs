using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadManager
    {
        List<Manager> ReadAll(); 
        Manager GetManagerByID(int id);
    }
}