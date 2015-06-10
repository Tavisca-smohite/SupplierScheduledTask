using System;
using Tavisca.SupplierScheduledTask.BusinessLogic;

namespace SupplierScheduledTask
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine(@"Started SupplierScheduledTask....");
        new SupplierDataController().Invoke();
            Console.ReadLine();
        }
    }
}
