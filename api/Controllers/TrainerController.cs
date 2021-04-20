using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using api.models;
using api.Database;
using api.interfaces;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        // GET: api/Trainer
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Trainer> Get()
        {
            IReadTrainer rt = new ReadTrainer();
            return rt.ReadAll();
        }

        // GET: api/Trainer/email
        [EnableCors("AnotherPolicy")]
        [HttpGet("{email}", Name = "GetTrainer")]
        public Trainer Get(string email)
        {
            IReadTrainer rt = new ReadTrainer();
            return rt.Read(email);
        }

        // GET: api/Trainer/id
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpGet]
        public Trainer GetTrainerByID(int id)
        {
            IReadTrainer rt = new ReadTrainer();
            return rt.GetTrainerByID(id);
        }

        // POST: api/Trainer
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Trainer t)
        {
            IWriteTrainer wt = new WriteTrainer();
             System.Console.WriteLine("made it to the post.");
            wt.Write(t);
        }

        // PUT: api/Trainer/5
        [EnableCors("AnotherPolicy")]
        [HttpPut]
        public void Put([FromBody] Trainer t)
        {
            IUpdateTrainer ut = new UpdateTrainer();
            System.Console.WriteLine("made it to the update.");
            ut.Update(t);
        }

        // DELETE: api/Trainer/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            IDeleteTrainer dt = new DeleteTrainer();
            dt.Delete(id);
        }
    }
}
