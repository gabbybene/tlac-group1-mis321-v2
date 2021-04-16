using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;

namespace api.Database{
    public class DeleteCustomer : IDeleteCustomer
    {
        public void Delete(int id){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            IReadCustomer readCust = new ReadCustomer();
            Customer deletedCustomer = readCust.GetCustomerByID(id);
            cmd.CommandText = @"delete from account where AcctID=@email";
            cmd.Parameters.AddWithValue("@email",deletedCustomer.email);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}