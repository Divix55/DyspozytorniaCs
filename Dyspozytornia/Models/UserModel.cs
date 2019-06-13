using System;

namespace Dyspozytornia.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String UserPassword { get; set; }
        public int Privileges { get; set; }
    }
}