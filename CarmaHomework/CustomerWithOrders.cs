using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarmaHomework
{
    class CustomerWithOrders
    {
        public Customer Customer { get; set; }
        public IList<Order> Orders { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }
}
