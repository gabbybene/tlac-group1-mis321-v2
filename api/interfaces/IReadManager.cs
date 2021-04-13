using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadManager
    {
        Manager Read();
        List<Manager> ReadAll(); 
    }
}