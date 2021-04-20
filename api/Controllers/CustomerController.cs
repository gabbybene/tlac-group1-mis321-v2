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
    public class CustomerController : ControllerBase
    {
        // GET: api/Customer
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Customer> Get()
        {
            IReadCustomer rc = new ReadCustomer();
            return rc.ReadAll();
        }

        // GET: api/Customer/email
        [EnableCors("AnotherPolicy")]
        [HttpGet("{email}", Name = "GetCustomer")]
        public Customer Get(string email)
        {
            IReadCustomer rc = new ReadCustomer();
            return rc.Read(email);
        }

        // GET: api/
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpGet]
        public Customer GetCustomerByID(int id)
        {
            IReadCustomer rc = new ReadCustomer();
            return rc.GetCustomerByID(id);
        }

        // POST: api/Customer
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] Customer c)
        {
            IWriteCustomer wc = new WriteCustomer();
            System.Console.WriteLine("made it to the post.");
            wc.Write(c);
        }

        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpPost]
        public void PostCustomerWithReferredBy([FromBody] Customer c, int id)
        {
            //get referredBy Customer to add to the customer sending into the Update() method
            IReadCustomer rc = new ReadCustomer();
            c.referredBy = rc.GetCustomerByID(id);
            Console.WriteLine("found a customer with that id. ID is " + c.referredBy.customerId);

            WriteCustomer wc = new WriteCustomer();
            wc.Write(c);
        }

        // PUT: api/Customer/
        [EnableCors("AnotherPolicy")]
        [HttpPut]
        public void Put([FromBody] Customer c)
        {
            IUpdateCustomer uc = new UpdateCustomer();
            System.Console.WriteLine("made it to the update.");
            uc.Update(c);
        }
        
        [EnableCors("AnotherPolicy")]
        [Route("[action]/{id}")]
        [HttpPut]
        public void PutCustomerWithReferredBy([FromBody] Customer c, int id)
        {
            //get referredBy Customer to add to the customer sending into the Update() method
            IReadCustomer rc = new ReadCustomer();
            c.referredBy = rc.GetCustomerByID(id);
            Console.WriteLine("found a customer with that id. ID is " + c.referredBy.customerId);

            IUpdateCustomer uc = new UpdateCustomer();
            System.Console.WriteLine("made it to the update.");
            uc.Update(c);
        }

        // DELETE: api/Customer/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            IDeleteCustomer dc = new DeleteCustomer();
            System.Console.WriteLine("deleted the customer.");
            dc.Delete(id);

        }
    }
}
