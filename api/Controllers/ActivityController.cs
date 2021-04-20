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
    public class ActivityController : ControllerBase
    {
        // GET: api/Activity
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Activity> Get()
        {
            IReadActivity ra = new ReadActivity();
            return ra.ReadAll();
        }

        // GET: api/Activity/5
        [EnableCors("AnotherPolicy")]
        [HttpGet("{id}", Name = "GetActivity")]
        public Activity Get(int id)
        {
            IReadActivity ra = new ReadActivity();
            return ra.Read(id);
        }

        //GET:  activityID and price associated with a TrainerID
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpGet]
        public List<Activity> GetTrainerActivities(int id)
        {
            IReadActivity ra = new ReadActivity();
            return ra.GetTrainerActivities(id);
        }


        // POST: api/Activity
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Activity a)
        {
            IWriteActivity wa = new WriteActivity();
             System.Console.WriteLine("made it to the post.");
            wa.Write(a);
        }

        // PUT: api/Activity/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] List<Activity> a)
        {
            IUpdateActivity ua = new UpdateActivity();
            System.Console.WriteLine("made it to the update.");
            ua.Update(a);
        }

        // DELETE: api/Activity/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            IDeleteActivity da = new DeleteActivity();
            da.Delete(id);
        }
    }
}
