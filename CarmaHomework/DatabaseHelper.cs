using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace CarmaHomework
{
    public static class DatabaseHelper
    {
        public static string ConnectionString { get; private set; }

        static DatabaseHelper()
        {
            var settings = ConfigurationManager.ConnectionStrings["CarmaDb"];
            if (settings != null)
            {
                ConnectionString = settings.ConnectionString;
            }
            else
            {
                ConnectionString = null;
            }
        }


        /// <summary>
        /// Create a new customer in the database.
        /// </summary>
        /// <param name="firstName">First name of the customer.</param>
        /// <param name="lastName">Last name of the customer.</param>
        public static void CreateCustomer(string firstName, string lastName)
        {
            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("First name and last name must not be null or empty.");
            }

            if ((firstName.Length > 50) || (lastName.Length > 50)) // We can change this constraint later
            {
                throw new ArgumentException("The length of first name and last name must be less than or equal to 50");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO [Customer] VALUES(@FirstName, @LastName)", connection)) // Use parameters to avoid SQL injection
                {
                    command.Parameters.AddWithValue("FirstName", firstName);
                    command.Parameters.AddWithValue("LastName", lastName);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieve all current customers in the database.
        /// </summary>
        /// <returns>List of current customers.</returns>
        public static IList<Customer> RetrieveCustomers()
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [Customer]", connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                        });
                    }
                }
            }

            return customers;
        }

        /// <summary>
        /// Create a new order in the database.
        /// </summary>
        /// <param name="price">Price of the order.</param>
        /// <param name="customerId">The corresponding customer's Id.</param>
        public static void CreateOrder(decimal price, int customerId)
        {
            // Can check for max value of price if need to. The SQL command will just throw an arithmetic overflow otherwise.
            if (price < 0)
            {
                throw new ArgumentException("Price must be non-negative.");
            }
            if (customerId <= 0)
            {
                throw new ArgumentException("Customer Id must be greater than 0.");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO [Order] VALUES(@Price, @CustomerId)", connection))
                {
                    command.Parameters.AddWithValue("Price", price);
                    command.Parameters.AddWithValue("CustomerId", customerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Create a new order in the database.
        /// </summary>
        /// <param name="price">The price of the order.</param>
        /// <param name="customer">The corresponding customer.</param>
        public static void CreateOrder(decimal price, Customer customer)
        {
            CreateOrder(price, customer.CustomerId);
        }

        /// <summary>
        /// Retrieve all the orders in the database.
        /// </summary>
        /// <returns>List of current orders.</returns>
        public static IList<Order> RetrieveOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [Order]", connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = reader.GetInt32(0),
                            Price = reader.GetDecimal(1),
                            CustomerId = reader.GetInt32(2),
                        });
                    }
                }
            }

            return orders;
        }

        /// <summary>
        /// Retrieve all customers along with their orders and the total price of the orders.
        /// </summary>
        /// <returns>List of current customers with their orders.</returns>
        public static IList<CustomerWithOrders> RetrieveCustomersWithOrders()
        {
            var customersWithOrders = new List<CustomerWithOrders>();

            var customers = RetrieveCustomers();
            var orders = RetrieveOrders();

            // There are many solutions to this problem (SQL queries, Linq, etc.). I think the one below is quite clear.
            // If performance is critical, we can revisit this function to do careful analysis/profiling and choose another solution if needed.
            // Regarding memory limit, this can handle a small number of customers and orders. If we want to handle more we may look into implementing pagination.
            var customersLeftJoinOrders = from customer in customers
                                          join order in orders on customer.CustomerId equals order.CustomerId into gj
                                          from subOrder in gj.DefaultIfEmpty()
                                          select new { Customer = customer, Order = subOrder ?? null };

            customersWithOrders = customersLeftJoinOrders.GroupBy(leftJoin => leftJoin.Customer.CustomerId)
               .Select(groupedLeftJoin => new CustomerWithOrders
               {
                   Customer = groupedLeftJoin.First().Customer,
                   Orders = groupedLeftJoin.Where(x => x.Order != null).Select(x => x.Order).ToList(),
                   TotalOrderPrice = groupedLeftJoin.Where(x => x.Order != null).Sum(x => x.Order.Price)
               }).ToList();

            return customersWithOrders;
        }
    }
}
