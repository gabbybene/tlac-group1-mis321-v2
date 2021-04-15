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
            cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,gender,AccountID,Refer_custID,phone) FROM Customer WHERE AccountID=@AccountID";
            cmd.Parameters.AddWithValue("@AccountID",emailAddress);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read()){
                new Customer(){customerId=rdr.GetInt32(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), email=rdr.GetString(6),referredBy=GetCustomerByID(rdr[7]),phoneNo=rdr.GetString(8)};
            }
            return null;
        }
        public Customer GetCustomerByID(object id){
            if (id==null){return null;}
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,gender,AccountID,phone) FROM Customer WHERE CustID=@CustID";
            cmd.Parameters.AddWithValue("@CustID",id);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read()){
                return new Customer(){customerId=rdr.GetInt32(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), email=rdr.GetString(6),phoneNo=rdr.GetString(7)};
            }
            return null;
        }
        public List<Customer> ReadAll(){

            //We may want to update this method further to see if we can retrieve all customers (comment by Gabby) 
            List<Customer> allCustomers = new List<Customer>();
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * from Customer";
            using MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read()){
                Customer temp = new Customer(){customerId=rdr.GetInt32(0), fName=rdr.GetString(2), lName=rdr.GetString(3), birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), email=rdr.GetString(7)};
                allCustomers.Add(temp);
            }

            // return allCustomers;
            throw new System.NotImplementedException();
        }
    }
}