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
            cmd.CommandText = @"SELECT custID,AccountID,fName,lName,DOB,Gender,Refer_CustID,Phone,AcctID,Password,IFNULL(fitnessGoal, @ifnullvalue) AS fitnessGoal FROM Customer c JOIN Account a ON c.AccountID = a.AcctID WHERE c.AccountID=@AccountID";
            cmd.Parameters.AddWithValue("@AccountID",emailAddress);
            cmd.Parameters.AddWithValue("@ifnullvalue", "Please enter your fitness goals here!");
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();

            Customer customer = new Customer();
            while(rdr.Read()){
                //if phoneNo and referredBy are both null, new Customer without either
                if(rdr.IsDBNull(6) && rdr.IsDBNull(7)){
                    System.Console.WriteLine("Read1");
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }
                else if(rdr.IsDBNull(6) && !rdr.IsDBNull(7)){
                    //if referredBy is null, but phoneNo is not, new Customer with phoneNo
                    System.Console.WriteLine("Read2");
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), phoneNo=rdr.GetString(7), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                    System.Console.WriteLine(customer.customerId);

                }
                else if(!rdr.IsDBNull(6) && rdr.IsDBNull(7)){
                    System.Console.WriteLine("Read3");
                    //if referredBy is not null, but phoneNo is null, new Customer with referredBy
                     Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), referredBy=GetCustomerByID(rdr.GetInt32(6)), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }

                else {
                    //if phoneNo and referredBy are both NOT null, new Customer with both
                    System.Console.WriteLine("Read4");
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }        
            }
           return customer;
        }
        public Customer GetCustomerByID(int id){
            // if (id==null){
            //     return new Customer(){customerId = -1};
            // }
            System.Console.WriteLine("Get Customer by ID");
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            // cmd.CommandText = @"SELECT (CustID,fName,lName,DOB,Gender,AccountID,Phone) FROM Customer WHERE CustID=@CustID";
            cmd.CommandText = @"SELECT custID,AccountID,fName,lName,DOB,Gender,Refer_CustID,Phone,AcctID,Password,IFNULL(fitnessGoal, @ifnullvalue) FROM Customer c JOIN Account a ON c.AccountID = a.AcctID WHERE c.CustID=@CustID";
            cmd.Parameters.AddWithValue("@CustID",id);
            cmd.Parameters.AddWithValue("@ifnullvalue", "Please enter your fitness goals here!");
            cmd.Prepare();
            using MySqlDataReader rdr = cmd.ExecuteReader();
    
            Customer customer = new Customer();
            while(rdr.Read()){
                if(rdr.IsDBNull(6) && rdr.IsDBNull(7)){
                    System.Console.WriteLine("Read5");
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }
                else if(rdr.IsDBNull(6) && !rdr.IsDBNull(7)){
                    System.Console.WriteLine("Read6");
                    //if referredBy is null, but phoneNo is not, new Customer with phoneNo
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), phoneNo=rdr.GetString(7), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }
                else if(!rdr.IsDBNull(6) && rdr.IsDBNull(7)){
                    System.Console.WriteLine("Read7");
                    //if referredBy is not null, but phoneNo is null, new Customer with referredBy
                     Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), referredBy=GetCustomerByID(rdr.GetInt32(6)), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    customer = temp;
                }

                else {
                    //if phoneNo and referredBy are both NOT null, new Customer with both
                    System.Console.WriteLine("Read8");
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
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
            cmd.CommandText = @"SELECT custID,AccountID,fName,lName,DOB,Gender,Refer_CustID,Phone,AcctID,Password,IFNULL(fitnessGoal, @ifnullvalue) FROM Customer c JOIN Account a ON c.AccountID = a.AcctID";
            cmd.Parameters.AddWithValue("@ifnullvalue", "Please enter your fitness goals here!");
            using MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read()){
                if(rdr.IsDBNull(6)){
                    //if referredBy is null, make a new Customer() without a referredBy property
                    
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5), /*phoneNo=rdr.GetString(7),*/ password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    allCustomers.Add(temp);
                }
                else {
                    //if referredBy is not null, make a new Customer() containing that referredBy customerId
                    Customer temp = new Customer(){customerId=rdr.GetInt32(0),email=rdr.GetString(1),fName=rdr.GetString(2),lName=rdr.GetString(3),birthDate=rdr.GetDateTime(4), gender=rdr.GetString(5),referredBy=GetCustomerByID(rdr.GetInt32(6)),phoneNo=rdr.GetString(7), password=rdr.GetString(9),fitnessGoals=rdr.GetString(10)};
                    temp.customerActivities = GetCustomerPreferredActivities(temp);
                    allCustomers.Add(temp);
                }      
            }
            return allCustomers;
        }
        private List<Activity> GetCustomerPreferredActivities(Customer cust){
            ConnectionString cs = new ConnectionString();
            using var con = new MySqlConnection(cs.cs);
            con.Open();
            using var cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT p.activityid,activityname from prefers p join activity a on p.activityid=a.activityid WHERE p.custID=@cust";
            cmd.Parameters.AddWithValue("@cust",cust.customerId);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            List<Activity> returnList = new List<Activity>();
            while(rdr.Read()){
                returnList.Add(new Activity(){activityId=rdr.GetInt32(0),activityName=rdr.GetString(1)});
            }
            return returnList;
        }
    }
}