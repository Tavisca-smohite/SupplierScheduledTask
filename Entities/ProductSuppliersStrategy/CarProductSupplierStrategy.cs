﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Tavisca.Singularity;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.DataAccessLayer;
using Tavisca.SupplierScheduledTask.Notifications;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    public class CarProductSupplierStrategy : IProductSupplier
    {
         public CarProductSupplierStrategy()
        {
            _supplierRepository = RuntimeContext.Resolver.Resolve<ISupplierLogRepository>("SupplierLogRepository");
        }

         public CarProductSupplierStrategy(ISupplierLogRepository supplierRepository)
         {
             _supplierRepository = supplierRepository;
         }

        private ISupplierLogRepository _supplierRepository;
        #region IProductSupplier Members

        public Dictionary<Supplier, string> GetFailureRateForProductSuppliers(List<Supplier> suppliersList)
        {
            int minutes = (!string.IsNullOrEmpty(Configuration.TimeDiffInMinutes)) ? Convert.ToInt32(Configuration.TimeDiffInMinutes) : 60;
            SupplierDataHelper.WriteIntoLogFile("inside carproductstrategy GetFailureRateForProductSuppliers....");
            var supplierAndFailureRateMapping = new Dictionary<Supplier, string>();
            foreach (Supplier supplier in suppliersList)
            {
                var supplierStats = _supplierRepository.GetFailureLogs(supplier, minutes);
                SupplierDataHelper.WriteIntoLogFile(string.Format("supplier stats for {0} ,id: {1} ,total count: {2} failure count :  {3}, failure rate : {4} ",
                                                      supplier.SupplierName,
                                                      supplier.SupplierId,
                                                      supplierStats.TotalCallsCount,
                                                      supplierStats.TotalFailureCallsCount,
                                                      supplierStats.FailureRate));
                if (supplierStats.IsEnabled == 1)
                {
                    var failureRate = (supplierStats.TotalRate < 100) ? string.Empty : supplierStats.FailureRate.ToString(CultureInfo.InvariantCulture);
                    supplier.TotalCallsCount = supplierStats.TotalCallsCount;
                    supplier.TotalSuccessfulCallsCount = supplierStats.TotalSuccessfulCallsCount;
                    supplier.TotalFailureCallsCount = supplierStats.TotalFailureCallsCount;
                    supplierAndFailureRateMapping.Add(supplier, failureRate);
                }    
            }

            return supplierAndFailureRateMapping;
        }

        #endregion

       
    }
}