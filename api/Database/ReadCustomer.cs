using api.models;
using api.interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace api.Database{
    public class ReadCustomer : IReadCustomer
    {
        public Customer Read(string emailAddress){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            // cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,Gender,AccountID,Refer_custID,Phone) FROM Customer WHERE AcctID=@AcctID";
            cmd.CommandText = @"SELECT * FROM Customer c JOIN Account a ON c.AccountID = a.AcctID WHERE c.AccountID=@AccountID";
            cmd.Parameters.AddWithValue("@AccountID",emailAddress);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            Customer customer = new Customer();
            while(rdr.Read()){
                if(rdr.IsDBNull(6)){
                    //if referredBy is null, return a new Customer, but leave referredBy as null
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                    customer = temp;
                }
                else {
                    //if referredBy is not null, return the new Customer that includes a referredBy
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                    customer = temp;
                }
               
            }
           return customer;
        }
        public Customer GetCustomerByID(object id){
            if (id==null){
                return new Customer(){customerId = -1};
            }
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            // cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,Gender,AccountID,Phone) FROM Customer WHERE CustID=@CustID";
            cmd.CommandText = @"SELECT * FROM Customer c JOIN Account a ON c.AccountID = a.AcctID WHERE CustID=@CustID";
            cmd.Parameters.AddWithValue("@CustID",id);
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
    
            Customer customer = new Customer();
            while(rdr.Read()){
                if(rdr.IsDBNull(6)){
                    //if referredBy is null, return a new Customer, but leave referredBy as null
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                    customer = temp;
                }
                else {
                    //if referredBy is not null, return the new Customer that includes a referredBy
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                    customer = temp;
                }
            }
            return customer;
        }
        public List<Customer> ReadAll(){
            List<Customer> allCustomers = new List<Customer>();

            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * from Customer c JOIN Account a ON c.AccountID = a.AcctID";
            using MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read()){
                if(rdr.IsDBNull(6)){
                    //if referredBy is null, make a new Customer() without a referredBy property
                    
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                }
                else {
                    //if referredBy is not null, make a new Customer() containing that referredBy customerId
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9)};
                    allCustomers.Add(temp);
                }      
            }
            return allCustomers;
        }
    }
}