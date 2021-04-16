using api.models;
using System.Collections.Generic;

namespace api.interfaces
{
    public interface IReadCustomer
    {
        Customer Read(string email);
        Customer GetCustomerByID(int id);
        List<Customer> ReadAll();
    }
}