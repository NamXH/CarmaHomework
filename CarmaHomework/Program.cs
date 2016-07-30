using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CarmaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(DatabaseHelper.RetrieveCustomersWithOrders()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // For simplicity, we don't implement logging service here.
            }
            Console.ReadLine();
        }
    }
}
