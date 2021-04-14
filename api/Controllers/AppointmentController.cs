using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        // GET: api/Appointment
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Appointment> Get()
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.ReadAll();
        }

        // GET: api/Appointment/5
        [HttpGet("{id}", Name = "Get")]
        public Appointment Get(int id)
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.Read(id);
        }
        
        // GetDistinctAvailableAppointments
        [HttpGet("{date}", Name = "GetDistinctAvailableAppointments")]
        public List<Appointment> GetDistinctAvailableAppointments()
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.ReadDistinctAvailableAppointments();
        }
        // GetAvailableAppointmentsByDate
        [HttpGet("{date}", Name = "GetAvailableAppointmentsByDate")]
        public List<Appointment> GetAvailableAppointmentsByDate(DateTime date)
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.ReadAvailableAppointmentsByDate(date);
        }

        // GetAvailableAppointmentsByDate for individual trainer
        [HttpGet("{date}", Name = "GetAvailableAppointmentsByDateForTrainer")]
        public List<Appointment> GetAvailableAppointmentsByDateForTrainer(int trainerId, DateTime date)
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.ReadAvailableAppointmentsByDateForTrainer(trainerId,date);
        }

        // POST: api/Appointment
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Appointment a)
        {
            IWriteAppointment wa = new WriteAppointment();
             System.Console.WriteLine("made it to the post.");
            wa.Write(a);
        }

        // PUT: api/Appointment/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Appointment a)
        {
            IUpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.Update(a);
        }
        // Update Appointments by adding a Customer to appointment
        [HttpPut("{customerid}", name = "UpdateByAddingCustomerId")]
        public void PutByAddingCustomerId(int id, [FromBody] Appointment a)
        {
            IUpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.UpdateAddCustomerId(a);
        }
        // Update Appointments by Deleting a customer from appointment
        [HttpPut("{customerid}", name = "UpdateByDeletingCustomerId")]
        public void PutByDeletingCustomerId(int id, [FromBody] Appointment a)
        {
            IUpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.UpdateDeleteCustomerId(a);
        }

        // DELETE: api/Appointment/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            IDeleteAppointment da = new DeleteAppointment();
            da.Delete(id);
        }
        
    }
}
