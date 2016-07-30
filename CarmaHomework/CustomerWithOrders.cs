using System.Collections.Generic;

namespace CarmaHomework
{
    public class CustomerWithOrders
    {
        public Customer Customer { get; set; }
        public IList<Order> Orders { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }
}
