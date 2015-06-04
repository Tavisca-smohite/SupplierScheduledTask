using System;
using System.Collections.Generic;
using System.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.Contracts;
using Tavisca.SupplierScheduledTask.BusinessLogic.Helper;
using Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy;
using Tavisca.SupplierScheduledTask.Notifications;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.BusinessLogic.Controller
{
    //TODO: Need to move this logic in Business 
    public class SupplierDataController : ISupplierDataController
    {
        private static readonly Dictionary<string, IProductSupplier> SupplierStatistics =
            new Dictionary<string, IProductSupplier>();

        public SupplierDataController()
        {


            SupplierStatistics.Add("Air", new AirProductSupplierStrategy());
            SupplierStatistics.Add("Hotel", new HotelProductSupplierStrategy());
            SupplierStatistics.Add("Car", new CarProductSupplierStrategy());
        }

        public void Invoke()
        {
            var suppliersToDisable = new Dictionary<Supplier, float>();

            try
            {
                Dictionary<string, List<Supplier>> productWiseSuppliersList =
                    new SupplierDataHelper().GetProductWiseSuppliersList();
                suppliersToDisable = GetListOfSuppliersToDisable(productWiseSuppliersList);
                //TODO: call sp to get statuses of suppliers whether they are enabled or disabled and update suppliers' field "IsDisabled"                             
            }
            catch (Exception exception)
            {
                //TODO: ignore or log
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            finally
            {
                bool isMailSend = new SendNotificationMail().SendNotificationEmail(suppliersToDisable);
            }
        }

        public Dictionary<Supplier, float> GetListOfSuppliersToDisable(
            Dictionary<string, List<Supplier>> productWiseSuppliersList)
        {
            var suppliersToDisable = new Dictionary<Supplier, float>();
            foreach (var productWiseSupplier in productWiseSuppliersList)
            {
                Dictionary<Supplier, float> suppliersWithFailureRate =
                    SupplierStatistics[productWiseSupplier.Key].GetFailureRateForProductSuppliers(
                        productWiseSupplier.Value);
                Dictionary<Supplier, float> supplierstoDisable = SupplierDataHelper.CompareThreshhold(suppliersWithFailureRate);
                suppliersToDisable = suppliersToDisable.Concat(supplierstoDisable).ToDictionary(x => x.Key, x => x.Value);
            }
            return suppliersToDisable;
        }

       
    }
}