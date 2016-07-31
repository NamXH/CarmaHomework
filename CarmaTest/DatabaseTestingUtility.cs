using CarmaHomework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarmaTest
{
    public static class DatabaseTestingUtility
    {
        public static IList<Customer> GenerateCustomers(int numberOfCustomers)
        {
            var customers = new List<Customer>();
            for (var i = 1; i <= numberOfCustomers; i++)
            {
                customers.Add(new Customer
                {
                    CustomerId = i,
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                });
            }
            return customers;
        }

        public static IList<Order> GenerateOrders(int numberOfOrders, int customerId, decimal price)
        {
            var orders = new List<Order>();
            for (var i = 1; i <= numberOfOrders; i++)
            {
                orders.Add(new Order 
                {
                    OrderId = i,
                    CustomerId = customerId,
                    Price = price,
                });
            }
            return orders;
        }

        public static IList<Order> GenerateOrders(int numberOfCustomers, int ordersPerCustomer, decimal priceSeed, decimal priceIncrement)
        {
            var orders = new List<Order>();
            var count = 1;
            for (var i = 1; i <= numberOfCustomers; i++)
            {
                for (var j = 1; j <= ordersPerCustomer; j++)
                {
                    orders.Add(new Order
                    {
                        OrderId = count,
                        CustomerId = i,
                        Price = priceSeed + priceIncrement * (count - 1),
                    });
                    count++;
                }
            }
            return orders;
        }
    }
}
