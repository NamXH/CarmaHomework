using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarmaHomework;
using System.Collections.Generic;

namespace CarmaTest
{
    [TestClass]
    public class DatabaseHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LinkCustomersWithOrders_NullCustomersAndOrders()
        {
            DatabaseHelper.LinkCustomersWithOrders(null, null);
        }

        [TestMethod]
        public void LinkCustomersWithOrders_EmptyCustomersAndOrders()
        {
            var customersWithOrders = DatabaseHelper.LinkCustomersWithOrders(new List<Customer> { }, new List<Order> { });
            Assert.AreEqual(0, customersWithOrders.Count, "Actual CustomersWithOrders's count is not 0.");
        }

        [TestMethod]
        public void LinkCustomersWithOrders_Normal()
        {
            var numberOfCustomer = 10;
            var ordersPerCustomer = 3;
            var priceSeed = 1;
            var priceIncrement = 0.5m;

            var customers = DatabaseTestingUtility.GenerateCustomers(numberOfCustomer);
            var orders = DatabaseTestingUtility.GenerateOrders(numberOfCustomer, ordersPerCustomer, priceSeed, priceIncrement);
            var customersWithOrders = DatabaseHelper.LinkCustomersWithOrders(customers, orders);

            Assert.AreEqual(numberOfCustomer, customersWithOrders.Count, String.Format("Expected CustomersWithOrders's count: {0}. Actual: {1}", numberOfCustomer, customersWithOrders.Count));
            Assert.AreEqual(ordersPerCustomer, customersWithOrders[0].Orders.Count, String.Format("Expected orders per customer: {0}. Actual: {1}", ordersPerCustomer, customersWithOrders[0].Orders.Count));

            // Calculate total price of orders of customer i (based 0). There are many ways to calculate this, some are more concise but this one is quite clear.
            var i = 9;
            var thisCustomerFirstOrderIndex = i * ordersPerCustomer;
            var thisCustomerFirstOrderPrice = priceSeed + thisCustomerFirstOrderIndex * priceIncrement;
            var thisCustomerTotalPrice = ((decimal)ordersPerCustomer / 2) * (2 * thisCustomerFirstOrderPrice + (ordersPerCustomer - 1) * priceIncrement);
            Assert.AreEqual(thisCustomerTotalPrice, customersWithOrders[i].TotalOrderPrice, String.Format("Expected total price of orders of customer {0}: {1}. Actual: {2}", i, thisCustomerTotalPrice, customersWithOrders[i - 1].TotalOrderPrice));
        }
    }
}
