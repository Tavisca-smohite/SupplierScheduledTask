using System;
using System.Collections.Generic;
using System.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.TravelNxt.Shared.Entities.Infrastructure;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public class SupplierLogRepository : ISupplierLogRepository
    {
        #region ISupplierLogRepository Members

        public SupplierStatistics GetFailureLogs(Supplier supplier, int minutes)
        {
            var supplierStatistics=new SupplierStatistics();
            var getFailureStatResult = new List<spGetLogBasedOnCallTypeResult>();                    
            try
            {
                SupplierDataManagerDBContext.UsingCommonContentDbRead(db =>
                    {
                        getFailureStatResult =
                            db.spGetLogBasedOnCallType(supplier.CallType, supplier.SupplierId, minutes,
                                                       supplier.SupplierName).ToList();
                    });                                             
            }               

            catch (Exception exception)
            {
                LogUtility.GetLogger().WriteAsync(exception.ToContextualEntry(), "Log Only Policy");
            }

            supplierStatistics = ParseLogBasedOnCallTypeResult(getFailureStatResult);
            return supplierStatistics;
        }

        private static SupplierStatistics ParseLogBasedOnCallTypeResult(List<spGetLogBasedOnCallTypeResult> getFailureStatResult)
        {
            
            var supplierStats = new SupplierStatistics();
            double perFailureRate = 0, perSuccessRate = 0;
            int isEnabled = 0, totalCount = 0, successCount = 0, failureCount = 0;
            if (getFailureStatResult != null && getFailureStatResult.Count > 0)
            {

                foreach (var result in getFailureStatResult)
                {

                    perFailureRate = result.PerFailure;
                    perSuccessRate = result.PerSuccess;
                    //TODO: use tryparse
                    totalCount = (successCount=(result.Success != null) ? Convert.ToInt32(result.Success) : 0) +
                                 (failureCount=(result.Failure != null) ? Convert.ToInt32(result.Failure) : 0);
                    isEnabled = result.IsEnabled;
                }
            }
            supplierStats.FailureRate = perFailureRate;
            supplierStats.SuccessRate = perSuccessRate;
            supplierStats.TotalRate = perFailureRate + perSuccessRate;
            supplierStats.IsEnabled = isEnabled;
            supplierStats.TotalSuccessfulCallsCount = successCount;
            supplierStats.TotalFailureCallsCount = failureCount;
            supplierStats.TotalCallsCount = totalCount;


            return supplierStats;
        }

        #endregion
    }
}