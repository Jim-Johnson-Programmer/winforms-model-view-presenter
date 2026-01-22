using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinFormsApp.Model
{
    public class CustomerModel
    {
        public string Name { get; set; } = "Jim";
        public string Email { get; set; } = "abc@abc.com";

        public bool IsValidEmail()
        {
            return Email.Contains("@");
        }

        public string FormatCustomerDetails()
        {
            return $"{Name} <{Email}>";
        }
    }
}
