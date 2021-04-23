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
        [Route("[action]/{id}")]
        [HttpGet]
        public Appointment GetAppointmentByID(int id)
        {
            IReadAppointment ra = new ReadAppointment();
            return ra.Read(id);
        }
        
        // GetDistinctAvailableAppointments
        [EnableCors("AnotherPolicy")]
        [Route("[action]/")]
        [HttpGet]
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
        [Route("[action]/{id}/{date}")]
        [HttpGet]
        // [HttpGet("{date}", Name = "GetAvailableAppointmentsByDateForTrainer")]
        public List<Appointment> GetAvailableAppointmentsByDateForTrainer(int id, DateTime date)
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadAvailableAppointmentsByDateForTrainer(id,date);
        }

        // Get Confirmed Appointments for Trainer
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpGet]
        public List<Appointment> GetConfirmedAppointmentsForTrainer(int id){
            ReadAppointment ra = new ReadAppointment();
            return ra.ReadConfirmedAppointmentsForTrainer(id);
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
        //Get maximum appointment ID
        [EnableCors("AnotherPolicy")]
        [Route("[action]")]
        [HttpGet]
        public int GetMaxID()
        {
            ReadAppointment ra = new ReadAppointment();
            return ra.GetMaxAppointmentID();
        }

        // POST: api/Appointment
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Appointment a)
        {
            IWriteAppointment wa = new WriteAppointment();
            wa.Write(a);
        }

        [EnableCors("AnotherPolicy")]
        [Route("[action]/{startTime}/{endTime}")]
        [HttpPost]
        public void WriteAvailableAppointment(Appointment i, string startTime, string endTime){
            WriteAppointment wa = new WriteAppointment();
            wa.WriteAvailableAppointment(i, startTime, endTime);
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

        //PUT: update available appointment
        // [EnableCors("AnotherPolicy")]
        // [Route("[action]/")]
        // [HttpPut]
        // public void PutAvailableAppointment([FromBody] Appointment a){
        //     UpdateAppointment ua = new UpdateAppointment();
        //     ua.UpdateAvailableAppointment(a);
        // }

        //PUT: update available appt w/ startTime, endTime, and Activity
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{apptId}/{startTime}/{endTime}/{activityId}")]
        [HttpPut]
        public void PutAvailableAppointment(int apptId, string startTime, string endTime, int activityId){
            UpdateAppointment ua = new UpdateAppointment();
            ua.UpdateAvailableAppointment(apptId, startTime, endTime, activityId);
        }

        // PUT: add Customer to appointment
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{ids}")]
        [HttpPut]
        public void PutByAddingCustomerId(int[] ids)
        {
            IUpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update");
            ua.UpdateAddCustomerId(ids);
        }

        [EnableCors("AnotherPolicy")]
        [Route("[action]/")]
        [HttpPut]
        public void PutByAddingCustomer([FromBody] Appointment a){
            UpdateAppointment ua = new UpdateAppointment();
            ua.UpdateAddCustomer(a);
        }
       
       
        // Update Appointments by Deleting a customer from appointment 
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{ids}")]
        [HttpPut]
        // [HttpPut("{ids}", Name = "UpdateByDeletingCustomerId")]
        public void PutByDeletingCustomerId(int[] ids/*int id, [FromBody] Appointment a*/)
        {
            UpdateAppointment ua = new UpdateAppointment();
            System.Console.WriteLine("made it to the update.");
            ua.UpdateDeleteCustomerId(ids);
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
