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

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Get")]
        public Customer Get(string email)
        {
            IReadCustomer rc = new ReadCustomer();
            return rc.Read(email);
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

        // PUT: api/Customer/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer c)
        {
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
