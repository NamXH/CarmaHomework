using Newtonsoft.Json;

namespace CarmaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = JsonConvert.SerializeObject(DatabaseHelper.RetrieveCustomersWithOrders());
        }
    }
}
