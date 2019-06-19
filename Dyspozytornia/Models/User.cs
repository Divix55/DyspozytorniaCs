using System.ComponentModel;

namespace Dyspozytornia.Models
{
    public class User
    {
        [DisplayName("Store ID")] public int StoreId { get; set; }

        [DisplayName("User name")] public string UserName { get; set; }

        [DisplayName("First name")] public string FirstName { get; set; }

        [DisplayName("Last name")] public string LastName { get; set; }

        [DisplayName("Email")] public string Email { get; set; }

        [DisplayName("Password")] public string UserPassword { get; set; }

        public int Privileges { get; set; }

        public bool isValidLogin()
        {
            return true;
        }

        public bool isValidRegister()
        {
            return true;
        }
    }
}