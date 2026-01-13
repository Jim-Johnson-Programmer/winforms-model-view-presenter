using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinFormsApp.View
{
    public interface ICustomerView
    {
        string CustomerName { get; set; }
        string CustomerEmail { get; set; }
        
        event EventHandler SaveClicked;
    }
}
