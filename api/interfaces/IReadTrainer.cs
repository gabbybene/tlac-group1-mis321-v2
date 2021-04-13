using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadTrainer
    {
         Trainer Read(string email);
         Trainer GetTrainerByID(int id);
         List<Trainer> ReadAll();
    }
}