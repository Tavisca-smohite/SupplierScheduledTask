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

        #region MockConstructors
        public SupplierDataController(IProductSupplier _productSupplier)
        {
            _updateFaresourcesConfig = RuntimeContext.Resolver.Resolve<IUpdateFaresourcesConfig>("UpdateFaresourcesConfig");                
            _resourceDataController = RuntimeContext.Resolver.Resolve<IResourceDataController>("ResourceDataController");
            _supplierStatistics = new Dictionary<string, IProductSupplier>
                {
                    {"Air", _productSupplier},
                    {"Hotel", _productSupplier},
                    {"Car", _productSupplier}
                };
        }
        public SupplierDataController(IProductSupplier _productSupplier, IUpdateFaresourcesConfig updateFaresourcesConfig)
        {
            _updateFaresourcesConfig = updateFaresourcesConfig;
            _resourceDataController = RuntimeContext.Resolver.Resolve<IResourceDataController>("ResourceDataController");
            _supplierStatistics = new Dictionary<string, IProductSupplier>
                {
                    {"Air", _productSupplier},
                    {"Hotel", _productSupplier},
                    {"Car", _productSupplier}
                };
        }
        #endregion


        public void Invoke()
        {
            SupplierDataHelper.WriteIntoLogFile("invoke execution started...");
            var suppliersToDisable = new Dictionary<Supplier, string>();
            var suppliersWhoHaveCrossedThreshhold = new Dictionary<Supplier, string>();
            bool isMailSend;
            try
            {
                Dictionary<string, List<Supplier>> productWiseSuppliersList =new SupplierDataHelper().GetProductWiseSuppliersList();
                suppliersToDisable = GetListOfSuppliersToDisable(productWiseSuppliersList);
                suppliersWhoHaveCrossedThreshhold = suppliersToDisable.Where(d => !string.Equals(d.Value, string.Empty)).ToDictionary(x => x.Key, x => x.Value);
                SupplierDataHelper.WriteIntoLogFile(string.Format("suppliersWhoHaveCrossedThreshhold count: {0}",suppliersWhoHaveCrossedThreshhold.Count));
                if (suppliersWhoHaveCrossedThreshhold.Count > 0)
                {                    
                    DisableSuppliers(suppliersWhoHaveCrossedThreshhold);
                }
                                            
            }
            catch (Exception exception)
            {
                //TODO: ignore or log
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
                SupplierDataHelper.WriteIntoLogFile(string.Format("exception occured in invoke method with message {0}",exception.Message));
            }
            finally
            {
                if (suppliersWhoHaveCrossedThreshhold.Any())
                {
                    isMailSend = new SendNotificationMail().SendNotificationEmail(suppliersToDisable);
                    SupplierDataHelper.WriteIntoLogFile(string.Format("mail send for disabled suppliers state:{0}",
                                                                      isMailSend));
                }
                EnableSuppliers();
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
            SupplierDataHelper.WriteIntoLogFile("returning suppliers list to disable...");
            return suppliersToDisable;

        }


        #region interface methods
        public bool DisableSuppliers(Dictionary<Supplier, string> suppliersWhoHaveCrossedThreshhold)
        {          
            var isConfiguredToDisable = Convert.ToInt32(Configuration.DisableAllSuppliers);
            SupplierDataHelper.WriteIntoLogFile(string.Format("isConfiguredToDisable value: {0}", isConfiguredToDisable));
            if (isConfiguredToDisable == 1)
            {
                var disabledSuppliers = new List<Supplier>();
                foreach (var supplierToDisable in suppliersWhoHaveCrossedThreshhold)
                {
                    SupplierDataHelper.WriteIntoLogFile(string.Format("supplier id: {0}, DisableIfCrossesThreshhold value :{1}", 
                                                                         supplierToDisable.Key.SupplierId,
                                                                         supplierToDisable.Key.DisableIfCrossesThreshhold));
                    if (supplierToDisable.Key.DisableIfCrossesThreshhold == 1)
                    {
                        var isDisabled = _updateFaresourcesConfig.DisableSupplier(supplierToDisable.Key.SupplierId);
                        supplierToDisable.Key.IsDisabled = isDisabled;
                        if (isDisabled)
                        {
                            disabledSuppliers.Add(supplierToDisable.Key);
                        }
                    }
                }
                //TODO:pass list to resx file to set info about suppliers who has disabled

                SupplierDataHelper.WriteIntoLogFile(string.Format("disabled suppliers' count: {0}", disabledSuppliers.Count));
                if (disabledSuppliers.Any())
                    _resourceDataController.UpdateResourceFile(disabledSuppliers);
            }
            return false;
        }
        
        public void EnableSuppliers()
        {
            //read resource file,compare each value with current time if it exceeds 30 minute enable supplier and remove entry from resource file
            SupplierDataHelper.WriteIntoLogFile("inside EnableSuppliers()");
            var enabledSuppliersKeys = new List<string>();
            var resourceEntries = _resourceDataController.ReadResourceFile();

            foreach (var resourceEntry in resourceEntries)
            {
                if (CompareTimeIntervals(resourceEntry.Value))
                {
                    var key = RetriveIdFromKey(resourceEntry.Key);
                    if (key != 0)
                    {
                        var isEnabled = _updateFaresourcesConfig.EnableSupplier(key);
                        if (isEnabled)
                        {
                            enabledSuppliersKeys.Add(resourceEntry.Key);
                        }
                    }                   
                }
            }                   
                if(enabledSuppliersKeys.Any())
                {
                    SupplierDataHelper.WriteIntoLogFile(string.Format("enabled suppliers count: {0}", enabledSuppliersKeys.Count));
                    _resourceDataController.RemoveEntriesFromResourceFile(enabledSuppliersKeys);  //remove supplier entries from resource file which are enabled (if exists)   
                    resourceEntries = _resourceDataController.ReadResourceFile();
                    var disabledSuppliers = resourceEntries.Keys.ToList();
                    new SendNotificationMail().SendNotificationEmail(enabledSuppliersKeys,disabledSuppliers);
                }
                       
        }

        #endregion
        #region private helper methods
        private bool CompareTimeIntervals(string timeWhenSuppplierWasDisabled)
        {
            var _timeWhenSuppplierWasDisabled = Convert.ToDateTime(timeWhenSuppplierWasDisabled);
            var timeInterval = (DateTime.Now - _timeWhenSuppplierWasDisabled).TotalMinutes;
            if (timeInterval >= 30)
                return true;

            return false;
        }

        private int RetriveIdFromKey(string key)
        {
            //TODO: Use tryparse
            var keys = new List<string>();
            var id = 0;
            if (string.IsNullOrEmpty(key))
                id=0;

            keys.AddRange(key.Split('_'));
            if (keys.Count == 2)
                id=Convert.ToInt32(keys[1]);
            return id;
        }

        #endregion

    }
}