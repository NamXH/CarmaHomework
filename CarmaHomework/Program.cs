using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarmaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            //DatabaseHelper.CreateCustomer("123456789012345678901234567890123456789012345678901234567890", "foo02");
            //DatabaseHelper.CreateCustomer("foo03", "foo03");
            //DatabaseHelper.CreateOrder(1, 2);
            var customers = DatabaseHelper.RetrieveCustomers();
            foreach (var c in customers)
            {
                Console.WriteLine(c.CustomerId + " " + c.FirstName + " " + c.LastName);
            }
            var orders = DatabaseHelper.RetrieveOrders();
            foreach (var o in orders)
            {
                Console.WriteLine(o.OrderId + " " + o.Price + " " + o.CustomerId);
            }
            Console.ReadLine();
        }
    }
}
