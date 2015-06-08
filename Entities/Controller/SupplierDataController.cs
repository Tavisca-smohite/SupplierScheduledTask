using System;
using System.Collections.Generic;
using System.Linq;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.BusinessLogic.Contracts;
using Tavisca.SupplierScheduledTask.BusinessLogic.Helper;
using Tavisca.SupplierScheduledTask.BusinessLogic.ProductSuppliersStrategy;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.Notifications;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.BusinessLogic.Controller
{
    //TODO: Need to move this logic in Business 
    public class SupplierDataController : ISupplierDataController
    {
        private static  Dictionary<string, IProductSupplier> _supplierStatistics;
       // private IUpdateFaresourcesConfig _updateFaresourcesConfig;

        public SupplierDataController()
        {
           // _updateFaresourcesConfig =RuntimeContext.Resolver.Resolve<IUpdateFaresourcesConfig>("UpdateFaresourcesConfig");
            _supplierStatistics = new Dictionary<string, IProductSupplier>();
            _supplierStatistics.Add("Air", new AirProductSupplierStrategy());
            _supplierStatistics.Add("Hotel", new HotelProductSupplierStrategy());
            _supplierStatistics.Add("Car", new CarProductSupplierStrategy());
        }

        public void Invoke()
        {
            var suppliersToDisable = new Dictionary<Supplier, string>();
            var suppliersWhoHaveCrossedThreshhold = new Dictionary<Supplier, string>();// suppliersToDisable.Where(d => !string.Equals(d.Value, string.Empty)).ToDictionary(x => x.Key, x => x.Value);
            try
            {
                Dictionary<string, List<Supplier>> productWiseSuppliersList =new SupplierDataHelper().GetProductWiseSuppliersList();
                suppliersToDisable = GetListOfSuppliersToDisable(productWiseSuppliersList);
                suppliersWhoHaveCrossedThreshhold = suppliersToDisable.Where(d => !string.Equals(d.Value, string.Empty)).ToDictionary(x => x.Key, x => x.Value);
           
                //if(suppliersWhoHaveCrossedThreshhold.Count>0)
                //{
                //    //TODO: call sp to get statuses of suppliers whether they are enabled or disabled and update suppliers' field "IsDisabled" 
                //    DisableSuppliers(suppliersWhoHaveCrossedThreshhold);
                //}
                                            
            }
            catch (Exception exception)
            {
                //TODO: ignore or log
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }
            finally
            {                
                if (suppliersWhoHaveCrossedThreshhold.Any())
                    new SendNotificationMail().SendNotificationEmail(suppliersToDisable);
            }
        }

        public Dictionary<Supplier, string> GetListOfSuppliersToDisable(Dictionary<string, List<Supplier>> productWiseSuppliersList)
        {
            var suppliersToDisable = new Dictionary<Supplier, string>();
            foreach (var productWiseSupplier in productWiseSuppliersList)
            {
                Dictionary<Supplier, string> suppliersWithFailureRate =
                    _supplierStatistics[productWiseSupplier.Key].GetFailureRateForProductSuppliers(
                        productWiseSupplier.Value);
                Dictionary<Supplier, string> supplierstoDisable = SupplierDataHelper.CompareThreshhold(suppliersWithFailureRate);
                suppliersToDisable = suppliersToDisable.Concat(supplierstoDisable).ToDictionary(x => x.Key, x => x.Value);
            }
            return suppliersToDisable;
        }



        //public bool DisableSuppliers(Dictionary<Supplier, string> suppliersToDisable)
        //{
        //    var disabledSuppliers = new List<Supplier>();
        //    foreach (var supplierToDisable in suppliersToDisable)
        //    {
        //       var isDisabled= _updateFaresourcesConfig.DisableSupplier(supplierToDisable.Key);
        //        supplierToDisable.Key.IsDisabled = isDisabled;
        //        if(isDisabled)
        //        {
        //            disabledSuppliers.Add(supplierToDisable.Key);
        //        }
        //    }
        //    _updateFaresourcesConfig.DisableSupplier(disabledSuppliers);
        //    return false;
        //}

        //public void EnableSuppliers()
        //{
        //    throw new NotImplementedException();
        //}
    }
}