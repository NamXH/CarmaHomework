using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarmaHomework
{
    public static class DatabaseHelper
    {
        public static readonly string ConnectionString = "Server=.\\SQLEXPRESS;Database=CarmaDb;Trusted_Connection=True";

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

            if ((firstName.Length > 50) || (lastName.Length > 50))
            {
                throw new ArgumentException("The length of first name and last name must be less than or equal to 50");
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO [Customer] VALUES(@FirstName, @LastName)"))
                {
                    command.Connection = connection;
                    command.Parameters.AddWithValue("FirstName", firstName);
                    command.Parameters.AddWithValue("LastName", lastName);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve all current customers in the database.
        /// </summary>
        /// <returns>List of customers.</returns>
        public static IList<Customer> RetrieveCustomers()
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [Customer]"))
                {
                    command.Connection = connection;
                    try
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
                    finally
                    {
                        connection.Close();
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
                using (SqlCommand command = new SqlCommand("INSERT INTO [Order] VALUES(@Price, @CustomerId)"))
                {
                    command.Connection = connection;
                    command.Parameters.AddWithValue("Price", price);
                    command.Parameters.AddWithValue("CustomerId", customerId);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
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
        /// <returns>List of orders.</returns>
        public static IList<Order> RetrieveOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM [Order]"))
                {
                    command.Connection = connection;
                    try
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
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return orders;
        }
    }
}
