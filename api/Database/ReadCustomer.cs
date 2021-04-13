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
            cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,Gender,AcctID,Refer_custID,Phone) FROM Trainer WHERE AcctID=@AcctID";
            cmd.Parameters.AddWithValue("@AcctID",emailAddress);
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
            cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,Gender,AcctID,Phone) FROM Trainer WHERE CustID=@CustID";
            cmd.Parameters.AddWithValue("@CustID",id);
            cmd.Prepare();
            cmd.ExecuteReader();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read()){
                return new Customer(){customerId=rdr.GetInt32(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), email=rdr.GetString(6),phoneNo=rdr.GetString(7)};
            }
            return null;
        }
        public List<models.Customer> ReadAll(){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            throw new System.NotImplementedException();
        }

        public Customer GetCustomerByID(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}