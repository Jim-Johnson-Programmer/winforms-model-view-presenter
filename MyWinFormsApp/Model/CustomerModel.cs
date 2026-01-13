using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinFormsApp.Model
{
    public class CustomerModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

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
