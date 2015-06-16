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
                SupplierDataHelper.WriteIntoLogFile("execution started....");
                new SupplierDataController().Invoke();
            }
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");    
                SupplierDataHelper.WriteIntoLogFile("exception occured in main...");
            }          
            Console.ReadLine();
        }
    }
}
