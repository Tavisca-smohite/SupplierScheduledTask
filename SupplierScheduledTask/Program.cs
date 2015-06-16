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
                SupplierDataHelper.WriteIntoLogFile("Execution started....");
                //TODO:create object using singularity
                new SupplierDataController().Invoke();
            }
            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");    
                SupplierDataHelper.WriteIntoLogFile("Exception occured in main...");
            }          
            Console.ReadLine();
        }
    }
}
