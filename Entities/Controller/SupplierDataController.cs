using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.Notifications;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    //TODO: Need to move this logic in Business 
    public class SupplierDataController : ISupplierDataController
    {
        private static  Dictionary<string, IProductSupplier> _supplierStatistics;
        private IUpdateFaresourcesConfig _updateFaresourcesConfig;
        private IResourceDataController _resourceDataController;

        public SupplierDataController()
        {
            _updateFaresourcesConfig = RuntimeContext.Resolver.Resolve<IUpdateFaresourcesConfig>("UpdateFaresourcesConfig");
            _resourceDataController = RuntimeContext.Resolver.Resolve<IResourceDataController>("ResourceDataController");
            _supplierStatistics = new Dictionary<string, IProductSupplier>
                {
                    {"Air", new AirProductSupplierStrategy()},
                    {"Hotel", new HotelProductSupplierStrategy()},
                    {"Car", new CarProductSupplierStrategy()}
                };
        }

        public SupplierDataController(IProductSupplier _productSupplier)
        {
           // _updateFaresourcesConfig = RuntimeContext.Resolver.Resolve<IUpdateFaresourcesConfig>("UpdateFaresourcesConfig");
           // _resourceDataController = RuntimeContext.Resolver.Resolve<IResourceDataController>("ResourceDataController");
            _supplierStatistics = new Dictionary<string, IProductSupplier>
                {
                    {"Air", _productSupplier},
                    {"Hotel", _productSupplier},
                    {"Car", _productSupplier}
                };
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



        //public bool DisableSuppliers(Dictionary<Supplier, string> suppliersWhoHaveCrossedThreshhold)
        //{
        //    var disabledSuppliers = new List<Supplier>();
        //    foreach (var supplierToDisable in suppliersWhoHaveCrossedThreshhold)
        //    {
        //        if (supplierToDisable.Key.DisableIfCrossesThreshhold == 1)
        //        {
        //            var isDisabled = _updateFaresourcesConfig.DisableSupplier(supplierToDisable.Key);
        //            supplierToDisable.Key.IsDisabled = isDisabled;
        //            if (isDisabled)
        //            {
        //                disabledSuppliers.Add(supplierToDisable.Key);
        //            }
        //        }
        //    }
        //    //TODO:pass list to resx file to set info about suppliers who has disabled
        //    _resourceDataController.UpdateResourceFile(disabledSuppliers);
        //    return false;
        //}

        //public void EnableSuppliers()
        //{
        //    //read resource file,compare each value with current time if it exceeds 30 minute enable supplier and remove entry from resource file
        //    var enabledSuppliers = new List<Supplier>();
        //    var resourceEntries = _resourceDataController.ReadResourceFile();
            
        //    foreach (var resourceEntry in resourceEntries)
        //    {
        //        //var key = disabledSupplier.SupplierId.ToString(CultureInfo.InvariantCulture);

        //        if (CompareTimeIntervals(resourceEntry.Value))
        //        { 
        //            //TODO:call enable supplier if is enabled add to list

        //        }
        //        //TODO:compare and call enable supplier. If enabled add t olist and pass list to remove keys

        //    }
        //    //Write the combined resource file


        //}

        private bool CompareTimeIntervals(string timeWhenSuppplierWasDisabled)
        {
            var _timeWhenSuppplierWasDisabled = Convert.ToDateTime(timeWhenSuppplierWasDisabled);
            var timeInterval = (DateTime.Now - _timeWhenSuppplierWasDisabled).TotalMinutes;
            if (timeInterval >= 30)
                return true;

            return false;
        }

    }
}