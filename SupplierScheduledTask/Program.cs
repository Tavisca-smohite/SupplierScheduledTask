using System;
using Tavisca.SupplierScheduledTask.BusinessLogic;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace SupplierScheduledTask
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine(@"Started SupplierScheduledTask....");
            try
            {
                new SupplierDataController().Invoke();
            }
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");               
            }          
            Console.ReadLine();
        }
    }
}
