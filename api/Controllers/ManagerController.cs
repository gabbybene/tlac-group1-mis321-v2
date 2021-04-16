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
    public class ManagerController : ControllerBase
    {
        // GET: api/Manager
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Manager> Get()
        {
            IReadManager rm = new ReadManager();
            return rm.ReadAll();
        }

        // GET: api/Manager/5
        [HttpGet("{id}", Name = "GetManager")]
        public Manager Get(int id)
        {
            IReadManager rm = new ReadManager();
            return rm.GetManagerByID(id);
        }

        // POST: api/Manager
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Manager m)
        {
            IWriteManager wm = new WriteManager();
             System.Console.WriteLine("made it to the post.");
            wm.Write(m);
        }

        // PUT: api/Manager/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Manager m)
        {
            IUpdateManager um = new UpdateManager();
            System.Console.WriteLine("made it to the update.");
            um.Update(m);
        }

        // DELETE: api/Manager/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            IDeleteManager dm = new DeleteManager();
            dm.Delete(id);
        }
    }
}
