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
        [EnableCors("AnotherPolicy")]
        [HttpGet("{id}", Name = "GetAppointment")]
        public Appointment Get(int id)
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.Read(id);
        }
        
        // GetDistinctAvailableAppointments
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{date}")]
        [HttpGet]
        //[HttpGet("{date}", Name = "GetDistinctAvailableAppointments")]
        public List<DateTime> GetDistinctAvailableAppointments()
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadDistinctAvailableAppointments();
        }

        // GetAvailableAppointmentsByDate
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{date}")]
        [HttpGet]
        //[HttpGet("{date}", Name = "GetAvailableAppointmentsByDate")]
        public List<Appointment> GetAvailableAppointmentsByDate(DateTime date)
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadAvailableAppointmentsByDate(date);
        }

        // GetAvailableAppointmentsByDate for individual trainer
        [EnableCors("AnotherPolicy")]
        [HttpGet("{date}", Name = "GetAvailableAppointmentsByDateForTrainer")]
        public List<Appointment> GetAvailableAppointmentsByDateForTrainer(int trainerId, DateTime date)
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadAvailableAppointmentsByDateForTrainer(trainerId,date);
        }

        //Get a Customer's Confirmed Appointments
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{customerId}")]
        [HttpGet]
        public List<Appointment> GetConfirmedAppointmentsForCustomer(int customerId)
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadConfirmedAppointmentsForCustomer(customerId);
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
        public void Put(int id, [FromBody] List<Appointment> a)
        {
            IUpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.Update(a);
        }
        
        
        // Update Appointments by adding a Customer to appointment
        [Route("[action]/{customerid}")]
        [HttpPut]
        //[HttpPut("{customerid}", Name = "UpdateByAddingCustomerId")]
          public void PutByAddingCustomerId(int id, [FromBody] Appointment a)
        {
            UpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.UpdateAddCustomerId(a,id);
        }
       
       
        // Update Appointments by Deleting a customer from appointment 

        [HttpPut("{customerid}", Name = "UpdateByDeletingCustomerId")]
        public void PutByDeletingCustomerId(int id, [FromBody] Appointment a)
        {
            UpdateAppointment ua = new UpdateAppointment();
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
