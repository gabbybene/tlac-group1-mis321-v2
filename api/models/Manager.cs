using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
namespace api.models
{
    public class Manager
    {
        public int managerId {get; set;}
        public string password {get; set;}
        public string fName {get; set;}
        public string lName {get; set;}
        public string email {get; set;}
    }
}